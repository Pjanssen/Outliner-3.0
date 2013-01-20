using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.MaxUtils;
using Outliner.Scene;

namespace Outliner.Commands
{
   public class CreateContainerCommand : Command
   {
      private IEnumerable<IMaxNode> nodes;
      private IINode containerNode;

      public CreateContainerCommand() : this(Enumerable.Empty<IMaxNode>()) { }
      public CreateContainerCommand(IEnumerable<IMaxNode> nodes)
      {
         Throw.IfArgumentIsNull(nodes, "nodes");

         this.nodes = nodes.ToList();
      }

      public override string Description
      {
         get
         {
            return OutlinerResources.Command_AddToNewContainer;
         }
      }

      public override void Do()
      {
         this.containerNode = MaxInterfaces.ContainerManager.CreateContainer(HelperMethods.ToIINodeTab(this.nodes));
         this.containerNode.SetAFlag(AnimatableFlags.Held);
      }

      public IMaxNode CreatedContainer
      {
         get { return MaxNodeWrapper.Create(this.containerNode); }
      }
   }
}
