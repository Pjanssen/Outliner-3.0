using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace Outliner.Controls.ContextMenu
{
   public class StringResourceConverter : MenuItemResourceConverter
   {
      override protected Boolean IsValidResource(PropertyInfo property)
      {
         return property.PropertyType.Equals(typeof(String));
      }
   }
}
