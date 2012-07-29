using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max.Plugins;
using Outliner.Controls;
using Autodesk.Max;
using Outliner.Scene;
using MaxUtils;

namespace Outliner
{
   //These values do not seem to be defined in Autodesk.Max.dll,
   //so they are copied from gup.h :
   public enum GupResult : uint
   {
      Keep = 0x00,
      NoKeep = 0x01,
      Abort = 0x03
   }

   public class OutlinerGUP : GUP
   {
      private List<IINodeWrapper> openedGroupHeads;
      private uint closeGroupHeadsCbKey = 0;

      public OutlinerGUP()
      {
         this.openedGroupHeads = new List<IINodeWrapper>();
      }

      public override uint Start
      {
         get 
         {
            return (uint)GupResult.Keep; 
         }
      }

      public override void Stop() 
      {
         this.UnRegisterCloseGroupHeadsCb();
      }



      /// <summary>
      /// Opens any closed group heads in the provided list of nodewrappers.
      /// When the selection changes, the opened heads are closed automatically as required.
      /// </summary>
      public void OpenSelectedGroupHeads(IEnumerable<IMaxNodeWrapper> nodes)
      {
         foreach (IMaxNodeWrapper node in nodes)
         {
            IINodeWrapper inode = node as IINodeWrapper;
            if (inode == null)
               continue;

            if (inode.IINode.IsGroupMember && !inode.IINode.IsOpenGroupMember)
            {
               IINodeWrapper parent = inode.Parent as IINodeWrapper;
               while (parent != null && (parent.IINode.IsGroupMember || parent.IINode.IsGroupHead))
               {
                  if (parent.IINode.IsGroupHead && !parent.IINode.IsOpenGroupHead)
                  {
                     HelperMethods.OpenCloseGroup(parent, true);
                     this.openedGroupHeads.Add(parent);
                  }
                  parent = parent.Parent as IINodeWrapper;
               }
               inode.IINode.SetGroupMemberOpen(true);
            }
         }

         if (this.closeGroupHeadsCbKey == 0)
         {
            CloseGroupHeadsNodeEventCb cb = new CloseGroupHeadsNodeEventCb(this);
            IISceneEventManager sceneEventMgr = MaxInterfaces.Global.ISceneEventManager;
            this.closeGroupHeadsCbKey = sceneEventMgr.RegisterCallback(cb, false, 50, true);
         }
      }

      /// <summary>
      /// Closes any group heads that were opened using OpenGroupHeads() and are no longer a parent of a selected node.
      /// </summary>
      public void CloseUnselectedGroupHeads()
      {
         for (int i = this.openedGroupHeads.Count - 1; i >= 0; i--)
         {
            IINodeWrapper groupHead = this.openedGroupHeads[i];
            if (!HelperMethods.IsParentOfSelected(groupHead))
            {
               HelperMethods.OpenCloseGroup(groupHead, false);
               this.openedGroupHeads.RemoveAt(i);
            }
         }

         if (this.openedGroupHeads.Count == 0)
         {
            this.UnRegisterCloseGroupHeadsCb();
         }
      }

      private void UnRegisterCloseGroupHeadsCb()
      {
         if (this.closeGroupHeadsCbKey == 0)
            return;

         IISceneEventManager sceneEventMgr = MaxInterfaces.Global.ISceneEventManager;
         sceneEventMgr.UnRegisterCallback(this.closeGroupHeadsCbKey);
         this.closeGroupHeadsCbKey = 0;
      }

      protected class CloseGroupHeadsNodeEventCb : INodeEventCallback
      {
         private OutlinerGUP gup;
         public CloseGroupHeadsNodeEventCb(OutlinerGUP gup)
         {
            this.gup = gup;
         }

         public override void SelectionChanged(ITab<UIntPtr> nodes)
         {
            gup.CloseUnselectedGroupHeads();
            gup.Max.RedrawViews(gup.Max.Time, RedrawFlags.Normal, null);
         }
      }
   }
}
