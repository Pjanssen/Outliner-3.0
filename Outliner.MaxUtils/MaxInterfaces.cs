using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Autodesk.Max.IColorManager;
using Autodesk.Max.Remoting;

namespace Outliner.MaxUtils
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
            IManager manager = Activator.GetObject( typeof(RManager)
                                                  , "tcp://localhost:9998/Manager") as IManager;
            if (manager != null)
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

   public static IINamedSelectionSetManager SelectionSetManager
   {
      get
      {
         return MaxInterfaces.Global.INamedSelectionSetManager.Instance;
      }
   }

   private static IInterval intervalForever;
   public static IInterval IntervalForever
   {
      get
      {
         if (intervalForever == null)
         {
            intervalForever = MaxInterfaces.Global.Interval.Create();
            intervalForever.SetInfinite();
         }
         return intervalForever;
      }
   }

   private static System.Windows.Forms.IWin32Window maxHwnd;
   public static System.Windows.Forms.IWin32Window MaxHwnd
   {
      get
      {
         if (maxHwnd == null)
            maxHwnd = new WindowWrapper(MaxInterfaces.COREInterface.MAXHWnd);

         return maxHwnd;
      }
   }

   public static IIColorManager ColorManager
   {
      get { return MaxInterfaces.Global.ColorManager; }
   }

   /// <summary>
   /// Indicates if the current color theme is 'light'.
   /// </summary>
   public static Boolean ColorThemeLightActive
   {
      get { return ColorManager.AppFrameColorTheme == AppFrameColorTheme.LightTheme; }
   }

   /// <summary>
   /// Indicates if the current color theme is 'dark'.
   /// </summary>
   public static Boolean ColorThemeDarkActive
   {
      get { return ColorManager.AppFrameColorTheme == AppFrameColorTheme.DarkTheme; }
   }

}
}
