using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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
   }
}
