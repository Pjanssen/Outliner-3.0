using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls;
using MaxUtils;
using Autodesk.Max;
using UiViewModels.Actions;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Outliner.Actions
{
public class TestAction : UiViewModels.Actions.CuiActionCommandAdapter
{
   public override string ActionText
   {
      get { return "Test"; }
   }

   public override string Category
   {
      get { return "Outliner Plugin"; }
   }

   [DllImport("user32.dll")]
   static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

   [DllImport("user32.dll")]
   static extern IntPtr CreateWindow(string lpClassName, string lpWindowName, uint dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

   [DllImport("user32.dll")]
   static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

   public override void Execute(object parameter)
   {
      IntPtr hwnd = MaxInterfaces.Global.CreateCUIFrameWindow(MaxInterfaces.COREInterface.MAXHWnd, "Test", 50, 50, 100, 100);
      IICUIFrame frame = MaxInterfaces.Global.CUIFrameMgr.GetICUIFrame("Test"); //hwnd.ToInt32());
      frame.ContentType = 1;
      frame.PosType = (uint)(DockStates.Dock.Floating | DockStates.Dock.Left | DockStates.Dock.Right);

      TestForm f = new TestForm();
      //f.Show();
      //SetParent(f.Handle, hwnd);
      
      IICustToolbar tb = MaxInterfaces.Global.GetICustToolbar(f.Handle);
      tb.LinkToCUIFrame(hwnd, null);
      tb.SetBottomBorder(false);
      tb.SetTopBorder(false);

      System.Drawing.Size sz = new System.Drawing.Size();
      tb.GetFloatingCUIFrameSize(sz, 0);
      System.Drawing.Rectangle rect = new System.Drawing.Rectangle(200, 200, sz.Width, sz.Height);
      MaxInterfaces.Global.CUIFrameMgr.FloatCUIWindow(hwnd, rect, 0);

      MaxInterfaces.Global.ReleaseICustToolbar(tb);
      MaxInterfaces.Global.ReleaseICUIFrame(frame);
   }

   public override string InternalActionText
   {
      get { return ActionText; }
   }

   public override string InternalCategory
   {
      get { return Category; }
   }
}

public class ParentWndWrapper : IWin32Window
{
   IntPtr m_Handle;
   public ParentWndWrapper(IntPtr pParent)
   {
      m_Handle = (IntPtr)pParent;
   }

   #region IWin32Window Members
   public IntPtr Handle
   {
      get { return m_Handle; }
   }
   #endregion
}
}
