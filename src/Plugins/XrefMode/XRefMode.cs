using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Max;
using Autodesk.Max.MaxSDK.AssetManagement;
using PJanssen.Outliner.Controls.Tree;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Modes;
using PJanssen.Outliner.Plugins;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.Modes.XRef
{
   [OutlinerPlugin(OutlinerPluginType.TreeMode)]
   [LocalizedDisplayName(typeof(Resources), "Str_XrefMode")]
   public class XRefMode : TreeMode
   {
      protected GlobalDelegates.Delegate5 proc_XrefSceneFlagsChanged;
      protected GlobalDelegates.Delegate5 proc_XrefSceneDeleted;
      protected GlobalDelegates.Delegate5 proc_XrefObjectRecordDeleted;

      public XRefMode(TreeView tree) : base(tree) 
      {
         proc_XrefSceneFlagsChanged = new GlobalDelegates.Delegate5(this.XrefSceneFlagsChanged);
         proc_XrefSceneDeleted = new GlobalDelegates.Delegate5(this.XrefSceneDeleted);
         proc_XrefObjectRecordDeleted = new GlobalDelegates.Delegate5(this.XrefObjectRecordDeleted);
      }

      public override void Start()
      {
         base.Start();

         this.RegisterSystemNotification(proc_XrefSceneFlagsChanged, XRefNotificationCodes.XRefSceneFlagsChanged);
         this.RegisterSystemNotification(proc_XrefSceneDeleted, XRefNotificationCodes.XRefSceneDeleted);
         this.RegisterSystemNotification(proc_XrefObjectRecordDeleted, XRefNotificationCodes.XRefObjectRecordDeleted);
      }

      #region FillTree
      
      protected override void FillTree()
      {
         AddXrefScenes();
         AddXrefObjects();
      }

      private void AddXrefScenes()
      {
         IINode root = MaxInterfaces.COREInterface.RootNode;
         for (int i = 0; i < root.XRefFileCount; i++)
         {
            TreeNodeCollection parentCollection = this.Tree.Nodes;
            IINode parent = root.GetXRefParent(i);
            if (parent != null)
            {
               TreeNode parentTn = this.AddNode(parent, this.Tree.Nodes);
               parentCollection = parentTn.Nodes;
            }

            IAssetUser asset = root.GetXRefFile(i);
            XRefSceneRecord xrefScene = new XRefSceneRecord(asset, i);
            this.AddNode(xrefScene, parentCollection);
         }
      }

      private void AddXrefObjects()
      {
         IIObjXRefManager8 objXrefManager = MaxInterfaces.Global.IObjXRefManager8.Instance;
         for (uint i = 0; i < objXrefManager.RecordCount; i++)
         {
            IIObjXRefRecord record = objXrefManager.GetRecord(i);
            XRefObjectRecord xrefRecord = new XRefObjectRecord(record);
            TreeNode tn = this.AddNode(xrefRecord, this.Tree.Nodes);

            foreach (object recordItem in xrefRecord.ChildBaseObjects)
            {
               this.AddNode(recordItem, tn.Nodes);
            }
         }
      }

      #endregion

      #region System notifications

      protected virtual void XrefSceneFlagsChanged(IntPtr param, IntPtr info)
      {
         IAssetUser asset = SystemNotifications.GetCallParam(info) as IAssetUser;
         if (asset != null)
            this.InvalidateObject(asset, false, false);
      }

      protected virtual void XrefSceneDeleted(IntPtr param, IntPtr info)
      {
         IAssetUser asset = SystemNotifications.GetCallParam(info) as IAssetUser;
         if (asset != null)
            this.RemoveNode(asset);
      }

      protected virtual void XrefObjectRecordDeleted(IntPtr param, IntPtr info)
      {
         IIObjXRefRecord record = SystemNotifications.GetCallParam(info) as IIObjXRefRecord;
         if (record != null)
            this.RemoveNode(record);
      }

      #endregion
   }
}
