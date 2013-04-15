using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinForms = System.Windows.Forms;
using Outliner.Scene;
using Outliner.Controls.Tree;

namespace Outliner.Modes.Layer
{
   internal class INodeDragDropHandler : MaxNodeDragDropHandler
   {
      public INodeDragDropHandler(IMaxNode data) : base(data) { }

      public override bool AllowDrag
      {
         get { return true; }
      }

      public override bool IsValidDropTarget(WinForms::IDataObject dragData)
      {
         return false;
      }
   }
}
