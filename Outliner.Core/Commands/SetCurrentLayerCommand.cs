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
      private ILayerWrapper newCurrentLayer;

      public SetCurrentLayerCommand(ILayerWrapper newActiveLayer)
      {
         Throw.IfArgumentIsNull(newActiveLayer, "newActiveLayer");

         this.newCurrentLayer = newActiveLayer;
      }

      public override string Description
      {
         get { return OutlinerResources.Command_SetCurrentLayer; }
      }

      public override void Do()
      {
         this.newCurrentLayer.IsCurrent = true;
      }
   }
}
