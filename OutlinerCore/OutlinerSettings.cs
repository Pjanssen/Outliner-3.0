using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Outliner
{
public class OutlinerSettings
{
   private Dictionary<String, String> properties;

   public OutlinerSettings()
   {
      this.properties = new Dictionary<string, string>();

      this.DragDropMouseButton = MouseButtons.Middle;
      this.TextDoubleClickAction = DoubleClickAction.Expand;
   }

   public String GetProperty(String key)
   {
      String value;
      if (this.properties.TryGetValue(key, out value))
         return value;
      else
         throw new ArgumentException("Cannot find property with specified key", "key");
   }

   public Boolean GetBooleanProperty(String key)
   {
      return Boolean.Parse(GetProperty(key));
         
   }
   public void SetProperty<T>(String key, T value)
   {
      this.properties[key] = value.ToString();
   }

   [XmlElement("DragDropMouseButton")]
   [DefaultValue(MouseButtons.Middle)]
   public MouseButtons DragDropMouseButton { get; set; }

   [XmlElement("DoubleClickAction")]
   [DefaultValue(DoubleClickAction.Expand)]
   public DoubleClickAction TextDoubleClickAction { get; set; }


   
}

public enum DoubleClickAction
{
   Expand,
   Rename
}
}
