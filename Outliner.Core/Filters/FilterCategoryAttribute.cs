using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.Filters
{
   [Flags]
   public enum FilterCategory
   {
      Hidden = 0x00,
      Classes = 0x01,
      Properties = 0x02,
      Custom = 0x04,
      Other = 0x08,
      All = 0xFF
   }

   /// <summary>
   /// Indicates whether the filter should be visible in the UI.
   /// </summary>
   [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
   sealed public class FilterCategoryAttribute : Attribute
   {
      readonly FilterCategory category;

      public FilterCategoryAttribute(FilterCategory category)
      {
         this.category = category;
      }

      public FilterCategory Category
      {
         get { return this.category; }
      }
   }
}
