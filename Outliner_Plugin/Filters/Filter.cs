using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.Filters
{
   public abstract class Filter<T>
   {
      protected Filter() { }

      public virtual Boolean AlwaysEnabled 
      {
         get { return false; }
      }

      /// <summary>
      /// Returns whether the node should be shown or hidden. 
      /// Must return either FilterResult.Show or FilterResult.Hide, not FilterResult.ShowChildren.
      /// </summary>
      public abstract Boolean ShowNode(T data);

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
