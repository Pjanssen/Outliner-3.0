namespace PJanssen.Outliner.Controls.Options
{
   partial class FilterCollectionEditor
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
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.filtersComboBox = new System.Windows.Forms.ComboBox();
         this.addFilterButton = new System.Windows.Forms.Button();
         this.filtersTree = new Outliner.Controls.Tree.TreeView();
         this.deleteFilterButton = new System.Windows.Forms.Button();
         this.filterPropertyGrid = new System.Windows.Forms.PropertyGrid();
         this.outlinerGroupBox1 = new Outliner.Controls.OutlinerGroupBox();
         this.tableLayoutPanel1.SuspendLayout();
         this.outlinerGroupBox1.SuspendLayout();
         this.SuspendLayout();
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 2;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tableLayoutPanel1.Controls.Add(this.filtersComboBox, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.addFilterButton, 1, 0);
         this.tableLayoutPanel1.Controls.Add(this.filtersTree, 0, 1);
         this.tableLayoutPanel1.Controls.Add(this.deleteFilterButton, 1, 1);
         this.tableLayoutPanel1.Controls.Add(this.filterPropertyGrid, 0, 2);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 3;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(463, 446);
         this.tableLayoutPanel1.TabIndex = 2;
         // 
         // filtersComboBox
         // 
         this.filtersComboBox.Dock = System.Windows.Forms.DockStyle.Top;
         this.filtersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.filtersComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.filtersComboBox.FormattingEnabled = true;
         this.filtersComboBox.Location = new System.Drawing.Point(3, 3);
         this.filtersComboBox.Name = "filtersComboBox";
         this.filtersComboBox.Size = new System.Drawing.Size(376, 21);
         this.filtersComboBox.TabIndex = 0;
         // 
         // addFilterButton
         // 
         this.addFilterButton.Dock = System.Windows.Forms.DockStyle.Top;
         this.addFilterButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.addFilterButton.Location = new System.Drawing.Point(385, 2);
         this.addFilterButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
         this.addFilterButton.Name = "addFilterButton";
         this.addFilterButton.Size = new System.Drawing.Size(75, 23);
         this.addFilterButton.TabIndex = 1;
         this.addFilterButton.Text = "Add";
         this.addFilterButton.UseVisualStyleBackColor = true;
         this.addFilterButton.Click += new System.EventHandler(this.addFilterButton_Click);
         // 
         // filtersTree
         // 
         this.filtersTree.AllowDrop = true;
         this.filtersTree.AutoScroll = true;
         this.filtersTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.filtersTree.Dock = System.Windows.Forms.DockStyle.Fill;
         this.filtersTree.Location = new System.Drawing.Point(3, 33);
         this.filtersTree.Name = "filtersTree";
         this.filtersTree.Size = new System.Drawing.Size(376, 243);
         this.filtersTree.TabIndex = 2;
         this.filtersTree.Text = "treeView1";
         this.filtersTree.SelectionChanged += new System.EventHandler<Outliner.Controls.Tree.SelectionChangedEventArgs>(this.filtersTree_SelectionChanged);
         // 
         // deleteFilterButton
         // 
         this.deleteFilterButton.Dock = System.Windows.Forms.DockStyle.Top;
         this.deleteFilterButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.deleteFilterButton.Location = new System.Drawing.Point(385, 33);
         this.deleteFilterButton.Name = "deleteFilterButton";
         this.deleteFilterButton.Size = new System.Drawing.Size(75, 23);
         this.deleteFilterButton.TabIndex = 3;
         this.deleteFilterButton.Text = "Delete";
         this.deleteFilterButton.UseVisualStyleBackColor = true;
         this.deleteFilterButton.Click += new System.EventHandler(this.deleteFilterButton_Click);
         // 
         // filterPropertyGrid
         // 
         this.filterPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
         this.filterPropertyGrid.HelpVisible = false;
         this.filterPropertyGrid.Location = new System.Drawing.Point(3, 282);
         this.filterPropertyGrid.Name = "filterPropertyGrid";
         this.filterPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
         this.filterPropertyGrid.Size = new System.Drawing.Size(376, 161);
         this.filterPropertyGrid.TabIndex = 4;
         this.filterPropertyGrid.ToolbarVisible = false;
         // 
         // outlinerGroupBox1
         // 
         this.outlinerGroupBox1.BorderColor = System.Drawing.Color.Black;
         this.outlinerGroupBox1.Controls.Add(this.tableLayoutPanel1);
         this.outlinerGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.outlinerGroupBox1.Location = new System.Drawing.Point(0, 0);
         this.outlinerGroupBox1.Name = "outlinerGroupBox1";
         this.outlinerGroupBox1.Size = new System.Drawing.Size(469, 465);
         this.outlinerGroupBox1.TabIndex = 5;
         this.outlinerGroupBox1.TabStop = false;
         this.outlinerGroupBox1.Text = "Filter Configuration";
         // 
         // FilterCollectionEditor
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.outlinerGroupBox1);
         this.Name = "FilterCollectionEditor";
         this.Size = new System.Drawing.Size(469, 465);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.outlinerGroupBox1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.ComboBox filtersComboBox;
      private System.Windows.Forms.Button addFilterButton;
      private Tree.TreeView filtersTree;
      private System.Windows.Forms.Button deleteFilterButton;
      private System.Windows.Forms.PropertyGrid filterPropertyGrid;
      private OutlinerGroupBox outlinerGroupBox1;

   }
}
