using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Scene;
using Autodesk.Max;
using PJanssen.Outliner.MaxUtils;
using Autodesk.Max.Plugins;

namespace PJanssen.Outliner
{
internal static class GroupHelpers
{
   private static List<INodeWrapper> openedGroupHeads;
   private static uint closeGroupHeadsCbKey = 0;
   private static GlobalDelegates.Delegate5 closeDelegate;

   private class CloseGroupHeadsNodeEventCb : INodeEventCallback
   {
      public CloseGroupHeadsNodeEventCb() { }

      public override void SelectionChanged(ITab<UIntPtr> nodes)
      {
         if (GroupHelpers.CloseUnselectedGroupHeads())
         {
            IInterface core = MaxInterfaces.COREInterface;
            core.RedrawViews(core.Time, RedrawFlags.Normal, null);
         }
      }
   }

   private static void Start()
   {
      if (GroupHelpers.closeGroupHeadsCbKey == 0)
      {
         CloseGroupHeadsNodeEventCb cb = new CloseGroupHeadsNodeEventCb();
         IISceneEventManager sceneEventMgr = MaxInterfaces.Global.ISceneEventManager;
         GroupHelpers.closeGroupHeadsCbKey = sceneEventMgr.RegisterCallback(cb, false, 50, true);
      }

      if (GroupHelpers.closeDelegate == null)
      {
         GroupHelpers.closeDelegate = new GlobalDelegates.Delegate5((param, info) => MaxInterfaces.COREInterface.ClearNodeSelection(false));
         IGlobal global = MaxInterfaces.Global;
         global.RegisterNotification(closeDelegate, null, SystemNotificationCode.SystemPreNew);
         global.RegisterNotification(closeDelegate, null, SystemNotificationCode.SystemPreReset);
         global.RegisterNotification(closeDelegate, null, SystemNotificationCode.FilePreOpen);
         global.RegisterNotification(closeDelegate, null, SystemNotificationCode.FilePreSave);
      }
   }

   public static void Stop()
   {
      if (GroupHelpers.closeGroupHeadsCbKey != 0)
      {
         IISceneEventManager sceneEventMgr = MaxInterfaces.Global.ISceneEventManager;
         sceneEventMgr.UnRegisterCallback(GroupHelpers.closeGroupHeadsCbKey);
         GroupHelpers.closeGroupHeadsCbKey = 0;
      }

      if (GroupHelpers.closeDelegate != null)
      {
         IGlobal global = MaxInterfaces.Global;
         global.UnRegisterNotification(closeDelegate, null, SystemNotificationCode.SystemPreNew);
         global.UnRegisterNotification(closeDelegate, null, SystemNotificationCode.SystemPreReset);
         global.UnRegisterNotification(closeDelegate, null, SystemNotificationCode.FilePreOpen);
         global.UnRegisterNotification(closeDelegate, null, SystemNotificationCode.FilePreSave);
         GroupHelpers.closeDelegate = null;
      }

      if (GroupHelpers.openedGroupHeads != null)
         GroupHelpers.openedGroupHeads.Clear();
   }

   /// <summary>
   /// Opens any closed group heads in the provided list of nodewrappers.
   /// When the selection changes, the opened heads are closed automatically as required.
   /// </summary>
   public static void OpenSelectedGroupHeads(IEnumerable<IMaxNode> nodes)
   {
      Throw.IfNull(nodes, "nodes");

      if (GroupHelpers.openedGroupHeads == null)
         GroupHelpers.openedGroupHeads = new List<INodeWrapper>();

      foreach (IMaxNode node in nodes)
      {
         INodeWrapper inode = node as INodeWrapper;
         if (inode == null)
            continue;

         if (inode.INode.IsGroupMember && !inode.INode.IsOpenGroupMember)
         {
            INodeWrapper parent = inode.Parent as INodeWrapper;
            while (parent != null && (parent.INode.IsGroupMember || parent.INode.IsGroupHead))
            {
               if (parent.INode.IsGroupHead && !parent.INode.IsOpenGroupHead)
               {
                  GroupHelpers.OpenCloseGroup(parent, true);
                  GroupHelpers.openedGroupHeads.Add(parent);
               }
               parent = parent.Parent as INodeWrapper;
            }
            inode.INode.SetGroupMemberOpen(true);
         }
      }

      GroupHelpers.Start();
   }

   /// <summary>
   /// Closes any group heads that were opened using OpenGroupHeads() and are no longer a parent of a selected node.
   /// </summary>
   public static Boolean CloseUnselectedGroupHeads()
   {
      if (GroupHelpers.openedGroupHeads == null)
         return false;

      Boolean groupsClosed = false;

      for (int i = GroupHelpers.openedGroupHeads.Count - 1; i >= 0; i--)
      {
         INodeWrapper groupHead = GroupHelpers.openedGroupHeads[i];
         if (!groupHead.IsParentOfSelected())
         {
            GroupHelpers.OpenCloseGroup(groupHead, false);
            GroupHelpers.openedGroupHeads.RemoveAt(i);
            groupsClosed = true;
         }
      }

      if (GroupHelpers.openedGroupHeads.Count == 0)
         GroupHelpers.Stop();

      return groupsClosed;
   }

   /// <summary>
   /// Opens or closes a group head.
   /// </summary>
   /// <param name="groupHead">The group head node to open or close.</param>
   public static void OpenCloseGroup(INodeWrapper groupHead, Boolean open)
   {
      if (groupHead == null)
         return;

      if (groupHead.INode.IsGroupHead)
         groupHead.INode.SetGroupHeadOpen(open);

      foreach (IMaxNode child in groupHead.ChildNodes)
      {
         INodeWrapper inodeChild = child as INodeWrapper;
         if (inodeChild != null && inodeChild.INode.IsGroupMember)
         {
            inodeChild.INode.SetGroupMemberOpen(open);
            OpenCloseGroup(inodeChild, open);
         }
      }
   }

   public static INodeWrapper CreateGroupHead()
   {
      IInterface ip = MaxInterfaces.COREInterface;
      IGlobal global = MaxInterfaces.Global;
      IClass_ID classID = global.Class_ID.Create((uint)BuiltInClassIDA.DUMMY_CLASS_ID, 0);
      IDummyObject dummy = ip.CreateInstance(SClass_ID.Helper, classID) as IDummyObject;
      dummy.Box = global.Box3.Create( global.Point3.Create(0, 0, 0)
                                    , global.Point3.Create(0, 0, 0));
      IINode groupHead = ip.CreateObjectNode(dummy);
      String newName = "Group";
      MaxInterfaces.COREInterface.MakeNameUnique(ref newName);
      groupHead.Name = newName;
      groupHead.SetGroupHead(true);

      return new INodeWrapper(groupHead);
   }
}
}
