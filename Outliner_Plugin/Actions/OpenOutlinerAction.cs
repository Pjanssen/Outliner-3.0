using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max.Plugins;
using UiViewModels.Actions;
using Autodesk.Max;
using Outliner.TreeModes;
using Outliner.Controls;
using Autodesk.Max.MaxSDK.Util;
using Outliner.Controls.Layout;

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
         get { return "Outliner"; }
      }

      public override Type ContentType
      {
         get { return typeof(Outliner.Controls.TestControl); }
      }

      public override object CreateDockableContent()
      {
         Outliner.Controls.TestControl tc = new Controls.TestControl();
         
         IIPathConfigMgr pathMgr = Autodesk.Max.GlobalInterface.Instance.IPathConfigMgr.PathConfigMgr;
         IGlobal.IGlobalMaxSDK.IGlobalUtil.IGlobalPath path = GlobalInterface.Instance.MaxSDK.Util.Path;
         IPath layoutFile = path.Create(pathMgr.GetDir(MaxDirectory.UserScripts));
         layoutFile.Append(path.Create("outliner_layout.xml"));
         tc.treeView1.TnLayout = TreeNodeLayout.FromXml(layoutFile.String);

         tc.treeView1.NodeSorter = new Outliner.NodeSorters.AlphabeticalSorter();
         TreeMode tm = new HierarchyMode(tc.treeView1, Autodesk.Max.GlobalInterface.Instance.COREInterface);
         //SelectionSetMode(tc.treeView1, Autodesk.Max.GlobalInterface.Instance.COREInterface);
         //LayerMode(tc.treeView1, Autodesk.Max.GlobalInterface.Instance.COREInterface);
         //HierarchyMode(tc.treeView1, Autodesk.Max.GlobalInterface.Instance.COREInterface);
         //FlatObjectListMode(tc.treeView1, Autodesk.Max.GlobalInterface.Instance.COREInterface);
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
            return DockStates.Dock.Left | DockStates.Dock.Right | DockStates.Dock.Floating | DockStates.Dock.Viewport;
         }
      }

      public override bool NeedsKeyboardFocus
      {
         get { return true; }
      }

      public override bool DestroyOnClose
      {
         get { return true; }
         set { }
      }

      public override bool IsMainContent
      {
         get { return true; }
      }

   }

}
