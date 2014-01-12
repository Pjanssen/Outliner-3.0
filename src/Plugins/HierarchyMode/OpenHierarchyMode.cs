using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Actions;
using PJanssen.Outliner.Controls;
using PJanssen.Outliner.Controls.Tree.Layout;
using PJanssen.Outliner.Controls.Tree;

namespace PJanssen.Outliner.Modes.Hierarchy
{
   //public class OpenHierarchyModeAction : OpenOutlinerAction
   //{
   //   public override string ActionText
   //   {
   //      get { return "Outliner Hierarchy"; }
   //   }

   //   public override string WindowTitle
   //   {
   //      get { return "Outliner Hierarchy"; }
   //   }

   //   protected override TreeMode CreateTreeController1(TestControl mainControl)
   //   {
   //      mainControl.treeView1.TreeNodeLayout = new TreeNodeLayout(OutlinerGUP.Instance.Layout);
   //      mainControl.treeView1.Colors = TreeViewColorScheme.MayaColors; //OutlinerGUP.Instance.ColorScheme;

   //      mainControl.treeView1.NodeSorter = new Outliner.NodeSorters.AlphabeticalSorter();
   //      return new HierarchyMode(mainControl.treeView1);
   //   }

   //   protected override TreeMode CreateTreeController2(TestControl mainControl)
   //   {
   //      mainControl.treeView2.TreeNodeLayout = new TreeNodeLayout(OutlinerGUP.Instance.Layout);
   //      mainControl.treeView2.Colors = TreeViewColorScheme.MayaColors;

   //      mainControl.treeView2.NodeSorter = new Outliner.NodeSorters.AlphabeticalSorter();
   //      return new HierarchyMode(mainControl.treeView2);
   //   }

   //   protected override System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
   //   {
   //      return null;
   //      //return Assembly.LoadFile(Path.Combine(OutlinerPlugins.PluginDirectory
   //      //                                     , "HierarchyMode.dll"));
   //   }
   //}
}
