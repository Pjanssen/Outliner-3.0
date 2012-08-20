using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Outliner.Controls.ContextMenu
{
   public class ToolStripCheckedSplitButton : ToolStripSplitButton
   {
      public ToolStripCheckedSplitButton() : base() { }
      public ToolStripCheckedSplitButton(String text) : base(text) { }
      public ToolStripCheckedSplitButton(Image image) : base(image) { }
      public ToolStripCheckedSplitButton(String text, Image image) : base(text, image) { }
      public ToolStripCheckedSplitButton(String text, Image image, EventHandler onClick) : base(text, image, onClick) { }
      public ToolStripCheckedSplitButton(String text, Image image, EventHandler onClick, String name) : base(text, image, onClick, name) { }
      public ToolStripCheckedSplitButton(String text, Image image, params ToolStripItem[] dropDownItems) : base(text, image, dropDownItems) { }

      private Boolean isChecked = false;
      public Boolean Checked
      {
         get { return this.isChecked; }
         set
         {
            this.isChecked = value;
            this.Invalidate();
         }
      }
   }
}
