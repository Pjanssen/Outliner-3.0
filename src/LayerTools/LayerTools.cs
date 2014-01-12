using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Autodesk.Max.Plugins;
using PJanssen.Outliner.MaxUtils;

namespace PJanssen.Outliner.LayerTools
{
   /// <summary>
   /// Provides common methods and constants for 3dsMax Layer management.
   /// </summary>
   public static class LayerTools
   {
      /// <summary>
      /// Sets the current layer in an undo context.
      /// </summary>
      /// <param name="newCurrentLayer">The new active layer.</param>
      public static void SetCurrentLayer(IILayer newCurrentLayer)
      {
         IHold theHold = MaxInterfaces.Global.TheHold;
         if (theHold.Holding)
         {
            SetCurrentLayerRestoreObj restoreObj = new SetCurrentLayerRestoreObj(newCurrentLayer);
            theHold.Put(restoreObj);
            restoreObj.Redo();
         }
      }

      private class SetCurrentLayerRestoreObj : RestoreObj
      {
         private IILayer oldCurrentLayer;
         private IILayer newCurrentLayer;
         public SetCurrentLayerRestoreObj(IILayer newCurrentLayer)
         {
            this.newCurrentLayer = newCurrentLayer;
         }

         public override void Redo()
         {
            this.oldCurrentLayer = MaxInterfaces.IILayerManager.CurrentLayer;
            this.SetCurrentLayer(this.newCurrentLayer);
         }

         public override void Restore(bool isUndo)
         {
            this.SetCurrentLayer(this.oldCurrentLayer);
         }

         private void SetCurrentLayer(IILayer layer)
         {
            String name = layer.Name;
            MaxInterfaces.IILayerManager.SetCurrentLayer(ref name);
            MaxInterfaces.Global.BroadcastNotification(LayerNotificationCode.LayerCurrentChanged, this.oldCurrentLayer);
            MaxInterfaces.Global.BroadcastNotification(LayerNotificationCode.LayerCurrentChanged, this.newCurrentLayer);
         }
      }
   }
}
