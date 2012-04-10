using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;

namespace Outliner
{
public static class MaxInterfaces
{
   private static IInterface_ID nodeLayerProperties;
   public static IInterface_ID NodeLayerProperties
   {
      get
      {
         if (nodeLayerProperties == null)
            nodeLayerProperties = GlobalInterface.Instance.Interface_ID.Create(0x44e025f8, 0x6b071e44);

         return nodeLayerProperties;
      }
   }

   private static IInterface_ID layerManagerID;
   public static IInterface_ID LayerManagerID
   {
      get
      {
         if (layerManagerID == null)
         {
            layerManagerID = GlobalInterface.Instance.Interface_ID.Create(
                                 (uint)BuiltInInterfaceIDA.LAYERMANAGER_INTERFACE,
                                 (uint)BuiltInInterfaceIDB.LAYERMANAGER_INTERFACE);
         }
         return layerManagerID;
      }
   }

   private static IIFPLayerManager iIFPLayerManager;
   public static IIFPLayerManager IIFPLayerManager
   {
      get 
      {
         if (iIFPLayerManager == null)
         {
            iIFPLayerManager = (IIFPLayerManager)GlobalInterface.Instance.GetCOREInterface(LayerManagerID);
         }
         return iIFPLayerManager;
      }
   }


   public static IIInstanceMgr InstanceMgr
   {
      get
      {
         return GlobalInterface.Instance.IInstanceMgr.InstanceMgr;
      }
   }
}
}
