using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using Outliner.Scene;

namespace Outliner.Controls.ContextMenu
{
   public class SeparatorMenuItemModel : MenuItemModel
   {
      public SeparatorMenuItemModel() : base(String.Empty, String.Empty, null) { }

      protected override void OnClick( Outliner.Controls.Tree.TreeView treeView
                                     , Outliner.Controls.Tree.TreeNode clickedTn)
      {
         //Separator doesn't execute anything.
      }

      public override ToolStripItem ToToolStripMenuItem( Outliner.Controls.Tree.TreeView treeView
                                                       , Outliner.Controls.Tree.TreeNode clickedTn)
      {
         ToolStripSeparator separator = new ToolStripSeparator();
         separator.Visible = base.Visible(treeView, clickedTn);

         return separator;
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

      [XmlElement("resource_type")]
      [DefaultValue("")]
      [Browsable(false)]
      public override string ResourceTypeName
      {
         get { return ""; }
         set { }
      }

      #endregion
   }
}
