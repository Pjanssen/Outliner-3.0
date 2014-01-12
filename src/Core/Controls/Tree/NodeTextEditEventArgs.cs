using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PJanssen.Outliner.Controls.Tree
{
/// <summary>
/// Provides data for the BeforeNodeTextEdit event.
/// </summary>
/// <remarks>This class inherits from CancelEventArgs, so it can be used to cancel 
/// the TreeNode text edit operation.</remarks>
public class BeforeNodeTextEditEventArgs : CancelEventArgs
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

   internal BeforeNodeTextEditEventArgs(TreeNode tn) : base(false)
   {
      if (tn == null)
         throw new ArgumentNullException("tn");

      this.TreeNode = tn;
      this.EditText = tn.Text;
   }
}

/// <summary>
/// Provides data for the AfterNodeTextEdit event.
/// </summary>
public class AfterNodeTextEditEventArgs : EventArgs
{
   /// <summary>
   /// Gets whether the TreeNode edit text operation was canceled.
   /// </summary>
   public Boolean Canceled { get; private set; }

   /// <summary>
   /// The old TreeNode text value.
   /// </summary>
   public String OldText { get; private set; }

   /// <summary>
   /// The new TreeNode text value.
   /// </summary>
   public String NewText { get; private set; }

   /// <summary>
   /// The TreeNode which is being edited.
   /// </summary>
   public TreeNode TreeNode { get; private set; }

   internal AfterNodeTextEditEventArgs(TreeNode tn, Boolean canceled, String oldText, String newText)
   {
      this.TreeNode = tn;
      this.Canceled = canceled;
      this.OldText = oldText;
      this.NewText = newText;
   }
}
}
