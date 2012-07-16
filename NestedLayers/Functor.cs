using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.LayerTools
{
   public delegate Boolean BinaryBooleanOperation(Boolean a, Boolean b);

   public static class Functor
   {
      public static Boolean And(Boolean a, Boolean b) { return a && b; }
      public static Boolean Or(Boolean a, Boolean b) { return a || b; }
   }
}
