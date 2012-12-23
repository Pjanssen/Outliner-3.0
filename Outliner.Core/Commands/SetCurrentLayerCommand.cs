using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.MaxUtils;
using Outliner.Scene;

namespace Outliner.Commands
{
   public class SetCurrentLayerCommand : Command
   {
      private IILayerWrapper newCurrentLayer;
      private IILayerWrapper oldCurrentLayer;

      public SetCurrentLayerCommand(IILayerWrapper newActiveLayer)
      {
         Throw.IfArgumentIsNull(newActiveLayer, "newActiveLayer");

         this.newCurrentLayer = newActiveLayer;
      }

      public override string Description
      {
         get { return OutlinerResources.Command_SetCurrentLayer; }
      }

      protected override void Do()
      {
         IILayerManager manager = MaxInterfaces.IILayerManager;
         if (manager != null)
            oldCurrentLayer = IMaxNodeWrapper.Create(manager.CurrentLayer) as IILayerWrapper;

         if (newCurrentLayer != null)
            newCurrentLayer.IsCurrent = true;
      }

      protected override void Undo()
      {
         if (this.oldCurrentLayer != null)
            this.oldCurrentLayer.IsCurrent = true;
      }
   }
}
