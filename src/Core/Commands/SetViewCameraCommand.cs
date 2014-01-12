using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Scene;
using Autodesk.Max;
using PJanssen.Outliner.MaxUtils;

namespace PJanssen.Outliner.Commands
{
   /// <summary>
   /// Sets the given viewport to use the given Camera.
   /// </summary>
   public class SetViewCameraCommand : Command
   {
      private IMaxNode cameraNode;
      private IViewExp viewport;

      /// <summary>
      /// Initializes a new instance of the SetViewCameraCommand class.
      /// </summary>
      /// <param name="cameraNode">The Camera node to use.</param>
      /// <param name="viewport">The viewport to set the camera for.</param>
      public SetViewCameraCommand(IMaxNode cameraNode, IViewExp viewport)
      {
         Throw.IfNull(cameraNode, "cameraNode");
         Throw.IfNull(viewport, "viewport");

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
