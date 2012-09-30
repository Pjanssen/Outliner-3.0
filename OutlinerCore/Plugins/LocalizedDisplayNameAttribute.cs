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
      get { return ResourceHelpers.LookupString(this.resManProvider, this.resKey); }
   }

   public LocalizedDisplayNameAttribute(Type resManProvider, String resKey) : base()
   {
      this.resManProvider = resManProvider;
      this.resKey = resKey;
   }
}
}
