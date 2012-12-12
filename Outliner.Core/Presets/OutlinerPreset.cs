using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Autodesk.Max;
using Outliner.Controls;
using Outliner.Controls.Tree;
using Outliner.Controls.Tree.Layout;
using Outliner.Filters;
using Outliner.MaxUtils;
using Outliner.Modes;
using Outliner.NodeSorters;
using Outliner.Plugins;
using Outliner.Scene;
using Outliner.Configuration;

namespace Outliner.Presets
{
public class OutlinerPreset : ConfigurationFile
{
   public OutlinerPreset() : base()
   {
      this.TreeModeTypeName = String.Empty;
      this.ContextMenuFile = String.Empty;
      this.NodeSorter = new AlphabeticalSorter();
      this.Filters = new MaxNodeFilterCombinator() { Enabled = false };
   }

   protected override string ImageBasePath
   {
      get { return OutlinerPaths.PresetsDir; }
   }

   [XmlIgnore]
   internal Boolean IsDefaultPreset { get; set; }

   [XmlElement("treemode")]
   public virtual String TreeModeTypeName { get; set; }

   [XmlIgnore]
   public Type TreeModeType
   {
      get
      {
         return OutlinerPlugins.GetPluginType( OutlinerPluginType.TreeMode
                                             , this.TreeModeTypeName);
      }
      set
      {
         if (value != null)
            this.TreeModeTypeName = value.FullName;
      }
   }

   [XmlElement("contextmenu")]
   public virtual String ContextMenuFile { get; set; }

   [XmlElement("nodesorter")]
   public virtual NodeSorter NodeSorter { get; set; }

   private String layoutFile;
   [XmlElement("treenodelayout")]
   public virtual String LayoutFile 
   {
      get { return this.layoutFile; }
      set
      {
         this.layoutFile = value;

         String path = value;
         if (!Path.IsPathRooted(value))
            path = Path.Combine(OutlinerPaths.LayoutsDir, value);
         if (path != null)
            this.TreeNodeLayout = XmlSerializationHelpers.Deserialize<TreeNodeLayout>(path);
            //this.TreeNodeLayout = XmlSerializationHelpers<TreeNodeLayout>.FromXml(path, OutlinerPlugins.GetSerializableTypes());
         }
   }

   private TreeNodeLayout layout;
   [XmlIgnore]
   public virtual TreeNodeLayout TreeNodeLayout 
   {
      get 
      {
         if (this.layout == null)
            return TreeNodeLayout.SimpleLayout;
         else
            return this.layout; 
      }
      protected set
      {
         this.layout = value;
      }
   }

   [XmlElement("filters")]
   public virtual MaxNodeFilterCombinator Filters { get; set; }


   public TreeMode CreateTreeMode(TreeView tree)
   {
      Throw.IfArgumentIsNull(tree, "tree");

      Type treeModeType = OutlinerPlugins.GetPluginType( OutlinerPluginType.TreeMode
                                                       , this.TreeModeTypeName);

      if (treeModeType == null)
         treeModeType = typeof(NullTreeMode);

      TreeMode mode = Activator.CreateInstance(treeModeType, new object[] { tree }) as TreeMode;
      mode.PermanentFilter = this.Filters;

      String contextMenuFile = Path.Combine(OutlinerPaths.ContextMenusDir, this.ContextMenuFile);
      if (File.Exists(contextMenuFile))
         mode.ContextMenu = XmlSerializationHelpers.Deserialize<Outliner.Controls.ContextMenu.ContextMenuModel>(contextMenuFile);

      return mode;
   }
}
}
