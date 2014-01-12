using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace PJanssen.Outliner.Filters
{
   /// <summary>
   /// A baseclass for a Filter of type T.
   /// </summary>
   /// <typeparam name="T">The type of object to be filtered.</typeparam>
   public abstract class Filter<T>
   {
      private Boolean enabled;
      private Boolean invert;

      protected Filter() 
      {
         this.enabled = true;
         this.invert = false;
      }
      
      /// <summary>
      /// Gets or sets whether this filter should be evaluated.
      /// </summary>
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

      /// <summary>
      /// Gets or sets whether the filter result should be inverted.
      /// </summary>
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
      /// Indicates whether a node should be shown or hidden.
      /// </summary>
      public virtual Boolean ShowNode(T data)
      {
         if (!this.Enabled)
            return true;

         Boolean showInternal = this.ShowNodeInternal(data);

         return !(showInternal == this.Invert);
      }

      protected abstract Boolean ShowNodeInternal(T data);

      public virtual event EventHandler FilterChanged;
      protected virtual void OnFilterChanged()
      {
         if (this.FilterChanged != null)
            this.FilterChanged(this, new EventArgs());
      }
   }
}
