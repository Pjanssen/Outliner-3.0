using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing;
using Outliner.MaxUtils;
using Autodesk.Max;

namespace Outliner.Controls
{
internal class ControlHelpers
{
   public static MessageBoxOptions GetLocalizedMessageBoxOptions()
   {
      if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
         return MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
      else
         return (MessageBoxOptions)0;
   }

   public static Boolean IsInDesignMode
   {
      get
      {
         System.Diagnostics.Process process = System.Diagnostics.Process.GetCurrentProcess();
         bool isInDesignMode = process.ProcessName == "devenv";
         process.Dispose();
         return isInDesignMode;
      }
   }

   public static void SetControlColors(Control control)
   {
      if (!ControlHelpers.IsInDesignMode)
      {
         Color foreColor = ColorHelpers.FromMaxGuiColor(GuiColors.WindowText);
         Color backColor = ColorHelpers.FromMaxGuiColor(GuiColors.Window);
         SetControlColor(control, foreColor, backColor);
      }
   }

   private static void SetControlColor(Control c, Color foreColor, Color backColor)
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
