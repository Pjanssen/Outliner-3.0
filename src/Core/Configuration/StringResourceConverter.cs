using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace Outliner.Configuration
{
   internal class StringResourceConverter : ConfigurationResourceConverter
   {
      override protected Boolean IsValidResource(PropertyInfo property)
      {
         return property.PropertyType.Equals(typeof(String));
      }
   }
}
