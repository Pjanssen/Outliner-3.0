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
         this.components = new System.ComponentModel.Container();
         this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
         this.itemsComboBox = new System.Windows.Forms.ComboBox();
         this.addBtn = new System.Windows.Forms.Button();
         this.itemTree = new Outliner.Controls.Tree.TreeView();
         this.itemPropertyGrid = new System.Windows.Forms.PropertyGrid();
         this.moveUpBtn = new System.Windows.Forms.Button();
         this.moveDownBtn = new System.Windows.Forms.Button();
         this.deleteBtn = new System.Windows.Forms.Button();
         this.outlinerPresetBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.outlinerGroupBox1 = new Outliner.Controls.OutlinerGroupBox();
         this.tableLayoutPanel2.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.outlinerPresetBindingSource)).BeginInit();
         this.outlinerGroupBox1.SuspendLayout();
         this.SuspendLayout();
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
         this.tableLayoutPanel2.Controls.Add(this.moveUpBtn, 1, 2);
         this.tableLayoutPanel2.Controls.Add(this.moveDownBtn, 1, 3);
         this.tableLayoutPanel2.Controls.Add(this.deleteBtn, 1, 1);
         this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
         this.tableLayoutPanel2.Name = "tableLayoutPanel2";
         this.tableLayoutPanel2.RowCount = 5;
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel2.Size = new System.Drawing.Size(448, 380);
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
         this.itemsComboBox.Size = new System.Drawing.Size(382, 21);
         this.itemsComboBox.TabIndex = 0;
         // 
         // addBtn
         // 
         this.addBtn.Dock = System.Windows.Forms.DockStyle.Fill;
         this.addBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.addBtn.Font = new System.Drawing.Font("Tahoma", 8F);
         this.addBtn.Location = new System.Drawing.Point(391, 2);
         this.addBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
         this.addBtn.Name = "addBtn";
         this.addBtn.Size = new System.Drawing.Size(54, 23);
         this.addBtn.TabIndex = 1;
         this.addBtn.Text = "Add";
         this.addBtn.UseVisualStyleBackColor = true;
         this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
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
         this.itemTree.Size = new System.Drawing.Size(382, 198);
         this.itemTree.TabIndex = 2;
         this.itemTree.SelectionChanged += new System.EventHandler<Outliner.Controls.Tree.SelectionChangedEventArgs>(this.itemTree_SelectionChanged);
         // 
         // itemPropertyGrid
         // 
         this.itemPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
         this.itemPropertyGrid.HelpVisible = false;
         this.itemPropertyGrid.Location = new System.Drawing.Point(3, 235);
         this.itemPropertyGrid.Name = "itemPropertyGrid";
         this.itemPropertyGrid.Size = new System.Drawing.Size(382, 142);
         this.itemPropertyGrid.TabIndex = 3;
         this.itemPropertyGrid.ToolbarVisible = false;
         this.itemPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.itemPropertyGrid_PropertyValueChanged);
         // 
         // moveUpBtn
         // 
         this.moveUpBtn.Dock = System.Windows.Forms.DockStyle.Fill;
         this.moveUpBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.moveUpBtn.Font = new System.Drawing.Font("Tahoma", 8F);
         this.moveUpBtn.Location = new System.Drawing.Point(391, 59);
         this.moveUpBtn.Name = "moveUpBtn";
         this.moveUpBtn.Size = new System.Drawing.Size(54, 22);
         this.moveUpBtn.TabIndex = 4;
         this.moveUpBtn.Text = "Up";
         this.moveUpBtn.UseVisualStyleBackColor = true;
         this.moveUpBtn.Click += new System.EventHandler(this.moveUpBtn_Click);
         // 
         // moveDownBtn
         // 
         this.moveDownBtn.Dock = System.Windows.Forms.DockStyle.Top;
         this.moveDownBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.moveDownBtn.Font = new System.Drawing.Font("Tahoma", 8F);
         this.moveDownBtn.Location = new System.Drawing.Point(391, 87);
         this.moveDownBtn.Name = "moveDownBtn";
         this.moveDownBtn.Size = new System.Drawing.Size(54, 22);
         this.moveDownBtn.TabIndex = 5;
         this.moveDownBtn.Text = "Down";
         this.moveDownBtn.UseVisualStyleBackColor = true;
         this.moveDownBtn.Click += new System.EventHandler(this.moveDownBtn_Click);
         // 
         // deleteBtn
         // 
         this.deleteBtn.Dock = System.Windows.Forms.DockStyle.Top;
         this.deleteBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.deleteBtn.Font = new System.Drawing.Font("Tahoma", 8F);
         this.deleteBtn.Location = new System.Drawing.Point(391, 31);
         this.deleteBtn.Name = "deleteBtn";
         this.deleteBtn.Size = new System.Drawing.Size(54, 22);
         this.deleteBtn.TabIndex = 6;
         this.deleteBtn.Text = "Delete";
         this.deleteBtn.UseVisualStyleBackColor = true;
         this.deleteBtn.Click += new System.EventHandler(this.deleteBtn_Click);
         // 
         // outlinerPresetBindingSource
         // 
         this.outlinerPresetBindingSource.DataSource = typeof(Outliner.Presets.OutlinerPreset);
         // 
         // outlinerGroupBox1
         // 
         this.outlinerGroupBox1.BorderColor = System.Drawing.Color.Black;
         this.outlinerGroupBox1.Controls.Add(this.tableLayoutPanel2);
         this.outlinerGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.outlinerGroupBox1.Location = new System.Drawing.Point(0, 0);
         this.outlinerGroupBox1.Name = "outlinerGroupBox1";
         this.outlinerGroupBox1.Size = new System.Drawing.Size(454, 399);
         this.outlinerGroupBox1.TabIndex = 7;
         this.outlinerGroupBox1.TabStop = false;
         this.outlinerGroupBox1.Text = "Context-Menu";
         // 
         // ContextMenuModelEditor
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.outlinerGroupBox1);
         this.Name = "ContextMenuModelEditor";
         this.Size = new System.Drawing.Size(454, 399);
         this.tableLayoutPanel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.outlinerPresetBindingSource)).EndInit();
         this.outlinerGroupBox1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
      private System.Windows.Forms.ComboBox itemsComboBox;
      private System.Windows.Forms.Button addBtn;
      private Tree.TreeView itemTree;
      private System.Windows.Forms.PropertyGrid itemPropertyGrid;
      private System.Windows.Forms.Button moveUpBtn;
      private System.Windows.Forms.Button moveDownBtn;
      private System.Windows.Forms.Button deleteBtn;
      private System.Windows.Forms.BindingSource outlinerPresetBindingSource;
      private OutlinerGroupBox outlinerGroupBox1;
   }
}
