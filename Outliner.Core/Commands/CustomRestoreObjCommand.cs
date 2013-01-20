using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Autodesk.Max.Plugins;
using Outliner.MaxUtils;

namespace Outliner.Commands
{
   public abstract class CustomRestoreObjCommand : RestoreObj
   {
      public void Execute(Boolean redrawViews)
      {
         IHold theHold = MaxInterfaces.Global.TheHold;
         theHold.Begin();

         theHold.Put(this);
         this.Redo();

         theHold.Accept(this.Description);

         if (redrawViews)
         {
            IInterface ip = MaxInterfaces.Global.COREInterface;
            ip.RedrawViews(ip.Time, RedrawFlags.Normal, null);
         }
      }
   }
}
