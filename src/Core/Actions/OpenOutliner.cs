using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.Integration;
using Outliner.Configuration;
using Outliner.Controls;
using Outliner.Controls.Tree;
using Outliner.MaxUtils;
using Outliner.Modes;
using Outliner.Scene;
using UiViewModels.Actions;
using PJanssen;
using WinForms = System.Windows.Forms;

namespace Outliner.Actions
{
public class OpenOutliner : CuiDockableContentAdapter
{
   public override string ActionText
   {
      get { return OutlinerResources.Action_ToggleOutliner; }
   }

   public override string Category
   {
      get { return OutlinerResources.Action_Category; }
   }

   public override string InternalActionText
   {
      get { return "Open Outliner"; }
   }

   public override string InternalCategory
   {
      get { return OutlinerActions.InternalCategory; }
   }


   public override Type ContentType
   {
      get { return typeof(WindowsFormsHost); }
      //get { return typeof(UserControl1); }
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

   protected OutlinerSplitContainer splitContainer;
   protected TreeView tree1;
   protected TreeView tree2;
   protected IEnumerable<IMaxNode> expandedNodes1;
   protected IEnumerable<IMaxNode> expandedNodes2;

   public override object CreateDockableContent()
   {
      //return new UserControl1();

      WindowsFormsHost host = new WindowsFormsHost();
      host.Loaded += new System.Windows.RoutedEventHandler(host_Loaded);
      host.Unloaded += new System.Windows.RoutedEventHandler(host_Unloaded);
      //host.MinWidth = 200;
      //host.MinHeight = 200;
      //host.Width = host.Height = 200;


      OutlinerGUP outlinerInstance = OutlinerGUP.Instance;
      if (!outlinerInstance.SettingsLoaded)
      {
         WinForms::MessageBox.Show(OutlinerResources.Warning_SettingsNotLoaded
                                  , OutlinerResources.Warning_SettingsNotLoaded_Title
                                  , WinForms.MessageBoxButtons.OK
                                  , WinForms.MessageBoxIcon.Error
                                  , WinForms.MessageBoxDefaultButton.Button1
                                  , ControlHelpers.CreateLocalizedMessageBoxOptions());

         host.Child = new Outliner.Controls.Options.OutlinerUserControl();
         host.Child.Width = 150;
         host.Child.Height = 200;

         return host;
      }

      MainControl mainControl = new MainControl();
      this.splitContainer = mainControl.outlinerSplitContainer1;
      this.tree1 = mainControl.TreeView1;
      this.tree2 = mainControl.TreeView2;

      OutlinerState outlinerState = outlinerInstance.State;

      this.tree1.Colors = outlinerInstance.ColorScheme.TreeViewColorScheme;
      this.tree2.Colors = outlinerInstance.ColorScheme.TreeViewColorScheme;

      this.splitContainer.Orientation = outlinerState.SplitterOrientation;
      this.splitContainer.SplitterDistance = outlinerState.SplitterDistance;
      this.splitContainer.Panel1Collapsed = outlinerState.Panel1Collapsed;
      this.splitContainer.Panel2Collapsed = outlinerState.Panel2Collapsed;

      TreeMode mode1 = outlinerInstance.SwitchPreset(tree1, outlinerState.Tree1Preset, false);
      TreeMode mode2 = outlinerInstance.SwitchPreset(tree2, outlinerState.Tree2Preset, false);

      mode1.Filters = outlinerState.Tree1Filters;
      mode2.Filters = outlinerState.Tree2Filters;

      mainControl.NameFilterBindingSource.DataSource = outlinerInstance.CommonNameFilter;

      this.splitContainer.PanelCollapsedChanged += panelCollapsedChanged;

      host.Child = mainControl;

      return host;
   }

   private void StartStopTree(OutlinerGUP outliner, TreeView tree, Boolean start)
   {
      TreeMode mode = outliner.GetActiveTreeMode(tree);
      if (mode != null)
      {
         if (start)
            mode.Start();
         else
            mode.Stop();
      }
   }

   private void host_Loaded(object sender, System.Windows.RoutedEventArgs e)
   {
      if (this.splitContainer == null || this.tree1 == null || this.tree2 == null)
         return;

      OutlinerGUP outliner = OutlinerGUP.Instance;

      this.StartStopTree(outliner, this.tree1, !this.splitContainer.Panel1Collapsed);
      this.StartStopTree(outliner, this.tree2, !this.splitContainer.Panel2Collapsed);

      this.RestoreExpandedNodes(outliner.GetActiveTreeMode(tree1), this.expandedNodes1);
      this.RestoreExpandedNodes(outliner.GetActiveTreeMode(tree2), this.expandedNodes2);
   }

   private void host_Unloaded(object sender, System.Windows.RoutedEventArgs e)
   {
      if (this.tree1 == null || this.tree2 == null)
         return;

      OutlinerGUP outliner = OutlinerGUP.Instance;

      this.expandedNodes1 = this.GetExpandedNodes(this.tree1.Root);
      this.expandedNodes2 = this.GetExpandedNodes(this.tree2.Root);

      this.StartStopTree(outliner, this.tree1, false);
      this.StartStopTree(outliner, this.tree2, false);

      this.UpdateState();
      outliner.StoreSettings();
   }

   private void panelCollapsedChanged(object sender, SplitPanelEventArgs args)
   {
      OutlinerGUP outlinerInstance = OutlinerGUP.Instance;
      TreeView tree = args.Panel.Controls[0] as TreeView;
      if (args.Panel == this.splitContainer.Panel1)
         outlinerInstance.SwitchPreset(tree, outlinerInstance.State.Tree1Preset, !args.IsCollapsed);
      else
         outlinerInstance.SwitchPreset(tree, outlinerInstance.State.Tree2Preset, !args.IsCollapsed);
   }

   private IEnumerable<IMaxNode> GetExpandedNodes(TreeNode tn)
   {
      List<IMaxNode> nodes = new List<IMaxNode>();

      if (tn.IsExpanded)
      {
         IMaxNode node = TreeMode.GetMaxNode(tn);
         if (node != null)
            nodes.Add(node);
      }

      foreach (TreeNode childTn in tn.Nodes)
      {
         nodes.AddRange(this.GetExpandedNodes(childTn));
      }

      return nodes;
   }

   private void RestoreExpandedNodes(TreeMode mode, IEnumerable<IMaxNode> nodes)
   {
      if (mode == null || nodes == null)
         return;

      mode.Tree.BeginUpdate();

      foreach (IMaxNode node in nodes)
      {
         IEnumerable<TreeNode> tns = mode.GetTreeNodes(node);
         if (tns != null)
         {
            tns.ForEach(tn => tn.IsExpanded = true);
         }
      }

      mode.Tree.EndUpdate();
   }

   private void UpdateState()
   {
      if (this.splitContainer == null || this.tree1 == null || this.tree2 == null)
         return;

      OutlinerGUP outliner = OutlinerGUP.Instance;
      OutlinerState state  = outliner.State;

      state.Panel1Collapsed     = this.splitContainer.Panel1Collapsed;
      state.Panel2Collapsed     = this.splitContainer.Panel2Collapsed;
      state.SplitterOrientation = this.splitContainer.Orientation;
      state.SplitterDistance    = this.splitContainer.SplitterDistance;

      OutlinerPreset preset1 = outliner.GetActivePreset(this.tree1);
      if (preset1 != null)
         state.Tree1Preset = preset1;

      OutlinerPreset preset2 = outliner.GetActivePreset(this.tree2);
      if (preset2 != null)
         state.Tree2Preset = preset2;

      state.Tree1Filters = outliner.GetActiveTreeMode(this.tree1).Filters;
      state.Tree2Filters = outliner.GetActiveTreeMode(this.tree2).Filters;
   }
}
}
