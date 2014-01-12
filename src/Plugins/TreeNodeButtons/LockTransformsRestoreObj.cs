using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Max;
using Autodesk.Max.Plugins;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.TreeNodeButtons
{
   internal class LockTransformsRestoreObj : RestoreObj
   {
      private List<INodeWrapper> nodes;
      private List<Tuple<IINode, List<Boolean>>> oldTransformLocks;
      private Boolean lockTransforms;

      public LockTransformsRestoreObj(IEnumerable<IMaxNode> nodes, Boolean lockTransforms)
      {
         this.nodes = nodes.OfType<INodeWrapper>().ToList();
         this.lockTransforms = lockTransforms;
      }

      public override void Redo()
      {
         this.oldTransformLocks = new List<Tuple<IINode, List<Boolean>>>();

         List<Boolean> allLocks = Enumerable.Repeat(this.lockTransforms, 9).ToList();
         foreach (INodeWrapper node in nodes)
         {
            IINode inode = node.INode;
            this.oldTransformLocks.Add(new Tuple<IINode, List<Boolean>>(inode, GetLocks(inode)));
            SetLocks(inode, allLocks);
         }
      }

      private List<Boolean> GetLocks(IINode node)
      {
         List<Boolean> locks = new List<Boolean>(9);
         locks.Add(node.GetTransformLock(0, 0));
         locks.Add(node.GetTransformLock(0, 1));
         locks.Add(node.GetTransformLock(0, 2));
         locks.Add(node.GetTransformLock(1, 0));
         locks.Add(node.GetTransformLock(1, 1));
         locks.Add(node.GetTransformLock(1, 2));
         locks.Add(node.GetTransformLock(2, 0));
         locks.Add(node.GetTransformLock(2, 1));
         locks.Add(node.GetTransformLock(2, 2));
         return locks;
      }

      public override void Restore(bool isUndo)
      {
         foreach (Tuple<IINode, List<Boolean>> nodeLocks in oldTransformLocks)
         {
            SetLocks(nodeLocks.Item1, nodeLocks.Item2);
         }
      }

      private static void SetLocks(IINode node, List<Boolean> locks)
      {
         node.SetTransformLock(0, 0, locks[0]);
         node.SetTransformLock(0, 1, locks[1]);
         node.SetTransformLock(0, 2, locks[2]);
         node.SetTransformLock(1, 0, locks[3]);
         node.SetTransformLock(1, 1, locks[4]);
         node.SetTransformLock(1, 2, locks[5]);
         node.SetTransformLock(2, 0, locks[6]);
         node.SetTransformLock(2, 1, locks[7]);
         node.SetTransformLock(2, 2, locks[8]);
      }
   }
}
