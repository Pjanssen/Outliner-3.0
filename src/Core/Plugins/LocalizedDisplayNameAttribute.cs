using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Resources;
using System.ComponentModel;

namespace PJanssen.Outliner.Plugins
{
/// <summary>
/// Specifies a localized display name for a class, retrieved from an embedded resource.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
sealed public class LocalizedDisplayNameAttribute : DisplayNameAttribute
{
   private String resKey;
   private Type resManProvider;

   /// <summary>
   /// Gets the displayname.
   /// </summary>
   public override String DisplayName
   {
      get { return ResourceHelpers.LookupString(this.resManProvider, this.resKey); }
   }

   /// <summary>
   /// Initializes a new instance of the LocalizedDisplayNameAttribute class.
   /// </summary>
   /// <param name="resManProvider">The type of the resource provider.</param>
   /// <param name="resKey">The key of the resource to use as the displayname.</param>
   public LocalizedDisplayNameAttribute(Type resManProvider, String resKey) : base()
   {
      this.resManProvider = resManProvider;
      this.resKey = resKey;
   }
}
}
