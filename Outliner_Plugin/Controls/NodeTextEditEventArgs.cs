using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Outliner.Controls
{
public class BeforeNodeTextEditEventArgs : EventArgs
{
   /// <summary>
   /// The TreeNode of which the text will be edited.
   /// </summary>
   public TreeNode TreeNode { get; private set; }

   /// <summary>
   /// The text that will be displayed in the textbox. 
   /// This can be altered by objects that process the event.
   /// </summary>
   public String EditText { get; set; }

   /// <summary>
   /// If true, the NodeTextEdit action will be cancelled.
   /// </summary>
   public Boolean Cancel { get; set; }

   public BeforeNodeTextEditEventArgs(TreeNode tn)
   {
      if (tn == null)
         throw new ArgumentNullException("tn");

      this.TreeNode = tn;
      this.EditText = tn.Text;
      this.Cancel = false;
   }
}

public class AfterNodeTextEditEventArgs : EventArgs
{
   public Boolean Canceled { get; private set; }
   public String OldText { get; private set; }
   public String NewText { get; private set; }
   public TreeNode TreeNode { get; private set; }

   public AfterNodeTextEditEventArgs(TreeNode tn, Boolean canceled, String oldText, String newText)
   {
      this.TreeNode = tn;
      this.Canceled = canceled;
      this.OldText = oldText;
      this.NewText = newText;
   }
}
}
