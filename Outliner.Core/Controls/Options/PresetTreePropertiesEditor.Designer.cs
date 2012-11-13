namespace Outliner.Controls.Options
{
   partial class PresetTreePropertiesEditor
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
         this.openContextMenuFileDialog = new System.Windows.Forms.OpenFileDialog();
         this.presetBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.outlinerGroupBox1 = new Outliner.Controls.OutlinerGroupBox();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.contextMenuBrowseBtn = new System.Windows.Forms.Button();
         this.textBox1 = new System.Windows.Forms.TextBox();
         this.sorterComboBox = new System.Windows.Forms.ComboBox();
         this.label6 = new System.Windows.Forms.Label();
         this.label10 = new System.Windows.Forms.Label();
         this.modeComboBox = new System.Windows.Forms.ComboBox();
         this.checkBox1 = new System.Windows.Forms.CheckBox();
         this.nodeSorterBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.label1 = new System.Windows.Forms.Label();
         ((System.ComponentModel.ISupportInitialize)(this.presetBindingSource)).BeginInit();
         this.outlinerGroupBox1.SuspendLayout();
         this.tableLayoutPanel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.nodeSorterBindingSource)).BeginInit();
         this.SuspendLayout();
         // 
         // openContextMenuFileDialog
         // 
         this.openContextMenuFileDialog.DefaultExt = "xml";
         this.openContextMenuFileDialog.Filter = "XML files|*.xml|All files|*.*";
         // 
         // presetBindingSource
         // 
         this.presetBindingSource.DataSource = typeof(Outliner.Presets.OutlinerPreset);
         this.presetBindingSource.BindingComplete += new System.Windows.Forms.BindingCompleteEventHandler(this.presetBindingSource_BindingComplete);
         // 
         // outlinerGroupBox1
         // 
         this.outlinerGroupBox1.BorderColor = System.Drawing.Color.Black;
         this.outlinerGroupBox1.Controls.Add(this.tableLayoutPanel1);
         this.outlinerGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
         this.outlinerGroupBox1.Location = new System.Drawing.Point(0, 0);
         this.outlinerGroupBox1.Name = "outlinerGroupBox1";
         this.outlinerGroupBox1.Padding = new System.Windows.Forms.Padding(5, 3, 5, 5);
         this.outlinerGroupBox1.Size = new System.Drawing.Size(481, 109);
         this.outlinerGroupBox1.TabIndex = 2;
         this.outlinerGroupBox1.TabStop = false;
         this.outlinerGroupBox1.Text = "Tree Settings";
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 3;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 89F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
         this.tableLayoutPanel1.Controls.Add(this.contextMenuBrowseBtn, 2, 2);
         this.tableLayoutPanel1.Controls.Add(this.textBox1, 1, 2);
         this.tableLayoutPanel1.Controls.Add(this.sorterComboBox, 1, 1);
         this.tableLayoutPanel1.Controls.Add(this.label6, 0, 1);
         this.tableLayoutPanel1.Controls.Add(this.label10, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.modeComboBox, 1, 0);
         this.tableLayoutPanel1.Controls.Add(this.checkBox1, 2, 1);
         this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 16);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 3;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(471, 88);
         this.tableLayoutPanel1.TabIndex = 2;
         // 
         // contextMenuBrowseBtn
         // 
         this.contextMenuBrowseBtn.Dock = System.Windows.Forms.DockStyle.Top;
         this.contextMenuBrowseBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.contextMenuBrowseBtn.Location = new System.Drawing.Point(399, 61);
         this.contextMenuBrowseBtn.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
         this.contextMenuBrowseBtn.Name = "contextMenuBrowseBtn";
         this.contextMenuBrowseBtn.Size = new System.Drawing.Size(69, 24);
         this.contextMenuBrowseBtn.TabIndex = 9;
         this.contextMenuBrowseBtn.Text = "Browse";
         this.contextMenuBrowseBtn.UseVisualStyleBackColor = true;
         this.contextMenuBrowseBtn.Click += new System.EventHandler(this.contextMenuBrowseBtn_Click);
         // 
         // textBox1
         // 
         this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.presetBindingSource, "ContextMenuFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.textBox1.Dock = System.Windows.Forms.DockStyle.Top;
         this.textBox1.Location = new System.Drawing.Point(94, 63);
         this.textBox1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
         this.textBox1.Name = "textBox1";
         this.textBox1.Size = new System.Drawing.Size(297, 20);
         this.textBox1.TabIndex = 8;
         // 
         // sorterComboBox
         // 
         this.sorterComboBox.Dock = System.Windows.Forms.DockStyle.Top;
         this.sorterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.sorterComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.sorterComboBox.FormattingEnabled = true;
         this.sorterComboBox.Location = new System.Drawing.Point(92, 33);
         this.sorterComboBox.Name = "sorterComboBox";
         this.sorterComboBox.Size = new System.Drawing.Size(301, 21);
         this.sorterComboBox.TabIndex = 5;
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Dock = System.Windows.Forms.DockStyle.Top;
         this.label6.Location = new System.Drawing.Point(3, 35);
         this.label6.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(83, 13);
         this.label6.TabIndex = 4;
         this.label6.Text = "Sorter";
         // 
         // label10
         // 
         this.label10.AutoSize = true;
         this.label10.Dock = System.Windows.Forms.DockStyle.Top;
         this.label10.Location = new System.Drawing.Point(3, 5);
         this.label10.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
         this.label10.Name = "label10";
         this.label10.Size = new System.Drawing.Size(83, 13);
         this.label10.TabIndex = 2;
         this.label10.Text = "Mode";
         // 
         // modeComboBox
         // 
         this.modeComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.presetBindingSource, "TreeModeType", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.modeComboBox.DataSource = this.presetBindingSource;
         this.modeComboBox.Dock = System.Windows.Forms.DockStyle.Top;
         this.modeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.modeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.modeComboBox.FormattingEnabled = true;
         this.modeComboBox.Location = new System.Drawing.Point(92, 3);
         this.modeComboBox.Name = "modeComboBox";
         this.modeComboBox.Size = new System.Drawing.Size(301, 21);
         this.modeComboBox.TabIndex = 3;
         // 
         // checkBox1
         // 
         this.checkBox1.AutoSize = true;
         this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.nodeSorterBindingSource, "Descending", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.checkBox1.Location = new System.Drawing.Point(400, 36);
         this.checkBox1.Margin = new System.Windows.Forms.Padding(4, 6, 3, 3);
         this.checkBox1.Name = "checkBox1";
         this.checkBox1.Size = new System.Drawing.Size(66, 17);
         this.checkBox1.TabIndex = 6;
         this.checkBox1.Text = "Reverse";
         this.checkBox1.UseVisualStyleBackColor = true;
         // 
         // nodeSorterBindingSource
         // 
         this.nodeSorterBindingSource.DataSource = typeof(Outliner.NodeSorters.NodeSorter);
         this.nodeSorterBindingSource.BindingComplete += new System.Windows.Forms.BindingCompleteEventHandler(this.nodeSorterBindingSource_BindingComplete);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(3, 66);
         this.label1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(73, 13);
         this.label1.TabIndex = 7;
         this.label1.Text = "Context-Menu";
         // 
         // PresetTreePropertiesEditor
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.outlinerGroupBox1);
         this.Name = "PresetTreePropertiesEditor";
         this.Size = new System.Drawing.Size(481, 114);
         ((System.ComponentModel.ISupportInitialize)(this.presetBindingSource)).EndInit();
         this.outlinerGroupBox1.ResumeLayout(false);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.tableLayoutPanel1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.nodeSorterBindingSource)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.BindingSource presetBindingSource;
      private OutlinerGroupBox outlinerGroupBox1;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label10;
      private System.Windows.Forms.OpenFileDialog openContextMenuFileDialog;
      private System.Windows.Forms.ComboBox sorterComboBox;
      private System.Windows.Forms.ComboBox modeComboBox;
      private System.Windows.Forms.CheckBox checkBox1;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.BindingSource nodeSorterBindingSource;
      private System.Windows.Forms.Button contextMenuBrowseBtn;
      private System.Windows.Forms.TextBox textBox1;
   }
}
