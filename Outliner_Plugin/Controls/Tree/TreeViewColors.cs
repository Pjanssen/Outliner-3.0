using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Autodesk.Max;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;

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

   public void UpdateColors()
   {
      if (this.ForegroundLight.IsGuiColor)
         this.ForegroundLight = new SerializableColor(this.ForegroundLight.GuiColor);
      if (this.ForegroundDark.IsGuiColor)
         this.ForegroundDark = new SerializableColor(this.ForegroundDark.GuiColor);
      if (this.Background.IsGuiColor)
         this.Background = new SerializableColor(this.Background.GuiColor);
      if (this.AltBackground.IsGuiColor)
         this.AltBackground = new SerializableColor(this.AltBackground.GuiColor);
      if (this.SelectionForeground.IsGuiColor)
         this.SelectionForeground = new SerializableColor(this.SelectionForeground.GuiColor);
      if (this.SelectionBackground.IsGuiColor)
         this.SelectionBackground = new SerializableColor(this.SelectionBackground.GuiColor);
      if (this.DropTargetForeground.IsGuiColor)
         this.DropTargetForeground = new SerializableColor(this.DropTargetForeground.GuiColor);
      if (this.DropTargetBackground.IsGuiColor)
         this.DropTargetBackground = new SerializableColor(this.DropTargetBackground.GuiColor);
      if (this.ParentForeground.IsGuiColor)
         this.ParentForeground = new SerializableColor(this.ParentForeground.GuiColor);
      if (this.ParentBackground.IsGuiColor)
         this.ParentBackground = new SerializableColor(this.ParentBackground.GuiColor);
   }

   public static TreeViewColors FromXml(String path)
   {
      using (FileStream stream = new FileStream(path, FileMode.Open))
      {
         return TreeViewColors.FromXml(stream);
      }
   }

   public static TreeViewColors FromXml(Stream stream)
   {
      XmlSerializer xs = new XmlSerializer(typeof(TreeViewColors));
      return xs.Deserialize(stream) as TreeViewColors;
   }

   public void ToXml(String path)
   {
      using (FileStream stream = new FileStream(path, FileMode.Create))
      {
         this.ToXml(stream);
      }
   }

   public void ToXml(Stream stream)
   {
      XmlSerializer xs = new XmlSerializer(typeof(TreeViewColors));
      xs.Serialize(stream, this);
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
