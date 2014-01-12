using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Modes.Layer;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.Commands
{
   public class SetCurrentLayerCommand : Command
   {
      private ILayerWrapper newCurrentLayer;

      public SetCurrentLayerCommand(ILayerWrapper newActiveLayer)
      {
         Throw.IfNull(newActiveLayer, "newActiveLayer");

         this.newCurrentLayer = newActiveLayer;
      }

      public override string Description
      {
         get { return Resources.Command_SetCurrentLayer; }
      }

      public override void Do()
      {
         this.newCurrentLayer.IsCurrent = true;
      }
   }
}
