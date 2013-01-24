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
using Outliner.Scene;
using Outliner.Configuration;
using WinForms = System.Windows.Forms;

namespace Outliner
{
public class OutlinerGUP
{
   public static OutlinerGUP Instance { get; private set; }
   
   public Boolean SettingsLoaded { get; private set; }
   public Exception SettingsLoadException { get; private set; }
   public OutlinerState State { get; private set; }
   public OutlinerColorScheme ColorScheme { get; private set; }
   public SettingsCollection Settings { get; private set; }

   public Dictionary<TreeView, TreeMode> TreeModes { get; private set; }
   private Dictionary<TreeView, OutlinerPreset> currentPresets;

   public NameFilter CommonNameFilter { get; private set; }

   private OutlinerGUP()
   {
      this.TreeModes = new Dictionary<TreeView, TreeMode>();
      this.currentPresets = new Dictionary<TreeView, OutlinerPreset>();

      this.CommonNameFilter = new NameFilter();

      OutlinerPlugins.LoadPlugins();
      
      this.ReloadSettings();
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

      if (Directory.Exists(OutlinerPaths.ConfigDir))
         XmlSerializationHelpers.Serialize<SettingsCollection>(OutlinerPaths.SettingsFile, this.Settings);
   }


   public IEnumerable<TreeView> TreeViews
   {
      get
      {
         return this.TreeModes.Keys;
      }
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
   public TreeMode SwitchPreset(TreeView tree, OutlinerPreset preset, Boolean start)
   {
      Throw.IfArgumentIsNull(tree, "tree");
      Throw.IfArgumentIsNull(preset, "preset");

      TreeMode oldMode = this.GetActiveTreeMode(tree);
      if (oldMode != null)
      {
         oldMode.Stop();
         this.UnRegisterTreeMode(oldMode);
      }

      this.currentPresets.Remove(tree);
      this.currentPresets.Add(tree, preset);

      tree.TreeNodeLayout = preset.TreeNodeLayout;
      tree.NodeSorter = preset.Sorter;
      tree.Settings.DragDropMouseButton = this.Settings.GetValue<WinForms::MouseButtons>(OutlinerSettings.TreeCategory, OutlinerSettings.DragDropMouseButton);
      tree.Settings.DoubleClickAction = this.Settings.GetValue<TreeNodeDoubleClickAction>(OutlinerSettings.TreeCategory, OutlinerSettings.DoubleClickAction);
      tree.Settings.ScrollToSelection = this.Settings.GetValue<Boolean>(OutlinerSettings.TreeCategory, OutlinerSettings.ScrollToSelection);
      tree.Settings.AutoExpandSelectionParents = this.Settings.GetValue<Boolean>(OutlinerSettings.TreeCategory, OutlinerSettings.AutoExpandSelectionParents);
      tree.Settings.CollapseAutoExpandedParents = this.Settings.GetValue<Boolean>(OutlinerSettings.TreeCategory, OutlinerSettings.CollapseAutoExpandedParents);
      
      TreeMode newMode = preset.CreateTreeMode(tree);

      this.RegisterTreeMode(tree, newMode);

      newMode.AddPermanentFilter(this.CommonNameFilter);

      if (start)
         newMode.Start();

      return newMode;
   }


   public Boolean ReloadSettings()
   {
      XmlSerializationHelpers.ClearSerializerCache();

      if (!Directory.Exists(OutlinerPaths.ConfigDir))
      {
         this.SettingsLoaded = false;
         this.SettingsLoadException = new Exception(String.Format("Outliner configuration directory does not exist. Expected:\r\n{0}", OutlinerPaths.ConfigDir));
         return false;
      }

      try
      {
         if (File.Exists(OutlinerPaths.SettingsFile))
            this.Settings = XmlSerializationHelpers.Deserialize<SettingsCollection>(OutlinerPaths.SettingsFile);
         else
            this.Settings = new SettingsCollection();
      }
      catch (Exception e)
      {
         this.Settings = new SettingsCollection();
         this.SettingsLoaded = false;
         this.SettingsLoadException = e;
         return false;
      }
      OutlinerSettings.PopulateWithDefaults(this.Settings);

      try
      {
         String colorSchemeFile = this.Settings.GetValue<String>(OutlinerSettings.CoreCategory, OutlinerSettings.ColorSchemeFile);
         colorSchemeFile = Path.Combine(OutlinerPaths.ColorSchemesDir, colorSchemeFile);
         this.ColorScheme = XmlSerializationHelpers.Deserialize<OutlinerColorScheme>(colorSchemeFile);
      }
      catch (Exception e)
      {
         this.ColorScheme = OutlinerColorScheme.Default;
         this.SettingsLoaded = false;
         this.SettingsLoadException = e;
         return false;
      }

      try
      {
         if (File.Exists(OutlinerPaths.StateFile))
            this.State = XmlSerializationHelpers.Deserialize<OutlinerState>(OutlinerPaths.StateFile);
         else
            this.State = defaultState();
      }
      catch (Exception e)
      {
         this.State = defaultState();
         this.SettingsLoaded = false;
         this.SettingsLoadException = e;
         return false;
      }

      this.SettingsLoaded = true;
      return this.SettingsLoaded;
   }


   private static OutlinerState defaultState()
   {
      OutlinerState state = new OutlinerState();
      IEnumerable<OutlinerPreset> presets = ConfigurationHelpers.GetConfigurations<OutlinerPreset>(OutlinerPaths.PresetsDir);
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

   public void Pause() 
   {
      foreach (TreeMode treeMode in this.TreeModes.Values)
      {
         treeMode.Stop();
      }
   }
   public void Resume() 
   {
      foreach (TreeMode treeMode in this.TreeModes.Values)
      {
         treeMode.Start();
      }
   }
}
}
