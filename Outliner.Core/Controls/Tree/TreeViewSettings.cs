using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Outliner.Controls.Tree
{
   /// <summary>
   /// Defines the configuration for a TreeView object.
   /// </summary>
   public class TreeViewSettings
   {
      /// <summary>
      /// Gets or sets whether multiple TreeNodes can be selected.
      /// </summary>
      public Boolean MultiSelect { get; set; }

      /// <summary>
      /// Gets or sets the action to be executed when a TreeNode is double-clicked.
      /// </summary>
      public TreeNodeDoubleClickAction DoubleClickAction { get; set; }

      /// <summary>
      /// Gets or sets the MouseButton to be used to initiate a drag &amp; drop operation.
      /// </summary>
      public MouseButtons DragDropMouseButton { get; set; }

      /// <summary>
      /// Gets or sets whether the TreeView should scroll to the selected TreeNode.
      /// </summary>
      public Boolean ScrollToSelection { get; set; }

      /// <summary>
      /// Gets or sets whether the parents of selected TreeNodes should be expanded.
      /// </summary>
      public Boolean AutoExpandSelectionParents { get; set; }

      /// <summary>
      /// Gets or sets whether parents of selected TreeNodes expanded by the TreeView
      /// should be collapsed when the selection changes.
      /// </summary>
      public Boolean CollapseAutoExpandedParents { get; set; }

      /// <summary>
      /// Initializes a new TreeViewSettings instance.
      /// </summary>
      public TreeViewSettings()
      {
         this.MultiSelect = true;
         this.DoubleClickAction = TreeNodeDoubleClickAction.Rename;
         this.DragDropMouseButton = MouseButtons.Left;

         this.ScrollToSelection = true;
         this.AutoExpandSelectionParents = true;
         this.CollapseAutoExpandedParents = true;
      }
   }
}
