namespace PJanssen.Outliner.Controls.Options
{
   partial class ConfigurationFilePropertiesEditor
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
         this.img24ComboBox = new System.Windows.Forms.ComboBox();
         this.configurationFileBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.img16ComboBox = new System.Windows.Forms.ComboBox();
         this.textComboBox = new System.Windows.Forms.ComboBox();
         this.label4 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.resTypeComboBox = new System.Windows.Forms.ComboBox();
         this.label3 = new System.Windows.Forms.Label();
         this.tableLayoutPanel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.configurationFileBindingSource)).BeginInit();
         this.SuspendLayout();
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 2;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.Controls.Add(this.img24ComboBox, 1, 3);
         this.tableLayoutPanel1.Controls.Add(this.img16ComboBox, 1, 2);
         this.tableLayoutPanel1.Controls.Add(this.textComboBox, 1, 1);
         this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
         this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
         this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.resTypeComboBox, 1, 0);
         this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 4;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(460, 108);
         this.tableLayoutPanel1.TabIndex = 1;
         // 
         // img24ComboBox
         // 
         this.img24ComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
         this.img24ComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
         this.img24ComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationFileBindingSource, "Image24Res", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.img24ComboBox.Dock = System.Windows.Forms.DockStyle.Top;
         this.img24ComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.img24ComboBox.Location = new System.Drawing.Point(93, 81);
         this.img24ComboBox.Name = "img24ComboBox";
         this.img24ComboBox.Size = new System.Drawing.Size(364, 21);
         this.img24ComboBox.Sorted = true;
         this.img24ComboBox.TabIndex = 7;
         // 
         // configurationFileBindingSource
         // 
         this.configurationFileBindingSource.AllowNew = false;
         this.configurationFileBindingSource.DataSource = typeof(Outliner.Configuration.ConfigurationFile);
         // 
         // img16ComboBox
         // 
         this.img16ComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
         this.img16ComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
         this.img16ComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationFileBindingSource, "Image16Res", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.img16ComboBox.Dock = System.Windows.Forms.DockStyle.Top;
         this.img16ComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.img16ComboBox.Location = new System.Drawing.Point(93, 55);
         this.img16ComboBox.Name = "img16ComboBox";
         this.img16ComboBox.Size = new System.Drawing.Size(364, 21);
         this.img16ComboBox.Sorted = true;
         this.img16ComboBox.TabIndex = 6;
         // 
         // textComboBox
         // 
         this.textComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
         this.textComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
         this.textComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationFileBindingSource, "TextRes", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.textComboBox.Dock = System.Windows.Forms.DockStyle.Top;
         this.textComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.textComboBox.Location = new System.Drawing.Point(93, 29);
         this.textComboBox.Name = "textComboBox";
         this.textComboBox.Size = new System.Drawing.Size(364, 21);
         this.textComboBox.Sorted = true;
         this.textComboBox.TabIndex = 5;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(3, 84);
         this.label4.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(68, 13);
         this.label4.TabIndex = 4;
         this.label4.Text = "Image 24x24";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(3, 32);
         this.label2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(28, 13);
         this.label2.TabIndex = 2;
         this.label2.Text = "Text";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(3, 6);
         this.label1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(80, 13);
         this.label1.TabIndex = 0;
         this.label1.Text = "Resource Type";
         // 
         // resTypeComboBox
         // 
         this.resTypeComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
         this.resTypeComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
         this.resTypeComboBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.configurationFileBindingSource, "ResourceType", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.resTypeComboBox.Dock = System.Windows.Forms.DockStyle.Top;
         this.resTypeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.resTypeComboBox.Location = new System.Drawing.Point(93, 3);
         this.resTypeComboBox.Name = "resTypeComboBox";
         this.resTypeComboBox.Size = new System.Drawing.Size(364, 21);
         this.resTypeComboBox.Sorted = true;
         this.resTypeComboBox.TabIndex = 1;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(3, 58);
         this.label3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(68, 13);
         this.label3.TabIndex = 3;
         this.label3.Text = "Image 16x16";
         // 
         // ConfigurationFilePropertiesEditor
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.tableLayoutPanel1);
         this.Name = "ConfigurationFilePropertiesEditor";
         this.Size = new System.Drawing.Size(460, 108);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.tableLayoutPanel1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.configurationFileBindingSource)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.ComboBox img24ComboBox;
      private System.Windows.Forms.ComboBox img16ComboBox;
      private System.Windows.Forms.ComboBox textComboBox;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.ComboBox resTypeComboBox;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.BindingSource configurationFileBindingSource;

   }
}
