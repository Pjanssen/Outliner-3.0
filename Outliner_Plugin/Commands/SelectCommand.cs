using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max.Plugins;
using Autodesk.Max;

namespace Outliner.Commands
{
   public class SelectCommand : Command
   {
      private IINodeTab prevSelection;
      private IINodeTab newSelection;

      public SelectCommand(IINodeTab nodes)
      {
         if (nodes == null)
            throw new ArgumentNullException("Select command nodes parameter cannot be null.");

         this.newSelection = nodes;
         this.prevSelection = GlobalInterface.Instance.INodeTabNS.Create();
      }

      public override string Description
      {
         get { return OutlinerResources.Command_Select; }
      }

      public override void Do()
      {
         IInterface ip = GlobalInterface.Instance.COREInterface;
         this.storePrevSelection(ip);
         this.SelectNodes(ip, this.newSelection);
      }

      public override void Undo()
      {
         IInterface ip = GlobalInterface.Instance.COREInterface;
         this.SelectNodes(ip, this.prevSelection);
      }

      protected void storePrevSelection(IInterface ip)
      {
         Int32 selNodeCount = ip.SelNodeCount;
         if (selNodeCount > 0)
         {
            this.prevSelection.Resize(selNodeCount);
            for (int i = 0; i < selNodeCount; i++)
            {
               this.prevSelection.AppendNode(ip.GetSelNode(i), true, 0);
            }
         }
      }

      protected void SelectNodes(IInterface ip, IINodeTab nodes)
      {
         Boolean empty = this.newSelection.Count == 0;
         ip.ClearNodeSelection(empty);
         if (!empty)
            ip.SelectNodeTab(this.newSelection, true, true);
      }
   }
}
