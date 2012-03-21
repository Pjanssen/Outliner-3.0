using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Autodesk.Max;

namespace Outliner.Commands
{
   public class SetActiveLayerCommand : Command
   {
      private IILayerWrapper newActiveLayer;
      private IILayerWrapper oldActiveLayer;

      public SetActiveLayerCommand(IILayerWrapper newActiveLayer)
      {
         this.newActiveLayer = newActiveLayer;
      }

      public override void Do()
      {
         IInterface ip = GlobalInterface.Instance.COREInterface;
         IILayerManager manager = ip.ScenePointer.GetReference(10) as IILayerManager;
         if (manager != null)
            oldActiveLayer = IMaxNodeWrapper.Create(manager.CurrentLayer) as IILayerWrapper;

         if (newActiveLayer != null)
            newActiveLayer.IsCurrent = true;
      }

      public override void Undo()
      {
         if (this.oldActiveLayer != null)
            this.oldActiveLayer.IsCurrent = true;
      }
   }
}
