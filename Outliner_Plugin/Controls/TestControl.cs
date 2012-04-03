using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Controls.Tree;

namespace Outliner.Controls
{
   public partial class TestControl : MaxCustomControls.MaxUserControl, MaxCustomControls.CuiUpdatable
   {
      public TestControl()
      {
         InitializeComponent();
         MaxCustomControls.MaxCuiBinder.GetInstance().RegisterForNotification(this);
      }


      public void UpdateColors()
      {
         this.BackColor = ColorHelpers.FromMaxGuiColor(Autodesk.Max.GuiColors.Background);
         this.treeView1.Colors.UpdateColors();
         this.treeView1.Update(TreeViewUpdateFlags.Brushes | TreeViewUpdateFlags.Redraw);
      }
   }
}
