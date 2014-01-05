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
using PJanssen;
using PJanssen.Logging;

namespace Outliner
{
/// <summary>
/// The main access point for the Outliner. It holds the settings, states and treemodes.
/// Use OutlinerGUP.Instance to get hold of an instance of this object once 3dsmax is loaded.
/// </summary>
public class OutlinerGUP
{
   /// <summary>
   /// Gets the active OutlinerGUP instance.
   /// </summary>
   public static OutlinerGUP Instance { get; private set; }
   
   /// <summary>
   /// Gets a flag indicating whether the settings have been loaded succesfully.
   /// </summary>
   public Boolean SettingsLoaded { get; private set; }

   /// <summary>
   /// Gets the exception thrown while trying to load the Outliner settings.
   /// </summary>
   public Exception SettingsLoadException { get; private set; }

   /// <summary>
   /// Gets the Settings collection for this Outliner instance.
   /// </summary>
   public SettingsCollection Settings { get; private set; }

   /// <summary>
   /// Gets the current Outliner state object.
   /// </summary>
   public OutlinerState State { get; private set; }

   /// <summary>
   /// Gets the ColorScheme object for this Outliner instance.
   /// </summary>
   public OutlinerColorScheme ColorScheme { get; private set; }

   /// <summary>
   /// Gets a collection of registered TreeModes and TreeView controls.
   /// </summary>
   public Dictionary<TreeView, TreeMode> TreeModes { get; private set; }
   private Dictionary<TreeView, OutlinerPreset> currentPresets;

   /// <summary>
   /// Gets the NameFilter used for all TreeModes.
   /// </summary>
   public NameFilter CommonNameFilter { get; private set; }

   //==========================================================================

   /// <summary>
   /// Gets the Outliner's Log object.
   /// </summary>
   public ITextLogger Log
   {
      get
      {
         if (this.log == null)
            log = CreateLog();

         return log;
      }
   }
   private ITextLogger log;

   private ITextLogger CreateLog()
   {
      return new MaxscriptListenerLogger("Outliner");
   }

   //==========================================================================

   private OutlinerGUP()
   {
      this.TreeModes = new Dictionary<TreeView, TreeMode>();
      this.currentPresets = new Dictionary<TreeView, OutlinerPreset>();

      this.CommonNameFilter = new NameFilter();
   }

   //==========================================================================

   internal static void Start()
   {
      OutlinerGUP.Instance = new OutlinerGUP();

      OutlinerPlugins.LoadPlugins();
      OutlinerGUP.Instance.ReloadSettings();
   }

   //==========================================================================

   internal void Stop() 
   {
      foreach (TreeMode treeMode in this.TreeModes.Values)
      {
         treeMode.Stop();
      }
      this.TreeModes.Clear();

      if (Directory.Exists(OutlinerPaths.ConfigDir))
         XmlSerialization.Serialize<SettingsCollection>(OutlinerPaths.SettingsFile, this.Settings);
   }

   //==========================================================================

   /// <summary>
   /// Gets all registered TreeView controls.
   /// </summary>
   public IEnumerable<TreeView> TreeViews
   {
      get
      {
         return this.TreeModes.Keys;
      }
   }

   //==========================================================================

   /// <summary>
   /// Gets the TreeMode for the given TreeView control.
   /// </summary>
   public TreeMode GetActiveTreeMode(TreeView tree)
   {
      TreeMode mode = null;
      if (this.TreeModes != null)
         this.TreeModes.TryGetValue(tree, out mode);
      return mode;
   }

   //==========================================================================

   internal void RegisterTreeMode(TreeView tree, TreeMode treeMode)
   {
      if (!this.TreeModes.ContainsKey(tree))
         this.TreeModes.Add(tree, treeMode);
   }

   //==========================================================================

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

   //==========================================================================

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

   //==========================================================================

   /// <summary>
   /// Switches the mode, layout, sorter and filter of a TreeView defined by 
   /// the given preset.
   /// </summary>
   /// <param name="start">If true, the new TreeMode will be started.</param>
   public TreeMode SwitchPreset(TreeView tree, OutlinerPreset preset, Boolean start)
   {
      Throw.IfNull(tree, "tree");
      Throw.IfNull(preset, "preset");

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

   //==========================================================================

   /// <summary>
   /// Reloads the Outliner settings.
   /// </summary>
   public Boolean ReloadSettings()
   {
      XmlSerialization.ClearSerializerCache();

      if (!Directory.Exists(OutlinerPaths.ConfigDir))
      {
         this.SettingsLoaded = false;
         this.SettingsLoadException = new Exception(String.Format("Outliner configuration directory does not exist. Expected:\r\n{0}", OutlinerPaths.ConfigDir));
         return false;
      }

      try
      {
         if (File.Exists(OutlinerPaths.SettingsFile))
            this.Settings = XmlSerialization.Deserialize<SettingsCollection>(OutlinerPaths.SettingsFile);
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
         this.ColorScheme = XmlSerialization.Deserialize<OutlinerColorScheme>(colorSchemeFile);
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
            this.State = XmlSerialization.Deserialize<OutlinerState>(OutlinerPaths.StateFile);
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

   //==========================================================================

   private static OutlinerState defaultState()
   {
      OutlinerState state = new OutlinerState();
      IEnumerable<OutlinerPreset> presets = Configurations.GetConfigurations<OutlinerPreset>(OutlinerPaths.PresetsDir);
      if (presets.Count() > 0)
      {
         state.Tree1Preset = presets.First();
         state.Tree2Preset = presets.First();
      }
      return state;
   }

   //==========================================================================

   /// <summary>
   /// Stores the current Outliner settings.
   /// </summary>
   public void StoreSettings()
   {
      if (!Directory.Exists(OutlinerPaths.ConfigDir))
         Directory.CreateDirectory(OutlinerPaths.ConfigDir);

      XmlSerialization.Serialize<OutlinerState>(OutlinerPaths.StateFile, this.State);
   }

   //==========================================================================

   /// <summary>
   /// Stops all registered TreeModes.
   /// </summary>
   public void Pause() 
   {
      foreach (TreeMode treeMode in this.TreeModes.Values)
      {
         treeMode.Stop();
      }
   }

   //==========================================================================

   /// <summary>
   /// Starts all registered TreeModes.
   /// </summary>
   public void Resume() 
   {
      foreach (TreeMode treeMode in this.TreeModes.Values)
      {
         treeMode.Start();
      }
   }

   //==========================================================================
}
}
