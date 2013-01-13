using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Controls.Tree;
using Outliner.MaxUtils;

namespace Outliner.Controls
{
public partial class MainControl : MaxCustomControls.MaxUserControl, MaxCustomControls.CuiUpdatable
{
   public MainControl()
   {
      InitializeComponent();
      MaxCustomControls.MaxCuiBinder.GetInstance().RegisterForNotification(this);
      this.nameFilterTextBox.GotFocus += new EventHandler(nameFilterTextBox_GotFocus);
      this.nameFilterTextBox.LostFocus += new EventHandler(nameFilterTextBox_LostFocus);
   }

   public TextBox NameFilterTextBox
   {
      get { return this.nameFilterTextBox; }
   }

   public Outliner.Controls.Tree.TreeView TreeView1
   {
      get { return this.treeView1; }
   }

   public Outliner.Controls.Tree.TreeView TreeView2
   {
      get { return this.treeView2; }
   }

      
   public void UpdateColors()
   {
      this.BackColor = ColorHelpers.FromMaxGuiColor(Autodesk.Max.GuiColors.Background);
      this.treeView1.Colors.UpdateColors();
      this.treeView1.Update(TreeViewUpdateFlags.Brushes | TreeViewUpdateFlags.Redraw);
      this.treeView2.Colors.UpdateColors();
      this.treeView2.Update(TreeViewUpdateFlags.Brushes | TreeViewUpdateFlags.Redraw);
      TreeViewColorScheme colorScheme = this.treeView1.Colors;
      this.nameFilterTextBox.BackColor = colorScheme.Background;
      this.nameFilterTextBox.ForeColor = ColorHelpers.SelectContrastingColor( colorScheme.Background
                                                                            , colorScheme.ForegroundLight
                                                                            , colorScheme.ForegroundDark);
   }

   public Outliner.Controls.Tree.TreeView ActiveTreeView
   {
      get
      {
         if (this.outlinerSplitContainer1.Panel1Collapsed)
            return this.treeView2;
         else
            return this.treeView1;
      }
   }

   private void nameFilterTextBox_GotFocus(object sender, EventArgs e)
   {
      MaxInterfaces.Global.DisableAccelerators();
   }

   private void nameFilterTextBox_LostFocus(object sender, EventArgs e)
   {
      MaxInterfaces.Global.EnableAccelerators();
   }

   private void nameFilterTextBox_KeyPress(object sender, KeyPressEventArgs e)
   {
      if (e.KeyChar == (char)Keys.Return)
      {
         Outliner.Controls.Tree.TreeView activeTree = this.ActiveTreeView;
         activeTree.SelectAllNodes(false);
         activeTree.SelectNodes(GetVisibleNodes(activeTree.Root.Nodes), true);
         activeTree.OnSelectionChanged();
         NativeMethods.SetFocus(MaxInterfaces.MaxHwnd.Handle);
         e.Handled = true;
      }
   }

   private IEnumerable<Tree.TreeNode> GetVisibleNodes(Tree.TreeNodeCollection nodes)
   {
      foreach (Tree.TreeNode tn in nodes)
      {
         if (tn.ShowNode)
            yield return tn;

         foreach (Tree.TreeNode childTn in GetVisibleNodes(tn.Nodes))
         {
            yield return childTn;
         }
      }
   }
}
}
