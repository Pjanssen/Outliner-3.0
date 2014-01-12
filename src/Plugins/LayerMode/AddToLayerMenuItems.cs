using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Autodesk.Max;
using PJanssen.Outliner.Controls.ContextMenu;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Plugins;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.Modes.Layer
{
[OutlinerPlugin(OutlinerPluginType.ContextMenuItemModel)]
[LocalizedDisplayName(typeof(Resources), "Str_AddToLayerItems")]
public class AddToLayerMenuItems : MenuItemModel
{
   public override ToolStripItem[] ToToolStripMenuItems( Outliner.Controls.Tree.TreeView treeView
                                                       , Outliner.Controls.Tree.TreeNode clickedTn)
   {
      List<ToolStripMenuItem> items = new List<ToolStripMenuItem>(MaxScene.LayerCount);

      IEnumerable<IMaxNode> layers = MaxScene.Layers
                                             .OrderBy(n => !((ILayerWrapper)n).IsDefault)
                                             .ThenBy(n => n.DisplayName);

      foreach (IMaxNode layer in layers)
      {
         ToolStripMenuItem item = new ToolStripMenuItem();
         item.Text = layer.DisplayName;
         item.Image = this.Image16;
         item.Tag = layer;
         item.Click += new EventHandler((sender, eventArgs) => this.item_Click(treeView, clickedTn, layer));

         items.Add(item);
      }

      return items.ToArray();
   }

   void item_Click(Outliner.Controls.Tree.TreeView treeView, Outliner.Controls.Tree.TreeNode clickedTn, IMaxNode layer)
   {
      IEnumerable<IMaxNode> nodes = TreeMode.GetMaxNodes(treeView.SelectedNodes);
      layer.AddChildNodes(nodes);
   }


   #region Hide members inherited from ConfigurationFile which aren't relevant.

   public override System.Drawing.Image Image16
   {
      get
      {
         return Resources.layer_16;
      }
   }

   [XmlElement("image16")]
   [DefaultValue("")]
   [Browsable(false)]
   public override string Image16Res
   {
      get { return ""; }
      set { }
   }

   [XmlElement("text")]
   [DefaultValue("")]
   [Browsable(false)]
   public override string TextRes
   {
      get { return ""; }
      set { }
   }

   [XmlIgnore]
   [DisplayName("Resource Provider")]
   [Category("1. UI Properties")]
   [Browsable(false)]
   public override Type ResourceType
   {
      get { return null; }
      set { base.ResourceType = value; }
   }

   #endregion

}
}
