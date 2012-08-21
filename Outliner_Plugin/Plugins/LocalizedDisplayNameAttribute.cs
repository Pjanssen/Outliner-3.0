using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Resources;
using System.ComponentModel;

namespace Outliner.Plugins
{
/// <summary>
/// Specifies a localized display name for a class, retrieved from an embedded resource.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
sealed public class LocalizedDisplayNameAttribute : DisplayNameAttribute
{
   private String resKey;
   private Type resManProvider;
   public override String DisplayName
   {
      get { return LookupResource(this.resManProvider, this.resKey); }
   }

   internal static String LookupResource(Type resourceManagerProvider, string resourceKey)
   {
      PropertyInfo property = resourceManagerProvider.GetProperty(resourceKey, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
      if (property != null)
      {
         object val = property.GetValue(null, null);
         String name = val as String;
         if (name != null)
            return name;
      }
         
      return resourceKey;
   }

   public LocalizedDisplayNameAttribute(Type resManProvider, String resKey) : base()
   {
      this.resManProvider = resManProvider;
      this.resKey = resKey;
   }
}
}
