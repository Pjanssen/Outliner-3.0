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
   /// <summary>
   /// Defines a baseclass for commands that can be registered in the 3dsmax undo system.
   /// </summary>
   public abstract class Command
   {
      /// <summary>
      /// The description of the command.
      /// </summary>
      public abstract String Description { get; }

      /// <summary>
      /// This method should contain the logic to modify the scene when executing the command.
      /// </summary>
      public abstract void Do();

      /// <summary>
      /// Executes the command in an undo context.
      /// </summary>
      public virtual void Execute()
      {
         IHold theHold = MaxInterfaces.Global.TheHold;
         theHold.Begin();

         this.Do();

         theHold.Accept(this.Description);
      }
   }
}
