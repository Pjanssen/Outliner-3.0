using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Commands;
using Outliner.MaxUtils;

namespace Outliner.Modes.Layer
{
   public class CreateLayerCommand : Command
   {
      private IILayer createdLayer;

      public CreateLayerCommand() { }

      protected override void Do()
      {
         this.createdLayer = MaxInterfaces.IILayerManager.CreateLayer();
      }

      protected override void Undo()
      {
         if (this.createdLayer != null)
         {
            String layerName = this.createdLayer.Name;
            MaxInterfaces.IILayerManager.DeleteLayer(ref layerName);
         }
      }
   }
}
