using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiViewModels.Actions;
using Outliner.Controls;
using Outliner.Controls.Tree;
using Outliner.Modes;

namespace Outliner.Actions
{
   public abstract class OpenOutlinerAction : CuiDockableContentAdapter
   {
      private const String OutlinerCategory = "Outliner Plugin";

      public override string Category
      {
         get { return OutlinerCategory; }
      }

      public override Type ContentType
      {
         get { return typeof(Outliner.Controls.TestControl); }
      }

      public override string WindowTitle
      {
         get { return OutlinerResources.Outliner_WindowTitle; }
      }

      public override DockStates.Dock DockingModes
      {
         get
         {
            return DockStates.Dock.Left 
                   | DockStates.Dock.Right
                   | DockStates.Dock.Floating 
                   | DockStates.Dock.Viewport;
         }
      }

      protected TreeMode treeController1;
      protected TreeMode treeController2;

      public override object CreateDockableContent()
      {
         AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

         Outliner.Controls.TestControl tc = new Controls.TestControl();
         tc.outlinerSplitContainer1.PanelCollapsedChanged += panelCollapsedChanged;

         this.treeController1 = CreateTreeController1(tc);
         this.treeController2 = CreateTreeController2(tc);

         if (!tc.outlinerSplitContainer1.Panel1Collapsed)
            this.treeController1.Start();
         if (!tc.outlinerSplitContainer1.Panel2Collapsed)
            this.treeController2.Start();

         AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve);

         return tc;
      }

      protected abstract System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args);

      void panelCollapsedChanged(object sender, SplitPanelEventArgs args)
      {
         TreeView tree = args.Panel.Controls[0] as TreeView;
         TreeMode tm;
         if (args.Panel == (sender as OutlinerSplitContainer).Panel1)
            tm = this.treeController1;
         else
            tm = this.treeController2;

         if (args.IsCollapsed)
            tm.Stop();
         else
            tm.Start();
      }

      protected abstract TreeMode CreateTreeController1(Outliner.Controls.TestControl mainControl);
      protected abstract TreeMode CreateTreeController2(Outliner.Controls.TestControl mainControl);
   }
}
