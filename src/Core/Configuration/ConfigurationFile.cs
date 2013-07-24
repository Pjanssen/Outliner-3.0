using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Drawing;
using System.IO;
using Outliner.Plugins;
using System.Reflection;

namespace Outliner.Configuration
{
/// <summary>
/// Defines an abstract base class for configuration files.
/// </summary>
public abstract class ConfigurationFile
{
   private Type resourceType;
   private String resourceTypeName;

   /// <summary>
   /// Initializes a new instance of the ConfigurationFile class.
   /// </summary>
   public ConfigurationFile() 
      : this(String.Empty, String.Empty, String.Empty, null) { }

   /// <summary>
   /// Initializes a new instance of the ConfigurationFile class.
   /// </summary>
   /// <param name="text">The text or string resource for this Configuration.</param>
   /// <param name="image16">The 16x16 image path or resource for this Configuration.</param>
   /// <param name="image24">The 24x24 image path or resource for this Configuration.</param>
   /// <param name="resType">The type of the resource provider for text and images. Null when not using resources.</param>
   public ConfigurationFile(String text, String image16, String image24, Type resType)
   {
      this.TextRes = text;
      this.Image16Res = image16;
      this.Image24Res = image24;
      if (resType != null)
         this.ResourceTypeName = resType.Name;
   }

   /// <summary>
   /// Gets or sets the text or string resource for this Configuration.
   /// </summary>
   [XmlElement("text")]
   [DefaultValue("")]
   [DisplayName("Text")]
   [Category("1. UI Properties")]
   [TypeConverter(typeof(StringResourceConverter))]
   public virtual String TextRes { get; set; }

   /// <summary>
   /// Gets or sets the 16x16 image path or resource for this Configuration.
   /// </summary>
   [XmlElement("image16")]
   [DefaultValue("")]
   [DisplayName("Image 16x16")]
   [Category("1. UI Properties")]
   [TypeConverter(typeof(ImageResourceConverter))]
   public virtual String Image16Res { get; set; }

   /// <summary>
   /// Gets or sets the 24x24 image path or resource for this Configuration.
   /// </summary>
   [XmlElement("image24")]
   [DefaultValue("")]
   [DisplayName("Image 24x24")]
   [Category("1. UI Properties")]
   [TypeConverter(typeof(ImageResourceConverter))]
   public virtual String Image24Res { get; set; }

   /// <summary>
   /// Gets or sets the full typename of the resource-provider for this Configuration.
   /// </summary>
   [XmlElement("resource_type")]
   [DefaultValue("")]
   [Browsable(false)]
   public virtual String ResourceTypeName 
   {
      get { return resourceTypeName; } 
      set
      {
         this.resourceTypeName = value;
         this.resourceType = null;
      }
   }

   /// <summary>
   /// Gets or sets the type of resource-provider for this Configuration.
   /// </summary>
   [XmlIgnore]
   [DisplayName("Resource Provider")]
   [Category("1. UI Properties")]
   [Browsable(true)]
   [TypeConverter(typeof(ResourceTypeConverter))]
   public virtual Type ResourceType
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
         this.resourceTypeName = value.FullName;
      }
   }

   /// <summary>
   /// Gets the text for this Configuration.
   /// </summary>
   [XmlIgnore]
   [Browsable(false)]
   public virtual String Text
   {
      get
      {
         if (this.ResourceType != null && this.TextRes != null)
            return ResourceHelpers.LookupString(this.ResourceType, this.TextRes);
         else
            return this.TextRes;
      }
   }

   /// <summary>
   /// Gets the 16x16 image for this Configuration.
   /// </summary>
   [XmlIgnore]
   [Browsable(false)]
   public virtual Image Image16
   {
      get 
      { 
         Image img = LookupImage(this.Image16Res);
         if (img != null)
            return img;
         else
            return Image16Default;
      }
   }

   /// <summary>
   /// Gets the fallback 16x16 image, when the Image16 is null.
   /// </summary>
   protected virtual Image Image16Default
   {
      get { return null; }
   }

   /// <summary>
   /// Gets the 24x24 image for this Configuration.
   /// </summary>
   [XmlIgnore]
   [Browsable(false)]
   public virtual Image Image24
   {
      get
      {
         Image img = LookupImage(this.Image24Res);
         if (img != null)
            return img;
         else
            return Image24Default;
      }
   }

   /// <summary>
   /// Gets the fallback 24x24 image, when the Image16 is null.
   /// </summary>
   protected virtual Image Image24Default
   {
      get { return null; }
   }

   /// <summary>
   /// Gets the base path to load images from, when not using resources.
   /// </summary>
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
