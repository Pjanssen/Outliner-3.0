using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Outliner.MaxUtils
{
   public delegate Boolean BinaryPredicate<T>(T a, T b);

   public static class Functor
   {
      private static Boolean PAnd(Boolean a, Boolean b) { return a && b; }
      private static BinaryPredicate<Boolean> and = new BinaryPredicate<bool>(PAnd);
      public static BinaryPredicate<Boolean> And  { get { return and; } }

      private static Boolean POr(Boolean a, Boolean b) { return a || b; }
      private static BinaryPredicate<Boolean> or = new BinaryPredicate<bool>(POr);
      public static BinaryPredicate<Boolean> Or { get { return or; } }

      public static String PredicateToString<T>(BinaryPredicate<T> predicate)
      {
         if (predicate.Equals(And))
            return "And";
         else if (predicate.Equals(Or))
            return "Or";
         else
            return predicate.ToString();
      }

      public static BinaryPredicate<Boolean> PredicateFromString(String predicate)
      {
         if (predicate == null)
            return Functor.And;

         if (predicate.Equals("And"))
            return Functor.And;
         else if (predicate.Equals("Or"))
            return Functor.Or;
         else
            return Functor.And;
      }
   }
}
