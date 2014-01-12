namespace PJanssen.Outliner.Controls.Options
{
   partial class PresetPropertiesEditor
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
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.contextMenuComboBox = new System.Windows.Forms.ComboBox();
         this.outlinerPresetBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.layoutComboBox = new System.Windows.Forms.ComboBox();
         this.label1 = new System.Windows.Forms.Label();
         this.modesComboBox = new System.Windows.Forms.ComboBox();
         this.label2 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.outlinerGroupBox1 = new Outliner.Controls.OutlinerGroupBox();
         this.tableLayoutPanel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.outlinerPresetBindingSource)).BeginInit();
         this.outlinerGroupBox1.SuspendLayout();
         this.SuspendLayout();
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 2;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tableLayoutPanel1.Controls.Add(this.contextMenuComboBox, 1, 2);
         this.tableLayoutPanel1.Controls.Add(this.layoutComboBox, 1, 1);
         this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.modesComboBox, 1, 0);
         this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
         this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 5;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(423, 262);
         this.tableLayoutPanel1.TabIndex = 0;
         // 
         // contextMenuComboBox
         // 
         this.contextMenuComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.outlinerPresetBindingSource, "ContextMenuFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.contextMenuComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.contextMenuComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.contextMenuComboBox.FormattingEnabled = true;
         this.contextMenuComboBox.Location = new System.Drawing.Point(93, 55);
         this.contextMenuComboBox.Name = "contextMenuComboBox";
         this.contextMenuComboBox.Size = new System.Drawing.Size(327, 21);
         this.contextMenuComboBox.TabIndex = 7;
         // 
         // outlinerPresetBindingSource
         // 
         this.outlinerPresetBindingSource.DataSource = typeof(Outliner.Configuration.OutlinerPreset);
         // 
         // layoutComboBox
         // 
         this.layoutComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.outlinerPresetBindingSource, "LayoutFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.layoutComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.layoutComboBox.FormattingEnabled = true;
         this.layoutComboBox.Location = new System.Drawing.Point(93, 29);
         this.layoutComboBox.Name = "layoutComboBox";
         this.layoutComboBox.Size = new System.Drawing.Size(327, 21);
         this.layoutComboBox.TabIndex = 3;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(3, 5);
         this.label1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(34, 13);
         this.label1.TabIndex = 0;
         this.label1.Text = "Mode";
         // 
         // modesComboBox
         // 
         this.modesComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.outlinerPresetBindingSource, "TreeModeType", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.modesComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.modesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.modesComboBox.FormattingEnabled = true;
         this.modesComboBox.Location = new System.Drawing.Point(93, 3);
         this.modesComboBox.Name = "modesComboBox";
         this.modesComboBox.Size = new System.Drawing.Size(327, 21);
         this.modesComboBox.TabIndex = 1;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(3, 31);
         this.label2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(58, 13);
         this.label2.TabIndex = 2;
         this.label2.Text = "Layout File";
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(3, 57);
         this.label4.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(73, 13);
         this.label4.TabIndex = 6;
         this.label4.Text = "Context-Menu";
         // 
         // outlinerGroupBox1
         // 
         this.outlinerGroupBox1.BorderColor = System.Drawing.Color.Black;
         this.outlinerGroupBox1.Controls.Add(this.tableLayoutPanel1);
         this.outlinerGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.outlinerGroupBox1.Location = new System.Drawing.Point(0, 0);
         this.outlinerGroupBox1.Name = "outlinerGroupBox1";
         this.outlinerGroupBox1.Size = new System.Drawing.Size(429, 281);
         this.outlinerGroupBox1.TabIndex = 8;
         this.outlinerGroupBox1.TabStop = false;
         this.outlinerGroupBox1.Text = "Preset Properties";
         // 
         // PresetPropertiesEditor
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.outlinerGroupBox1);
         this.Name = "PresetPropertiesEditor";
         this.Size = new System.Drawing.Size(429, 281);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.tableLayoutPanel1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.outlinerPresetBindingSource)).EndInit();
         this.outlinerGroupBox1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.ComboBox modesComboBox;
      private System.Windows.Forms.BindingSource outlinerPresetBindingSource;
      private System.Windows.Forms.ComboBox layoutComboBox;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.ComboBox contextMenuComboBox;
      private System.Windows.Forms.Label label4;
      private OutlinerGroupBox outlinerGroupBox1;
   }
}
