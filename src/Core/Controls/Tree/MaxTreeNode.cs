﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Scene;

namespace PJanssen.Outliner.Controls.Tree
{
   public class MaxTreeNode : TreeNode
   {
      public virtual IMaxNode MaxNode { get; private set; }

      public MaxTreeNode(IMaxNode maxNode)
      {
         this.MaxNode = maxNode;
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
            System.Drawing.FontStyle style = base.FontStyle;
            if (this.MaxNode is INodeWrapper && ((INodeWrapper)this.MaxNode).IsInstance)
               return style | System.Drawing.FontStyle.Bold;
            else
               return style;
         }
         set 
         {
            base.FontStyle = value;
         }
      }

      public override string ToString()
      {
         return String.Format("MaxTreeNode ({0})", this.Text);
      }
   }
}
