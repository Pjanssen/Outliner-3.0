using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max.Plugins;
using UiViewModels.Actions;
using Autodesk.Max;
using Outliner.Modes;
using Outliner.Controls;
using Autodesk.Max.MaxSDK.Util;
using Outliner.Controls.Tree.Layout;
using Outliner.Controls.Tree;
using MaxUtils;
using Outliner.NodeSorters;
using Outliner.Modes.SelectionSet;
using Outliner.Modes.Hierarchy;
using Outliner.Modes.Layer;
using Outliner.LayerTools;
using Outliner.Modes.FlatList;

namespace Outliner.Actions
{
   public class OpenOutlinerAction : CuiDockableContentAdapter
   {
      public override string ActionText
      {
         get { return OutlinerResources.Action_ToggleOutliner; }
      }

      public override string Category
      {
         get { return "Outliner Plugin"; }
      }

      public override Type ContentType
      {
         get
         {
            return typeof(Outliner.Controls.TestControl); 
            //return typeof(Control);
         }
      }

      private TreeMode treeController1;
      private TreeMode treeController2;

      public override object CreateDockableContent()
      {
         Outliner.Controls.TestControl tc = new Controls.TestControl();
         tc.outlinerSplitContainer1.PanelCollapsedChanged += outlinerSplitContainer1_PanelCollapsedChanged;
         
         tc.treeView1.TreeNodeLayout = TreeNodeLayout.MayaLayout; //OutlinerGUP.Instance.Layout;
         tc.treeView1.Colors = TreeViewColorScheme.MayaColors; //OutlinerGUP.Instance.ColorScheme;

         tc.treeView1.NodeSorter = new Outliner.NodeSorters.AlphabeticalSorter();
         this.treeController1 = new HierarchyMode(tc.treeView1);

         //this.treeController1.Filters.Add(new Filters.ColorTagsFilter(ColorTag.Green | ColorTag.Blue));
         //this.treeController1.Filters.Enabled = true;
         if (!tc.outlinerSplitContainer1.Panel1Collapsed)
            this.treeController1.Start();

         tc.treeView2.NodeSorter = new Outliner.NodeSorters.AlphabeticalSorter();
         tc.treeView2.TreeNodeLayout = TreeNodeLayout.MayaLayout;
         tc.treeView2.Colors = TreeViewColorScheme.MayaColors;
         this.treeController2 = new HierarchyMode(tc.treeView2);

         //if (!tc.outlinerSplitContainer1.Panel2Collapsed)
         //   this.treeController2.Start();
         
         return tc;
      }

      void outlinerSplitContainer1_PanelCollapsedChanged(object sender, SplitPanelEventArgs args)
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

      public override string WindowTitle
      {
         get { return "Outliner"; }
      }

      public override DockStates.Dock DockingModes
      {
         get
         {
            return DockStates.Dock.Left | DockStates.Dock.Right 
                   | DockStates.Dock.Floating | DockStates.Dock.Viewport;
         }
      }
   }

}
