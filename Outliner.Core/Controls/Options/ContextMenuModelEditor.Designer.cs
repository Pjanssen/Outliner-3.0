namespace Outliner.Controls.Options
{
   partial class ContextMenuModelEditor
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
         this.outlinerGroupBox2 = new Outliner.Controls.OutlinerGroupBox();
         this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
         this.itemsComboBox = new System.Windows.Forms.ComboBox();
         this.addBtn = new System.Windows.Forms.Button();
         this.itemTree = new Outliner.Controls.Tree.TreeView();
         this.itemPropertyGrid = new System.Windows.Forms.PropertyGrid();
         this.moveUpBtn = new System.Windows.Forms.Button();
         this.moveDownBtn = new System.Windows.Forms.Button();
         this.deleteBtn = new System.Windows.Forms.Button();
         this.outlinerGroupBox1 = new Outliner.Controls.OutlinerGroupBox();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.newBtn = new System.Windows.Forms.Button();
         this.contextMenuFileComboBox = new System.Windows.Forms.ComboBox();
         this.outlinerGroupBox2.SuspendLayout();
         this.tableLayoutPanel2.SuspendLayout();
         this.outlinerGroupBox1.SuspendLayout();
         this.tableLayoutPanel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // outlinerGroupBox2
         // 
         this.outlinerGroupBox2.BorderColor = System.Drawing.Color.Black;
         this.outlinerGroupBox2.Controls.Add(this.tableLayoutPanel2);
         this.outlinerGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.outlinerGroupBox2.Location = new System.Drawing.Point(0, 53);
         this.outlinerGroupBox2.Name = "outlinerGroupBox2";
         this.outlinerGroupBox2.Padding = new System.Windows.Forms.Padding(5);
         this.outlinerGroupBox2.Size = new System.Drawing.Size(454, 346);
         this.outlinerGroupBox2.TabIndex = 9;
         this.outlinerGroupBox2.TabStop = false;
         this.outlinerGroupBox2.Text = "Context-Menu Items";
         // 
         // tableLayoutPanel2
         // 
         this.tableLayoutPanel2.ColumnCount = 2;
         this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
         this.tableLayoutPanel2.Controls.Add(this.itemsComboBox, 0, 0);
         this.tableLayoutPanel2.Controls.Add(this.addBtn, 1, 0);
         this.tableLayoutPanel2.Controls.Add(this.itemTree, 0, 1);
         this.tableLayoutPanel2.Controls.Add(this.itemPropertyGrid, 0, 4);
         this.tableLayoutPanel2.Controls.Add(this.moveUpBtn, 1, 1);
         this.tableLayoutPanel2.Controls.Add(this.moveDownBtn, 1, 2);
         this.tableLayoutPanel2.Controls.Add(this.deleteBtn, 1, 3);
         this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 18);
         this.tableLayoutPanel2.Name = "tableLayoutPanel2";
         this.tableLayoutPanel2.RowCount = 5;
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel2.Size = new System.Drawing.Size(444, 323);
         this.tableLayoutPanel2.TabIndex = 0;
         // 
         // itemsComboBox
         // 
         this.itemsComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.itemsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.itemsComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.itemsComboBox.FormattingEnabled = true;
         this.itemsComboBox.Location = new System.Drawing.Point(3, 3);
         this.itemsComboBox.Name = "itemsComboBox";
         this.itemsComboBox.Size = new System.Drawing.Size(378, 21);
         this.itemsComboBox.TabIndex = 0;
         // 
         // addBtn
         // 
         this.addBtn.Dock = System.Windows.Forms.DockStyle.Fill;
         this.addBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.addBtn.Font = new System.Drawing.Font("Tahoma", 8F);
         this.addBtn.Location = new System.Drawing.Point(387, 2);
         this.addBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
         this.addBtn.Name = "addBtn";
         this.addBtn.Size = new System.Drawing.Size(54, 23);
         this.addBtn.TabIndex = 1;
         this.addBtn.Text = "Add";
         this.addBtn.UseVisualStyleBackColor = true;
         // 
         // itemTree
         // 
         this.itemTree.AllowDrop = true;
         this.itemTree.AutoScroll = true;
         this.itemTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.itemTree.Dock = System.Windows.Forms.DockStyle.Fill;
         this.itemTree.Location = new System.Drawing.Point(3, 31);
         this.itemTree.Name = "itemTree";
         this.tableLayoutPanel2.SetRowSpan(this.itemTree, 3);
         this.itemTree.Size = new System.Drawing.Size(378, 169);
         this.itemTree.TabIndex = 2;
         this.itemTree.Text = "treeView1";
         this.itemTree.SelectionChanged += new System.EventHandler<Outliner.Controls.Tree.SelectionChangedEventArgs>(this.itemTree_SelectionChanged);
         // 
         // itemPropertyGrid
         // 
         this.itemPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
         this.itemPropertyGrid.HelpVisible = false;
         this.itemPropertyGrid.Location = new System.Drawing.Point(3, 206);
         this.itemPropertyGrid.Name = "itemPropertyGrid";
         this.itemPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
         this.itemPropertyGrid.Size = new System.Drawing.Size(378, 114);
         this.itemPropertyGrid.TabIndex = 3;
         this.itemPropertyGrid.ToolbarVisible = false;
         // 
         // moveUpBtn
         // 
         this.moveUpBtn.Dock = System.Windows.Forms.DockStyle.Fill;
         this.moveUpBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.moveUpBtn.Font = new System.Drawing.Font("Tahoma", 8F);
         this.moveUpBtn.Location = new System.Drawing.Point(387, 31);
         this.moveUpBtn.Name = "moveUpBtn";
         this.moveUpBtn.Size = new System.Drawing.Size(54, 22);
         this.moveUpBtn.TabIndex = 4;
         this.moveUpBtn.Text = "Up";
         this.moveUpBtn.UseVisualStyleBackColor = true;
         // 
         // moveDownBtn
         // 
         this.moveDownBtn.Dock = System.Windows.Forms.DockStyle.Fill;
         this.moveDownBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.moveDownBtn.Font = new System.Drawing.Font("Tahoma", 8F);
         this.moveDownBtn.Location = new System.Drawing.Point(387, 59);
         this.moveDownBtn.Name = "moveDownBtn";
         this.moveDownBtn.Size = new System.Drawing.Size(54, 22);
         this.moveDownBtn.TabIndex = 5;
         this.moveDownBtn.Text = "Down";
         this.moveDownBtn.UseVisualStyleBackColor = true;
         // 
         // deleteBtn
         // 
         this.deleteBtn.Dock = System.Windows.Forms.DockStyle.Top;
         this.deleteBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.deleteBtn.Font = new System.Drawing.Font("Tahoma", 8F);
         this.deleteBtn.Location = new System.Drawing.Point(387, 87);
         this.deleteBtn.Name = "deleteBtn";
         this.deleteBtn.Size = new System.Drawing.Size(54, 22);
         this.deleteBtn.TabIndex = 6;
         this.deleteBtn.Text = "Delete";
         this.deleteBtn.UseVisualStyleBackColor = true;
         // 
         // outlinerGroupBox1
         // 
         this.outlinerGroupBox1.BorderColor = System.Drawing.Color.Black;
         this.outlinerGroupBox1.Controls.Add(this.tableLayoutPanel1);
         this.outlinerGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
         this.outlinerGroupBox1.Location = new System.Drawing.Point(0, 0);
         this.outlinerGroupBox1.Name = "outlinerGroupBox1";
         this.outlinerGroupBox1.Padding = new System.Windows.Forms.Padding(8, 5, 5, 5);
         this.outlinerGroupBox1.Size = new System.Drawing.Size(454, 53);
         this.outlinerGroupBox1.TabIndex = 8;
         this.outlinerGroupBox1.TabStop = false;
         this.outlinerGroupBox1.Text = "Context-Menu File";
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 2;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
         this.tableLayoutPanel1.Controls.Add(this.newBtn, 1, 0);
         this.tableLayoutPanel1.Controls.Add(this.contextMenuFileComboBox, 0, 0);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(8, 18);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 1;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 308F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(441, 30);
         this.tableLayoutPanel1.TabIndex = 4;
         // 
         // newBtn
         // 
         this.newBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.newBtn.Font = new System.Drawing.Font("Tahoma", 8F);
         this.newBtn.Location = new System.Drawing.Point(384, 2);
         this.newBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
         this.newBtn.Name = "newBtn";
         this.newBtn.Size = new System.Drawing.Size(54, 23);
         this.newBtn.TabIndex = 0;
         this.newBtn.Text = "New";
         this.newBtn.UseVisualStyleBackColor = true;
         // 
         // contextMenuFileComboBox
         // 
         this.contextMenuFileComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.contextMenuFileComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.contextMenuFileComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.contextMenuFileComboBox.FormattingEnabled = true;
         this.contextMenuFileComboBox.Location = new System.Drawing.Point(3, 3);
         this.contextMenuFileComboBox.Name = "contextMenuFileComboBox";
         this.contextMenuFileComboBox.Size = new System.Drawing.Size(375, 21);
         this.contextMenuFileComboBox.TabIndex = 1;
         // 
         // ContextMenuModelEditor
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.outlinerGroupBox2);
         this.Controls.Add(this.outlinerGroupBox1);
         this.Name = "ContextMenuModelEditor";
         this.Size = new System.Drawing.Size(454, 399);
         this.outlinerGroupBox2.ResumeLayout(false);
         this.tableLayoutPanel2.ResumeLayout(false);
         this.outlinerGroupBox1.ResumeLayout(false);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private OutlinerGroupBox outlinerGroupBox1;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.Button newBtn;
      private System.Windows.Forms.ComboBox contextMenuFileComboBox;
      private OutlinerGroupBox outlinerGroupBox2;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
      private System.Windows.Forms.ComboBox itemsComboBox;
      private System.Windows.Forms.Button addBtn;
      private Tree.TreeView itemTree;
      private System.Windows.Forms.PropertyGrid itemPropertyGrid;
      private System.Windows.Forms.Button moveUpBtn;
      private System.Windows.Forms.Button moveDownBtn;
      private System.Windows.Forms.Button deleteBtn;
   }
}
