using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Outliner.Plugins;

namespace Outliner.Controls
{
public abstract class UIItemModel
{
   public UIItemModel() : this(String.Empty, String.Empty, String.Empty, null) { }
   public UIItemModel(String text, String image16, String image24, Type resType)
   {
      this.TextRes = text;
      this.Image16Res = image16;
      this.Image24Res = image24;
      if (resType != null)
         this.ResourceTypeName = resType.Name;
   }

   [XmlElement("text")]
   [DisplayName("Text")]
   [TypeConverter(typeof(StringResourceConverter))]
   public String TextRes { get; set; }

   [XmlElement("image16")]
   [DefaultValue("")]
   [DisplayName("Image 16x16")]
   [TypeConverter(typeof(ImageResourceConverter))]
   public String Image16Res { get; set; }

   [XmlElement("image24")]
   [DefaultValue("")]
   [DisplayName("Image 24x24")]
   [TypeConverter(typeof(ImageResourceConverter))]
   public String Image24Res { get; set; }

   [XmlElement("resource_type")]
   [DefaultValue("")]
   [Browsable(false)]
   public String ResourceTypeName { get; set; }

   private Type resourceType;
   [XmlIgnore]
   [Browsable(true)]
   [TypeConverter(typeof(ResourceTypeConverter))]
   public Type ResourceType
   {
      get
      {
         if (this.resourceType == null && !String.IsNullOrEmpty(this.ResourceTypeName))
         {
            foreach (Assembly pluginAssembly in OutlinerPlugins.PluginAssemblies)
            {
               this.resourceType = pluginAssembly.GetType(this.ResourceTypeName);
               if (this.resourceType != null)
                  break;
            }
         }
         return this.resourceType;
      }
      set
      {
         this.resourceType = value;
         this.ResourceTypeName = value.Name;
      }
   }

   [XmlIgnore]
   [Browsable(false)]
   public String Text
   {
      get
      {
         if (this.ResourceType != null && this.TextRes != null)
            return ResourceHelpers.LookupString(this.ResourceType, this.TextRes);
         else
            return this.TextRes;
      }
   }

   [XmlIgnore]
   [Browsable(false)]
   public Image Image16
   {
      get { return LookupImage(this.Image16Res); }
   }

   [XmlIgnore]
   [Browsable(false)]
   public Image Image24
   {
      get { return LookupImage(this.Image24Res); }
   }


   protected abstract String ImageBasePath { get; }

   private Image LookupImage(String image)
   {
      if (String.IsNullOrEmpty(image))
         return null;

      if (this.ResourceType != null)
         return ResourceHelpers.LookupImage(this.ResourceType, image);
      else
         return this.ImageFromFile(image);
   }

   private Image ImageFromFile(String image)
   {
      Image img = null;
      if (!String.IsNullOrEmpty(image))
      {
         if (!Path.IsPathRooted(image))
            image = Path.Combine(this.ImageBasePath, image);

         if (!Path.HasExtension(image))
            image = Path.ChangeExtension(image, "png");

         if (File.Exists(image))
            img = Image.FromFile(image);
      }

      return img;
   }
}
}
