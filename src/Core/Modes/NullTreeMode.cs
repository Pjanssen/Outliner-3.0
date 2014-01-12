using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PJanssen.Outliner.Controls.Tree;

namespace PJanssen.Outliner.Modes
{
   /// <summary>
   /// A TreeMode that does nothing.
   /// </summary>
   class NullTreeMode : TreeMode
   {
      public NullTreeMode(TreeView tree) : base(tree) { }

      protected override void FillTree() { }

      public override void Start() { }
      public override void Stop() { }
   }
}
