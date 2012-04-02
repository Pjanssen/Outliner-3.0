using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Autodesk.Max;
using System.Reflection;
using System.Xml.Serialization;

namespace Outliner.Controls.Tree
{
public class TreeViewColors
{
   public SerializableColor ForegroundLight { get; set; }
   public SerializableColor ForegroundDark { get; set; }

   public SerializableColor Background { get; set; }
   public SerializableColor AltBackground { get; set; }

   public SerializableColor SelectionForeground { get; set; }
   public SerializableColor SelectionBackground { get; set; }

   public SerializableColor DropTargetForeground { get; set; }
   public SerializableColor DropTargetBackground { get; set; }

   public SerializableColor ParentForeground { get; set; }
   public SerializableColor ParentBackground { get; set; }

   public TreeViewColors()
   {
      this.ForegroundLight      = new SerializableColor(Color.White);
      this.ForegroundDark       = new SerializableColor(Color.Black);
      this.Background           = new SerializableColor(Color.White);
      this.AltBackground        = new SerializableColor(Color.LightGray);
      this.SelectionForeground  = new SerializableColor(SystemColors.HighlightText);
      this.SelectionBackground  = new SerializableColor(SystemColors.Highlight);
      this.DropTargetForeground = new SerializableColor(SystemColors.WindowText);
      this.DropTargetBackground = new SerializableColor(255, 177, 177);
      this.ParentForeground     = new SerializableColor(SystemColors.WindowText);
      this.ParentBackground     = new SerializableColor(177, 255, 177);
   }

   public static TreeViewColors MaxColors
   {
      get
      {
         TreeViewColors c = new TreeViewColors();
         
         c.ForegroundLight      = new SerializableColor(200, 200, 200);
         c.ForegroundDark       = new SerializableColor(42, 42, 42);
         c.Background           = new SerializableColor(GuiColors.Window);
         c.AltBackground        = (c.Background.Color.GetBrightness() < 0.5f)
            ? new SerializableColor(c.Background.Color.R + 10, c.Background.Color.G + 10, c.Background.Color.B + 10)
            : new SerializableColor(c.Background.Color.R - 10, c.Background.Color.G - 10, c.Background.Color.B - 10);
         c.SelectionForeground  = new SerializableColor(GuiColors.HilightText);
         c.SelectionBackground  = new SerializableColor(GuiColors.Hilight);
         c.DropTargetForeground = c.ForegroundDark;
         c.DropTargetBackground = new SerializableColor(255, 177, 177);
         c.ParentForeground     = c.ForegroundDark;
         c.ParentBackground     = new SerializableColor(177, 255, 177);

         return c;
      }
   }

   public static TreeViewColors MayaColors
   {
      get
      {
         TreeViewColors c = new TreeViewColors();
         
         c.ForegroundLight      = new SerializableColor(220, 220, 220);
         c.ForegroundDark       = new SerializableColor(32, 32, 32);
         c.Background           = new SerializableColor(42, 42, 42);
         c.AltBackground        = new SerializableColor(52, 52, 52);
         c.SelectionForeground  = new SerializableColor(255, 255, 255);
         c.SelectionBackground  = new SerializableColor(103, 141, 178);
         c.DropTargetForeground = new SerializableColor(0, 0, 0);
         c.DropTargetBackground = new SerializableColor(103, 141, 178);
         c.ParentForeground     = new SerializableColor(220, 220, 220);
         c.ParentBackground     = new SerializableColor(65, 77, 90);

         return c;
      }
   }
}
}
