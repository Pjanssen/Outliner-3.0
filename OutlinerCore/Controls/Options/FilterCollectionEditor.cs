using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Filters;
using Outliner.Scene;
using Outliner.Plugins;
using Outliner.MaxUtils;
using Autodesk.Max;
using Outliner.Controls.Tree.Layout;
using Outliner.Presets;

namespace Outliner.Controls.Options
{
public partial class FilterCollectionEditor : UserControl
{
   private PresetEditor owningEditor;
   private OutlinerPreset preset;

   public FilterCollectionEditor()
   {
      InitializeComponent();
   }

   public FilterCollectionEditor( PresetEditor owningEditor
                                , OutlinerPreset preset)
      : this()
   {
      this.owningEditor = owningEditor;
      this.preset = preset;

      Color windowColor = ColorHelpers.FromMaxGuiColor(GuiColors.Window);
      Color windowTextColor = ColorHelpers.FromMaxGuiColor(GuiColors.WindowText);

      this.SetControlColor(this.filtersComboBox, windowColor, windowTextColor);
      this.SetControlColor(this.filtersTree, windowColor, windowTextColor);

      this.filterPropertyGrid.ViewBackColor = windowColor;
      this.filterPropertyGrid.ViewForeColor = windowTextColor;
      this.filterPropertyGrid.LineColor = Color.Gray;

      this.filtersTree.TreeNodeLayout = new TreeNodeLayout();
      this.filtersTree.TreeNodeLayout.LayoutItems.Add(new TreeNodeIndent() { UseVisualStyles = false });
      this.filtersTree.TreeNodeLayout.LayoutItems.Add(new TreeNodeText());
      this.filtersTree.TreeNodeLayout.LayoutItems.Add(new EmptySpace());
      this.filtersTree.TreeNodeLayout.FullRowSelect = true;

      this.enabledCheckBox.Checked = this.preset.Filters.Enabled;

      this.FillFilterComboBox();
      this.FillTree();
      this.SetEnabledStates();
   }

   private void SetControlColor(Control c, Color backColor, Color foreColor)
   {
      c.BackColor = backColor;
      c.ForeColor = foreColor;
   }

   private void FillFilterComboBox()
   {
      IEnumerable<OutlinerPluginData> filters = OutlinerPlugins.GetPluginsByType(OutlinerPluginType.Filter);
      foreach (OutlinerPluginData filter in filters)
      {
         this.filtersComboBox.Items.Add(filter);
      }
      this.filtersComboBox.DisplayMember = "DisplayName";
      this.filtersComboBox.SelectedIndex = 0;
   }

   private void FillTree()
   {
      this.filtersTree.Nodes.Clear();
      this.AddFilterToTree(this.preset.Filters, this.filtersTree.Nodes);
      this.filtersTree.Root.ExpandAll();
   }

   private void AddFilterToTree(Filter<IMaxNodeWrapper> filter, Tree.TreeNodeCollection parentCollection)
   {
      FilterCombinator<IMaxNodeWrapper> combinator = filter as FilterCombinator<IMaxNodeWrapper>;
      String nodeName = String.Empty;
      if (combinator != null)
      {
         nodeName = Functor.PredicateToString<Boolean>(combinator.Predicate);
      }
      else
      {
         OutlinerPluginData filterPluginData = OutlinerPlugins.GetPluginDataByType(filter.GetType());
         if (filterPluginData != null)
            nodeName = filterPluginData.DisplayName;
         else
            nodeName = filter.GetType().Name;
      }

      Tree.TreeNode tn = new Tree.TreeNode(nodeName);
      tn.Tag = filter;

      if (combinator != null)
      {
         foreach (Filter<IMaxNodeWrapper> child in combinator.Filters)
         {
            this.AddFilterToTree(child, tn.Nodes);
         }
      }

      parentCollection.Add(tn);
   }

   private void SetEnabledStates()
   {
      Boolean enabled = this.preset.Filters.Enabled;
      this.filtersComboBox.Enabled    = enabled;
      this.filtersTree.Enabled        = enabled;
      this.addFilterButton.Enabled    = enabled;
      this.deleteFilterButton.Enabled = enabled;
      this.filterPropertyGrid.Enabled = enabled;
   }

   private void enabledCheckBox_CheckedChanged(object sender, EventArgs e)
   {
      this.preset.Filters.Enabled = this.enabledCheckBox.Checked;
      this.SetEnabledStates();
      this.owningEditor.UpdatePreviewTree();
   }

   private void addFilterButton_Click(object sender, EventArgs e)
   {
      OutlinerPluginData selPlugin = this.filtersComboBox.SelectedItem as OutlinerPluginData;
      if (selPlugin != null)
      {
         //this.filterCollection.Add(Activator.CreateInstance(selPlugin.Type, null) as Filter<IMaxNodeWrapper>);
         this.FillTree();
         this.owningEditor.UpdatePreviewTree();
      }
   }

   private void deleteFilterButton_Click(object sender, EventArgs e)
   {
      foreach (Tree.TreeNode tn in this.filtersTree.SelectedNodes)
      {
         Filter<IMaxNodeWrapper> filter = tn.Tag as Filter<IMaxNodeWrapper>;
         //if (filter != null)
         //   this.filterCollection.Remove(filter);
      }
      this.FillTree();
      this.owningEditor.UpdatePreviewTree();
   }

   private void combinatorOR_CheckedChanged(object sender, EventArgs e)
   {
      //this.filterCollection.Predicate = Functor.Or;
      this.owningEditor.UpdatePreviewTree();
   }

   private void combinatorAND_CheckedChanged(object sender, EventArgs e)
   {
      //this.filterCollection.Predicate = Functor.And;
      this.owningEditor.UpdatePreviewTree();
   }

   private void filtersTree_SelectionChanged(object sender, Tree.SelectionChangedEventArgs e)
   {
      this.filterPropertyGrid.SelectedObjects = e.Nodes.Select(tn => tn.Tag).ToArray();
   }
}
}
