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
using MaxUtils;

namespace Outliner.Controls.Options
{
public partial class FilterCollectionEditor : UserControl
{
   private PresetEditor owningEditor;
   private FilterCollection<IMaxNodeWrapper> filterCollection;

   public FilterCollectionEditor()
   {
      InitializeComponent();
   }

   public FilterCollectionEditor( PresetEditor owningEditor
                                , FilterCollection<IMaxNodeWrapper> collection) : this()
   {
      this.owningEditor = owningEditor;
      this.filterCollection = collection;

      this.enabledCheckBox.Checked = this.filterCollection.Enabled;
      this.combinatorOR.Checked = this.filterCollection.Combinator == Functor.Or;
      this.combinatorAND.Checked = this.filterCollection.Combinator == Functor.And;

      this.FillFilterComboBox();
      this.FillTree();
      this.SetEnabledStates();
   }

   private void FillFilterComboBox()
   {
      IEnumerable<OutlinerPluginData> filters = OutlinerPlugins.GetPluginsByType(OutlinerPluginType.Filter);
      foreach (OutlinerPluginData filter in filters)
      {
         this.filtersComboBox.Items.Add(filter.DisplayName);
      }
   }

   private void FillTree()
   {
      //this.filterEnabledCheckBox.Checked = editingPreset.Filters.Enabled;
      this.filtersTree.Nodes.Clear();
      foreach (Filter<IMaxNodeWrapper> item in this.filterCollection)
      {
         this.filtersTree.Nodes.Add(new Tree.TreeNode(item.GetType().Name));
      }
   }

   private void SetEnabledStates()
   {
      Boolean enabled = this.filterCollection.Enabled;
      this.combinatorLabel.Enabled    = enabled;
      this.combinatorOR.Enabled       = enabled;
      this.combinatorAND.Enabled      = enabled;
      this.filtersComboBox.Enabled    = enabled;
      this.filtersTree.Enabled        = enabled;
      this.addFilterButton.Enabled    = enabled;
      this.deleteFilterButton.Enabled = enabled;
      this.filterPropertyGrid.Enabled = enabled;
   }

   private void enabledCheckBox_CheckedChanged(object sender, EventArgs e)
   {
      this.filterCollection.Enabled = this.enabledCheckBox.Checked;
      this.SetEnabledStates();
      this.owningEditor.UpdatePreviewTree();
   }
}
}
