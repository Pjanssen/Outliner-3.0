using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Autodesk.Max;

namespace Outliner.Controls
{
public static class ColorHelpers
{
   /// <summary>
   /// Workaround 3dsMax Color issues. (Alpha + flipped components)
   /// </summary>
   /// <param name="c">The color value from 3dsMax.</param>
   /// <returns>A correct color value.</returns>
   public static Color FromMaxColor(Color color)
   {
      return Color.FromArgb(255, color.B, color.G, color.R);
   }

   /// <summary>
   /// Converts an Autodesk.Max.IColor struct to a System.Drawing.Color struct.
   /// </summary>
   /// <param name="color"></param>
   /// <returns></returns>
   public static Color FromMaxColor(IColor color)
   {
      return FromMaxColor(Color.FromArgb((int)color.ToRGB));
   }

   /// <summary>
   /// Extends the ColorTranslator.ToHtml method with an alpha value.
   /// </summary>
   public static String ToHtml(Color c)
   {
      if (c.IsKnownColor || c.IsNamedColor || c.IsSystemColor)
         return ColorTranslator.ToHtml(c);
      else
         return "#" + c.A.ToString("X2", null)
                    + c.R.ToString("X2", null)
                    + c.G.ToString("X2", null)
                    + c.B.ToString("X2", null);
   }

   /// <summary>
   /// Extends the ColorTranslator.FromHtml method to accept alpha values.
   /// </summary>
   /// <param name="htmlColor"></param>
   /// <returns></returns>
   public static Color FromHtml(String htmlColor)
   {
      if (htmlColor.Length == 9 && htmlColor[0] == '#')
         return Color.FromArgb(Convert.ToInt32(htmlColor.Substring(1, 2), 16),
                                 Convert.ToInt32(htmlColor.Substring(3, 2), 16),
                                 Convert.ToInt32(htmlColor.Substring(5, 2), 16),
                                 Convert.ToInt32(htmlColor.Substring(7, 2), 16));
      else
         return ColorTranslator.FromHtml(htmlColor);
   }

   /// <summary>
   /// Resolves a GuiColors enum value to a Color value.
   /// </summary>
   public static Color FromMaxGuiColor(GuiColors color)
   {
      IGlobal ip = GlobalInterface.Instance;
      if (ip == null)
         throw new NullReferenceException("Could not get 3dsMax global interface");
      IIColorManager cm = GlobalInterface.Instance.ColorManager;
      return ColorHelpers.FromMaxColor(cm.GetColor(color));
   }


   public static Color OverlayColor(Color baseColor, Color overlayColor)
   {
      float overlayAmount = overlayColor.A / 255f;
      float baseAmount = 1.0f - overlayAmount;

      return Color.FromArgb(
         255,
         (byte)Math.Round(baseColor.R * baseAmount + overlayColor.R * overlayAmount),
         (byte)Math.Round(baseColor.G * baseAmount + overlayColor.G * overlayAmount),
         (byte)Math.Round(baseColor.B * baseAmount + overlayColor.B * overlayAmount));
   }

}
}
