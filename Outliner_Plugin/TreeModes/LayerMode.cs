using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Controls;
using Outliner.Controls.FiltersBase;

namespace Outliner.TreeModes
{
public class LayerMode : TreeMode
{
   public LayerMode(Outliner.Controls.TreeView tree, Autodesk.Max.IInterface ip)
      : base(tree, ip)
   {
      IGlobal iGlobal = GlobalInterface.Instance;
      iGlobal.RegisterNotification(LayerCreated, null, SystemNotificationCode.LayerCreated);
      iGlobal.RegisterNotification(LayerDeleted, null, SystemNotificationCode.LayerDeleted);
      //The following notifications don't actually seem to work in the SDK... :
      iGlobal.RegisterNotification(LayerRenamed, null, SystemNotificationCode.LayerRenamed);
      iGlobal.RegisterNotification(LayerPropChanged, null, SystemNotificationCode.LayerHiddenStateChanged);
      iGlobal.RegisterNotification(LayerPropChanged, null, SystemNotificationCode.LayerFrozenStateChanged);
   }

   private const int IILAYERMANAGER_REF_INDEX = 10;

   public override void FillTree()
   {
      this.tree.BeginUpdate();
      /*
      IGlobal g = GlobalInterface.Instance;
      IInterface_ID int_ID = g.Interface_ID.Create((uint)BuiltInInterfaceIDA.LAYERMANAGER_INTERFACE,
                                                   (uint)BuiltInInterfaceIDB.LAYERMANAGER_INTERFACE);
      IIFPLayerManager lm = (IIFPLayerManager)g.GetCOREInterface(int_ID);

      for (int i = 0; i < lm.Count; i++)
      {
         IILayerProperties l = lm.GetLayer(i);
         this.addNode(l, this.tree.Nodes);
      }*/
      
      IINode rootNode = this.ip.RootNode;
      for (int i = 0; i < rootNode.NumberOfChildren; i++)
      {
         addNode(rootNode.GetChildNode(i));
      }

      this.tree.EndUpdate();
      this.tree.TimedSort(false);
   }



   private TreeNode addNode(IILayer layer, TreeNodeCollection parentCol)
   {
      IMaxNodeWrapper wrapper = IMaxNodeWrapper.Create(layer);
      FilterResult filterResult = this.Filters.ShowNode(wrapper);
      if (filterResult != FilterResult.Hide && !this.nodes.ContainsKey(layer))
      {
         TreeNode tn = HelperMethods.CreateTreeNode(wrapper);
         tn.FilterResult = filterResult;

         this.nodes.Add(layer, tn);
         parentCol.Add(tn);
         return tn;
      }
      return null;
   }

   private void addNode(IINode node)
   {
      if (HelperMethods.IsPFHelper(node))
         return;

      IMaxNodeWrapper wrapper = IMaxNodeWrapper.Create(node);
      FilterResult filterResult = this.Filters.ShowNode(wrapper);
      if (filterResult != FilterResult.Hide && !this.nodes.ContainsKey(node))
      {
         //Add layer node if it doesn't exist yet.
         IILayer l = (IILayer)node.GetReference((int)ReferenceNumbers.NodeLayerRef);
         TreeNode parentTn = null;
         if (!this.nodes.TryGetValue(l, out parentTn))
            parentTn = this.addNode(l, this.tree.Nodes);

         if (parentTn != null)
         {
            TreeNode tn = HelperMethods.CreateTreeNode(wrapper);
            tn.FilterResult = filterResult;

            this.nodes.Add(node, tn);
            parentTn.Nodes.Add(tn);

            if (node.Selected)
               this.tree.SelectNode(tn, true);

            for (int i = 0; i < node.NumberOfChildren; i++)
               this.addNode(node.GetChildNode(i));
         }
      }
   }


   #region NodeEventCallbacks
   
   public override void LayerChanged(ITab<UIntPtr> nodes)
   {
      foreach (IINode node in nodes.NodeKeysToINodeList())
      {
         this.RemoveTreeNode(node);
         //this.addNode(node);
      }
   }

   #endregion


   #region System notifications

   public virtual void LayerCreated(IntPtr paramPtr, IntPtr infoPtr)
   {
      INotifyInfo info = HelperMethods.GetNotifyInfo(infoPtr);
      if (info != null && info.CallParam is IILayer)
         this.addNode((IILayer)info.CallParam, this.tree.Nodes);
   }

   public virtual void LayerDeleted(IntPtr paramPtr, IntPtr infoPtr)
   {
      INotifyInfo info = HelperMethods.GetNotifyInfo(infoPtr);
      if (info != null && info.CallParam is IILayer)
         this.RemoveTreeNode(info.CallParam);
   }

   public virtual void LayerRenamed(IntPtr paramPtr, IntPtr infoPtr)
   {
      INotifyInfo info = HelperMethods.GetNotifyInfo(infoPtr);
   }

   public virtual void LayerPropChanged(IntPtr paramPtr, IntPtr infoPtr)
   {
      INotifyInfo info = HelperMethods.GetNotifyInfo(infoPtr);
      if (info != null && info.CallParam is IILayer)
      {
         TreeNode tn = this.GetTreeNode(info.CallParam);
         if (tn != null)
            this.tree.Invalidate(tn.Bounds);
      }
   }

   #endregion
}
}
