using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Outliner.Controls.Tree
{
   public class TreeViewSettings
   {
      public Boolean MultiSelect { get; set; }
      public TreeNodeDoubleClickAction DoubleClickAction { get; set; }
      public MouseButtons DragDropMouseButton { get; set; }

      public Boolean ScrollToSelection { get; set; }
      public Boolean AutoExpandSelectionParents { get; set; }
      public Boolean CollapseAutoExpandedParents { get; set; }

      public TreeViewSettings()
      {
         this.MultiSelect = true;
         this.DoubleClickAction = TreeNodeDoubleClickAction.Rename;
         this.DragDropMouseButton = MouseButtons.Left;

         this.ScrollToSelection = true;
         this.AutoExpandSelectionParents = true;
         this.CollapseAutoExpandedParents = true;
      }
   }
}
