using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PJanssen.Outliner.Plugins
{
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
sealed public class LocalizedDisplayImageAttribute : Attribute
{
   private Type resManProvider;
   private String resKeySmall;
   private String resKeyLarge;
   
   public Image DisplayImageSmall
   {
      get { return ResourceHelpers.LookupImage(this.resManProvider, this.resKeySmall); }
   }
   public Image DisplayImageLarge
   {
      get { return ResourceHelpers.LookupImage(this.resManProvider, this.resKeyLarge); }
   }

   public LocalizedDisplayImageAttribute(Type resManProvider, String resKey)
      : this(resManProvider, resKey, String.Empty) { }

   public LocalizedDisplayImageAttribute(Type resManProvider, String resKeySmall, String resKeyLarge)
   {
      this.resManProvider = resManProvider;
      this.resKeySmall = resKeySmall;
      this.resKeyLarge = resKeyLarge;
   }
}
}
