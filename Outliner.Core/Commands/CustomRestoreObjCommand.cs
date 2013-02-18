using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Autodesk.Max.Plugins;
using Outliner.MaxUtils;

namespace Outliner.Commands
{
   /// <summary>
   /// Defines a base class for commands that require custom restore logic for the 3dsmax undo system.
   /// </summary>
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
            Viewports.Redraw();
      }
   }
}
