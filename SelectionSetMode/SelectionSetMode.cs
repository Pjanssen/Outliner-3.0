using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Controls.Tree;
using Outliner.Filters;
using Outliner.MaxUtils;
using System.Runtime.InteropServices;
using Outliner.Plugins;
using System.Drawing;

namespace Outliner.Modes.SelectionSet
{
[OutlinerPlugin(OutlinerPluginType.TreeMode)]
[LocalizedDisplayName(typeof(Resources), "Mode_SelectionSets")]
public class SelectionSetMode : TreeMode
{
   private AllObjectsSelectionSetWrapper allObjectsSelSet;

   public SelectionSetMode(TreeView tree) : base(tree)
   {
      proc_NamedSelSetCreated    = new GlobalDelegates.Delegate5(this.NamedSelSetCreated);
      proc_NamedSelSetDeleted    = new GlobalDelegates.Delegate5(this.NamedSelSetDeleted);
      proc_NamedSelSetPreModify  = new GlobalDelegates.Delegate5(this.NamedSelSetPreModify);
      proc_NamedSelSetPostModify = new GlobalDelegates.Delegate5(this.NamedSelSetPostModify);
      proc_NamedSelSetRenamed    = new GlobalDelegates.Delegate5(this.NamedSelSetRenamed);

      this.allObjectsSelSet = new AllObjectsSelectionSetWrapper();
      this.Tree.DragDropHandler = new TreeViewDragDropHandler();
   }

   protected override void FillTree()
   {
      this.Tree.BeginUpdate();

      TreeNode allObjTn = this.AddNode(this.allObjectsSelSet, this.Tree.Nodes);
      allObjTn.FontStyle = System.Drawing.FontStyle.Italic;

      IINamedSelectionSetManager selSetMan = MaxInterfaces.SelectionSetManager;
      for (int i = 0; i < selSetMan.NumNamedSelSets; i++)
      {
         this.AddNode(new SelectionSetWrapper(i), this.Tree.Nodes);
      }

      this.Tree.Sort();
      this.Tree.EndUpdate();
   }

   public override TreeNode AddNode(IMaxNode wrapper, TreeNodeCollection parentCol)
   {
      TreeNode tn = base.AddNode(wrapper, parentCol);

      if (wrapper is SelectionSetWrapper || wrapper is AllObjectsSelectionSetWrapper)
      {
         foreach (Object child in wrapper.ChildBaseObjects)
            this.AddNode(child, tn.Nodes);
      }

      return tn;
   }

   override public DragDropHandler CreateDragDropHandler(IMaxNode wrapper)
   {
      if (wrapper is SelectionSetWrapper)
         return new SelectionSetDragDropHandler((SelectionSetWrapper)wrapper);
      else if (wrapper is INodeWrapper)
         return new INodeDragDropHandler(wrapper);

      return base.CreateDragDropHandler(wrapper);
   }


   public override void Start()
   {
      this.RegisterSystemNotification(proc_NamedSelSetCreated, SystemNotificationCode.NamedSelSetCreated);
      this.RegisterSystemNotification(proc_NamedSelSetDeleted, SystemNotificationCode.NamedSelSetDeleted);
      this.RegisterSystemNotification(proc_NamedSelSetRenamed, SystemNotificationCode.NamedSelSetRenamed);
      this.RegisterSystemNotification(proc_NamedSelSetPreModify, SystemNotificationCode.NamedSelSetPreModify);
      this.RegisterSystemNotification(proc_NamedSelSetPostModify, SystemNotificationCode.NamedSelSetPostModify);

      this.RegisterNodeEventCallbackObject(new SelectionSetNodeEventCallbacks(this));

      base.Start();
   }


   #region NodeEventCallbacks

   protected class SelectionSetNodeEventCallbacks : TreeModeNodeEventCallbacks
   {
      protected new SelectionSetMode TreeMode;
      public SelectionSetNodeEventCallbacks(SelectionSetMode treeMode)
         : base(treeMode)
      {
         this.TreeMode = treeMode;
      }

