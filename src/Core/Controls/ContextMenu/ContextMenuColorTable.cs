using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using PJanssen.Outliner.Controls;

namespace PJanssen.Outliner.Controls.ContextMenu
{
   [System.CodeDom.Compiler.GeneratedCode("Outliner T4 template", "3.0")]
   public class ContextMenuColorTable : ProfessionalColorTable
   {
      [XmlElement("Text")]
      public SerializableColor S_Text { get; set; }
      public Color Text
      {
         get { return S_Text; } 
      }
      
      [XmlElement("Arrow")]
      public SerializableColor S_Arrow { get; set; }
      public Color Arrow
      {
         get { return S_Arrow; } 
      }

      
      [XmlElement("MenuBorder")]
      public SerializableColor S_MenuBorder { get; set; }
      public override Color MenuBorder
      {
         get { return S_MenuBorder; } 
      }

      
      [XmlElement("ToolStripBorder")]
      public SerializableColor S_ToolStripBorder { get; set; }
      public override Color ToolStripBorder
      {
         get { return S_ToolStripBorder; } 
      }

      
      [XmlElement("ToolStripDropDownBackground")]
      public SerializableColor S_ToolStripDropDownBackground { get; set; }
      public override Color ToolStripDropDownBackground
      {
         get { return S_ToolStripDropDownBackground; } 
      }

      
      [XmlElement("SeparatorLight")]
      public SerializableColor S_SeparatorLight { get; set; }
      public override Color SeparatorLight
      {
         get { return S_SeparatorLight; } 
      }

      
      [XmlElement("SeparatorDark")]
      public SerializableColor S_SeparatorDark { get; set; }
      public override Color SeparatorDark
      {
         get { return S_SeparatorDark; } 
      }

      
      [XmlElement("ButtonCheckedHighlight")]
      public SerializableColor S_ButtonCheckedHighlight { get; set; }
      public override Color ButtonCheckedHighlight
      {
         get { return S_ButtonCheckedHighlight; } 
      }

      
      [XmlElement("ButtonCheckedHighlightBorder")]
      public SerializableColor S_ButtonCheckedHighlightBorder { get; set; }
      public override Color ButtonCheckedHighlightBorder
      {
         get { return S_ButtonCheckedHighlightBorder; } 
      }

      
      [XmlElement("ButtonCheckedGradientBegin")]
      public SerializableColor S_ButtonCheckedGradientBegin { get; set; }
      public override Color ButtonCheckedGradientBegin
      {
         get { return S_ButtonCheckedGradientBegin; } 
      }

      
      [XmlElement("ButtonCheckedGradientMiddle")]
      public SerializableColor S_ButtonCheckedGradientMiddle { get; set; }
      public override Color ButtonCheckedGradientMiddle
      {
         get { return S_ButtonCheckedGradientMiddle; } 
      }

      
      [XmlElement("ButtonCheckedGradientEnd")]
      public SerializableColor S_ButtonCheckedGradientEnd { get; set; }
      public override Color ButtonCheckedGradientEnd
      {
         get { return S_ButtonCheckedGradientEnd; } 
      }

      
      [XmlElement("ButtonSelectedBorder")]
      public SerializableColor S_ButtonSelectedBorder { get; set; }
      public override Color ButtonSelectedBorder
      {
         get { return S_ButtonSelectedBorder; } 
      }

      
      [XmlElement("ButtonSelectedHighlightBorder")]
      public SerializableColor S_ButtonSelectedHighlightBorder { get; set; }
      public override Color ButtonSelectedHighlightBorder
      {
         get { return S_ButtonSelectedHighlightBorder; } 
      }

      
      [XmlElement("ButtonSelectedGradientBegin")]
      public SerializableColor S_ButtonSelectedGradientBegin { get; set; }
      public override Color ButtonSelectedGradientBegin
      {
         get { return S_ButtonSelectedGradientBegin; } 
      }

      
      [XmlElement("ButtonSelectedGradientMiddle")]
      public SerializableColor S_ButtonSelectedGradientMiddle { get; set; }
      public override Color ButtonSelectedGradientMiddle
      {
         get { return S_ButtonSelectedGradientMiddle; } 
      }

      
      [XmlElement("ButtonSelectedGradientEnd")]
      public SerializableColor S_ButtonSelectedGradientEnd { get; set; }
      public override Color ButtonSelectedGradientEnd
      {
         get { return S_ButtonSelectedGradientEnd; } 
      }

      
      [XmlElement("ButtonPressedBorder")]
      public SerializableColor S_ButtonPressedBorder { get; set; }
      public override Color ButtonPressedBorder
      {
         get { return S_ButtonPressedBorder; } 
      }

      
      [XmlElement("ButtonPressedHighlightBorder")]
      public SerializableColor S_ButtonPressedHighlightBorder { get; set; }
      public override Color ButtonPressedHighlightBorder
      {
         get { return S_ButtonPressedHighlightBorder; } 
      }

      
      [XmlElement("ButtonPressedGradientBegin")]
      public SerializableColor S_ButtonPressedGradientBegin { get; set; }
      public override Color ButtonPressedGradientBegin
      {
         get { return S_ButtonPressedGradientBegin; } 
      }

      
      [XmlElement("ButtonPressedGradientMiddle")]
      public SerializableColor S_ButtonPressedGradientMiddle { get; set; }
      public override Color ButtonPressedGradientMiddle
      {
         get { return S_ButtonPressedGradientMiddle; } 
      }

      
      [XmlElement("ButtonPressedGradientEnd")]
      public SerializableColor S_ButtonPressedGradientEnd { get; set; }
      public override Color ButtonPressedGradientEnd
      {
         get { return S_ButtonPressedGradientEnd; } 
      }

      
      [XmlElement("MenuItemPressedGradientBegin")]
      public SerializableColor S_MenuItemPressedGradientBegin { get; set; }
      public override Color MenuItemPressedGradientBegin
      {
         get { return S_MenuItemPressedGradientBegin; } 
      }

      
      [XmlElement("MenuItemPressedGradientMiddle")]
      public SerializableColor S_MenuItemPressedGradientMiddle { get; set; }
      public override Color MenuItemPressedGradientMiddle
      {
         get { return S_MenuItemPressedGradientMiddle; } 
      }

      
      [XmlElement("MenuItemPressedGradientEnd")]
      public SerializableColor S_MenuItemPressedGradientEnd { get; set; }
      public override Color MenuItemPressedGradientEnd
      {
         get { return S_MenuItemPressedGradientEnd; } 
      }

      
      [XmlElement("MenuItemSelected")]
      public SerializableColor S_MenuItemSelected { get; set; }
      public override Color MenuItemSelected
      {
         get { return S_MenuItemSelected; } 
      }

      
      [XmlElement("ImageMarginGradientBegin")]
      public SerializableColor S_ImageMarginGradientBegin { get; set; }
      public override Color ImageMarginGradientBegin
      {
         get { return S_ImageMarginGradientBegin; } 
      }

      
      [XmlElement("ImageMarginGradientMiddle")]
      public SerializableColor S_ImageMarginGradientMiddle { get; set; }
      public override Color ImageMarginGradientMiddle
      {
         get { return S_ImageMarginGradientMiddle; } 
      }

      
      [XmlElement("ImageMarginGradientEnd")]
      public SerializableColor S_ImageMarginGradientEnd { get; set; }
      public override Color ImageMarginGradientEnd
      {
         get { return S_ImageMarginGradientEnd; } 
      }

      
   }
}