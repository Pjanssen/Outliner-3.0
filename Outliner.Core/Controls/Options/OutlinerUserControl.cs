using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Outliner.MaxUtils;
using Autodesk.Max;

namespace Outliner.Controls.Options
{
public class OutlinerUserControl : UserControl
{
   public OutlinerUserControl() { }

   protected override void OnLoad(EventArgs e)
   {
      this.SetControlColors();
      base.OnLoad(e);
   }

   protected Boolean IsInDesignMode
   {
      get
      {
         System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
         bool isInDesignMode = process.ProcessName == "devenv";
         process.Dispose();
         return isInDesignMode;
      }
   }

   protected void SetControlColors()
   {
      if (!this.IsInDesignMode)
      {
         Color foreColor = ColorHelpers.FromMaxGuiColor(GuiColors.WindowText);
         Color backColor = ColorHelpers.FromMaxGuiColor(GuiColors.Window);
         SetControlColor(this, foreColor, backColor);
      }
   }

   private void SetControlColor(Control c, Color foreColor, Color backColor)
   {
      if (!(c is UserControl || c is Panel
                             || c is GroupBox
                             || c is Label
                             || c is RadioButton
                             || c is CheckBox))
      {
         c.ForeColor = foreColor;
         c.BackColor = backColor;

         PropertyGrid propertyGrid = c as PropertyGrid;
         if (propertyGrid != null)
         {
            propertyGrid.ViewBackColor = backColor;
            propertyGrid.ViewForeColor = foreColor;
            propertyGrid.LineColor = Color.Gray;
         }
      }

      foreach (Control child in c.Controls)
      {
         SetControlColor(child, foreColor, backColor);
      }
   }
}
}
