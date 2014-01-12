using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;

namespace PJanssen.Outliner.MaxUtils
{
   /// <summary>
   /// Provides methods for common 3dsMax viewport operations.
   /// </summary>
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

      /// <summary>
      /// Forces a complete redraw of all views.
      /// </summary>
      public static void ForceRedraw()
      {
         IInterface ip = MaxInterfaces.Global.COREInterface;
         ip.ForceCompleteRedraw(false);
      }

      /// <summary>
      /// Gets the active viewport.
      /// </summary>
      public static IViewExp ActiveView
      {
         get
         {
            return MaxInterfaces.COREInterface.ActiveViewExp;
         }
      }
   }
}
