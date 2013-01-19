using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Autodesk.Max;
using Outliner.MaxUtils;

namespace Outliner.Commands
{
public class SelectCommand : Command
{
   private Boolean openGroups;
   private IEnumerable<IMaxNode> nodes;
   private IEnumerable<IMaxNode> oldSelection;

   public SelectCommand(IEnumerable<IMaxNode> nodes) : this(nodes, true) { }

   public SelectCommand(IEnumerable<IMaxNode> nodes, Boolean openGroups) 
   {
      Throw.IfArgumentIsNull(nodes, "nodes");

      this.nodes = nodes.ToList();
      this.openGroups = openGroups;
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
      List<IMaxNode> oldSel = new List<IMaxNode>();
      for (int i = 0; i < selNodeCount; i++)
      {
         oldSel.Add(MaxNodeWrapper.Create(ip.GetSelNode(i)));
      }
      this.oldSelection = oldSel;

      if (this.openGroups)
         GroupHelpers.OpenSelectedGroupHeads(this.nodes);

      //Select new selection.
      SelectCommand.SelectNodes(ip, this.nodes);
   }

   protected override void Undo()
   {
      SelectCommand.SelectNodes(MaxInterfaces.COREInterface, this.oldSelection);
   }

   protected static void SelectNodes(IInterface ip, IEnumerable<IMaxNode> nodes)
   {
      if (ip == null || nodes == null)
         return;

      //Clear previous selection.
      ip.ClearNodeSelection(false);

      foreach (IMaxNode node in nodes)
      {
         node.IsSelected = true;
      }
      ////Select INodes.
      //IEnumerable<IMaxNodeWrapper> inodes = nodes.Where(n => n is IINodeWrapper);
      //if (inodes.Count() > 0)
      //   ip.SelectNodeTab(HelperMethods.ToIINodeTab(inodes), true, false);

      ////Select Layers.
      //IEnumerable<IILayerWrapper> layers = nodes.Where(n => n is IILayerWrapper).Cast<IILayerWrapper>();
      //foreach (IILayerWrapper layer in layers)
      //{
      //   layer.IILayerProperties.Select(true);
      //}

      ////Select SelectionSets
      //IEnumerable<SelectionSetWrapper> selsets = nodes.Where(n => n is SelectionSetWrapper).Cast<SelectionSetWrapper>();
      //foreach (SelectionSetWrapper selset in selsets)
      //{
      //   ip.SelectNodeTab(HelperMethods.ToIINodeTab(selset.ChildNodes), true, false);
      //}
   }
}
}
