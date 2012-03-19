using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Autodesk.Max;

namespace Outliner.Commands
{
public class SelectCommand : Command
{
   private IINodeTab newSelection;
   private IINodeTab prevSelection;

   public SelectCommand(IEnumerable<IMaxNodeWrapper> nodes)
   {
      if (nodes == null)
         throw new ArgumentNullException("nodes");

      this.newSelection = HelperMethods.ToIINodeTab(nodes);
   }

   public override string Description
   {
      get { return OutlinerResources.Command_Select; }
   }

   public override void Do()
   {
      IInterface ip = GlobalInterface.Instance.COREInterface;
      Int32 selNodeCount = ip.SelNodeCount;
      this.prevSelection = GlobalInterface.Instance.INodeTabNS.Create();
      for (int i = 0; i < selNodeCount; i++)
      {
         this.prevSelection.AppendNode(ip.GetSelNode(i), true, 0);
      }

      this.SelectNodes(ip, this.newSelection);
   }

   public override void Undo()
   {
      this.SelectNodes(GlobalInterface.Instance.COREInterface, this.prevSelection);
   }

   protected void SelectNodes(IInterface ip, IINodeTab nodes)
   {
      if (nodes == null)
         return;

      ip.ClearNodeSelection(false);
      if (nodes.Count > 0)
         ip.SelectNodeTab(nodes, true, false);
   }
}
}
