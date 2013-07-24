using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Drawing;
using Outliner.MaxUtils;

namespace Outliner
{
/// <summary>
/// Provides methods for common Embedded Resource operations.
/// </summary>
public static class ResourceHelpers
{
   /// <summary>
   /// Looks up an object with the given key in the given resource provider.
   /// </summary>
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

   /// <summary>
   /// Looks up a string with the given key in the given resource provider.
   /// </summary>
   public static String LookupString(Type resourceManagerProvider, String resourceKey)
   {
      Object resource = LookupObject(resourceManagerProvider, resourceKey);
      String str = resource as String;
      if (str != null)
         return str;
      else
         return resourceKey;
   }

   /// <summary>
   /// Looks up an image with the given key in the given resource provider.
   /// </summary>
   public static Image LookupImage(Type resourceManagerProvider, String resourceKey)
   {
      Object resource = LookupObject(resourceManagerProvider, resourceKey);
      return resource as Image;
   }
}
}
