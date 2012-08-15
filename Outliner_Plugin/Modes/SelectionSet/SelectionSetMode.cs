using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Controls.Tree;
using Outliner.Filters;
using MaxUtils;
using System.Runtime.InteropServices;

namespace Outliner.Modes.SelectionSet
{
public class SelectionSetMode : TreeMode
{
   protected AllObjectsSelectionSet allObjectsSelSet;

   public SelectionSetMode(TreeView tree) : base(tree)
   {
      this.allObjectsSelSet = new AllObjectsSelectionSet();
      this.tree.DragDropHandler = new TreeViewDragDropHandler();

      this.RegisterNodeEventCallbackObject(new SelectionSetNodeEventCallbacks(this));
      this.RegisterSystemNotifications();

      this.tree.ContextMenu = new System.Windows.Forms.ContextMenu();
      this.tree.ContextMenu.MenuItems.Add("test");
      
   }

   public override void FillTree()
   {
      this.tree.BeginUpdate();

      TreeNode allObjTn = this.AddNode(this.allObjectsSelSet, this.tree.Nodes);
      allObjTn.FontStyle = System.Drawing.FontStyle.Italic;

      IINamedSelectionSetManager selSetMan = MaxInterfaces.SelectionSetManager;
      for (int i = 0; i < selSetMan.NumNamedSelSets; i++)
      {
         this.AddNode(new SelectionSetWrapper(i), this.tree.Nodes);
      }

      this.tree.Sort();
      this.tree.EndUpdate();
   }

   public override TreeNode AddNode(IMaxNodeWrapper wrapper, TreeNodeCollection parentCol)
   {
      TreeNode tn = base.AddNode(wrapper, parentCol);
      tn.DragDropHandler = this.createDragDropHandler(wrapper);

      if (wrapper is SelectionSetWrapper)
      {
         foreach (Object child in wrapper.ChildNodes)
            this.AddNode(child, tn.Nodes);
      }

      return tn;
   }

   private DragDropHandler createDragDropHandler(IMaxNodeWrapper wrapper)
   {
      if (wrapper is SelectionSetWrapper)
         return new SelectionSetDragDropHandler((SelectionSetWrapper)wrapper);
      else if (wrapper is IINodeWrapper)
         return new IINodeDragDropHandler(wrapper);

      return null;
   }



   #region NodeEventCallbacks

   protected class SelectionSetNodeEventCallbacks : TreeModeNodeEventCallbacks
   {
      protected new SelectionSetMode treeMode;
      public SelectionSetNodeEventCallbacks(SelectionSetMode treeMode) 
         : base(treeMode) 
      {
         this.treeMode = treeMode;
      }

      public override void Added(ITab<UIntPtr> nodes)
      {
         TreeNode allObjSelSetTn = this.treeMode.GetFirstTreeNode(this.treeMode.allObjectsSelSet);
         if(allObjSelSetTn == null)
            return;

         foreach (IINode node in IINodeHelpers.NodeKeysToINodeList(nodes))
         {
            this.treeMode.AddNode(node, allObjSelSetTn.Nodes);
            this.tree.AddToSortQueue(allObjSelSetTn.Nodes);
         }
         this.tree.StartTimedSort(true);
      }
   }

   #endregion

   #region System notifications

   private void RegisterSystemNotifications()
   {
      this.RegisterSystemNotification(this.NamedSelSetCreated, SystemNotificationCode.NamedSelSetCreated);
      this.RegisterSystemNotification(this.NamedSelSetDeleted, SystemNotificationCode.NamedSelSetDeleted);
      this.RegisterSystemNotification(this.NamedSelSetRenamed, SystemNotificationCode.NamedSelSetRenamed);
      this.RegisterSystemNotification(this.NamedSelSetPreModify, SystemNotificationCode.NamedSelSetPreModify);
      this.RegisterSystemNotification(this.NamedSelSetPostModify, SystemNotificationCode.NamedSelSetPostModify);
   }

   private String modifyingSelSetName = null;

   public virtual void NamedSelSetCreated(IntPtr param, IntPtr info)
   {
      IntPtr callParam = (IntPtr)MaxUtils.HelperMethods.GetCallParam(info);
      String selSetName = Marshal.PtrToStringUni(callParam);

      if (this.modifyingSelSetName == null || this.modifyingSelSetName != selSetName)
      {
         SelectionSetWrapper wrapper = new SelectionSetWrapper(selSetName);
         TreeNode tn = this.AddNode(wrapper, this.tree.Nodes);
         this.tree.AddToSortQueue(tn);
         this.tree.AddToSortQueue(tn.Nodes);
         this.tree.StartTimedSort(true);
      }
   }

   public virtual void NamedSelSetDeleted(IntPtr param, IntPtr info)
   {
      IntPtr callParam = (IntPtr)MaxUtils.HelperMethods.GetCallParam(info);
      String selSetName = Marshal.PtrToStringUni(callParam);

      if (this.modifyingSelSetName == null || this.modifyingSelSetName != selSetName)
         this.RemoveNode(selSetName);
   }

   protected virtual void NamedSelSetPreModify(IntPtr param, IntPtr info)
   {
      IntPtr callParam = (IntPtr)MaxUtils.HelperMethods.GetCallParam(info);
      String selSetName = Marshal.PtrToStringUni(callParam);
      this.modifyingSelSetName = selSetName;
   }

   protected virtual void NamedSelSetPostModify(IntPtr param, IntPtr info)
   {
      if (this.modifyingSelSetName == null)
         return;

      TreeNode tn = this.GetFirstTreeNode(this.modifyingSelSetName);
      IMaxNodeWrapper wrapper = HelperMethods.GetMaxNode(tn);
      if (tn == null || wrapper == null)
         return;

      List<TreeNode> tnNodes = tn.Nodes.ToList();
      foreach (TreeNode childTn in tnNodes)
      {
         this.RemoveTreeNode(childTn);
      }

      foreach (Object node in wrapper.ChildNodes)
      {
         this.AddNode(node, tn.Nodes);
      }

      this.tree.StartTimedSort(tn.Nodes);

      this.modifyingSelSetName = null;
   }


   protected struct NameChange
   {
      [MarshalAs(UnmanagedType.LPWStr)]
      public String oldName;
      [MarshalAs(UnmanagedType.LPWStr)]
      public String newName;
   }

   protected virtual void NamedSelSetRenamed(IntPtr param, IntPtr info)
   {
      IntPtr callParam = (IntPtr)MaxUtils.HelperMethods.GetCallParam(info);

      NameChange nameChange = (NameChange)Marshal.PtrToStructure(callParam, typeof(NameChange));
      TreeNode tn = this.GetFirstTreeNode(nameChange.oldName);
      SelectionSetWrapper wrapper = HelperMethods.GetMaxNode(tn) as SelectionSetWrapper;
      if (tn != null && wrapper != null)
      {
         this.UnregisterNode(wrapper);
         wrapper.UpdateName(nameChange.newName);
         this.RegisterNode(wrapper, tn);
         tn.Invalidate();
         this.tree.StartTimedSort(tn);
      }
   }

   #endregion
}
}
