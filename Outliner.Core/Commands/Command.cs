using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max.Plugins;
using Autodesk.Max;
using Outliner.MaxUtils;
using Outliner.Scene;

namespace Outliner.Commands
{
   public abstract class Command
   {
      public abstract String Description { get; }

      public abstract void Do();

      /// <summary>
      /// Executes the command in an undo context.
      /// </summary>
      public virtual void Execute(Boolean redrawViews)
      {
         IHold theHold = MaxInterfaces.Global.TheHold;
         theHold.Begin();

         this.Do();

         theHold.Accept(this.Description);

         if (redrawViews)
            Viewports.Redraw();
      }
   }
}
