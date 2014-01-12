using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace PJanssen.Outliner.UpdateClient
{
   public static class DispatcherExtensions
   {
      public static void SyncInvoke(this Dispatcher dispatcher, Action action)
      {
         if (dispatcher.CheckAccess())
            action();
         else
            dispatcher.Invoke(action);
      }
   }
}
