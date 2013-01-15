using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Outliner.Plugins;

namespace Outliner.Controls.ContextMenu
{
   [OutlinerPlugin(OutlinerPluginType.ContextMenuItemModel)]
   [LocalizedDisplayName(typeof(ContextMenuResources), "Str_HideFreezeItems")]
   public class HideFreezeItems : MenuItemModel
   {
      public override ToolStripItem[] ToToolStripMenuItems( Outliner.Controls.Tree.TreeView treeView
                                                          , Outliner.Controls.Tree.TreeNode clickedTn)
      {
         List<ToolStripItem> items = new List<ToolStripItem>(4);

         ActionMenuItemModel hideItem = new ActionMenuItemModel();
         hideItem.ResourceType = typeof(ContextMenuResources);
         hideItem.TextRes = "Context_HideSelection";
         hideItem.Image16Res = "hide";
         hideItem.EnabledPredicate = "HideEnabled";
         hideItem.OnClickAction = "Hide";
         items.Add(hideItem.ToToolStripMenuItems(treeView, clickedTn)[0]);

         ActionMenuItemModel unhideItem = new ActionMenuItemModel();
         unhideItem.ResourceType = typeof(ContextMenuResources);
         unhideItem.TextRes = "Context_UnhideSelection";
         unhideItem.EnabledPredicate = "UnhideEnabled";
         unhideItem.OnClickAction = "Unhide";
         items.Add(unhideItem.ToToolStripMenuItems(treeView, clickedTn)[0]);

         ActionMenuItemModel freezeItem = new ActionMenuItemModel();
         freezeItem.ResourceType = typeof(ContextMenuResources);
         freezeItem.TextRes = "Context_FreezeSelection";
         freezeItem.Image16Res = "freeze";
         freezeItem.EnabledPredicate = "FreezeEnabled";
         freezeItem.OnClickAction = "Freeze";
         items.Add(freezeItem.ToToolStripMenuItems(treeView, clickedTn)[0]);

         ActionMenuItemModel unfreezeItem = new ActionMenuItemModel();
         unfreezeItem.ResourceType = typeof(ContextMenuResources);
         unfreezeItem.TextRes = "Context_UnfreezeSelection";
         unfreezeItem.EnabledPredicate = "UnfreezeEnabled";
         unfreezeItem.OnClickAction = "Unfreeze";
         items.Add(unfreezeItem.ToToolStripMenuItems(treeView, clickedTn)[0]);

         return items.ToArray();
      }


      #region Hide members inherited from ConfigurationFile which aren't relevant.

      public override System.Drawing.Image Image16
      {
         get
         {
            return null;
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
