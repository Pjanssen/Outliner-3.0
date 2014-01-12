using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using PJanssen.Outliner.Commands;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.TreeNodeButtons
{
   class LockTransformsCommand : Command
   {
      private List<INodeWrapper> nodes;
      private Boolean lockTransforms;

      public LockTransformsCommand(IEnumerable<IMaxNode> nodes, Boolean lockTransforms)
      {
         this.nodes = nodes.OfType<INodeWrapper>().ToList();
         this.lockTransforms = lockTransforms;
      }

      public override string Description
      {
         get 
         {
            if (this.lockTransforms)
               return Resources.Command_LockTransforms;
            else
               return Resources.Command_UnlockTransforms;
         }
      }

      public override void Do()
      {
         LockTransformsRestoreObj restoreObj = new LockTransformsRestoreObj(nodes, lockTransforms);
         restoreObj.Redo();

         IHold theHold = MaxInterfaces.Global.TheHold;
         if (theHold.Holding)
            theHold.Put(restoreObj);
      }
   }
}
