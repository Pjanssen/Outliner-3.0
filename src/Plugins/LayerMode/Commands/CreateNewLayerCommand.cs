using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Autodesk.Max;
using Outliner.MaxUtils;
using Outliner.Modes.Layer;

namespace Outliner.Commands
{
public class CreateNewLayerCommand : Command
{
   private IEnumerable<IMaxNode> nodes;
   private String name;
   private IILayer createdLayer;

   public CreateNewLayerCommand()
      : this(Enumerable.Empty<IMaxNode>(), null) { }

   public CreateNewLayerCommand(String name)
      : this(Enumerable.Empty<IMaxNode>(), name) { }

   public CreateNewLayerCommand(IEnumerable<IMaxNode> nodes) 
      : this(nodes, null) { }

   public CreateNewLayerCommand(IEnumerable<IMaxNode> nodes, String name)
   {
      Throw.IfNull(nodes, "nodes");

      this.nodes = nodes.ToList();
      this.name = name;
   }

   public override string Description
   {
      get { return Resources.Command_CreateNewLayer; }
   }


   public override void Do()
   {
      if (this.name != null)
         this.createdLayer = MaxInterfaces.IILayerManager.CreateLayer(ref this.name);
      else
         this.createdLayer = MaxInterfaces.IILayerManager.CreateLayer();

      this.CreatedLayer.AddChildNodes(this.nodes);
   }

   public ILayerWrapper CreatedLayer
   {
      get
      {
         return new ILayerWrapper(this.createdLayer);
      }
   }
}
}
