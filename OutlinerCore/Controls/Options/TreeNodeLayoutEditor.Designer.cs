namespace Outliner.Controls.Options
{
   partial class TreeNodeLayoutEditor
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
         this.layoutBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.outlinerGroupBox1 = new Outliner.Controls.OutlinerGroupBox();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.paddingRightSpinner = new System.Windows.Forms.NumericUpDown();
         this.label3 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.paddingLeftSpinner = new System.Windows.Forms.NumericUpDown();
         this.itemHeightSpinner = new System.Windows.Forms.NumericUpDown();
         this.label1 = new System.Windows.Forms.Label();
         this.fullRowSelectCheckBox = new System.Windows.Forms.CheckBox();
         this.outlinerGroupBox2 = new Outliner.Controls.OutlinerGroupBox();
         this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
         this.layoutComboBox = new System.Windows.Forms.ComboBox();
         this.addButton = new System.Windows.Forms.Button();
         this.deleteButton = new System.Windows.Forms.Button();
         this.upButton = new System.Windows.Forms.Button();
         this.downButton = new System.Windows.Forms.Button();
         this.layoutTree = new Outliner.Controls.Tree.TreeView();
         this.itemProperties = new System.Windows.Forms.PropertyGrid();
         ((System.ComponentModel.ISupportInitialize)(this.layoutBindingSource)).BeginInit();
         this.outlinerGroupBox1.SuspendLayout();
         this.tableLayoutPanel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.paddingRightSpinner)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.paddingLeftSpinner)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.itemHeightSpinner)).BeginInit();
         this.outlinerGroupBox2.SuspendLayout();
         this.tableLayoutPanel3.SuspendLayout();
         this.SuspendLayout();
         // 
         // layoutBindingSource
         // 
         this.layoutBindingSource.DataSource = typeof(Outliner.Controls.Tree.Layout.TreeNodeLayout);
         this.layoutBindingSource.BindingComplete += new System.Windows.Forms.BindingCompleteEventHandler(this.layoutBindingSource_BindingComplete);
         // 
         // outlinerGroupBox1
         // 
         this.outlinerGroupBox1.BorderColor = System.Drawing.Color.Black;
         this.outlinerGroupBox1.Controls.Add(this.tableLayoutPanel1);
         this.outlinerGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
         this.outlinerGroupBox1.Location = new System.Drawing.Point(0, 0);
         this.outlinerGroupBox1.Name = "outlinerGroupBox1";
         this.outlinerGroupBox1.Padding = new System.Windows.Forms.Padding(8, 5, 5, 5);
         this.outlinerGroupBox1.Size = new System.Drawing.Size(467, 77);
         this.outlinerGroupBox1.TabIndex = 3;
         this.outlinerGroupBox1.TabStop = false;
         this.outlinerGroupBox1.Text = "Layout Properties";
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 4;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.Controls.Add(this.paddingRightSpinner, 2, 1);
         this.tableLayoutPanel1.Controls.Add(this.label3, 3, 1);
         this.tableLayoutPanel1.Controls.Add(this.label2, 3, 0);
         this.tableLayoutPanel1.Controls.Add(this.paddingLeftSpinner, 2, 0);
         this.tableLayoutPanel1.Controls.Add(this.itemHeightSpinner, 0, 1);
         this.tableLayoutPanel1.Controls.Add(this.label1, 1, 1);
         this.tableLayoutPanel1.Controls.Add(this.fullRowSelectCheckBox, 0, 0);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(8, 18);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 2;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(454, 54);
         this.tableLayoutPanel1.TabIndex = 3;
         // 
         // paddingRightSpinner
         // 
         this.paddingRightSpinner.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.layoutBindingSource, "PaddingRight", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.paddingRightSpinner.Dock = System.Windows.Forms.DockStyle.Fill;
         this.paddingRightSpinner.Location = new System.Drawing.Point(230, 28);
         this.paddingRightSpinner.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
         this.paddingRightSpinner.Name = "paddingRightSpinner";
         this.paddingRightSpinner.Size = new System.Drawing.Size(44, 20);
         this.paddingRightSpinner.TabIndex = 6;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(280, 31);
         this.label3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(74, 13);
         this.label3.TabIndex = 5;
         this.label3.Text = "Padding Right";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(280, 6);
         this.label2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(67, 13);
         this.label2.TabIndex = 4;
         this.label2.Text = "Padding Left";
         // 
         // paddingLeftSpinner
         // 
         this.paddingLeftSpinner.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.layoutBindingSource, "PaddingLeft", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.paddingLeftSpinner.Dock = System.Windows.Forms.DockStyle.Fill;
         this.paddingLeftSpinner.Location = new System.Drawing.Point(230, 3);
         this.paddingLeftSpinner.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
         this.paddingLeftSpinner.Name = "paddingLeftSpinner";
         this.paddingLeftSpinner.Size = new System.Drawing.Size(44, 20);
         this.paddingLeftSpinner.TabIndex = 3;
         // 
         // itemHeightSpinner
         // 
         this.itemHeightSpinner.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.layoutBindingSource, "ItemHeight", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.itemHeightSpinner.Dock = System.Windows.Forms.DockStyle.Fill;
         this.itemHeightSpinner.Location = new System.Drawing.Point(3, 28);
         this.itemHeightSpinner.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
         this.itemHeightSpinner.Name = "itemHeightSpinner";
         this.itemHeightSpinner.Size = new System.Drawing.Size(44, 20);
         this.itemHeightSpinner.TabIndex = 0;
         this.itemHeightSpinner.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(53, 31);
         this.label1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(56, 13);
         this.label1.TabIndex = 1;
         this.label1.Text = "Itemheight";
         // 
         // fullRowSelectCheckBox
         // 
         this.fullRowSelectCheckBox.AutoSize = true;
         this.tableLayoutPanel1.SetColumnSpan(this.fullRowSelectCheckBox, 2);
         this.fullRowSelectCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.layoutBindingSource, "FullRowSelect", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.fullRowSelectCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.fullRowSelectCheckBox.Location = new System.Drawing.Point(3, 3);
         this.fullRowSelectCheckBox.Name = "fullRowSelectCheckBox";
         this.fullRowSelectCheckBox.Size = new System.Drawing.Size(101, 18);
         this.fullRowSelectCheckBox.TabIndex = 2;
         this.fullRowSelectCheckBox.Text = "Full-row Select";
         this.fullRowSelectCheckBox.UseVisualStyleBackColor = true;
         // 
         // outlinerGroupBox2
         // 
         this.outlinerGroupBox2.BorderColor = System.Drawing.Color.Black;
         this.outlinerGroupBox2.Controls.Add(this.tableLayoutPanel3);
         this.outlinerGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.outlinerGroupBox2.Location = new System.Drawing.Point(0, 77);
         this.outlinerGroupBox2.Name = "outlinerGroupBox2";
         this.outlinerGroupBox2.Padding = new System.Windows.Forms.Padding(5);
         this.outlinerGroupBox2.Size = new System.Drawing.Size(467, 292);
         this.outlinerGroupBox2.TabIndex = 4;
         this.outlinerGroupBox2.TabStop = false;
         this.outlinerGroupBox2.Text = "Layout Items";
         // 
         // tableLayoutPanel3
         // 
         this.tableLayoutPanel3.ColumnCount = 3;
         this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 177F));
         this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
         this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel3.Controls.Add(this.layoutComboBox, 0, 0);
         this.tableLayoutPanel3.Controls.Add(this.addButton, 1, 0);
         this.tableLayoutPanel3.Controls.Add(this.deleteButton, 1, 3);
         this.tableLayoutPanel3.Controls.Add(this.upButton, 1, 1);
         this.tableLayoutPanel3.Controls.Add(this.downButton, 1, 2);
         this.tableLayoutPanel3.Controls.Add(this.layoutTree, 0, 1);
         this.tableLayoutPanel3.Controls.Add(this.itemProperties, 2, 1);
         this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 18);
         this.tableLayoutPanel3.Name = "tableLayoutPanel3";
         this.tableLayoutPanel3.RowCount = 4;
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tableLayoutPanel3.Size = new System.Drawing.Size(457, 269);
         this.tableLayoutPanel3.TabIndex = 2;
         // 
         // layoutComboBox
         // 
         this.layoutComboBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
         this.layoutComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.layoutComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.layoutComboBox.FormattingEnabled = true;
         this.layoutComboBox.Location = new System.Drawing.Point(3, 5);
         this.layoutComboBox.Name = "layoutComboBox";
         this.layoutComboBox.Size = new System.Drawing.Size(171, 21);
         this.layoutComboBox.TabIndex = 0;
         // 
         // addButton
         // 
         this.addButton.Dock = System.Windows.Forms.DockStyle.Fill;
         this.addButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.addButton.Location = new System.Drawing.Point(180, 2);
         this.addButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
         this.addButton.Name = "addButton";
         this.addButton.Size = new System.Drawing.Size(46, 27);
         this.addButton.TabIndex = 1;
         this.addButton.Text = "Add";
         this.addButton.UseVisualStyleBackColor = true;
         // 
         // deleteButton
         // 
         this.deleteButton.Dock = System.Windows.Forms.DockStyle.Top;
         this.deleteButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.deleteButton.Location = new System.Drawing.Point(180, 99);
         this.deleteButton.Name = "deleteButton";
         this.deleteButton.Size = new System.Drawing.Size(46, 27);
         this.deleteButton.TabIndex = 2;
         this.deleteButton.Text = "Delete";
         this.deleteButton.UseVisualStyleBackColor = true;
         // 
         // upButton
         // 
         this.upButton.Dock = System.Windows.Forms.DockStyle.Top;
         this.upButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.upButton.Location = new System.Drawing.Point(180, 35);
         this.upButton.Name = "upButton";
         this.upButton.Size = new System.Drawing.Size(46, 24);
         this.upButton.TabIndex = 3;
         this.upButton.Text = "Up";
         this.upButton.UseVisualStyleBackColor = true;
         // 
         // downButton
         // 
         this.downButton.Dock = System.Windows.Forms.DockStyle.Top;
         this.downButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.downButton.Location = new System.Drawing.Point(180, 65);
         this.downButton.Name = "downButton";
         this.downButton.Size = new System.Drawing.Size(46, 27);
         this.downButton.TabIndex = 4;
         this.downButton.Text = "Down";
         this.downButton.UseVisualStyleBackColor = true;
         // 
         // layoutTree
         // 
         this.layoutTree.AllowDrop = true;
         this.layoutTree.AutoScroll = true;
         this.layoutTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.layoutTree.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutTree.Location = new System.Drawing.Point(3, 35);
         this.layoutTree.Name = "layoutTree";
         this.tableLayoutPanel3.SetRowSpan(this.layoutTree, 3);
         this.layoutTree.Size = new System.Drawing.Size(171, 231);
         this.layoutTree.TabIndex = 5;
         this.layoutTree.Text = "treeView2";
         // 
         // itemProperties
         // 
         this.itemProperties.CommandsVisibleIfAvailable = false;
         this.itemProperties.Dock = System.Windows.Forms.DockStyle.Fill;
         this.itemProperties.HelpVisible = false;
         this.itemProperties.Location = new System.Drawing.Point(232, 35);
         this.itemProperties.Name = "itemProperties";
         this.itemProperties.PropertySort = System.Windows.Forms.PropertySort.NoSort;
         this.tableLayoutPanel3.SetRowSpan(this.itemProperties, 3);
         this.itemProperties.Size = new System.Drawing.Size(222, 231);
         this.itemProperties.TabIndex = 6;
         this.itemProperties.ToolbarVisible = false;
         // 
         // TreeNodeLayoutEditor
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.outlinerGroupBox2);
         this.Controls.Add(this.outlinerGroupBox1);
         this.Name = "TreeNodeLayoutEditor";
         this.Size = new System.Drawing.Size(467, 369);
         ((System.ComponentModel.ISupportInitialize)(this.layoutBindingSource)).EndInit();
         this.outlinerGroupBox1.ResumeLayout(false);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.tableLayoutPanel1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.paddingRightSpinner)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.paddingLeftSpinner)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.itemHeightSpinner)).EndInit();
         this.outlinerGroupBox2.ResumeLayout(false);
         this.tableLayoutPanel3.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.BindingSource layoutBindingSource;
      private OutlinerGroupBox outlinerGroupBox1;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.NumericUpDown paddingRightSpinner;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.NumericUpDown paddingLeftSpinner;
      private System.Windows.Forms.NumericUpDown itemHeightSpinner;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.CheckBox fullRowSelectCheckBox;
      private OutlinerGroupBox outlinerGroupBox2;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
      private System.Windows.Forms.ComboBox layoutComboBox;
      private System.Windows.Forms.Button addButton;
      private System.Windows.Forms.Button deleteButton;
      private System.Windows.Forms.Button upButton;
      private System.Windows.Forms.Button downButton;
      private Tree.TreeView layoutTree;
      private System.Windows.Forms.PropertyGrid itemProperties;
   }
}
