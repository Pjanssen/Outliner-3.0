using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Autodesk.Max.IColorManager;
using Autodesk.Max.Remoting;

namespace PJanssen.Outliner.MaxUtils
{
/// <summary>
/// Provides (mostly cached) access to common 3dsMax interfaces.
/// </summary>
public static class MaxInterfaces
{
   //==========================================================================

   private static IGlobal global;
   private static IInterface_ID nodeLayerProperties;
   private static IIFPLayerManager iIFPLayerManager;
   private static IILayerManager iILayerManager;
   private static IIContainerManager containerManager;
   private static IInterval intervalForever;
   private static System.Windows.Forms.IWin32Window maxHwnd;

   //==========================================================================

   /// <summary>
   /// Gets the Global interface.
   /// </summary>
   public static IGlobal Global
   {
      get 
      {
         if (global == null)
            global = GlobalInterface.Instance;

         #if DEBUG
         if (global == null)
         {
            try
            {
               IManager manager = Activator.GetObject( typeof(RManager)
                                                     , "tcp://localhost:9998/Manager") as IManager;
               if (manager != null)
                  global = manager.Global;
            }
            catch (System.Net.Sockets.SocketException)
            {
               //Remoting is disabled or unavailable for some other reason.
            }
         }
         #endif

         return global;
      }
   }

   //==========================================================================

   /// <summary>
   /// Gets the COREInterface.
   /// </summary>
   public static IInterface COREInterface
   {
      get { return MaxInterfaces.Global.COREInterface; }
   }

   //==========================================================================

   public static ProductVersionType MaxVersionType
   {
      get
      {
         int value = MaxInterfaces.COREInterface.ProductVersion;
         return (ProductVersionType)Enum.ToObject(typeof(ProductVersionType), value);
      }
   }

   //==========================================================================

   /// <summary>
   /// Gets the NodeLayerProperties interface
   /// </summary>
   public static IInterface_ID NodeLayerProperties
   {
      get
      {
         if (nodeLayerProperties == null)
            nodeLayerProperties = MaxInterfaces.Global.Interface_ID.Create(0x44e025f8, 0x6b071e44);

         return nodeLayerProperties;
      }
   }

   //==========================================================================

   /// <summary>
   /// Gets the Function-Published layer manager.
   /// </summary>
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

   //==========================================================================

   /// <summary>
   /// Gets the layer manager.
   /// </summary>
   public static IILayerManager IILayerManager
   {
      get
      {
         if (iILayerManager == null)
            iILayerManager = MaxInterfaces.Global.COREInterface13.LayerManager;

         return iILayerManager;
      }
   }

   //==========================================================================

   /// <summary>
   /// Gets the instance manager.
   /// </summary>
   public static IIInstanceMgr InstanceMgr
   {
      get
      {
         return MaxInterfaces.Global.IInstanceMgr.InstanceMgr;
      }
   }

   //==========================================================================

   /// <summary>
   /// Gets the selection-set manager.
   /// </summary>
   public static IINamedSelectionSetManager SelectionSetManager
   {
      get
      {
         return MaxInterfaces.Global.INamedSelectionSetManager.Instance;
      }
   }

   //==========================================================================

   /// <summary>
   /// Gets the container manager.
   /// </summary>
   public static IIContainerManager ContainerManager
   {
      get
      {
         if (containerManager == null)
            containerManager = MaxInterfaces.Global.ContainerManagerInterface;

         return containerManager;
      }
   }

   //==========================================================================

   /// <summary>
   /// Gets an interval with an infinite duration.
   /// </summary>
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

   //==========================================================================

   /// <summary>
   /// Gets the 3dsMax main window handle.
   /// </summary>
   public static System.Windows.Forms.IWin32Window MaxHwnd
   {
      get
      {
         if (maxHwnd == null)
            maxHwnd = new WindowWrapper(MaxInterfaces.COREInterface.MAXHWnd);

         return maxHwnd;
      }
   }

   //==========================================================================

   /// <summary>
   /// Gets the Material Editor handle.
   /// </summary>
   public static IntPtr MtlEditorHwnd
   {
      get
      {
         IntPtr mtlPtr = IntPtr.Zero;
         NativeMethods.EnumWindows(
             (IntPtr hwnd, IntPtr lparam) => {
                if (HwndIsMtlEditor(hwnd))
                {
                   mtlPtr = hwnd;
                   return false;
                }
                return true;
             }, IntPtr.Zero);
         return mtlPtr;
      }
   }

   //==========================================================================

   /// <summary>
   /// Gets the Slate Material Editor handle.
   /// </summary>
   public static IntPtr SlateMtlEditorHwnd
   {
      get
      {
         IntPtr mtlPtr = IntPtr.Zero;
         NativeMethods.EnumWindows(
             (IntPtr hwnd, IntPtr lparam) => {
                if (HwndIsSlateMtlEditor(hwnd))
                {
                   mtlPtr = hwnd;
                   return false;
                }
                return true;
             }, IntPtr.Zero);
         return mtlPtr;
      }
   }

   private static String getHwndTitle(IntPtr hwnd)
   {
      int textLength = NativeMethods.GetWindowTextLength(hwnd);
      StringBuilder windowText = new StringBuilder(textLength + 1);
      if (NativeMethods.GetWindowText(hwnd, windowText, windowText.Capacity) > 0)
         return windowText.ToString();
      else
         return String.Empty;
   }

   private static bool HwndIsMtlEditor(IntPtr hwnd)
   {
      //TODO: verify if this works in non-English 3dsmax versions
      return getHwndTitle(hwnd).StartsWith("Material Editor", StringComparison.Ordinal);
   }

   private static bool HwndIsSlateMtlEditor(IntPtr hwnd)
   {
      return getHwndTitle(hwnd) == "Slate Material Editor";
   }

   //==========================================================================

   /// <summary>
   /// Gets the color manager.
   /// </summary>
   public static IIColorManager ColorManager
   {
      get { return MaxInterfaces.Global.ColorManager; }
   }

   //==========================================================================

   /// <summary>
   /// Indicates if the current color theme is 'light'.
   /// </summary>
   public static Boolean ColorThemeLightActive
   {
      get { return ColorManager.AppFrameColorTheme == AppFrameColorTheme.LightTheme; }
   }

   //==========================================================================

   /// <summary>
   /// Indicates if the current color theme is 'dark'.
   /// </summary>
   public static Boolean ColorThemeDarkActive
   {
      get { return ColorManager.AppFrameColorTheme == AppFrameColorTheme.DarkTheme; }
   }

   //==========================================================================
}
}
