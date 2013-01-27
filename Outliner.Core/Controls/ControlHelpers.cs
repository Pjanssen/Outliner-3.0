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
public class ControlHelpers
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

   /// <summary>
   /// Returns true if the Control key is pressed, possibly in combination with other keys.
   /// </summary>
   public static Boolean ControlPressed
   {
      get { return Control.ModifierKeys.HasFlag(Keys.Control); }
   }

   /// <summary>
   /// Returns true if the Alt key is pressed, possibly in combination with other keys.
   /// </summary>
   public static Boolean AltPressed
   {
      get { return Control.ModifierKeys.HasFlag(Keys.Alt); }
   }

   /// <summary>
   /// Returns true if the Shift key is pressed, possibly in combination with other keys.
   /// </summary>
   public static Boolean ShiftPressed
   {
      get { return Control.ModifierKeys.HasFlag(Keys.Shift); }
   }

   /// <summary>
   /// Calculates the distance between two points.
   /// </summary>
   public static Double Distance(Point pt1, Point pt2)
   {
      double a = Math.Pow(pt1.X - pt2.X, 2);
      double b = Math.Pow(pt1.Y - pt2.Y, 2);
      return Math.Sqrt(a + b);
   }
}
}
