using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Autodesk.Max;
using Autodesk.Max.MaxSDK.Util;
using Autodesk.Max.Plugins;
using Outliner.MaxUtils;
using Outliner.Controls;
using Outliner.Controls.Tree;
using Outliner.Controls.Tree.Layout;
using Outliner.Filters;
using Outliner.Modes;
using Outliner.NodeSorters;
using Outliner.Plugins;
using Outliner.Presets;
using Outliner.Scene;

namespace Outliner
{
public class OutlinerGUP
{
   public static OutlinerGUP Instance { get; private set; }

   public Boolean SettingsLoaded { get; private set; }
   public OutlinerState State { get; private set; }
   public TreeViewColorScheme ColorScheme { get; private set; }

   public Dictionary<TreeView, TreeMode> TreeModes { get; private set; }
   private Dictionary<TreeView, OutlinerPreset> currentPresets;

   public OutlinerGUP()
   {
      this.TreeModes = new Dictionary<TreeView, TreeMode>();
      this.currentPresets = new Dictionary<TreeView, OutlinerPreset>();

      OutlinerPlugins.LoadPlugins();

      this.SettingsLoaded = true;

      try
      {
         this.ColorScheme = this.loadColors(OutlinerPaths.ColorFile);
      }
      catch
      {
         this.ColorScheme = TreeViewColorScheme.MayaColors;
         this.SettingsLoaded = false;
      }

      try
      {
         OutlinerPresets.LoadPresets();
      }
      catch
      {
         this.SettingsLoaded = false;
      }

      try
      {
         this.State = this.loadState(OutlinerPaths.StateFile);
      }
      catch
      {
         this.State = defaultState();
         this.SettingsLoaded = false;
      }
   }

   internal static void Start()
   {
      OutlinerGUP.Instance = new OutlinerGUP();
   }

   internal void Stop() 
   {
      foreach (TreeMode treeMode in this.TreeModes.Values)
      {
         treeMode.Stop();
      }
      this.TreeModes.Clear();
   }


   public TreeMode GetActiveTreeMode(TreeView tree)
   {
      TreeMode mode = null;
      if (this.TreeModes != null)
         this.TreeModes.TryGetValue(tree, out mode);
      return mode;
   }

   internal void RegisterTreeMode(TreeView tree, TreeMode treeMode)
   {
      if (!this.TreeModes.ContainsKey(tree))
         this.TreeModes.Add(tree, treeMode);
   }

   internal void UnRegisterTreeMode(TreeMode treeMode)
   {
      foreach (KeyValuePair<TreeView, TreeMode> item in this.TreeModes)
      {
         if (item.Value.Equals(treeMode))
         {
            this.TreeModes.Remove(item.Key);
            break;
         }
      }
   }


   /// <summary>
   /// Looks up the preset registered for the given treeview.
   /// </summary>
   public OutlinerPreset GetActivePreset(TreeView tree)
   {
      OutlinerPreset preset = null;
      if (this.currentPresets != null)
         this.currentPresets.TryGetValue(tree, out preset);
      return preset;
   }

   /// <summary>
   /// Switches the mode, layout, sorter and filter of a TreeView defined by 
   /// the given preset.
   /// </summary>
   /// <param name="start">If true, the new TreeMode will be started.</param>
   public void SwitchPreset(TreeView tree, OutlinerPreset preset, Boolean start)
   {
      TreeMode oldMode = this.GetActiveTreeMode(tree);
      if (oldMode != null)
      {
         oldMode.Stop();
         this.UnRegisterTreeMode(oldMode);
      }

      this.currentPresets.Remove(tree);
      this.currentPresets.Add(tree, preset);

      tree.TreeNodeLayout = preset.TreeNodeLayout;
      tree.NodeSorter = preset.NodeSorter;
      TreeMode newMode = preset.CreateTreeMode(tree);

      this.RegisterTreeMode(tree, newMode);

      if (start)
         newMode.Start();
   }


   public void ReloadSettings()
   {
      XmlSerializationHelpers.ClearSerializerCache();
      OutlinerPresets.LoadPresets();
      this.ColorScheme = this.loadColors(OutlinerPaths.ColorFile);
      this.State       = this.loadState(OutlinerPaths.StateFile);
   }


   private TreeViewColorScheme loadColors(String colorFile)
   {
      if (File.Exists(colorFile))
         return XmlSerializationHelpers.Deserialize<TreeViewColorScheme>(colorFile);
      else
         return TreeViewColorScheme.MayaColors;
   }

   private OutlinerState loadState(String stateFile)
   {
      if (File.Exists(stateFile))
         return XmlSerializationHelpers.Deserialize<OutlinerState>(stateFile);
      else
      {
         return defaultState();
      }
   }

   private OutlinerState defaultState()
   {
      OutlinerState state = new OutlinerState();
      IEnumerable<OutlinerPreset> presets = OutlinerPresets.Presets;
      if (presets.Count() > 0)
      {
         state.Tree1Preset = presets.First();
         state.Tree2Preset = presets.First();
      }
      return state;
   }


   public void StoreSettings()
   {
      if (!Directory.Exists(OutlinerPaths.ConfigDir))
         Directory.CreateDirectory(OutlinerPaths.ConfigDir);

      XmlSerializationHelpers.Serialize<OutlinerState>( OutlinerPaths.StateFile
                                                            , this.State);
   }

}
}
