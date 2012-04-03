using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Autodesk.Max;
using System.Drawing;

namespace Outliner.Controls
{
   /// <summary>
   /// A Color structure which can be serialized to Xml.
   /// Also includes 
   /// </summary>
public struct SerializableColor : IXmlSerializable
{
   private const String ValueAttributeName = "value";
   public static SerializableColor Empty { get { return new SerializableColor(Color.Empty); } }

   private Boolean isGuiColor;
   private GuiColors guiColor;
   private Color color;

   public Boolean IsGuiColor { get { return this.isGuiColor; } }
   public GuiColors GuiColor { get { return this.guiColor; } }
   public Color Color { get { return this.color; } }

   public SerializableColor(Color color)
   {
      this.isGuiColor = false;
      this.guiColor = GuiColors.Text;
      this.color = color;
   }

   public SerializableColor(int red, int green, int blue)
      : this(Color.FromArgb(red, green, blue)) { }

   public SerializableColor(int alpha, int red, int green, int blue)
      : this(Color.FromArgb(alpha, red, green, blue)) { }

   public SerializableColor(GuiColors color)
   {
      this.isGuiColor = true;
      this.guiColor = color;
      this.color = ColorHelpers.FromMaxGuiColor(color);
   }

   public System.Xml.Schema.XmlSchema GetSchema()
   {
      return null;
   }

   public void ReadXml(System.Xml.XmlReader reader)
   {
      String c = reader.GetAttribute(ValueAttributeName);
      if (c == null)
         throw new System.Xml.XmlException("Expected value attribute in SerializableColor element.");

      if (c.StartsWith("GuiColors."))
      {
         this.isGuiColor = true;
         this.guiColor = (GuiColors)Enum.Parse(typeof(GuiColors), c.Substring(10));
         this.color = ColorHelpers.FromMaxGuiColor(this.guiColor);
      }
      else
      {
         this.isGuiColor = false;
         this.color = ColorHelpers.FromHtml(c);
      }
   }

   public void WriteXml(System.Xml.XmlWriter writer)
   {
      if (this.isGuiColor)
         writer.WriteAttributeString(ValueAttributeName, "GuiColors." + this.guiColor.ToString());
      else
         writer.WriteAttributeString(ValueAttributeName, ColorHelpers.ToHtml(this.Color));
   }

   public override bool Equals(object obj)
   {
      if (obj is SerializableColor)
      {
         SerializableColor color = (SerializableColor)obj;
         if (this.isGuiColor)
            return color.isGuiColor && this.guiColor == color.guiColor;
         else
            return !color.isGuiColor && this.color == color.color;
      }
      return false;
   }

   public override int GetHashCode()
   {
      return this.isGuiColor.GetHashCode() ^ this.guiColor.GetHashCode() ^ this.color.GetHashCode();
   }

   public override string ToString()
   {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(base.GetType().Name);
      stringBuilder.Append(" [");
      if (this.isGuiColor)
         stringBuilder.Append(this.guiColor.ToString());
      else
         stringBuilder.Append(this.color.ToString());

      return stringBuilder.ToString();
   }
}
}
