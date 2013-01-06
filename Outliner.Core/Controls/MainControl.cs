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

   private void nameFilterTextBox_KeyUp(object sender, KeyEventArgs e)
   {
      //This appears to be more reliable than e.KeyCode.HasFlag(Keys.Enter),
      //which also returns true for 'o' ?!
      if (e.KeyValue == 13)
      {
         Outliner.Controls.Tree.TreeView activeTree = this.ActiveTreeView;
         activeTree.SelectAllNodes(true);
         activeTree.OnSelectionChanged();
      }
   }
}
}
