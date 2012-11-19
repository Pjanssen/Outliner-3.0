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
   public partial class TestControl : MaxCustomControls.MaxUserControl, MaxCustomControls.CuiUpdatable
   {
      public TestControl()
      {
         InitializeComponent();
         MaxCustomControls.MaxCuiBinder.GetInstance().RegisterForNotification(this);
         this.nameFilterTextBox.GotFocus += new EventHandler(nameFilterTextBox_GotFocus);
         this.nameFilterTextBox.LostFocus += new EventHandler(nameFilterTextBox_LostFocus);
      }

      
      public void UpdateColors()
      {
         this.BackColor = ColorHelpers.FromMaxGuiColor(Autodesk.Max.GuiColors.Background);
         this.treeView1.Colors.UpdateColors();
         this.treeView1.Update(TreeViewUpdateFlags.Brushes | TreeViewUpdateFlags.Redraw);
         this.treeView2.Colors.UpdateColors();
         this.treeView2.Update(TreeViewUpdateFlags.Brushes | TreeViewUpdateFlags.Redraw);
         this.nameFilterTextBox.BackColor = this.treeView1.Colors.Background.Color;
         this.nameFilterTextBox.ForeColor = this.treeView1.Colors.ForegroundLight.Color;
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

      private void nameFilterTextBox_KeyUp(object sender, KeyEventArgs e)
      {
         if ((e.KeyData & Keys.Enter) == Keys.Enter)
         {
            Outliner.Controls.Tree.TreeView activeTree = this.ActiveTreeView;
            activeTree.SelectAllNodes(true);
            activeTree.OnSelectionChanged();
         }
      }
   }
}
