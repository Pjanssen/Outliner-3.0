using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Autodesk.Max;
using Outliner.MaxUtils;

namespace Outliner.Commands
{
public class CreateNewLayerCommand : Command
{
   private IEnumerable<MaxNodeWrapper> nodes;
   private String name;
   private IILayer createdLayer;

   public CreateNewLayerCommand() 
      : this(Enumerable.Empty<MaxNodeWrapper>(), null) { }

   public CreateNewLayerCommand(String name) 
      : this(Enumerable.Empty<MaxNodeWrapper>(), name) { }

   public CreateNewLayerCommand(IEnumerable<MaxNodeWrapper> nodes) 
      : this(nodes, null) { }

   public CreateNewLayerCommand(IEnumerable<MaxNodeWrapper> nodes, String name)
   {
      Throw.IfArgumentIsNull(nodes, "nodes");

      this.nodes = nodes.ToList();
      this.name = name;
   }

   public override string Description
   {
      get { return OutlinerResources.Command_CreateNewLayer; }
   }

   public IILayerWrapper CreatedLayer
   {
      get
      {
         return new IILayerWrapper(this.createdLayer);
      }
   }

   protected override void Do()
   {
      //if (this.name != null)
      //   this.createdLayer = MaxInterfaces.IILayerManager.CreateLayer(ref this.name);
      //else
      //   this.createdLayer = MaxInterfaces.IILayerManager.CreateLayer();
      
      //Temporary maxscript implementation due to crashes.
      ManagedServices.MaxscriptSDK.ExecuteMaxscriptCommand("layermanager.newlayer()");
      this.createdLayer = MaxInterfaces.IILayerManager.GetLayer(MaxInterfaces.IILayerManager.LayerCount - 1);

      this.CreatedLayer.AddChildNodes(this.nodes);
   }

   protected override void Undo() 
   {
      if (this.createdLayer != null)
      {
         this.CreatedLayer.RemoveChildNodes(this.nodes);

         String layerName = this.createdLayer.Name;
         this.createdLayer = null;
         //Boolean del = MaxInterfaces.IILayerManager.DeleteLayer(ref layerName);
         
         //Temporary maxscript implementation due to crashes.
         ManagedServices.MaxscriptSDK.ExecuteMaxscriptCommand("layermanager.deleteLayerByName " + layerName);
      }
   }
}
}
