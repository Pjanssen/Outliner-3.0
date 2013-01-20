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
using Outliner.Controls.ContextMenu;
using Outliner.Controls.Tree;
using Outliner.Controls.Tree.Layout;
using Outliner.Filters;
using Outliner.MaxUtils;
using Outliner.Modes;
using Outliner.NodeSorters;
using Outliner.Plugins;
using Outliner.Scene;

namespace Outliner.Configuration
{
public class OutlinerPreset : ConfigurationFile, ISorterConfiguration
{
   public OutlinerPreset() : base()
   {
      this.TreeModeTypeName = String.Empty;
      this.ContextMenuFile = String.Empty;
      this.Sorter = new AlphabeticalSorter();
      this.Filters = new MaxNodeFilterCombinator() { Enabled = false };
      this.IsDefaultPreset = false;
   }

   protected override string ImageBasePath
   {
      get { return OutlinerPaths.PresetsDir; }
   }

   protected override Image Image24Default
   {
      get
      {
         return OutlinerResources.default_preset_24;
      }
   }

   [XmlElement("isDefaultPreset")]
   [DefaultValue(false)]
   public Boolean IsDefaultPreset { get; set; }

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

   private string contextMenuFile;
   [XmlElement("contextmenu")]
   public virtual String ContextMenuFile 
   {
      get { return this.contextMenuFile; }
      set
      {
         this.contextMenuFile = value;

         String path = value;
         if (!Path.IsPathRooted(value))
            path = Path.Combine(OutlinerPaths.ContextMenusDir, value);
         if (path != null && File.Exists(path))
            this.ContextMenu = XmlSerializationHelpers.Deserialize<ContextMenuModel>(path);
      }
   }

   [XmlIgnore]
   public virtual ContextMenuModel ContextMenu
   {
      get; protected set;
   }

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
         if (path != null && File.Exists(path))
            this.TreeNodeLayout = XmlSerializationHelpers.Deserialize<TreeNodeLayout>(path);
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

   [XmlElement("sorter")]
   public NodeSorter Sorter { get; set; }

   [XmlElement("filters")]
   public MaxNodeFilterCombinator Filters { get; set; }


   public TreeMode CreateTreeMode(TreeView tree)
   {
      Throw.IfArgumentIsNull(tree, "tree");

      Type treeModeType = OutlinerPlugins.GetPluginType( OutlinerPluginType.TreeMode
                                                       , this.TreeModeTypeName);

      if (treeModeType == null)
         treeModeType = typeof(NullTreeMode);

      TreeMode mode = Activator.CreateInstance(treeModeType, new object[] { tree }) as TreeMode;
      foreach (Filter<IMaxNode> filter in this.Filters.Filters)
      {
         mode.AddPermanentFilter(filter);
      }
      //mode.PermanentFilter = this.Filters;

      String contextMenuFile = Path.Combine(OutlinerPaths.ContextMenusDir, this.ContextMenuFile);
      if (File.Exists(contextMenuFile))
         mode.ContextMenu = XmlSerializationHelpers.Deserialize<Outliner.Controls.ContextMenu.ContextMenuModel>(contextMenuFile);

      return mode;
   }
}
}
