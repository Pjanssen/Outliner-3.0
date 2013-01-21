using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;

namespace Outliner.MaxUtils
{
   public static class Viewports
   {
      /// <summary>
      /// Redraws all views.
      /// </summary>
      public static void Redraw()
      {
         IInterface ip = MaxInterfaces.Global.COREInterface;
         ip.RedrawViews(ip.Time, RedrawFlags.Normal, null);
      }

      public static void ForceRedraw()
      {
         IInterface ip = MaxInterfaces.Global.COREInterface;
         ip.ForceCompleteRedraw(false);
      }
   }
}
