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
         OutlinerGUP.Start();
      }

      public static void AssemblyShutdown() 
      {
         OutlinerGUP.Instance.Stop();
      }
   }
}
