using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Drawing;
using Outliner.MaxUtils;

namespace Outliner
{
public static class ResourceHelpers
{
   public static Object LookupObject(Type resourceManagerProvider, String resourceKey)
   {
      Throw.IfArgumentIsNull(resourceManagerProvider, "resourceManagerProvider");
      Throw.IfArgumentIsNull(resourceKey, "resourceKey");

      PropertyInfo property = resourceManagerProvider.GetProperty(resourceKey, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
      if (property != null)
         return property.GetValue(null, null);
      else
         return null;
   }

   public static String LookupString(Type resourceManagerProvider, String resourceKey)
   {
      Object resource = LookupObject(resourceManagerProvider, resourceKey);
      String str = resource as String;
      if (str != null)
         return str;
      else
         return resourceKey;
   }

   public static Image LookupImage(Type resourceManagerProvider, String resourceKey)
   {
      Object resource = LookupObject(resourceManagerProvider, resourceKey);
      return resource as Image;
   }
}
}
