﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PJanssen.Outliner.Controls.Tree.Layout
{
/// <summary>
/// Provides a baseclass for all node buttons. It provides a Tooltip and changes 
/// the cursor when the mouse moves over it.
/// </summary>
public abstract class TreeNodeButton : TreeNodeLayoutItem, IDisposable
{
   protected const int TOOLTIP_INITIAL_DELAY = 750;
   protected const int TOOLTIP_AUTOPOP_DELAY = 4000;

   private ToolTip tooltip;


   /// <summary>
   /// Returns the text for the tooltip for the given TreeNode.
   /// </summary>
   /// <remarks>If this method returns null, no tooltip will be shown.</remarks>
   protected virtual String GetTooltipText(TreeNode tn)
   {
      return null;
   }

   /// <summary>
   /// Determines whether the TreeNodeButton can be clicked for the given TreeNode.
   /// </summary>
   protected virtual Boolean Clickable(TreeNode tn)
   {
      return true;
   }

   protected IEnumerable<TreeNode> GetContextNodes(TreeNode clickedTn)
   {
      if (ControlHelpers.ControlPressed || !clickedTn.IsSelected)
         return clickedTn.ToIEnumerable();

      return this.Layout.TreeView.SelectedNodes;
   }

   public override void HandleMouseEnter(MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout == null || this.Layout.TreeView == null)
         return;

      String tooltipText = this.GetTooltipText(tn);
      if (this.Clickable(tn))
         this.Layout.TreeView.Cursor = Cursors.Hand;

      if (tooltipText != null)
      {
         this.tooltip = new ToolTip();
         this.tooltip.InitialDelay = TOOLTIP_INITIAL_DELAY;
         this.tooltip.AutoPopDelay = TOOLTIP_AUTOPOP_DELAY;
         this.tooltip.ShowAlways = true;
         this.tooltip.SetToolTip(this.Layout.TreeView, tooltipText);
      }
   }

   public override void HandleMouseLeave(MouseEventArgs e, TreeNode tn)
   {
      if (this.Layout != null && this.Layout.TreeView != null)
      {
         this.Layout.TreeView.Cursor = Cursors.Default;
         if (this.tooltip != null)
         {
            this.tooltip.Dispose();
            this.tooltip = null;
         }
      }
   }

   public void Dispose()
   {
      this.Dispose(true);
      GC.SuppressFinalize(this);
   }

   protected virtual void Dispose(Boolean disposing)
   {
      if (disposing)
      {
         if (this.tooltip != null)
            this.tooltip.Dispose();
      }
   }
}
}
