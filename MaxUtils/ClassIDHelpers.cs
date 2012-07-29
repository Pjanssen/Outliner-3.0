using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;

namespace MaxUtils
{
   public static class ClassIDHelpers
   {
      public static bool Equals(IClass_ID cid, BuiltInClassIDA cidA)
      {
         return ClassIDHelpers.Equals(cid, (uint)cidA);
      }

      public static bool Equals(IClass_ID cid, BuiltInClassIDA cidA, BuiltInClassIDB cidB)
      {
         return ClassIDHelpers.Equals(cid, (uint)cidA, (uint)cidB);
      }

      public static bool Equals(IClass_ID cid, uint cidA)
      {
         return ClassIDHelpers.Equals(cid, cidA, 0);
      }

      public static bool Equals(IClass_ID cid, uint cidA, uint cidB)
      {
         if (cid == null)
            return false;
         else
            return cid.PartA == cidA && cid.PartB == cidB;
      }


      public const uint BIPED_CLASSIDA = 0x9155;
      public const uint SKELOBJ_CLASSIDA = 0x9125;
      public const uint CATBONE_CLASSIDA = 0x2E6A0C09;
      public const uint CATBONE_CLASSIDB = 0x43D5C9C0;
      public const uint CATHUB_CLASSIDA = 0x73DC4833;
      public const uint CATHUB_CLASSIDB = 0x65C93CAA;
      public const uint PARTICLECHANNEL_CLASSIDB = 0x1eb34100;
      public const uint PFACTION_CLASSIDB = 0x1eb34200;
      public const uint PFACTOR_CLASSIDB = 0x1eb34300;
      public const uint PFMATERIAL_CLASSIDB = 0x1eb34400;  
   }
}
