using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Scene;

namespace Outliner.Controls.ContextMenu
{
   public class SeparatorMenuItemData : MenuItemData
   {
      public SeparatorMenuItemData() : base(String.Empty, String.Empty, null) { }

      public override void OnClick( Outliner.Controls.Tree.TreeNode clickedTn
                                  , IEnumerable<IMaxNodeWrapper> context)
      {
         //Separator doesn't execute anything.
      }

      public override ToolStripItem ToToolStripMenuItem( Tree.TreeNode clickedTn
                                                       , IEnumerable<IMaxNodeWrapper> context)
      {
         ToolStripSeparator separator = new ToolStripSeparator();
         separator.Visible = context.Any(n => n.IsNodeType(this.VisibleTypes));

         return separator;
      }
   }
}
