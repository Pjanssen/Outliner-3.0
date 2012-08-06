using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Autodesk.Max.Remoting;

namespace MaxUtils
{
public static class MaxInterfaces
{
   private static IGlobal global;
   public static IGlobal Global
   {
      get 
      {
         if (global == null)
            global = GlobalInterface.Instance;

         #if DEBUG
         if (global == null)
         {
            IManager manager = (IManager)Activator.GetObject(typeof(RManager),
                                                             "tcp://localhost:9998/Manager");
            global = manager.Global;
         }
         #endif

         return global;
      }
   }

   public static IInterface COREInterface
   {
      get { return MaxInterfaces.Global.COREInterface; }
   }

   private static IInterface_ID nodeLayerProperties;
   public static IInterface_ID NodeLayerProperties
   {
      get
      {
         if (nodeLayerProperties == null)
            nodeLayerProperties = MaxInterfaces.Global.Interface_ID.Create(0x44e025f8, 0x6b071e44);

         return nodeLayerProperties;
      }
   }


   private static IIFPLayerManager iIFPLayerManager;
   public static IIFPLayerManager IIFPLayerManager
   {
      get 
      {
         if (iIFPLayerManager == null)
         {
            IInterface_ID iIFPLayerManagerID = MaxInterfaces.Global.Interface_ID.Create(
                                 (uint)BuiltInInterfaceIDA.LAYERMANAGER_INTERFACE,
                                 (uint)BuiltInInterfaceIDB.LAYERMANAGER_INTERFACE);
            iIFPLayerManager = (IIFPLayerManager)MaxInterfaces.Global.GetCOREInterface(iIFPLayerManagerID);
         }
         return iIFPLayerManager;
      }
   }

   private static IILayerManager iILayerManager;
   public static IILayerManager IILayerManager
   {
      get
      {
         if (iILayerManager == null)
            iILayerManager = (IILayerManager)MaxInterfaces.Global.COREInterface.ScenePointer.GetReference(10);

         return iILayerManager;
      }
   }


   public static IIInstanceMgr InstanceMgr
   {
      get
      {
         return MaxInterfaces.Global.IInstanceMgr.InstanceMgr;
      }
   }

   private static IINamedSelectionSetManager selectionSetManager;
   public static IINamedSelectionSetManager SelectionSetManager
   {
      get
      {
         if (selectionSetManager == null)
            selectionSetManager = MaxInterfaces.Global.INamedSelectionSetManager.Instance;

         return selectionSetManager;
      }
   }
}
}
