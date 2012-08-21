using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaxUtils
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
   }
}
