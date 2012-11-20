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
public partial class FilterCollectionEditor : OutlinerUserControl
{
   private FilterCombinator<IMaxNodeWrapper> rootFilter;
   private Dictionary<Filter<IMaxNodeWrapper>, Tree.TreeNode> treeNodes;

   public FilterCollectionEditor()
   {
      InitializeComponent();

      this.treeNodes = new Dictionary<Filter<IMaxNodeWrapper>, Tree.TreeNode>();

      this.filtersTree.TreeNodeLayout = new TreeNodeLayout();
      this.filtersTree.TreeNodeLayout.LayoutItems.Add(new TreeNodeIndent() { UseVisualStyles = false });
      this.filtersTree.TreeNodeLayout.LayoutItems.Add(new TreeNodeText());
      this.filtersTree.TreeNodeLayout.LayoutItems.Add(new EmptySpace());
      this.filtersTree.TreeNodeLayout.FullRowSelect = true;
   }

   private void FillFilterComboBox()
   {
      IEnumerable<OutlinerPluginData> filters = OutlinerPlugins.GetPlugins(OutlinerPluginType.Filter);
      foreach (OutlinerPluginData filter in filters)
      {
         this.filtersComboBox.Items.Add(filter);
      }
      this.filtersComboBox.DisplayMember = "DisplayName";
      this.filtersComboBox.SelectedIndex = 0;
   }

   [Browsable(false)]
   [DefaultValue(null)]
   public FilterCombinator<IMaxNodeWrapper> RootFilter
   {
      get { return this.rootFilter; }
      set
      {
         this.rootFilter = value;
         if (value != null)
         {
            this.enabledCheckBox.Checked = this.rootFilter.Enabled;
            this.FillFilterComboBox();
            this.FillTree();
         }
      }
   }

   public Action UpdateAction { get; set; }

   private void FillTree()
   {
      this.treeNodes.Clear();
      this.filtersTree.Nodes.Clear();
      this.AddFilterToTree(this.rootFilter, this.filtersTree.Nodes);
      this.filtersTree.Root.ExpandAll();
   }

   private void AddFilterToTree(Filter<IMaxNodeWrapper> filter, Tree.TreeNodeCollection parentCollection)
   {
      filter.FilterChanged += filterChanged;

      Tree.TreeNode tn = new Tree.TreeNode();
      tn.Text = GetTreeNodeText(filter, tn);
      tn.Tag = filter;

      this.treeNodes.Add(filter, tn);

      FilterCombinator<IMaxNodeWrapper> combinator = filter as FilterCombinator<IMaxNodeWrapper>;
      if (combinator != null)
      {
         foreach (Filter<IMaxNodeWrapper> child in combinator.Filters)
         {
            this.AddFilterToTree(child, tn.Nodes);
         }
         combinator.Filters.FilterAdded += this.filterAdded;
         combinator.Filters.FilterRemoved += this.filterRemoved;
         tn.DragDropHandler = new FilterCombinatorDragDropHandler(combinator);
      }
      else
         tn.DragDropHandler = new FilterDragDropHandler(filter);

      parentCollection.Add(tn);
   }

   private String GetTreeNodeText(Filter<IMaxNodeWrapper> filter, Tree.TreeNode tn)
   {
      FilterCombinator<IMaxNodeWrapper> combinator = filter as FilterCombinator<IMaxNodeWrapper>;
      if (combinator != null)
      {
         return String.Format( "Combinator: {0}"
                             , Functor.PredicateToString<Boolean>(combinator.Predicate).ToUpper());
      }
      else if (filter is NodePropertyFilter)
      {
         return String.Format( "Property: {0}{1}"
                             , (filter.Invert ? "not " : "")
                             , (filter as NodePropertyFilter).Property.ToString());
      }
      else
      {
         OutlinerPluginData filterPluginData = OutlinerPlugins.GetPlugin(filter.GetType());
         if (filterPluginData != null)
            return filterPluginData.DisplayName;
         else
            return filter.GetType().Name;
      }
   }


   private void enabledCheckBox_CheckedChanged(object sender, EventArgs e)
   {
      this.rootFilter.Enabled = this.enabledCheckBox.Checked;
      if (this.UpdateAction != null)
         this.UpdateAction();
   }

   private void addFilterButton_Click(object sender, EventArgs e)
   {
      OutlinerPluginData selPlugin = this.filtersComboBox.SelectedItem as OutlinerPluginData;

      FilterCombinator<IMaxNodeWrapper> combinator = null;
      Tree.TreeNode selCombinatorTn = this.filtersTree.SelectedNodes.FirstOrDefault();
      while (selCombinatorTn != null && combinator == null)
      {
         combinator = selCombinatorTn.Tag as FilterCombinator<IMaxNodeWrapper>;
         selCombinatorTn = selCombinatorTn.Parent;
      }

      if (combinator == null)
         combinator = this.rootFilter;

      if (selPlugin != null)
      {
         Filter<IMaxNodeWrapper> filter = Activator.CreateInstance(selPlugin.Type, null) as Filter<IMaxNodeWrapper>;
         filter.FilterChanged += filterChanged;
         combinator.Filters.Add(filter);
      }
   }

   private void deleteFilterButton_Click(object sender, EventArgs e)
   {
      this.filtersTree.BeginUpdate();

      foreach (Tree.TreeNode tn in this.filtersTree.SelectedNodes)
      {
         Filter<IMaxNodeWrapper> filter = tn.Tag as Filter<IMaxNodeWrapper>;
         if (filter != null && tn.Parent != null)
         {
            FilterCombinator<IMaxNodeWrapper> combinator = tn.Parent.Tag as FilterCombinator<IMaxNodeWrapper>;
            if (combinator != null)
               combinator.Filters.Remove(filter);
         }
      }

      this.filtersTree.EndUpdate();
   }

   private void filtersTree_SelectionChanged(object sender, Tree.SelectionChangedEventArgs e)
   {
      this.filterPropertyGrid.SelectedObjects = e.Nodes.Select(tn => tn.Tag).ToArray();
   }

   private void filterChanged(object sender, EventArgs args)
   {
      if (this.UpdateAction != null)
         this.UpdateAction();

      Filter<IMaxNodeWrapper> filter = sender as Filter<IMaxNodeWrapper>;
      Tree.TreeNode tn;
      if (this.treeNodes.TryGetValue(filter, out tn))
         tn.Text = GetTreeNodeText(filter, tn);
   }

   private void filterAdded(object sender, FilterChangedEventArgs<IMaxNodeWrapper> args)
   {
      FilterCollection<IMaxNodeWrapper> collection = sender as FilterCollection<IMaxNodeWrapper>;
      if (collection != null)
      {
         Tree.TreeNode parentTn;
         if (this.treeNodes.TryGetValue(collection.Owner, out parentTn))
         {
            this.AddFilterToTree(args.Filter, parentTn.Nodes);
            if (this.UpdateAction != null)
               this.UpdateAction();
         }
      }
   }

   private void filterRemoved(object sender, FilterChangedEventArgs<IMaxNodeWrapper> args)
   {
      Tree.TreeNode tn;
      if (this.treeNodes.TryGetValue(args.Filter, out tn))
      {
         tn.Remove();
         this.treeNodes.Remove(args.Filter);
         if (this.UpdateAction != null)
            this.UpdateAction();
      }
   }
}
}
