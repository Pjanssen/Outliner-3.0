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
         IINodeTab nodeTab = this.nodes.ToIINodeTab();
         this.containerNode = MaxInterfaces.ContainerManager.CreateContainer(nodeTab);
         this.containerNode.SetAFlag(AnimatableFlags.Held);
      }

      public IMaxNode CreatedContainer
      {
         get { return new INodeWrapper(this.containerNode); }
      }
   }
}