      public override void Added(ITab<UIntPtr> nodes)
      {
         TreeNode allObjSelSetTn = this.TreeMode.GetFirstTreeNode(this.TreeMode.allObjectsSelSet);
         if (allObjSelSetTn == null)
            return;

         foreach (IINode node in IINodeHelpers.NodeKeysToINodeList(nodes))
         {
            this.TreeMode.AddNode(node, allObjSelSetTn.Nodes);
            this.Tree.AddToSortQueue(allObjSelSetTn.Nodes);
         }
         this.Tree.StartTimedSort(true);
      }
   }

   #endregion


   #region System notifications

   private String modifyingSelSetName = null;

   protected GlobalDelegates.Delegate5 proc_NamedSelSetCreated;
   protected virtual void NamedSelSetCreated(IntPtr param, IntPtr info)
   {
      IntPtr callParam = (IntPtr)MaxUtils.HelperMethods.GetCallParam(info);
      String selSetName = Marshal.PtrToStringUni(callParam);

      if (this.modifyingSelSetName == null || this.modifyingSelSetName != selSetName)
      {
         SelectionSetWrapper wrapper = new SelectionSetWrapper(selSetName);
         TreeNode tn = this.AddNode(wrapper, this.Tree.Nodes);
         this.Tree.AddToSortQueue(tn);
         this.Tree.AddToSortQueue(tn.Nodes);
         this.Tree.StartTimedSort(true);
      }
   }

   protected GlobalDelegates.Delegate5 proc_NamedSelSetDeleted;
   protected virtual void NamedSelSetDeleted(IntPtr param, IntPtr info)
   {
      IntPtr callParam = (IntPtr)MaxUtils.HelperMethods.GetCallParam(info);
      String selSetName = Marshal.PtrToStringUni(callParam);

      if (this.modifyingSelSetName == null || this.modifyingSelSetName != selSetName)
         this.RemoveNode(selSetName);
   }

   protected GlobalDelegates.Delegate5 proc_NamedSelSetPreModify;
   protected virtual void NamedSelSetPreModify(IntPtr param, IntPtr info)
   {
      IntPtr callParam = (IntPtr)MaxUtils.HelperMethods.GetCallParam(info);
      String selSetName = Marshal.PtrToStringUni(callParam);
      this.modifyingSelSetName = selSetName;
   }

   protected GlobalDelegates.Delegate5 proc_NamedSelSetPostModify;
   protected virtual void NamedSelSetPostModify(IntPtr param, IntPtr info)
   {
      if (this.modifyingSelSetName == null)
         return;

      TreeNode tn = this.GetFirstTreeNode(this.modifyingSelSetName);
      IMaxNode wrapper = HelperMethods.GetMaxNode(tn);
      if (tn == null || wrapper == null)
         return;

      List<TreeNode> tnNodes = tn.Nodes.ToList();
      foreach (TreeNode childTn in tnNodes)
      {
         //TODO: verify that this is correct.
         this.UnregisterNode(HelperMethods.GetMaxNode(childTn), childTn);
         childTn.Remove();
      }

      foreach (Object node in wrapper.ChildBaseObjects)
      {
         this.AddNode(node, tn.Nodes);
      }

      this.Tree.StartTimedSort(tn.Nodes);

      this.modifyingSelSetName = null;
   }


   protected struct NameChange
   {
      [MarshalAs(UnmanagedType.LPWStr)]
      public String oldName;
      [MarshalAs(UnmanagedType.LPWStr)]
      public String newName;
   }

   protected GlobalDelegates.Delegate5 proc_NamedSelSetRenamed;
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
         this.Tree.StartTimedSort(tn);
      }
   }

   //Invalidate selection sets explicitly when colortag has changed.
   //override protected void ColorTagChanged(IntPtr param, IntPtr info)
   //{
   //   base.ColorTagChanged(param, info);
   //   IAnimatable node = MaxUtils.HelperMethods.GetCallParam(info) as IAnimatable;
   //   List<TreeNode> tns = this.GetTreeNodes(node);
   //   tns.ForEach(tn => tn.Parent.Invalidate());
   //}

   #endregion
}
}
