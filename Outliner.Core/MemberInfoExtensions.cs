using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Outliner
{
   public static class MemberInfoExtensions
   {
      public static Boolean HasAttribute<T>(this MemberInfo memberInfo) where T : Attribute
      {
         Throw.IfArgumentIsNull(memberInfo, "memberInfo");

         object[] attributes = memberInfo.GetCustomAttributes(typeof(T), false);
         return attributes != null && attributes.Count() > 0;
      }

      public static T GetAttribute<T>(this MemberInfo memberInfo) where T : Attribute
      {
         Throw.IfArgumentIsNull(memberInfo, "memberInfo");

         object[] attributes = memberInfo.GetCustomAttributes(typeof(T), false);
         if (attributes != null && attributes.Count() > 0)
            return attributes[0] as T;
         else
            return null;
      }
   }
}
