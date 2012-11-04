using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Outliner.Filters
{
   public abstract class Filter<T>
   {
      private Boolean enabled;
      private Boolean invert;

      protected Filter() 
      {
         this.enabled = true;
         this.invert = false;
      }
      
      [DefaultValue(true)]
      [XmlAttribute("enabled")]
      public virtual Boolean Enabled 
      {
         get { return this.enabled; }
         set
         {
            this.enabled = value;
            this.OnFilterChanged();
         }
      }

      [DefaultValue(false)]
      [XmlAttribute("invert")]
      public virtual Boolean Invert 
      {
         get { return this.invert; }
         set
         {
            this.invert = value;
            this.OnFilterChanged();
         }
      }

      /// <summary>
      /// Returns whether the node should be shown or hidden.
      /// </summary>
      public virtual Boolean ShowNode(T data)
      {
         if (!this.Enabled)
            return true;

         Boolean showInternal = this.ShowNodeInternal(data);

         return !(showInternal == this.Invert);
      }

      protected abstract Boolean ShowNodeInternal(T data);

      public event EventHandler FilterChanged;
      protected virtual void OnFilterChanged()
      {
         if (this.FilterChanged != null)
            this.FilterChanged(this, new EventArgs());
      }
   }


   [Flags]
   public enum FilterCategories
   {
      Hidden     = 0x00,
      Classes    = 0x01,
      Properties = 0x02,
      Custom     = 0x04,
      Other      = 0x08,
      All        = 0xFF
   }

   /// <summary>
   /// Indicates whether the filter should be visible in the UI.
   /// </summary>
   [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
   sealed public class FilterCategoryAttribute : Attribute
   {
      readonly FilterCategories category;

      public FilterCategoryAttribute(FilterCategories category)
      {
         this.category = category;
      }

      public FilterCategories Category
      {
         get { return this.category; }
      }
   }

   [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
   sealed public class FilterImageAttribute : Attribute
   {
      readonly System.Drawing.Image image;

      public FilterImageAttribute(System.Drawing.Image image)
      {
         this.image = image;
      }

      public System.Drawing.Image Image
      {
         get { return this.image; }
      }
   }
}
