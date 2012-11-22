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

      public event EventHandler FilterChanged;
      protected virtual void OnFilterChanged()
      {
         if (this.FilterChanged != null)
            this.FilterChanged(this, new EventArgs());
      }
   }
}
