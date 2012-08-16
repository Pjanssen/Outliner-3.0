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
   public class OpenLayerOutlinerAction : CuiDockableContentAdapter
   {
      public override string ActionText
      {
         get { return "Open LayerMode"; }
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

      public override object CreateDockableContent()
      {
         //return OutlinerGUP.Instance.GetContainer();
         
         Outliner.Controls.TestControl tc = new Controls.TestControl();

         tc.treeView1.TreeNodeLayout = TreeNodeLayout.MayaLayout; //OutlinerGUP.Instance.Layout;
         tc.treeView1.Colors = TreeViewColorScheme.MayaColors; //OutlinerGUP.Instance.ColorScheme;

         tc.treeView1.NodeSorter = new Outliner.NodeSorters.AlphabeticalSorter();
         TreeMode tm = new
            //HierarchyMode(tc.treeView1);
            //SelectionSetMode(tc.treeView1);
            LayerMode(tc.treeView1);
            //FlatObjectListMode(tc.treeView1);
         
         //tm.Filters.Add(new Filters.ColorTagsFilter(ColorTag.Green | ColorTag.Blue));
         //tm.Filters.Enabled = true;
         tm.FillTree();
         
         return tc;
      }

      public override string WindowTitle
      {
         get { return "Outliner - Layer Mode"; }
      }

      public override DockStates.Dock DockingModes
      {
         get
         {
            return DockStates.Dock.Left | DockStates.Dock.Right 
                   | DockStates.Dock.Floating | DockStates.Dock.Viewport;
         }
      }

      public override bool DestroyOnClose
      {
         get { return true; }
         set { }
      }

      public override bool IsMainContent
      {
         get { return false; }
      }
   }

}
