using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.Controls
{
   [Flags]
   public enum TreeNodeState : int
   {
      Unselected       = 0x00,
      Selected         = 0x01,
      ParentOfSelected = 0x02,
      DropTarget       = 0x04
   }
}
