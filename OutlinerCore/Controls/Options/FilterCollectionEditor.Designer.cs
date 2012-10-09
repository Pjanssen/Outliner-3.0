namespace Outliner.Controls.Options
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
         this.outlinerGroupBox1 = new Outliner.Controls.OutlinerGroupBox();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.filtersComboBox = new System.Windows.Forms.ComboBox();
         this.addFilterButton = new System.Windows.Forms.Button();
         this.filtersTree = new Outliner.Controls.Tree.TreeView();
         this.deleteFilterButton = new System.Windows.Forms.Button();
         this.filterPropertyGrid = new System.Windows.Forms.PropertyGrid();
         this.enabledCheckBox = new System.Windows.Forms.CheckBox();
         this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
         this.combinatorLabel = new System.Windows.Forms.Label();
         this.combinatorOR = new System.Windows.Forms.RadioButton();
         this.combinatorAND = new System.Windows.Forms.RadioButton();
         this.outlinerGroupBox1.SuspendLayout();
         this.tableLayoutPanel1.SuspendLayout();
         this.tableLayoutPanel2.SuspendLayout();
         this.SuspendLayout();
         // 
         // outlinerGroupBox1
         // 
         this.outlinerGroupBox1.BorderColor = System.Drawing.Color.Black;
         this.outlinerGroupBox1.Controls.Add(this.tableLayoutPanel1);
         this.outlinerGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.outlinerGroupBox1.Location = new System.Drawing.Point(0, 0);
         this.outlinerGroupBox1.Name = "outlinerGroupBox1";
         this.outlinerGroupBox1.Padding = new System.Windows.Forms.Padding(5);
         this.outlinerGroupBox1.Size = new System.Drawing.Size(469, 465);
         this.outlinerGroupBox1.TabIndex = 2;
         this.outlinerGroupBox1.TabStop = false;
         this.outlinerGroupBox1.Text = "Filters";
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 3;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.Controls.Add(this.filtersComboBox, 0, 1);
         this.tableLayoutPanel1.Controls.Add(this.addFilterButton, 1, 1);
         this.tableLayoutPanel1.Controls.Add(this.filtersTree, 0, 2);
         this.tableLayoutPanel1.Controls.Add(this.deleteFilterButton, 1, 2);
         this.tableLayoutPanel1.Controls.Add(this.filterPropertyGrid, 2, 2);
         this.tableLayoutPanel1.Controls.Add(this.enabledCheckBox, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 18);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 3;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(459, 442);
         this.tableLayoutPanel1.TabIndex = 2;
         // 
         // filtersComboBox
         // 
         this.filtersComboBox.Dock = System.Windows.Forms.DockStyle.Top;
         this.filtersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.filtersComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.filtersComboBox.FormattingEnabled = true;
         this.filtersComboBox.Location = new System.Drawing.Point(3, 33);
         this.filtersComboBox.Name = "filtersComboBox";
         this.filtersComboBox.Size = new System.Drawing.Size(183, 21);
         this.filtersComboBox.TabIndex = 0;
         // 
         // addFilterButton
         // 
         this.addFilterButton.Dock = System.Windows.Forms.DockStyle.Top;
         this.addFilterButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.addFilterButton.Location = new System.Drawing.Point(192, 32);
         this.addFilterButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
         this.addFilterButton.Name = "addFilterButton";
         this.addFilterButton.Size = new System.Drawing.Size(74, 23);
         this.addFilterButton.TabIndex = 1;
         this.addFilterButton.Text = "Add";
         this.addFilterButton.UseVisualStyleBackColor = true;
         // 
         // filtersTree
         // 
         this.filtersTree.AllowDrop = true;
         this.filtersTree.AutoScroll = true;
         this.filtersTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.filtersTree.Dock = System.Windows.Forms.DockStyle.Fill;
         this.filtersTree.Location = new System.Drawing.Point(3, 63);
         this.filtersTree.Name = "filtersTree";
         this.filtersTree.Size = new System.Drawing.Size(183, 376);
         this.filtersTree.TabIndex = 2;
         this.filtersTree.Text = "treeView1";
         // 
         // deleteFilterButton
         // 
         this.deleteFilterButton.Dock = System.Windows.Forms.DockStyle.Top;
         this.deleteFilterButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.deleteFilterButton.Location = new System.Drawing.Point(192, 63);
         this.deleteFilterButton.Name = "deleteFilterButton";
         this.deleteFilterButton.Size = new System.Drawing.Size(74, 23);
         this.deleteFilterButton.TabIndex = 3;
         this.deleteFilterButton.Text = "Delete";
         this.deleteFilterButton.UseVisualStyleBackColor = true;
         // 
         // filterPropertyGrid
         // 
         this.filterPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
         this.filterPropertyGrid.HelpVisible = false;
         this.filterPropertyGrid.Location = new System.Drawing.Point(272, 63);
         this.filterPropertyGrid.Name = "filterPropertyGrid";
         this.filterPropertyGrid.Size = new System.Drawing.Size(184, 376);
         this.filterPropertyGrid.TabIndex = 4;
         this.filterPropertyGrid.ToolbarVisible = false;
         // 
         // enabledCheckBox
         // 
         this.enabledCheckBox.AutoSize = true;
         this.enabledCheckBox.Location = new System.Drawing.Point(3, 3);
         this.enabledCheckBox.Name = "enabledCheckBox";
         this.enabledCheckBox.Size = new System.Drawing.Size(65, 17);
         this.enabledCheckBox.TabIndex = 5;
         this.enabledCheckBox.Text = "Enabled";
         this.enabledCheckBox.UseVisualStyleBackColor = true;
         this.enabledCheckBox.CheckedChanged += new System.EventHandler(this.enabledCheckBox_CheckedChanged);
         // 
         // tableLayoutPanel2
         // 
         this.tableLayoutPanel2.ColumnCount = 3;
         this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 2);
         this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
         this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 57F));
         this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 132F));
         this.tableLayoutPanel2.Controls.Add(this.combinatorLabel, 0, 0);
         this.tableLayoutPanel2.Controls.Add(this.combinatorOR, 1, 0);
         this.tableLayoutPanel2.Controls.Add(this.combinatorAND, 2, 0);
         this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel2.Location = new System.Drawing.Point(189, 0);
         this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
         this.tableLayoutPanel2.Name = "tableLayoutPanel2";
         this.tableLayoutPanel2.RowCount = 1;
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel2.Size = new System.Drawing.Size(270, 30);
         this.tableLayoutPanel2.TabIndex = 9;
         // 
         // combinatorLabel
         // 
         this.combinatorLabel.AutoSize = true;
         this.combinatorLabel.Location = new System.Drawing.Point(3, 5);
         this.combinatorLabel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
         this.combinatorLabel.Name = "combinatorLabel";
         this.combinatorLabel.Size = new System.Drawing.Size(60, 13);
         this.combinatorLabel.TabIndex = 8;
         this.combinatorLabel.Text = "Combinator";
         // 
         // combinatorOR
         // 
         this.combinatorOR.AutoSize = true;
         this.combinatorOR.Location = new System.Drawing.Point(84, 3);
         this.combinatorOR.Name = "combinatorOR";
         this.combinatorOR.Size = new System.Drawing.Size(41, 17);
         this.combinatorOR.TabIndex = 6;
         this.combinatorOR.TabStop = true;
         this.combinatorOR.Text = "OR";
         this.combinatorOR.UseVisualStyleBackColor = true;
         // 
         // combinatorAND
         // 
         this.combinatorAND.AutoSize = true;
         this.combinatorAND.Location = new System.Drawing.Point(141, 3);
         this.combinatorAND.Name = "combinatorAND";
         this.combinatorAND.Size = new System.Drawing.Size(48, 17);
         this.combinatorAND.TabIndex = 7;
         this.combinatorAND.TabStop = true;
         this.combinatorAND.Text = "AND";
         this.combinatorAND.UseVisualStyleBackColor = true;
         // 
         // FilterCollectionEditor
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.outlinerGroupBox1);
         this.Name = "FilterCollectionEditor";
         this.Size = new System.Drawing.Size(469, 465);
         this.outlinerGroupBox1.ResumeLayout(false);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.tableLayoutPanel1.PerformLayout();
         this.tableLayoutPanel2.ResumeLayout(false);
         this.tableLayoutPanel2.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private OutlinerGroupBox outlinerGroupBox1;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.ComboBox filtersComboBox;
      private System.Windows.Forms.Button addFilterButton;
      private Tree.TreeView filtersTree;
      private System.Windows.Forms.Button deleteFilterButton;
      private System.Windows.Forms.PropertyGrid filterPropertyGrid;
      private System.Windows.Forms.CheckBox enabledCheckBox;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
      private System.Windows.Forms.Label combinatorLabel;
      private System.Windows.Forms.RadioButton combinatorOR;
      private System.Windows.Forms.RadioButton combinatorAND;

   }
}
