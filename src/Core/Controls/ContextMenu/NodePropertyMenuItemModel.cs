using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Commands;
using PJanssen.Outliner.Scene;
using PJanssen.Outliner.Plugins;
using System.Windows.Forms;
using PJanssen.Outliner.Modes;

namespace PJanssen.Outliner.Controls.ContextMenu
{
[OutlinerPlugin(OutlinerPluginType.ContextMenuItemModel)]
[LocalizedDisplayName(typeof(ContextMenuResources), "Str_NodePropertyMenuItemModel")]
public class NodePropertyMenuItemModel : MenuItemModel
{
   public NodePropertyMenuItemModel() : base() 
   {
      this.Property = BooleanNodeProperty.None;
   }

   public NodePropertyMenuItemModel( String text, String image, Type resType
                              , BooleanNodeProperty property) : base(text, image, resType)
   {
      this.Property = property;
   }

   [XmlElement("property")]
   [DefaultValue(BooleanNodeProperty.None)]
   [DisplayName("Node Property")]
   [Category("2. Node Properties")]
   public BooleanNodeProperty Property { get; set; }


   protected override Boolean Enabled( ToolStripMenuItem clickedItem
                                     , Outliner.Controls.Tree.TreeView treeView
                                     , Outliner.Controls.Tree.TreeNode clickedTn)
   {
      Throw.IfNull(treeView, "treeView");

      IEnumerable<IMaxNode> context = TreeMode.GetMaxNodes(treeView.SelectedNodes);
      return !context.All(n => n.IsNodePropertyInherited(this.Property));
   }


   protected override Boolean Checked( ToolStripMenuItem clickedItem
                                     , Outliner.Controls.Tree.TreeView treeView
                                     , Outliner.Controls.Tree.TreeNode clickedTn)
   {
      Throw.IfNull(treeView, "treeView");

      IEnumerable<IMaxNode> context = TreeMode.GetMaxNodes(treeView.SelectedNodes);
      return context.Any(n => n.GetNodeProperty(this.Property));
   }


   protected override void OnClick( ToolStripMenuItem clickedItem
                                  , Outliner.Controls.Tree.TreeView treeView
                                  , Outliner.Controls.Tree.TreeNode clickedTn)
   {
      Throw.IfNull(treeView, "treeView");

      IEnumerable<IMaxNode> context = TreeMode.GetMaxNodes(treeView.SelectedNodes);
      Boolean newValue = !this.Checked(clickedItem, treeView, clickedTn);
      NodeProperty prop = NodeProperties.ToProperty(this.Property);
      SetNodePropertyCommand<Boolean> cmd = new SetNodePropertyCommand<Boolean>(context, prop, newValue);
      cmd.Execute(true);

      clickedItem.Checked = newValue;
   }
}
}
