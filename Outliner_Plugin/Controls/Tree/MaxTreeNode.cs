using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outliner.Scene;

namespace Outliner.Controls.Tree
{
   public class MaxTreeNode : TreeNode
   {
      public IMaxNodeWrapper MaxNode { get; private set; }

      public MaxTreeNode(IMaxNodeWrapper maxNode)
      {
         this.MaxNode = maxNode;
         this.Tag = maxNode; //TODO temporary
      }

      public override string Text
      {
         get { return this.MaxNode.DisplayName; }
         set { }
      }

      public override string ImageKey
      {
         get { return this.MaxNode.ImageKey; }
         set { }
      }

      public override System.Drawing.FontStyle FontStyle
      {
         get
         {
            if (this.MaxNode is IINodeWrapper && ((IINodeWrapper)this.MaxNode).IsInstance)
               return System.Drawing.FontStyle.Bold;
            else
               return System.Drawing.FontStyle.Regular;
         }
         set { }
      }
   }
}
