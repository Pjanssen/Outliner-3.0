using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Autodesk.Max;
using MaxUtils;

namespace Outliner.Commands
{
public class DeleteCommand : Command
{
   private IEnumerable<IMaxNodeWrapper> nodes;

   public DeleteCommand(IEnumerable<IMaxNodeWrapper> nodes)
   {
      ExceptionHelpers.ThrowIfArgumentIsNull(nodes, "nodes");

      this.nodes = nodes;
   }

   public override string Description
   {
      get { return OutlinerResources.Command_Delete; }
   }

   protected override void Do()
   {
      foreach (IMaxNodeWrapper node in this.nodes)
      {
         IINodeWrapper iinodeWrapper = node as IINodeWrapper;
         if (iinodeWrapper != null)
         {
            IInterface ip = MaxInterfaces.COREInterface;
            ip.DeleteNode(iinodeWrapper.IINode, false, false);
         }
      }
   }

   protected override void Undo() { }
}
}
