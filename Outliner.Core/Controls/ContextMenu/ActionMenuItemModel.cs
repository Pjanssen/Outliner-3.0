using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using System.Xml.Serialization;
using System.ComponentModel;
using Outliner.Plugins;
using System.Windows.Forms;

namespace Outliner.Controls.ContextMenu
{
public class ActionMenuItemModel : MenuItemModel
{
   public ActionMenuItemModel() : this(String.Empty, String.Empty, null) { }

   public ActionMenuItemModel(String text, String image, Type resType) 
      : base(text, image, resType)
   {
      this.EnabledPredicate = "";
      this.OnClickAction = "";
      this.CheckedPredicate = String.Empty;
   }

   [XmlAttribute("enabled")]
   [DefaultValue("")]
   public String EnabledPredicate { get; set; }

   [XmlAttribute("checked")]
   [DefaultValue("")]
   public String CheckedPredicate { get; set; }

   [XmlAttribute("onclick")]
   public String OnClickAction { get; set; }

   public override void OnClick( Outliner.Controls.Tree.TreeNode clickedTn
                               , IEnumerable<IMaxNodeWrapper> context)
   {
      OutlinerAction action = OutlinerActions.GetAction(this.OnClickAction);

      if (action != null)
         action(clickedTn, context);
      else
         MessageBox.Show( String.Format(ContextMenuResources.Str_ActionNotFound, this.OnClickAction)
                        , ContextMenuResources.Str_ContextMenuWarningTitle
                        , MessageBoxButtons.OK
                        , MessageBoxIcon.Warning);
   }

   protected override Boolean Enabled( Outliner.Controls.Tree.TreeNode clickedTn
                                     , IEnumerable<IMaxNodeWrapper> context)
   {
      OutlinerPredicate predicate = OutlinerActions.GetPredicate(this.EnabledPredicate);

      if (predicate != null)
         return predicate(clickedTn, context);
      else
         return base.Enabled(clickedTn, context);
   }

   protected override bool Checked( Outliner.Controls.Tree.TreeNode clickedTn
                                  , IEnumerable<IMaxNodeWrapper> context)
   {
      OutlinerPredicate predicate = OutlinerActions.GetPredicate(this.CheckedPredicate);

      if (predicate != null)
         return predicate(clickedTn, context);
      else
         return base.Checked(clickedTn, context);
   }
}
}
