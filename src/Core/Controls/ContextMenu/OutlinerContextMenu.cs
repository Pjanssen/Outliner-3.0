using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Outliner.MaxUtils;
using PJanssen;

namespace Outliner.Controls.ContextMenu
{
class OutlinerContextMenu : ToolStripDropDown
{
   ContextMenuStrip menu;

   public OutlinerContextMenu(ContextMenuStrip menu)
   {
      Throw.IfNull(menu, "menu");

      this.menu = menu;
   }

   protected override void OnOpened(EventArgs e)
   {
      Rectangle newBounds = this.Bounds;
      Point location = newBounds.Location;

      this.menu.Renderer = this.Renderer;
      menu.Closing += new ToolStripDropDownClosingEventHandler(menu_Closing);
      menu.Show(location);

      newBounds.Y = menu.Bounds.Top - newBounds.Height - 10;
      this.Bounds = newBounds;

      base.OnOpened(e);
   }

   void menu_Closing(object sender, System.Windows.Forms.ToolStripDropDownClosingEventArgs e)
   {
      if (e.CloseReason == ToolStripDropDownCloseReason.AppClicked && buttonBoundsContains(MousePosition))
         this.Focus();
      else
         this.Close();
   }

   Boolean buttonBoundsContains(Point p)
   {
      foreach (ToolStripItem item in this.Items)
      {
         if (!(item is ToolStripSeparator))
         {
            Rectangle localBounds  = item.Bounds;
            Point screenLocation   = this.PointToScreen(localBounds.Location);
            Rectangle screenBounds = new Rectangle(screenLocation, localBounds.Size);
            if (screenBounds.Contains(p))
               return true;
         }
      }

      return false;
   }
}
}
