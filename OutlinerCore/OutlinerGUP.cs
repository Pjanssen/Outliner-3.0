using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Autodesk.Max;
using Autodesk.Max.MaxSDK.Util;
using Autodesk.Max.Plugins;
using MaxUtils;
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
   public OutlinerState State { get; private set; }
   public IEnumerable<OutlinerPreset> Presets { get; private set; }

   public Dictionary<TreeView, TreeMode> TreeModes { get; private set; }
   private Dictionary<TreeView, OutlinerPreset> currentPresets;
   public TreeViewColorScheme ColorScheme { get; private set; }

   public OutlinerGUP()
   {
      this.TreeModes = new Dictionary<TreeView, TreeMode>();
      this.currentPresets = new Dictionary<TreeView, OutlinerPreset>();

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
      newMode.Filters = preset.Filters;

      this.RegisterTreeMode(tree, newMode);

      if (start)
         newMode.Start();
   }


   public void ReloadSettings()
   {
      this.State = XmlSerializationHelpers.FromXml<OutlinerState>("C:/Users/Pier/Desktop/test.xml");
      this.Presets     = this.loadPresets();
      this.ColorScheme = this.loadColors();
   }


   private IEnumerable<OutlinerPreset> loadPresets()
   {
      String presetsDir = OutlinerPaths.Presets;

      List<OutlinerPreset> presets = new List<OutlinerPreset>();

      if (Directory.Exists(presetsDir))
      {
         String[] presetFiles = Directory.GetFiles(presetsDir, "*.xml");
         foreach (String preset in presetFiles)
         {
            presets.Add(XmlSerializationHelpers.FromXml<OutlinerPreset>(preset));
         }
      }

      IEnumerable<OutlinerPreset> defaultPresets = OutlinerPlugins.GetPluginsByType(OutlinerPluginType.DefaultPreset)
                                                                  .Select(p => Activator.CreateInstance(p.Type) as OutlinerPreset);
      presets.AddRange(defaultPresets.Where(t => !presets.Any(p => p.Name.Equals(t.Name))));
      
      presets.Where(p => defaultPresets.Any(t => t.Name.Equals(p.Name)))
             .ForEach(p => p.IsDefaultPreset = true);

      presets.Sort((p, q) => p.Name.CompareTo(q.Name));

      XmlSerializationHelpers.ToXml<OutlinerPreset>("C:/Users/Pier/Desktop/sertest.xml", presets.Find(p => p.Name == "Layers"));

      return presets;
   }


   private TreeViewColorScheme loadColors()
   {
      IIPathConfigMgr pathMgr = MaxInterfaces.Global.IPathConfigMgr.PathConfigMgr;
      IGlobal.IGlobalMaxSDK.IGlobalUtil.IGlobalPath path = MaxInterfaces.Global.MaxSDK.Util.Path;
      IPath scriptDir = path.Create(pathMgr.GetDir(MaxDirectory.UserScripts));
      IPath colorFile = path.Create(scriptDir).Append(path.Create("outliner_colors.xml"));
      if (colorFile.Exists)
         return TreeViewColorScheme.FromXml(colorFile.String);
      else
      {
         return TreeViewColorScheme.MayaColors;
         //tc.treeView1.Colors.ToXml(colorFile.String);
      }
   }


   public void StoreSettings()
   {
      //XmlSerializationHelpers.ToXml<OutlinerState>("C:/Users/Pier/Desktop/test.xml", this.State);
   }

}
}
