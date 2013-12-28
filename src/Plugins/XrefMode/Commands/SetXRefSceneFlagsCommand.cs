using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outliner;
using Outliner.Commands;
using Outliner.Scene;

namespace Outliner.Modes.XRefMode.Commands
{
   public class SetXRefSceneFlagsCommand : Command
   {
      private IEnumerable<XRefSceneRecord> xrefScenes;
      private XRefSceneFlags flags;
      private Boolean value;

      public SetXRefSceneFlagsCommand(IEnumerable<IMaxNode> nodes, XRefSceneFlags flags, Boolean value)
      {
         Throw.IfNull(nodes, "nodes");

         this.xrefScenes = nodes.OfType<XRefSceneRecord>()
                                .ToList();
         this.flags = flags;
         this.value = value;
      }

      public override string Description
      {
         get 
         {
            if (this.value)
               return Resources.Command_SetXrefSceneEnabled;
            else
               return Resources.Command_SetXrefSceneDisabled;
         }
      }

      public override void Do()
      {
         foreach (XRefSceneRecord xrefScene in this.xrefScenes)
         {
            if (this.value)
               xrefScene.SetFlags(this.flags);
            else
               xrefScene.RemoveFlags(this.flags);
         }
      }
   }
}
