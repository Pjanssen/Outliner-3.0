using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.Filters
{
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
