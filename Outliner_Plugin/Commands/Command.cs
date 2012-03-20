using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max.Plugins;
using Autodesk.Max;

namespace Outliner.Commands
{
   public abstract class Command : RestoreObj
   {
      public abstract void Do();
      public abstract void Undo();

      public override void Redo()
      {
         this.Do();
      }

      public override void Restore(bool isUndo)
      {
         this.Undo();
      }

      /// <summary>
      /// Registers a command in the hold and executes it by running its Redo method.
      /// </summary>
      public virtual void Execute(Boolean redrawViews) 
      {
         IGlobal iGlobal = Autodesk.Max.GlobalInterface.Instance;
         IHold theHold = iGlobal.TheHold;
         if (!theHold.Holding)
            theHold.Begin();

         this.Do();

         theHold.Put(this);
         theHold.Accept(this.Description);

         if (redrawViews)
         {
            IInterface ip = iGlobal.COREInterface;
            ip.RedrawViews(ip.Time, RedrawFlags.Normal, null);
         }
      }
   }
}
