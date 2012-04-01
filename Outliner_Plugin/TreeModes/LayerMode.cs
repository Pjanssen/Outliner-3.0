using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Outliner.Scene;
using Outliner.Controls;
using Outliner.Controls.FiltersBase;
using System.Drawing;

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

      this.tree.Sort();

      this.tree.EndUpdate();
   }



   private TreeNode addNode(IILayer layer, TreeNodeCollection parentCol)
   {
      IMaxNodeWrapper wrapper = IMaxNodeWrapper.Create(layer);
      FilterResults filterResult = this.Filters.ShowNode(wrapper);
      if (filterResult != FilterResults.Hide && !this.treeNodes.ContainsKey(layer))
      {
         TreeNode tn = HelperMethods.CreateTreeNode(wrapper);
         tn.FilterResult = filterResult;

         if (this.tree.TreeNodeLayout.UseLayerColors)
         {
            tn.BackColor = Color.FromArgb(255, wrapper.WireColor);
            if (tn.BackColor.GetBrightness() > 0.5)
               tn.ForeColor = Color.Black;
            else
               tn.ForeColor = Color.White;
         }
         this.treeNodes.Add(layer, tn);
         parentCol.Add(tn);
         return tn;
      }
      return null;
   }

   private void addNode(IINode node)
   {
      IINodeWrapper wrapper = new IINodeWrapper(node);
      FilterResults filterResult = this.Filters.ShowNode(wrapper);
      if (filterResult != FilterResults.Hide && !this.treeNodes.ContainsKey(node))
      {
         //Add layer node if it doesn't exist yet.
         IILayer l = (IILayer)node.GetReference((int)ReferenceNumbers.NodeLayerRef);
         TreeNode parentTn = null;
         if (!this.treeNodes.TryGetValue(l, out parentTn))
            parentTn = this.addNode(l, this.tree.Nodes);

         if (parentTn != null)
         {
            TreeNode tn = HelperMethods.CreateTreeNode(wrapper);
            tn.FilterResult = filterResult;

            if (this.tree.TreeNodeLayout.UseLayerColors)
            {
               tn.BackColor = Color.FromArgb(170, HelperMethods.FromMaxColor(wrapper.Layer.WireColor));
               if (tn.BackColor.GetBrightness() > 0.5)
                  tn.ForeColor = Color.Black;
               else
                  tn.ForeColor = Color.White;
            }

            this.treeNodes.Add(node, tn);
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

   public virtual void LayerCreated(IntPtr param, IntPtr info)
   {
      INotifyInfo notifyInfo = HelperMethods.GetNotifyInfo(info);
      if (notifyInfo != null && notifyInfo.CallParam is IILayer)
         this.addNode((IILayer)notifyInfo.CallParam, this.tree.Nodes);
   }

   public virtual void LayerDeleted(IntPtr param, IntPtr info)
   {
      INotifyInfo notifyInfo = HelperMethods.GetNotifyInfo(info);
      if (notifyInfo != null && notifyInfo.CallParam is IILayer)
         this.RemoveTreeNode(notifyInfo.CallParam);
   }

   public virtual void LayerRenamed(IntPtr param, IntPtr info)
   {
      INotifyInfo notifyInfo = HelperMethods.GetNotifyInfo(info);
      Console.WriteLine(notifyInfo.CallParam);
   }

   public virtual void LayerPropChanged(IntPtr param, IntPtr info)
   {
      INotifyInfo notifyInfo = HelperMethods.GetNotifyInfo(info);
      if (notifyInfo != null && notifyInfo.CallParam is IILayer)
      {
         TreeNode tn = this.GetTreeNode(notifyInfo.CallParam);
         if (tn != null)
            this.tree.Invalidate(tn.Bounds);
      }
   }

   #endregion
}
}
