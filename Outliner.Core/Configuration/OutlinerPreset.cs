using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using Outliner.Controls.ContextMenu;
using Outliner.Controls.Tree;
using Outliner.Controls.Tree.Layout;
using Outliner.Filters;
using Outliner.Modes;
using Outliner.NodeSorters;
using Outliner.Plugins;
using Outliner.Scene;

namespace Outliner.Configuration
{
/// <summary>
/// A preset for the Outliner, defining the TreeMode, Layout, ContextMenu, Filters and NodeSorter.
/// </summary>
public class OutlinerPreset : ConfigurationFile, ISorterConfiguration
{
   private string contextMenuFile;
   private String layoutFile;
   private TreeNodeLayout layout;

   /// <summary>
   /// Initializes a new instance of the OutlinerPreset class.
   /// </summary>
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

   /// <summary>
   /// Gets or sets whether this is a default, or a user-created preset.
   /// </summary>
   [XmlElement("isDefaultPreset")]
   [DefaultValue(false)]
   public Boolean IsDefaultPreset { get; set; }

   /// <summary>
   /// Gets or sets the type-name of the TreeMode to be used with this Preset.
   /// </summary>
   [XmlElement("treemode")]
   public virtual String TreeModeTypeName { get; set; }

   /// <summary>
   /// Gets or sets the type of the TreeMode to be used with this Preset.
   /// </summary>
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

   /// <summary>
   /// Gets or sets the filepath containing the ContextMenu associated with this Preset.
   /// </summary>
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
            this.ContextMenu = XmlSerialization.Deserialize<ContextMenuModel>(path);
      }
   }

   /// <summary>
   /// Gets the ContextMenu associated with this Preset.
   /// </summary>
   [XmlIgnore]
   public virtual ContextMenuModel ContextMenu
   {
      get; protected set;
   }

   /// <summary>
   /// Gets or sets the filepath containing the TreeNodeLayout associated with this Preset.
   /// </summary>
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
            this.TreeNodeLayout = XmlSerialization.Deserialize<TreeNodeLayout>(path);
      }
   }

   /// <summary>
   /// Gets the TreeNodeLayout associated with this Preset.
   /// </summary>
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

   /// <summary>
   /// Gets or sets the NodeSorter associated with this Preset.
   /// </summary>
   [XmlElement("sorter")]
   public NodeSorter Sorter { get; set; }

   /// <summary>
   /// Gets or sets the Filters associated with this Preset.
   /// </summary>
   [XmlElement("filters")]
   public MaxNodeFilterCombinator Filters { get; set; }

   /// <summary>
   /// Creates a new TreeMode from this Preset.
   /// </summary>
   /// <param name="tree">The TreeView control to associate the TreeMode with.</param>
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

      String contextMenuFile = Path.Combine(OutlinerPaths.ContextMenusDir, this.ContextMenuFile);
      if (File.Exists(contextMenuFile))
         mode.ContextMenu = XmlSerialization.Deserialize<Outliner.Controls.ContextMenu.ContextMenuModel>(contextMenuFile);

      return mode;
   }
}
}
