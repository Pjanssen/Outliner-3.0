using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Autodesk.Max;
using PJanssen.Outliner.Controls.ContextMenu;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Plugins;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.Modes.Material
{
[OutlinerPlugin(OutlinerPluginType.ContextMenuItemModel)]
[LocalizedDisplayName(typeof(Resources), "Str_EditMaterialItems")]
public class EditMtlInEditorSlotItems : MenuItemModel
{
   public override ToolStripItem[] ToToolStripMenuItems(Controls.Tree.TreeView treeView
                                                       , Controls.Tree.TreeNode clickedTn)
   {
      List<ToolStripMenuItem> items = new List<ToolStripMenuItem>(24);

      MaterialWrapper mat = TreeMode.GetMaxNode(clickedTn) as MaterialWrapper;
      if (mat == null)
         return items.ToArray();

      IIMtlEditInterface mtlEditor = MaxInterfaces.Global.MtlEditInterface;

      for (int i = 0; i < 24; i++)
      {
         IMtlBase slotMtl = mtlEditor.GetTopMtlSlot(i);

         ToolStripMenuItem item = new ToolStripMenuItem();
         item.Text = (i + 1).ToString() + ": " + slotMtl.Name;
         item.Tag = i;
         item.Click += new EventHandler((sender, eventArgs) => this.item_Click(clickedTn, item));

         if (mat.Material.Handle == slotMtl.Handle)
         {
            item.Font = new Font(item.Font, FontStyle.Bold);
            item.Checked = true;
         }

         items.Add(item);
      }


      return items.ToArray();
   }

   private void item_Click(Controls.Tree.TreeNode clickedTn, ToolStripMenuItem item)
   {
      MaterialWrapper mat = TreeMode.GetMaxNode(clickedTn) as MaterialWrapper;
      if (mat == null)
         return;

      int slot = (int)item.Tag;
      IIMtlEditInterface mtlEditor = MaxInterfaces.Global.MtlEditInterface;
      mtlEditor.PutMtlToMtlEditor(mat.Material, slot);
      mtlEditor.SetActiveMtlSlot(slot, false);
   }

   #region Hide members inherited from ConfigurationFile which aren't relevant.

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
