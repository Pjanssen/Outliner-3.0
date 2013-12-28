using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using PJanssen;

namespace Outliner
{
   /// <summary>
   /// Provides extension methods for MemberInfo objects.
   /// </summary>
   public static class MemberInfoExtensions
   {
      /// <summary>
      /// Tests if a member has an attribute of a given type.
      /// </summary>
      /// <typeparam name="T">The type of attribute to find.</typeparam>
      public static Boolean HasAttribute<T>(this MemberInfo memberInfo) where T : Attribute
      {
         Throw.IfNull(memberInfo, "memberInfo");

         object[] attributes = memberInfo.GetCustomAttributes(typeof(T), false);
         return attributes != null && attributes.Count() > 0;
      }

      /// <summary>
      /// Gets the attribute of the given type.
      /// </summary>
      /// <typeparam name="T">The type of attribute to find.</typeparam>
      public static T GetAttribute<T>(this MemberInfo memberInfo) where T : Attribute
      {
         Throw.IfNull(memberInfo, "memberInfo");

         object[] attributes = memberInfo.GetCustomAttributes(typeof(T), false);
         if (attributes != null && attributes.Count() > 0)
            return attributes[0] as T;
         else
            return null;
      }
   }
}
