using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Autodesk.Max;

namespace Outliner.Controls
{
public class TreeViewColors
{
   public Color BackColor { get; set; }
   public Color AltBackColor { get; set; }
   public Color LineColor { get; set; }

   public Color NodeForeColor { get; set; }
   public Color NodeBackColor { get; set; }

   public Color FrozenForeColor { get; set; }
   public Color FrozenBackColor { get; set; }

   public Color HiddenForeColor { get; set; }
   public Color HiddenBackColor { get; set; }

   public Color SelectionForeColor { get; set; }
   public Color SelectionBackColor { get; set; }

   public Color LinkForeColor { get; set; }
   public Color LinkBackColor { get; set; }

   public Color ParentForeColor { get; set; }
   public Color ParentBackColor { get; set; }

   public Color LayerForeColor { get; set; }
   public Color LayerBackColor { get; set; }

   public TreeViewColors()
   {
      BackColor = SystemColors.Window;
      AltBackColor = (BackColor.GetBrightness() < 0.5) ?
         Color.FromArgb(BackColor.R + 20, BackColor.G + 20, BackColor.B + 20) :
         Color.FromArgb(BackColor.R - 20, BackColor.G - 20, BackColor.B - 20);
      LineColor = SystemColors.ControlText;

      NodeForeColor = SystemColors.WindowText;
      NodeBackColor = SystemColors.Window;
      FrozenForeColor = SystemColors.GrayText;
      FrozenBackColor = SystemColors.Window;
      HiddenForeColor = SystemColors.GrayText;
      HiddenBackColor = SystemColors.Window;

      SelectionForeColor = SystemColors.HighlightText;
      SelectionBackColor = SystemColors.Highlight;

      LinkForeColor = SystemColors.WindowText;
      LinkBackColor = Color.FromArgb(255, 255, 177, 177);

      ParentForeColor = SystemColors.WindowText;
      ParentBackColor = Color.FromArgb(255, 177, 255, 177);

      LayerForeColor = SystemColors.WindowText;
      LayerBackColor = Color.FromArgb(255, 177, 228, 255);
   }

   public static TreeViewColors MaxColors
   {
      get
      {
         TreeViewColors c = new TreeViewColors();
         IIColorManager cm = GlobalInterface.Instance.ColorManager;
         c.BackColor = HelperMethods.FromMaxColor(cm.GetColor(GuiColors.Window));
         c.AltBackColor = (c.BackColor.GetBrightness() < 0.5) ?
            Color.FromArgb(c.BackColor.R + 10, c.BackColor.G + 10, c.BackColor.B + 10) :
            Color.FromArgb(c.BackColor.R - 10, c.BackColor.G - 10, c.BackColor.B - 10);
         c.LineColor = HelperMethods.FromMaxColor(cm.GetColor(GuiColors.WindowText));

         c.NodeForeColor = c.LineColor;
         c.NodeBackColor = c.BackColor;
         c.FrozenForeColor = SystemColors.GrayText;
         c.FrozenBackColor = c.NodeBackColor;
         c.HiddenForeColor = SystemColors.GrayText;
         c.HiddenBackColor = c.NodeBackColor;

         c.SelectionForeColor = HelperMethods.FromMaxColor(cm.GetColor(GuiColors.HilightText));
         c.SelectionBackColor = HelperMethods.FromMaxColor(cm.GetColor(GuiColors.Hilight));

         c.LinkForeColor = c.NodeForeColor;
         c.LinkBackColor = Color.FromArgb(255, 255, 177, 177);

         c.ParentForeColor = c.NodeForeColor;
         c.ParentBackColor = Color.FromArgb(255, 177, 255, 177);

         c.LayerForeColor = c.NodeForeColor;
         c.LayerBackColor = Color.FromArgb(255, 177, 228, 255);

         return c;
      }
   }

   public static TreeViewColors MayaColors
   {
      get
      {
         TreeViewColors c = new TreeViewColors();
         IIColorManager cm = GlobalInterface.Instance.ColorManager;
         c.BackColor = Color.FromArgb(255, 42, 42, 42);
         c.AltBackColor = (c.BackColor.GetBrightness() < 0.5) ?
            Color.FromArgb(c.BackColor.R + 10, c.BackColor.G + 10, c.BackColor.B + 10) :
            Color.FromArgb(c.BackColor.R - 10, c.BackColor.G - 10, c.BackColor.B - 10);
         c.LineColor = Color.FromArgb(255, 200, 200, 200);

         c.NodeForeColor = c.LineColor;
         c.NodeBackColor = c.BackColor;
         c.FrozenForeColor = SystemColors.GrayText;
         c.FrozenBackColor = c.NodeBackColor;
         c.HiddenForeColor = SystemColors.GrayText;
         c.HiddenBackColor = c.NodeBackColor;

         c.SelectionForeColor = Color.FromArgb(255, 255, 255, 255);
         c.SelectionBackColor = Color.FromArgb(255, 103, 141, 178);

         c.LinkForeColor = HelperMethods.FromMaxColor(cm.GetColor(GuiColors.WindowText));
         c.LinkBackColor = Color.FromArgb(255, 255, 177, 177);

         c.ParentForeColor = HelperMethods.FromMaxColor(cm.GetColor(GuiColors.WindowText));
         c.ParentBackColor = Color.FromArgb(255, 65, 77, 90);

         c.LayerForeColor = HelperMethods.FromMaxColor(cm.GetColor(GuiColors.WindowText));
         c.LayerBackColor = Color.FromArgb(255, 177, 228, 255);

         return c;
      }
   }
}
}
