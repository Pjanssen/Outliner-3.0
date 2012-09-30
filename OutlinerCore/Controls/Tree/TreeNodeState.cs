using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.Controls.Tree
{
   [Flags]
   public enum TreeNodeStates : int
   {
      None             = 0x00,
      Selected         = 0x01,
      ParentOfSelected = 0x02,
      DropTarget       = 0x04
   }
}
