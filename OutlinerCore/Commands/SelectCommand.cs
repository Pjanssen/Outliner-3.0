using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Autodesk.Max;
using MaxUtils;

namespace Outliner.Commands
{
public class SelectCommand : Command
{
   private IEnumerable<IMaxNodeWrapper> nodes;
   private IEnumerable<IMaxNodeWrapper> oldSelection;

   public SelectCommand(IEnumerable<IMaxNodeWrapper> nodes) 
   {
      ExceptionHelpers.ThrowIfArgumentIsNull(nodes, "nodes");

      this.nodes = nodes;
   }
 

   public override string Description
   {
      get { return OutlinerResources.Command_Select; }
   }

   protected override void Do()
   {
      IInterface ip = MaxInterfaces.COREInterface;

      //Store old selection.
      Int32 selNodeCount = ip.SelNodeCount;
      List<IMaxNodeWrapper> oldSel = new List<IMaxNodeWrapper>();
      for (int i = 0; i < selNodeCount; i++)
      {
         oldSel.Add(IMaxNodeWrapper.Create(ip.GetSelNode(i)));
      }
      this.oldSelection = oldSel;

      GroupHelpers.OpenSelectedGroupHeads(this.nodes);

      //Select new selection.
      SelectCommand.SelectNodes(ip, this.nodes);
   }

   protected override void Undo()
   {
      SelectCommand.SelectNodes(MaxInterfaces.COREInterface, this.oldSelection);
   }

   protected static void SelectNodes(IInterface ip, IEnumerable<IMaxNodeWrapper> nodes)
   {
      if (ip == null || nodes == null)
         return;

      //Clear previous selection.
      ip.ClearNodeSelection(false);

      //Select INodes.
      IEnumerable<IMaxNodeWrapper> inodes = nodes.Where(n => n is IINodeWrapper);
      if (inodes.Count() > 0)
         ip.SelectNodeTab(HelperMethods.ToIINodeTab(inodes), true, false);

      //Select Layers.
      IEnumerable<IILayerWrapper> layers = nodes.Where(n => n is IILayerWrapper).Cast<IILayerWrapper>();
      foreach (IILayerWrapper layer in layers)
      {
         layer.IILayerProperties.Select(true);
      }

      //Select SelectionSets
      IEnumerable<SelectionSetWrapper> selsets = nodes.Where(n => n is SelectionSetWrapper).Cast<SelectionSetWrapper>();
      foreach (SelectionSetWrapper selset in selsets)
      {
         ip.SelectNodeTab(HelperMethods.ToIINodeTab(selset.ChildNodes), true, false);
      }
   }
}
}
