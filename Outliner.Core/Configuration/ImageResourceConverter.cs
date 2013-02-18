using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Drawing;

namespace Outliner.Configuration
{
   //TODO: remove size restriction, or find a flexible way to keep it.
   internal class ImageResourceConverter : ConfigurationResourceConverter
   {
      private static readonly Type imageType = typeof(Image);
      private static readonly Size imageSize = new Size(16, 16);

      protected override bool IsValidResource(PropertyInfo property)
      {
         Type propType = property.PropertyType;
         if (propType.Equals(imageType) || propType.IsSubclassOf(imageType))
         {
            Image img = property.GetValue(null, null) as Image;
            if (img != null && img.Size == imageSize)
               return true;
         }

         return false;
      }
   }
}
