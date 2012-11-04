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
   public IEnumerable<OutlinerPreset> Presets { get; private set; }
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
         this.Presets = this.loadPresets(OutlinerPaths.PresetsDir);
      }
      catch
      {
         this.Presets = new List<OutlinerPreset>();
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
      this.Presets     = this.loadPresets(OutlinerPaths.PresetsDir);
      this.ColorScheme = this.loadColors(OutlinerPaths.ColorFile);
      this.State       = this.loadState(OutlinerPaths.StateFile);
   }


   private IEnumerable<OutlinerPreset> loadPresets(String presetsDir)
   {
      List<OutlinerPreset> presets = new List<OutlinerPreset>();

      if (Directory.Exists(presetsDir))
      {
         String[] presetFiles = Directory.GetFiles(presetsDir, "*.xml");
         Type[] extraTypes = OutlinerPlugins.GetSerializableTypes();
         foreach (String preset in presetFiles)
         {
            presets.Add(XmlSerializationHelpers<OutlinerPreset>.FromXml(preset, extraTypes));
         }
      }

      IEnumerable<OutlinerPreset> defaultPresets = OutlinerPlugins.GetPluginsByType(OutlinerPluginType.DefaultPreset)
                                                                  .Select(p => Activator.CreateInstance(p.Type) as OutlinerPreset);
      presets.AddRange(defaultPresets.Where(t => !presets.Any(p => p.Name.Equals(t.Name))));
      
      presets.Where(p => defaultPresets.Any(t => t.Name.Equals(p.Name)))
             .ForEach(p => p.IsDefaultPreset = true);

      presets.Sort((p, q) => p.Name.CompareTo(q.Name));

      return presets;
   }

   private TreeViewColorScheme loadColors(String colorFile)
   {
      if (File.Exists(colorFile))
         return XmlSerializationHelpers<TreeViewColorScheme>.FromXml(colorFile);
      else
         return TreeViewColorScheme.MayaColors;
   }

   private OutlinerState loadState(String stateFile)
   {
      if (File.Exists(stateFile))
         return XmlSerializationHelpers<OutlinerState>.FromXml(stateFile, OutlinerPlugins.GetSerializableTypes());
      else
      {
         return defaultState();
      }
   }

   private OutlinerState defaultState()
   {
      OutlinerState state = new OutlinerState();
      if (this.Presets != null && this.Presets.Count() > 0)
      {
         state.Tree1Preset = this.Presets.First();
         state.Tree2Preset = this.Presets.First();
      }
      return state;
   }


   public void StoreSettings()
   {
      if (!Directory.Exists(OutlinerPaths.ConfigDir))
         Directory.CreateDirectory(OutlinerPaths.ConfigDir);

      XmlSerializationHelpers<OutlinerState>.ToXml( OutlinerPaths.StateFile
                                                  , OutlinerPlugins.GetSerializableTypes()
                                                  , this.State);
   }

}
}
