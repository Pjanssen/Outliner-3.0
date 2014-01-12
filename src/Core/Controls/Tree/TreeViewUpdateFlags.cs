using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PJanssen.Outliner.Controls.Tree
{
   /// <summary>
   /// Specifies which parts of the TreeView should be updated.
   /// </summary>
   [Flags]
   public enum TreeViewUpdateFlags
   {
      /// <summary>
      /// Specifies that nothing will be updated.
      /// </summary>
      None = 0x00,

      /// <summary>
      /// Specifies that the control has to be redrawn.
      /// </summary>
      Redraw = 0x01,

      /// <summary>
      /// Specifies that the bounds of the TreeNodes should be updated.
      /// </summary>
      TreeNodeBounds = 0x02,

      /// <summary>
      /// Specifies that the Scrollbar values should be updated.
      /// </summary>
      Scrollbars = 0x04,

      /// <summary>
      /// Specifies that the brushes should be invalidated.
      /// </summary>
      Brushes = 0x08,

      /// <summary>
      /// Specifies that the TreeView should be completely refreshed.
      /// </summary>
      All = Redraw | TreeNodeBounds | Scrollbars | Brushes
   }
}
