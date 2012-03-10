using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;

namespace Outliner
{
   public static class AssemblyFunctions
   {
      public static void AssemblyMain()
      {
         IGlobal g = GlobalInterface.Instance;
         g.COREInterface.AddClass(new OutlinerDescriptor(g));
      }

      public static void AssemblyShutdown()
      {
      }
   }
}
