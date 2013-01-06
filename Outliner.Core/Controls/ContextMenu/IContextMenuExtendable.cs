using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.Controls.ContextMenu
{
   public interface IContextMenuExtendable
   {
      MenuItemModel ContextMenuItem { get; set; }
   }
}
