using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Modes;
using Outliner.Controls;
using Outliner.Controls.Tree.Layout;
using Outliner.Controls.Tree;
using Outliner.Modes.Hierarchy;
using System.Reflection;
using System.IO;
using Outliner.Plugins;

namespace Outliner.Actions
{
   public class OpenHierarchyModeAction : OpenOutlinerBaseAction
   {
      public override string ActionText
      {
         get { return Resources.OpenHierarchy; }
      }

      public override string WindowTitle
      {
         get { return "Outliner - Hierarchy Mode"; }
      }

      protected override TreeMode CreateTreeController1(TestControl mainControl)
      {
         //mainControl.treeView1.TreeNodeLayout = new TreeNodeLayout(OutlinerGUP.Instance.Layout);
         mainControl.treeView1.Colors = TreeViewColorScheme.MayaColors; //OutlinerGUP.Instance.ColorScheme;

         mainControl.treeView1.NodeSorter = new Outliner.NodeSorters.AlphabeticalSorter();
         return new HierarchyMode(mainControl.treeView1);
      }

      protected override TreeMode CreateTreeController2(TestControl mainControl)
      {
         //mainControl.treeView2.TreeNodeLayout = new TreeNodeLayout(OutlinerGUP.Instance.Layout);
         mainControl.treeView2.Colors = TreeViewColorScheme.MayaColors;

         mainControl.treeView2.NodeSorter = new Outliner.NodeSorters.AlphabeticalSorter();
         return new HierarchyMode(mainControl.treeView2);
      }

      protected override System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
      {
         return Assembly.LoadFile(Path.Combine( OutlinerPaths.Plugins
                                              , "HierarchyMode.dll"));
      }
   }
}
