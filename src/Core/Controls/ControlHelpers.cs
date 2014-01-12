using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing;
using PJanssen.Outliner.MaxUtils;
using Autodesk.Max;

namespace PJanssen.Outliner.Controls
{
/// <summary>
/// Provides methods for common Control operations.
/// </summary>
public class ControlHelpers
{
   /// <summary>
   /// Creates a MessageBoxOptions object for the current UI culture.
   /// </summary>
   /// <returns></returns>
   public static MessageBoxOptions CreateLocalizedMessageBoxOptions()
   {
      if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
         return MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
      else
         return (MessageBoxOptions)0;
   }

   /// <summary>
   /// Indicates if the control is drawn in (Visual Studio) design mode.
   /// </summary>
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

   /// <summary>
   /// Sets the fore- and backcolor of the given control to GuiColors.WindowText and GuiColors.Window.
   /// </summary>
   /// <param name="control">The control to set the colors on.</param>
   public static void Set3dsMaxControlColors(Control control)
   {
      if (!ControlHelpers.IsInDesignMode)
      {
         Color foreColor = Colors.FromMaxGuiColor(GuiColors.WindowText);
         Color backColor = Colors.FromMaxGuiColor(GuiColors.Window);
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
