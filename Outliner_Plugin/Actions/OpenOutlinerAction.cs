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

namespace Outliner.Actions
{
   public class OpenOutlinerAction : CuiDockableContentAdapter
   {
      public override string ActionText
      {
         get { return "Toggle Outliner"; }
      }

      public override string Category
      {
         get { return "Outliner Plugin"; }
      }

      public override Type ContentType
      {
         get { return typeof(Outliner.Controls.TestControl); }
      }

      public override object CreateDockableContent()
      {
         Outliner.Controls.TestControl tc = new Controls.TestControl();

         tc.treeView1.TreeNodeLayout = OutlinerGUP.Instance.Layout;
         tc.treeView1.Colors = OutlinerGUP.Instance.ColorScheme;

         tc.treeView1.NodeSorter = new Outliner.NodeSorters.AlphabeticalSorter();
         TreeMode tm = new
            //HierarchyMode(tc.treeView1, MaxInterfaces.Global.COREInterface);
            SelectionSetMode(tc.treeView1, MaxInterfaces.Global.COREInterface);
            //LayerMode(tc.treeView1, MaxInterfaces.Global.COREInterface);
            //FlatObjectListMode(tc.treeView1, MaxInterfaces.Global.COREInterface);
         
         //tm.Filters.Add(new Filters.HelperFilter());
         //tm.Filters.Enabled = true;
         tm.FillTree();
         
         return tc;
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
