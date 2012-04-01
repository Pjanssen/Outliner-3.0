using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Autodesk.Max;
using System.Reflection;

namespace Outliner.Controls.Tree
{
public class TreeViewColors : System.Xml.Serialization.IXmlSerializable
{
   public Color ForeColorLight { get; set; }
   public Color ForeColorDark { get; set; }

   public Color BackColor { get; set; }
   public Color AltBackColor { get; set; }

   public Color SelectionForeColor { get; set; }
   public Color SelectionBackColor { get; set; }

   public Color LinkForeColor { get; set; }
   public Color LinkBackColor { get; set; }

   public Color ParentForeColor { get; set; }
   public Color ParentBackColor { get; set; }

   public TreeViewColors()
   {
      ForeColorLight = Color.White;
      ForeColorDark = Color.Black;

      BackColor = SystemColors.Window;
      AltBackColor = (BackColor.GetBrightness() < 0.5) ?
         Color.FromArgb(BackColor.R + 20, BackColor.G + 20, BackColor.B + 20) :
         Color.FromArgb(BackColor.R - 20, BackColor.G - 20, BackColor.B - 20);

      SelectionForeColor = SystemColors.HighlightText;
      SelectionBackColor = SystemColors.Highlight;

      LinkForeColor = SystemColors.WindowText;
      LinkBackColor = Color.FromArgb(255, 255, 177, 177);

      ParentForeColor = SystemColors.WindowText;
      ParentBackColor = Color.FromArgb(255, 177, 255, 177);
   }

   public static TreeViewColors MaxColors
   {
      get
      {
         TreeViewColors c = new TreeViewColors();
         IIColorManager cm = GlobalInterface.Instance.ColorManager;
         c.ForeColorLight = Color.FromArgb(255, 200, 200, 200);
         c.ForeColorDark = Color.FromArgb(255, 42, 42, 42);
         c.BackColor = ColorHelpers.FromMaxColor(cm.GetColor(GuiColors.Window));
         c.AltBackColor = (c.BackColor.GetBrightness() < 0.5) ?
            Color.FromArgb(c.BackColor.R + 10, c.BackColor.G + 10, c.BackColor.B + 10) :
            Color.FromArgb(c.BackColor.R - 10, c.BackColor.G - 10, c.BackColor.B - 10);

         c.SelectionForeColor = ColorHelpers.FromMaxColor(cm.GetColor(GuiColors.HilightText));
         c.SelectionBackColor = ColorHelpers.FromMaxColor(cm.GetColor(GuiColors.Hilight));

         c.LinkForeColor = c.ForeColorDark;
         c.LinkBackColor = Color.FromArgb(255, 255, 177, 177);

         c.ParentForeColor = c.ForeColorDark;
         c.ParentBackColor = Color.FromArgb(255, 177, 255, 177);

         return c;
      }
   }

   public static TreeViewColors MayaColors
   {
      get
      {
         TreeViewColors c = new TreeViewColors();
         IIColorManager cm = GlobalInterface.Instance.ColorManager;
         c.ForeColorLight = Color.FromArgb(255, 200, 200, 200);
         c.ForeColorDark = Color.FromArgb(255, 42, 42, 42);
         c.BackColor = Color.FromArgb(255, 42, 42, 42);
         c.AltBackColor = (c.BackColor.GetBrightness() < 0.5) ?
            Color.FromArgb(c.BackColor.R + 10, c.BackColor.G + 10, c.BackColor.B + 10) :
            Color.FromArgb(c.BackColor.R - 10, c.BackColor.G - 10, c.BackColor.B - 10);

         c.SelectionForeColor = Color.FromArgb(255, 255, 255, 255);
         c.SelectionBackColor = Color.FromArgb(255, 103, 141, 178);

         c.LinkForeColor = ColorHelpers.FromMaxColor(cm.GetColor(GuiColors.WindowText));
         c.LinkBackColor = Color.FromArgb(255, 255, 177, 177);

         c.ParentForeColor = ColorHelpers.FromMaxColor(cm.GetColor(GuiColors.WindowText));
         c.ParentBackColor = Color.FromArgb(255, 65, 77, 90);

         return c;
      }
   }

   public System.Xml.Schema.XmlSchema GetSchema()
   {
      return null;
   }

   public void ReadXml(System.Xml.XmlReader reader)
   {
      Type t = this.GetType();

      PropertyInfo[] properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

      while (reader.IsStartElement())
      {
         reader.ReadStartElement();
         PropertyInfo prop = properties.FirstOrDefault(p => p.Name == reader.Name);
         if (prop != null)
         {
            String c = reader.GetAttribute("value");
            if (c == null)
               throw new System.Xml.XmlException("Expected value attribute in element " + prop.Name);

            prop.SetValue(this, ColorHelpers.ColorFromHtml(c), null);
         }
      }
   }

   public void WriteXml(System.Xml.XmlWriter writer)
   {
      Type t = this.GetType();

      foreach (PropertyInfo prop in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
      {
         if (prop.PropertyType == typeof(Color))
         {
            writer.WriteStartElement(prop.Name);
            writer.WriteAttributeString("value", ColorHelpers.ColorToHtml((Color)prop.GetValue(this, null)));
            writer.WriteEndElement();
         }
      }
   }
}
}
