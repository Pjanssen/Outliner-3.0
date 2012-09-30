using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UiViewModels.Actions;
using Outliner.Modes;
using Outliner.Controls;
using Outliner.Controls.Tree;
using System.Windows.Forms.Integration;
using Outliner.Presets;

namespace Outliner.Actions
{
public class OpenOutliner : CuiDockableContentAdapter
{
   private const String OutlinerCategory = "Outliner Plugin";

   public override string ActionText
   {
      get { return "Open Outliner"; }
   }

   public override string Category
   {
      get { return OutlinerCategory; }
   }

   public override Type ContentType
   {
      get { return typeof(WindowsFormsHost); }
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

   public override object CreateDockableContent()
   {
      WindowsFormsHost host = new WindowsFormsHost();
      host.Loaded += new System.Windows.RoutedEventHandler(host_Loaded);
      host.Unloaded += new System.Windows.RoutedEventHandler(host_Unloaded);

      TestControl mainControl = new TestControl();
      this.splitContainer = mainControl.outlinerSplitContainer1;
      this.tree1 = mainControl.treeView1;
      this.tree2 = mainControl.treeView2;

      OutlinerGUP outlinerInstance = OutlinerGUP.Instance;
      OutlinerState outlinerState = outlinerInstance.State;

      this.tree1.Colors = OutlinerGUP.Instance.ColorScheme;
      this.tree2.Colors = OutlinerGUP.Instance.ColorScheme;

      this.splitContainer.Orientation = outlinerState.SplitterOrientation;
      this.splitContainer.SplitterDistance = outlinerState.SplitterDistance;
      this.splitContainer.Panel1Collapsed = outlinerState.Panel1Collapsed;
      this.splitContainer.Panel2Collapsed = outlinerState.Panel2Collapsed;

      outlinerInstance.SwitchPreset(tree1, outlinerState.Tree1Preset, false);
      outlinerInstance.SwitchPreset(tree2, outlinerState.Tree2Preset, false);

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
   }

   private void host_Unloaded(object sender, System.Windows.RoutedEventArgs e)
   {
      if (this.tree1 == null || this.tree2 == null)
         return;

      OutlinerGUP outliner = OutlinerGUP.Instance;

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
         outlinerInstance.SwitchPreset(tree, outlinerInstance.State.Tree1Preset, true);
      else
         outlinerInstance.SwitchPreset(tree, outlinerInstance.State.Tree2Preset, true);
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
   }
}
}
