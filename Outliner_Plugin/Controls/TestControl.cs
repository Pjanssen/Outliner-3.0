using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
         Autodesk.Max.IIColorManager cM = Autodesk.Max.GlobalInterface.Instance.ColorManager;
         this.BackColor = Color.FromArgb(255, cM.GetColor(Autodesk.Max.GuiColors.Background));
         this.treeView1.Colors = TreeViewColors.GetMaxColors();
      }
   }
}
