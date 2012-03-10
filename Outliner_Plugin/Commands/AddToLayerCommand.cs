using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;

namespace Outliner.Commands
{
public class AddToLayerCommand : Command
{
   protected IEnumerable<IINode> nodes;
   protected IILayer layer;
   protected Dictionary<IINode, IILayer> prevLayers;

   public AddToLayerCommand(IEnumerable<IINode> nodes, IILayer layer)
   {
      this.nodes = nodes;
      this.layer = layer;
   }

   public override string Description
   {
      get { return OutlinerResources.Command_AddToLayer; }
   }

   public override void Do()
   {
      if (this.nodes == null || this.layer == null)
         return;
      
      this.prevLayers = new Dictionary<IINode, IILayer>(this.nodes.Count());
      foreach (IINode node in this.nodes)
      {
         this.prevLayers[node] = (IILayer)node.GetReference((int)ReferenceNumbers.NodeLayerRef);
         this.layer.AddToLayer(node);
      }
   }

   public override void Undo()
   {
      if (this.prevLayers == null)
         return;

      foreach (KeyValuePair<IINode, IILayer> kvp in this.prevLayers)
      {
         kvp.Value.AddToLayer(kvp.Key);
      }
   }
}
}
