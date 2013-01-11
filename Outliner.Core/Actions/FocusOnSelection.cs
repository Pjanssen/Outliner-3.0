using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Controls.Tree;
using UiViewModels.Actions;

namespace Outliner.Actions
{
   public class FocusOnSelection : CuiActionCommandAdapter
   {
      public override string ActionText
      {
         get { return OutlinerResources.Action_FocusOnSelection; }
      }

      public override string Category
      {
         get { return OutlinerResources.Action_Category; }
      }

      public override string InternalActionText
      {
         get { return ActionText; }
      }

      public override string InternalCategory
      {
         get { return OutlinerActions.InternalCategory; }
      }

      public override void Execute(object parameter)
      {
         OutlinerGUP outliner = OutlinerGUP.Instance;
         foreach (TreeView tree in outliner.TreeViews)
         {
            IEnumerable<TreeNode> selectedNodes = tree.SelectedNodes;
            foreach (TreeNode tn in selectedNodes)
            {
               tree.AutoExpandParents(tn);
            }
            if (selectedNodes.Count() > 0 && !tree.IsSelectionInView())
               tree.ScrollTreeNodeIntoView(selectedNodes.First());
         }
      }

   }
}
