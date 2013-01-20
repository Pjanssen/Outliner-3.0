using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;
using Autodesk.Max;
using Outliner.MaxUtils;

namespace Outliner.Commands
{
   public class SetViewCameraCommand : Command
   {
      private IMaxNode cameraNode;
      private IViewExp viewport;

      public SetViewCameraCommand(IMaxNode cameraNode, IViewExp viewport)
      {
         Throw.IfArgumentIsNull(cameraNode, "cameraNode");
         Throw.IfArgumentIsNull(viewport, "viewport");

         this.cameraNode = cameraNode;
         this.viewport = viewport;
      }

      public override string Description
      {
         get
         {
            return "Set Camera";
         }
      }

      public override void Do()
      {
         if (this.viewport == null || this.cameraNode == null)
            return;

         if (!this.cameraNode.IsValid)
            return;

         viewport.ViewCamera = (IINode)this.cameraNode.BaseObject;
      }
   }
}
