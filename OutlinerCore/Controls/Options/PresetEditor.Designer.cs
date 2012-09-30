namespace Outliner.Controls.Options
{
   partial class PresetEditor
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

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.propertiesPanel = new System.Windows.Forms.TableLayoutPanel();
         this.layoutGroupBox = new Outliner.Controls.OutlinerGroupBox();
         this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
         this.layoutComboBox = new System.Windows.Forms.ComboBox();
         this.button3 = new System.Windows.Forms.Button();
         this.button4 = new System.Windows.Forms.Button();
         this.button5 = new System.Windows.Forms.Button();
         this.button6 = new System.Windows.Forms.Button();
         this.layoutTree = new Outliner.Controls.Tree.TreeView();
         this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
         this.addDeletePanel = new System.Windows.Forms.TableLayoutPanel();
         this.deleteBtn = new System.Windows.Forms.Button();
         this.addBtn = new System.Windows.Forms.Button();
         this.presetsTree = new Outliner.Controls.Tree.TreeView();
         this.okCancelPanel = new System.Windows.Forms.TableLayoutPanel();
         this.cancelBtn = new System.Windows.Forms.Button();
         this.okBtn = new System.Windows.Forms.Button();
         this.filterGroupBox = new Outliner.Controls.OutlinerGroupBox();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.filtersComboBox = new System.Windows.Forms.ComboBox();
         this.button1 = new System.Windows.Forms.Button();
         this.filtersTree = new Outliner.Controls.Tree.TreeView();
         this.button2 = new System.Windows.Forms.Button();
         this.propertyGrid2 = new System.Windows.Forms.PropertyGrid();
         this.propertiesGroupBox = new Outliner.Controls.OutlinerGroupBox();
         this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
         this.sorterComboBox = new System.Windows.Forms.ComboBox();
         this.label3 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.nameTextBox = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.modeComboBox = new System.Windows.Forms.ComboBox();
         this.propertiesPanel.SuspendLayout();
         this.layoutGroupBox.SuspendLayout();
         this.tableLayoutPanel3.SuspendLayout();
         this.addDeletePanel.SuspendLayout();
         this.okCancelPanel.SuspendLayout();
         this.filterGroupBox.SuspendLayout();
         this.tableLayoutPanel1.SuspendLayout();
         this.propertiesGroupBox.SuspendLayout();
         this.tableLayoutPanel2.SuspendLayout();
         this.SuspendLayout();
         // 
         // propertiesPanel
         // 
         this.propertiesPanel.ColumnCount = 3;
         this.propertiesPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
         this.propertiesPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
         this.propertiesPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.propertiesPanel.Controls.Add(this.layoutGroupBox, 1, 2);
         this.propertiesPanel.Controls.Add(this.addDeletePanel, 0, 3);
         this.propertiesPanel.Controls.Add(this.presetsTree, 0, 0);
         this.propertiesPanel.Controls.Add(this.okCancelPanel, 2, 4);
         this.propertiesPanel.Controls.Add(this.filterGroupBox, 1, 1);
         this.propertiesPanel.Controls.Add(this.propertiesGroupBox, 1, 0);
         this.propertiesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.propertiesPanel.Location = new System.Drawing.Point(0, 0);
         this.propertiesPanel.Name = "propertiesPanel";
         this.propertiesPanel.RowCount = 5;
         this.propertiesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 106F));
         this.propertiesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 144F));
         this.propertiesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.propertiesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
         this.propertiesPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
         this.propertiesPanel.Size = new System.Drawing.Size(704, 543);
         this.propertiesPanel.TabIndex = 9;
         // 
         // layoutGroupBox
         // 
         this.layoutGroupBox.BorderColor = System.Drawing.Color.Black;
         this.propertiesPanel.SetColumnSpan(this.layoutGroupBox, 2);
         this.layoutGroupBox.Controls.Add(this.tableLayoutPanel3);
         this.layoutGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutGroupBox.Location = new System.Drawing.Point(203, 253);
         this.layoutGroupBox.Margin = new System.Windows.Forms.Padding(3, 3, 5, 3);
         this.layoutGroupBox.Name = "layoutGroupBox";
         this.layoutGroupBox.Size = new System.Drawing.Size(496, 219);
         this.layoutGroupBox.TabIndex = 20;
         this.layoutGroupBox.TabStop = false;
         this.layoutGroupBox.Text = "Layout";
         // 
         // tableLayoutPanel3
         // 
         this.tableLayoutPanel3.ColumnCount = 3;
         this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
         this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel3.Controls.Add(this.layoutComboBox, 0, 3);
         this.tableLayoutPanel3.Controls.Add(this.button3, 1, 3);
         this.tableLayoutPanel3.Controls.Add(this.button4, 1, 2);
         this.tableLayoutPanel3.Controls.Add(this.button5, 1, 0);
         this.tableLayoutPanel3.Controls.Add(this.button6, 1, 1);
         this.tableLayoutPanel3.Controls.Add(this.layoutTree, 0, 0);
         this.tableLayoutPanel3.Controls.Add(this.propertyGrid1, 2, 0);
         this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
         this.tableLayoutPanel3.Name = "tableLayoutPanel3";
         this.tableLayoutPanel3.RowCount = 4;
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
         this.tableLayoutPanel3.Size = new System.Drawing.Size(490, 200);
         this.tableLayoutPanel3.TabIndex = 0;
         // 
         // layoutComboBox
         // 
         this.layoutComboBox.Dock = System.Windows.Forms.DockStyle.Top;
         this.layoutComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.layoutComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.layoutComboBox.FormattingEnabled = true;
         this.layoutComboBox.Location = new System.Drawing.Point(3, 173);
         this.layoutComboBox.Name = "layoutComboBox";
         this.layoutComboBox.Size = new System.Drawing.Size(219, 21);
         this.layoutComboBox.TabIndex = 0;
         // 
         // button3
         // 
         this.button3.Dock = System.Windows.Forms.DockStyle.Top;
         this.button3.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.button3.Location = new System.Drawing.Point(228, 172);
         this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
         this.button3.Name = "button3";
         this.button3.Size = new System.Drawing.Size(34, 23);
         this.button3.TabIndex = 1;
         this.button3.Text = "Add";
         this.button3.UseVisualStyleBackColor = true;
         // 
         // button4
         // 
         this.button4.Dock = System.Windows.Forms.DockStyle.Top;
         this.button4.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.button4.Location = new System.Drawing.Point(228, 63);
         this.button4.Name = "button4";
         this.button4.Size = new System.Drawing.Size(34, 23);
         this.button4.TabIndex = 2;
         this.button4.Text = "Del";
         this.button4.UseVisualStyleBackColor = true;
         // 
         // button5
         // 
         this.button5.Dock = System.Windows.Forms.DockStyle.Top;
         this.button5.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.button5.Location = new System.Drawing.Point(228, 3);
         this.button5.Name = "button5";
         this.button5.Size = new System.Drawing.Size(34, 23);
         this.button5.TabIndex = 3;
         this.button5.Text = "Up";
         this.button5.UseVisualStyleBackColor = true;
         // 
         // button6
         // 
         this.button6.Dock = System.Windows.Forms.DockStyle.Top;
         this.button6.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.button6.Location = new System.Drawing.Point(228, 33);
         this.button6.Name = "button6";
         this.button6.Size = new System.Drawing.Size(34, 23);
         this.button6.TabIndex = 4;
         this.button6.Text = "Dwn";
         this.button6.UseVisualStyleBackColor = true;
         // 
         // layoutTree
         // 
         this.layoutTree.AllowDrop = true;
         this.layoutTree.AutoScroll = true;
         this.layoutTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.layoutTree.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutTree.Location = new System.Drawing.Point(3, 3);
         this.layoutTree.Name = "layoutTree";
         this.tableLayoutPanel3.SetRowSpan(this.layoutTree, 3);
         this.layoutTree.Size = new System.Drawing.Size(219, 164);
         this.layoutTree.TabIndex = 5;
         this.layoutTree.Text = "treeView2";
         this.layoutTree.SelectionChanged += new System.EventHandler<Outliner.Controls.Tree.SelectionChangedEventArgs>(this.layoutTree_SelectionChanged);
         // 
         // propertyGrid1
         // 
         this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.propertyGrid1.HelpVisible = false;
         this.propertyGrid1.Location = new System.Drawing.Point(268, 3);
         this.propertyGrid1.Name = "propertyGrid1";
         this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.NoSort;
         this.tableLayoutPanel3.SetRowSpan(this.propertyGrid1, 3);
         this.propertyGrid1.Size = new System.Drawing.Size(219, 164);
         this.propertyGrid1.TabIndex = 6;
         this.propertyGrid1.ToolbarVisible = false;
         // 
         // addDeletePanel
         // 
         this.addDeletePanel.ColumnCount = 2;
         this.addDeletePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.addDeletePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.addDeletePanel.Controls.Add(this.deleteBtn, 1, 0);
         this.addDeletePanel.Controls.Add(this.addBtn, 0, 0);
         this.addDeletePanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.addDeletePanel.Location = new System.Drawing.Point(0, 475);
         this.addDeletePanel.Margin = new System.Windows.Forms.Padding(0);
         this.addDeletePanel.Name = "addDeletePanel";
         this.addDeletePanel.RowCount = 1;
         this.addDeletePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.addDeletePanel.Size = new System.Drawing.Size(200, 34);
         this.addDeletePanel.TabIndex = 16;
         // 
         // deleteBtn
         // 
         this.deleteBtn.Dock = System.Windows.Forms.DockStyle.Fill;
         this.deleteBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.deleteBtn.Location = new System.Drawing.Point(103, 3);
         this.deleteBtn.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
         this.deleteBtn.Name = "deleteBtn";
         this.deleteBtn.Size = new System.Drawing.Size(93, 28);
         this.deleteBtn.TabIndex = 1;
         this.deleteBtn.Text = "Delete";
         this.deleteBtn.UseVisualStyleBackColor = true;
         // 
         // addBtn
         // 
         this.addBtn.Dock = System.Windows.Forms.DockStyle.Fill;
         this.addBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.addBtn.Location = new System.Drawing.Point(6, 3);
         this.addBtn.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
         this.addBtn.Name = "addBtn";
         this.addBtn.Size = new System.Drawing.Size(91, 28);
         this.addBtn.TabIndex = 0;
         this.addBtn.Text = "Add";
         this.addBtn.UseVisualStyleBackColor = true;
         // 
         // presetsTree
         // 
         this.presetsTree.AllowDrop = true;
         this.presetsTree.AutoScroll = true;
         this.presetsTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.presetsTree.Dock = System.Windows.Forms.DockStyle.Fill;
         this.presetsTree.Location = new System.Drawing.Point(6, 9);
         this.presetsTree.Margin = new System.Windows.Forms.Padding(6, 9, 4, 3);
         this.presetsTree.Name = "presetsTree";
         this.propertiesPanel.SetRowSpan(this.presetsTree, 3);
         this.presetsTree.Size = new System.Drawing.Size(190, 463);
         this.presetsTree.TabIndex = 12;
         this.presetsTree.Text = "treeView1";
         this.presetsTree.SelectionChanged += new System.EventHandler<Outliner.Controls.Tree.SelectionChangedEventArgs>(this.presetsTree_SelectionChanged);
         this.presetsTree.BeforeNodeTextEdit += new System.EventHandler<Outliner.Controls.Tree.BeforeNodeTextEditEventArgs>(this.presetsTree_BeforeNodeTextEdit);
         // 
         // okCancelPanel
         // 
         this.okCancelPanel.ColumnCount = 2;
         this.okCancelPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.okCancelPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
         this.okCancelPanel.Controls.Add(this.cancelBtn, 0, 0);
         this.okCancelPanel.Controls.Add(this.okBtn, 0, 0);
         this.okCancelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.okCancelPanel.Location = new System.Drawing.Point(260, 509);
         this.okCancelPanel.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
         this.okCancelPanel.Name = "okCancelPanel";
         this.okCancelPanel.RowCount = 1;
         this.okCancelPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.okCancelPanel.Size = new System.Drawing.Size(441, 34);
         this.okCancelPanel.TabIndex = 18;
         // 
         // cancelBtn
         // 
         this.cancelBtn.Dock = System.Windows.Forms.DockStyle.Right;
         this.cancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.cancelBtn.Location = new System.Drawing.Point(344, 3);
         this.cancelBtn.Name = "cancelBtn";
         this.cancelBtn.Size = new System.Drawing.Size(94, 28);
         this.cancelBtn.TabIndex = 19;
         this.cancelBtn.Text = "Cancel";
         this.cancelBtn.UseVisualStyleBackColor = true;
         this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
         // 
         // okBtn
         // 
         this.okBtn.Dock = System.Windows.Forms.DockStyle.Right;
         this.okBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.okBtn.Location = new System.Drawing.Point(244, 3);
         this.okBtn.Name = "okBtn";
         this.okBtn.Size = new System.Drawing.Size(94, 28);
         this.okBtn.TabIndex = 18;
         this.okBtn.Text = "OK";
         this.okBtn.UseVisualStyleBackColor = true;
         // 
         // filterGroupBox
         // 
         this.filterGroupBox.BorderColor = System.Drawing.Color.Black;
         this.propertiesPanel.SetColumnSpan(this.filterGroupBox, 2);
         this.filterGroupBox.Controls.Add(this.tableLayoutPanel1);
         this.filterGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.filterGroupBox.Location = new System.Drawing.Point(203, 109);
         this.filterGroupBox.Margin = new System.Windows.Forms.Padding(3, 3, 5, 3);
         this.filterGroupBox.Name = "filterGroupBox";
         this.filterGroupBox.Size = new System.Drawing.Size(496, 138);
         this.filterGroupBox.TabIndex = 19;
         this.filterGroupBox.TabStop = false;
         this.filterGroupBox.Text = "Filter";
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 3;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
         this.tableLayoutPanel1.Controls.Add(this.filtersComboBox, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.button1, 1, 0);
         this.tableLayoutPanel1.Controls.Add(this.filtersTree, 0, 1);
         this.tableLayoutPanel1.Controls.Add(this.button2, 1, 1);
         this.tableLayoutPanel1.Controls.Add(this.propertyGrid2, 2, 1);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 2;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.36975F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75.63025F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(490, 119);
         this.tableLayoutPanel1.TabIndex = 0;
         // 
         // filtersComboBox
         // 
         this.filtersComboBox.Dock = System.Windows.Forms.DockStyle.Top;
         this.filtersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.filtersComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.filtersComboBox.FormattingEnabled = true;
         this.filtersComboBox.Location = new System.Drawing.Point(3, 3);
         this.filtersComboBox.Name = "filtersComboBox";
         this.filtersComboBox.Size = new System.Drawing.Size(204, 21);
         this.filtersComboBox.TabIndex = 0;
         // 
         // button1
         // 
         this.button1.Dock = System.Windows.Forms.DockStyle.Top;
         this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.button1.Location = new System.Drawing.Point(213, 2);
         this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(74, 23);
         this.button1.TabIndex = 1;
         this.button1.Text = "Add";
         this.button1.UseVisualStyleBackColor = true;
         // 
         // filtersTree
         // 
         this.filtersTree.AllowDrop = true;
         this.filtersTree.AutoScroll = true;
         this.filtersTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.filtersTree.Dock = System.Windows.Forms.DockStyle.Fill;
         this.filtersTree.Location = new System.Drawing.Point(3, 32);
         this.filtersTree.Name = "filtersTree";
         this.filtersTree.Size = new System.Drawing.Size(204, 84);
         this.filtersTree.TabIndex = 2;
         this.filtersTree.Text = "treeView1";
         // 
         // button2
         // 
         this.button2.Dock = System.Windows.Forms.DockStyle.Top;
         this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.button2.Location = new System.Drawing.Point(213, 32);
         this.button2.Name = "button2";
         this.button2.Size = new System.Drawing.Size(74, 23);
         this.button2.TabIndex = 3;
         this.button2.Text = "Delete";
         this.button2.UseVisualStyleBackColor = true;
         // 
         // propertyGrid2
         // 
         this.propertyGrid2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.propertyGrid2.HelpVisible = false;
         this.propertyGrid2.Location = new System.Drawing.Point(293, 32);
         this.propertyGrid2.Name = "propertyGrid2";
         this.propertyGrid2.Size = new System.Drawing.Size(194, 84);
         this.propertyGrid2.TabIndex = 4;
         this.propertyGrid2.ToolbarVisible = false;
         // 
         // propertiesGroupBox
         // 
         this.propertiesGroupBox.BorderColor = System.Drawing.Color.Black;
         this.propertiesPanel.SetColumnSpan(this.propertiesGroupBox, 2);
         this.propertiesGroupBox.Controls.Add(this.tableLayoutPanel2);
         this.propertiesGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.propertiesGroupBox.Location = new System.Drawing.Point(203, 3);
         this.propertiesGroupBox.Margin = new System.Windows.Forms.Padding(3, 3, 5, 3);
         this.propertiesGroupBox.Name = "propertiesGroupBox";
         this.propertiesGroupBox.Size = new System.Drawing.Size(496, 100);
         this.propertiesGroupBox.TabIndex = 21;
         this.propertiesGroupBox.TabStop = false;
         this.propertiesGroupBox.Text = "Properties ";
         // 
         // tableLayoutPanel2
         // 
         this.tableLayoutPanel2.ColumnCount = 2;
         this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
         this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel2.Controls.Add(this.sorterComboBox, 1, 2);
         this.tableLayoutPanel2.Controls.Add(this.label3, 0, 2);
         this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
         this.tableLayoutPanel2.Controls.Add(this.nameTextBox, 1, 0);
         this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
         this.tableLayoutPanel2.Controls.Add(this.modeComboBox, 1, 1);
         this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
         this.tableLayoutPanel2.Name = "tableLayoutPanel2";
         this.tableLayoutPanel2.RowCount = 3;
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.07692F));
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.92308F));
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
         this.tableLayoutPanel2.Size = new System.Drawing.Size(490, 81);
         this.tableLayoutPanel2.TabIndex = 0;
         // 
         // sorterComboBox
         // 
         this.sorterComboBox.Dock = System.Windows.Forms.DockStyle.Top;
         this.sorterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.sorterComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.sorterComboBox.FormattingEnabled = true;
         this.sorterComboBox.Location = new System.Drawing.Point(48, 51);
         this.sorterComboBox.Name = "sorterComboBox";
         this.sorterComboBox.Size = new System.Drawing.Size(439, 21);
         this.sorterComboBox.TabIndex = 5;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Dock = System.Windows.Forms.DockStyle.Top;
         this.label3.Location = new System.Drawing.Point(3, 53);
         this.label3.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(39, 13);
         this.label3.TabIndex = 4;
         this.label3.Text = "Sorter";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Dock = System.Windows.Forms.DockStyle.Top;
         this.label1.Location = new System.Drawing.Point(3, 5);
         this.label1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(39, 13);
         this.label1.TabIndex = 0;
         this.label1.Text = "Name";
         // 
         // nameTextBox
         // 
         this.nameTextBox.Dock = System.Windows.Forms.DockStyle.Top;
         this.nameTextBox.Location = new System.Drawing.Point(50, 3);
         this.nameTextBox.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
         this.nameTextBox.Name = "nameTextBox";
         this.nameTextBox.Size = new System.Drawing.Size(435, 20);
         this.nameTextBox.TabIndex = 1;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Dock = System.Windows.Forms.DockStyle.Top;
         this.label2.Location = new System.Drawing.Point(3, 28);
         this.label2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(39, 13);
         this.label2.TabIndex = 2;
         this.label2.Text = "Mode";
         // 
         // modeComboBox
         // 
         this.modeComboBox.Dock = System.Windows.Forms.DockStyle.Top;
         this.modeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.modeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.modeComboBox.FormattingEnabled = true;
         this.modeComboBox.Location = new System.Drawing.Point(48, 26);
         this.modeComboBox.Name = "modeComboBox";
         this.modeComboBox.Size = new System.Drawing.Size(439, 21);
         this.modeComboBox.TabIndex = 3;
         // 
         // PresetEditor
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(704, 543);
         this.Controls.Add(this.propertiesPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
         this.MinimumSize = new System.Drawing.Size(500, 300);
         this.Name = "PresetEditor";
         this.ShowIcon = false;
         this.ShowInTaskbar = false;
         this.Text = "Preset Editor";
         this.propertiesPanel.ResumeLayout(false);
         this.layoutGroupBox.ResumeLayout(false);
         this.tableLayoutPanel3.ResumeLayout(false);
         this.addDeletePanel.ResumeLayout(false);
         this.okCancelPanel.ResumeLayout(false);
         this.filterGroupBox.ResumeLayout(false);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.propertiesGroupBox.ResumeLayout(false);
         this.tableLayoutPanel2.ResumeLayout(false);
         this.tableLayoutPanel2.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TableLayoutPanel propertiesPanel;
      private Tree.TreeView presetsTree;
      private System.Windows.Forms.TableLayoutPanel addDeletePanel;
      private System.Windows.Forms.Button deleteBtn;
      private System.Windows.Forms.Button addBtn;
      private System.Windows.Forms.TableLayoutPanel okCancelPanel;
      private System.Windows.Forms.Button cancelBtn;
      private System.Windows.Forms.Button okBtn;
      private Outliner.Controls.OutlinerGroupBox layoutGroupBox;
      private Outliner.Controls.OutlinerGroupBox filterGroupBox;
      private Outliner.Controls.OutlinerGroupBox propertiesGroupBox;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
      private System.Windows.Forms.ComboBox sorterComboBox;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox nameTextBox;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.ComboBox modeComboBox;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.ComboBox filtersComboBox;
      private System.Windows.Forms.Button button1;
      private Tree.TreeView filtersTree;
      private System.Windows.Forms.Button button2;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
      private System.Windows.Forms.ComboBox layoutComboBox;
      private System.Windows.Forms.Button button3;
      private System.Windows.Forms.Button button4;
      private System.Windows.Forms.Button button5;
      private System.Windows.Forms.Button button6;
      private Tree.TreeView layoutTree;
      private System.Windows.Forms.PropertyGrid propertyGrid1;
      private System.Windows.Forms.PropertyGrid propertyGrid2;
   }
}