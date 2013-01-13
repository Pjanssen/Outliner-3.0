using Outliner.Controls.Tree;
namespace Outliner.Controls
{
   partial class MainControl
   {
      /// <summary> 
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.components = new System.ComponentModel.Container();
         Outliner.Controls.Tree.TreeViewSettings treeViewSettings1 = new Outliner.Controls.Tree.TreeViewSettings();
         Outliner.Controls.Tree.TreeViewSettings treeViewSettings2 = new Outliner.Controls.Tree.TreeViewSettings();
         this.panel1 = new System.Windows.Forms.Panel();
         this.outlinerSplitContainer1 = new Outliner.Controls.OutlinerSplitContainer();
         this.treeView1 = new Outliner.Controls.Tree.TreeView();
         this.treeView2 = new Outliner.Controls.Tree.TreeView();
         this.nameFilterTextBox = new System.Windows.Forms.TextBox();
         this.NameFilterBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.panel2 = new System.Windows.Forms.Panel();
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.outlinerSplitContainer1)).BeginInit();
         this.outlinerSplitContainer1.Panel1.SuspendLayout();
         this.outlinerSplitContainer1.Panel2.SuspendLayout();
         this.outlinerSplitContainer1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.NameFilterBindingSource)).BeginInit();
         this.panel2.SuspendLayout();
         this.SuspendLayout();
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.outlinerSplitContainer1);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panel1.Location = new System.Drawing.Point(0, 24);
         this.panel1.Name = "panel1";
         this.panel1.Padding = new System.Windows.Forms.Padding(4, 1, 4, 4);
         this.panel1.Size = new System.Drawing.Size(314, 392);
         this.panel1.TabIndex = 1;
         // 
         // outlinerSplitContainer1
         // 
         this.outlinerSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.outlinerSplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
         this.outlinerSplitContainer1.Location = new System.Drawing.Point(4, 1);
         this.outlinerSplitContainer1.Name = "outlinerSplitContainer1";
         this.outlinerSplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
         // 
         // outlinerSplitContainer1.Panel1
         // 
         this.outlinerSplitContainer1.Panel1.Controls.Add(this.treeView1);
         // 
         // outlinerSplitContainer1.Panel2
         // 
         this.outlinerSplitContainer1.Panel2.Controls.Add(this.treeView2);
         this.outlinerSplitContainer1.Panel2Collapsed = true;
         this.outlinerSplitContainer1.Size = new System.Drawing.Size(306, 387);
         this.outlinerSplitContainer1.SplitterDistance = 144;
         this.outlinerSplitContainer1.TabIndex = 2;
         // 
         // treeView1
         // 
         this.treeView1.AllowDrop = true;
         this.treeView1.AutoScroll = true;
         this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.treeView1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.treeView1.Location = new System.Drawing.Point(0, 0);
         this.treeView1.Name = "treeView1";
         treeViewSettings1.AutoExpandSelectionParents = true;
         treeViewSettings1.CollapseAutoExpandedParents = true;
         treeViewSettings1.DoubleClickAction = Outliner.Controls.Tree.TreeNodeDoubleClickAction.Rename;
         treeViewSettings1.DragDropMouseButton = System.Windows.Forms.MouseButtons.Left;
         treeViewSettings1.MultiSelect = true;
         treeViewSettings1.ScrollToSelection = true;
         this.treeView1.Settings = treeViewSettings1;
         this.treeView1.Size = new System.Drawing.Size(306, 387);
         this.treeView1.TabIndex = 0;
         // 
         // treeView2
         // 
         this.treeView2.AllowDrop = true;
         this.treeView2.AutoScroll = true;
         this.treeView2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.treeView2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.treeView2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.treeView2.Location = new System.Drawing.Point(0, 0);
         this.treeView2.Name = "treeView2";
         treeViewSettings2.AutoExpandSelectionParents = true;
         treeViewSettings2.CollapseAutoExpandedParents = true;
         treeViewSettings2.DoubleClickAction = Outliner.Controls.Tree.TreeNodeDoubleClickAction.Rename;
         treeViewSettings2.DragDropMouseButton = System.Windows.Forms.MouseButtons.Left;
         treeViewSettings2.MultiSelect = true;
         treeViewSettings2.ScrollToSelection = true;
         this.treeView2.Settings = treeViewSettings2;
         this.treeView2.Size = new System.Drawing.Size(150, 46);
         this.treeView2.TabIndex = 1;
         // 
         // nameFilterTextBox
         // 
         this.nameFilterTextBox.AcceptsReturn = true;
         this.nameFilterTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.nameFilterTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.NameFilterBindingSource, "SearchString", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.nameFilterTextBox.Dock = System.Windows.Forms.DockStyle.Top;
         this.nameFilterTextBox.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.nameFilterTextBox.Location = new System.Drawing.Point(4, 4);
         this.nameFilterTextBox.Name = "nameFilterTextBox";
         this.nameFilterTextBox.Size = new System.Drawing.Size(306, 20);
         this.nameFilterTextBox.TabIndex = 0;
         this.nameFilterTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nameFilterTextBox_KeyPress);
         // 
         // NameFilterBindingSource
         // 
         this.NameFilterBindingSource.DataSource = typeof(Outliner.Filters.NameFilter);
         // 
         // panel2
         // 
         this.panel2.Controls.Add(this.nameFilterTextBox);
         this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
         this.panel2.Location = new System.Drawing.Point(0, 0);
         this.panel2.Name = "panel2";
         this.panel2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 0);
         this.panel2.Size = new System.Drawing.Size(314, 24);
         this.panel2.TabIndex = 0;
         // 
         // MainControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.panel1);
         this.Controls.Add(this.panel2);
         this.MinimumSize = new System.Drawing.Size(100, 150);
         this.Name = "MainControl";
         this.Size = new System.Drawing.Size(314, 416);
         this.panel1.ResumeLayout(false);
         this.outlinerSplitContainer1.Panel1.ResumeLayout(false);
         this.outlinerSplitContainer1.Panel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.outlinerSplitContainer1)).EndInit();
         this.outlinerSplitContainer1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.NameFilterBindingSource)).EndInit();
         this.panel2.ResumeLayout(false);
         this.panel2.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Panel panel1;
      internal OutlinerSplitContainer outlinerSplitContainer1;
      internal System.Windows.Forms.BindingSource NameFilterBindingSource;
      private System.Windows.Forms.Panel panel2;
      private System.Windows.Forms.TextBox nameFilterTextBox;
      private TreeView treeView1;
      private TreeView treeView2;


   }
}
