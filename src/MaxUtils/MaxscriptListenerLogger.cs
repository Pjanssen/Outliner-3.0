using Autodesk.Max;
using PJanssen.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.MaxUtils
{
   public class MaxscriptListenerLogger : TextLogger
   {
      private const int InfoStyle = 0;
      private const int DebugStyle = 1;
      private const int ErrorStyle = 2;

      private IListener listener;

      public MaxscriptListenerLogger(string toolName) 
      {
         this.listener = MaxInterfaces.Global.TheListener;

         base.TimeFormat = "[" + toolName + "] ";
      }

      protected override void WriteToLog(LogTypes logType, string message)
      {
         SetListenerStyle(logType);

         this.listener.EditStream.Wputs(message + Environment.NewLine);
         this.listener.EditStream.Flush();

         ResetListenerStyle();
      }

      private void SetListenerStyle(LogTypes logType)
      {
         switch (logType)
         {
            case LogTypes.Debug:
               this.listener.SetStyle(DebugStyle);
               break;
            case LogTypes.Error:
               this.listener.SetStyle(ErrorStyle);
               break;
            case LogTypes.Warning:
               this.listener.SetStyle(ErrorStyle);
               break;
            default:
               this.listener.SetStyle(InfoStyle);
               break;
         }
      }

      private void ResetListenerStyle()
      {
         this.listener.SetStyle(InfoStyle);
      }
   }
}
