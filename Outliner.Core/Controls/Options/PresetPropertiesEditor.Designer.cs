namespace Outliner.Controls.Options
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
         this.openImageFileDialog = new System.Windows.Forms.OpenFileDialog();
         this.outlinerGroupBox2 = new Outliner.Controls.OutlinerGroupBox();
         this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
         this.image16FileLbl = new System.Windows.Forms.Label();
         this.image16ResLbl = new System.Windows.Forms.Label();
         this.imageResTypeLbl = new System.Windows.Forms.Label();
         this.imgResRadioButton = new System.Windows.Forms.RadioButton();
         this.imgFileRadioButton = new System.Windows.Forms.RadioButton();
         this.image16FileBrowseBtn = new System.Windows.Forms.Button();
         this.image16FileTextBox = new System.Windows.Forms.TextBox();
         this.presetBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.image24FileLbl = new System.Windows.Forms.Label();
         this.image24FileBrowseBtn = new System.Windows.Forms.Button();
         this.image24FileTextBox = new System.Windows.Forms.TextBox();
         this.image24ResLbl = new System.Windows.Forms.Label();
         this.imageResTypeComboBox = new System.Windows.Forms.ComboBox();
         this.resTypeBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.image16ResComboBox = new System.Windows.Forms.ComboBox();
         this.image24ResComboBox = new System.Windows.Forms.ComboBox();
         this.outlinerGroupBox1 = new Outliner.Controls.OutlinerGroupBox();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.contextMenuBrowseBtn = new System.Windows.Forms.Button();
         this.contextMenuTextBox = new System.Windows.Forms.TextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.checkBox1 = new System.Windows.Forms.CheckBox();
         this.nodeSorterBindingSource = new System.Windows.Forms.BindingSource(this.components);
         this.sorterComboBox = new System.Windows.Forms.ComboBox();
         this.label6 = new System.Windows.Forms.Label();
         this.modeComboBox = new System.Windows.Forms.ComboBox();
         this.label10 = new System.Windows.Forms.Label();
         this.label9 = new System.Windows.Forms.Label();
         this.nameTextBox = new System.Windows.Forms.TextBox();
         this.openContextMenuFileDialog = new System.Windows.Forms.OpenFileDialog();
         this.outlinerGroupBox2.SuspendLayout();
         this.tableLayoutPanel3.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.presetBindingSource)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.resTypeBindingSource)).BeginInit();
         this.outlinerGroupBox1.SuspendLayout();
         this.tableLayoutPanel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.nodeSorterBindingSource)).BeginInit();
         this.SuspendLayout();
         // 
         // openImageFileDialog
         // 
         this.openImageFileDialog.Filter = "GIF files|*.gif|JPEG files|*.jpg,*.jpeg|PNG files|*.png|All files|*.*";
         this.openImageFileDialog.Title = "Select image";
         // 
         // outlinerGroupBox2
         // 
         this.outlinerGroupBox2.BorderColor = System.Drawing.Color.Black;
         this.outlinerGroupBox2.Controls.Add(this.tableLayoutPanel3);
         this.outlinerGroupBox2.Dock = System.Windows.Forms.DockStyle.Top;
         this.outlinerGroupBox2.Location = new System.Drawing.Point(0, 135);
         this.outlinerGroupBox2.Name = "outlinerGroupBox2";
         this.outlinerGroupBox2.Padding = new System.Windows.Forms.Padding(5, 3, 5, 5);
         this.outlinerGroupBox2.Size = new System.Drawing.Size(481, 196);
         this.outlinerGroupBox2.TabIndex = 3;
         this.outlinerGroupBox2.TabStop = false;
         this.outlinerGroupBox2.Text = "Images";
         // 
         // tableLayoutPanel3
         // 
         this.tableLayoutPanel3.ColumnCount = 3;
         this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
         this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
         this.tableLayoutPanel3.Controls.Add(this.image16FileLbl, 0, 1);
         this.tableLayoutPanel3.Controls.Add(this.image16ResLbl, 0, 5);
         this.tableLayoutPanel3.Controls.Add(this.imageResTypeLbl, 0, 4);
         this.tableLayoutPanel3.Controls.Add(this.imgResRadioButton, 0, 3);
         this.tableLayoutPanel3.Controls.Add(this.imgFileRadioButton, 0, 0);
         this.tableLayoutPanel3.Controls.Add(this.image16FileBrowseBtn, 2, 1);
         this.tableLayoutPanel3.Controls.Add(this.image16FileTextBox, 1, 1);
         this.tableLayoutPanel3.Controls.Add(this.image24FileLbl, 0, 2);
         this.tableLayoutPanel3.Controls.Add(this.image24FileBrowseBtn, 2, 2);
         this.tableLayoutPanel3.Controls.Add(this.image24FileTextBox, 1, 2);
         this.tableLayoutPanel3.Controls.Add(this.image24ResLbl, 0, 6);
         this.tableLayoutPanel3.Controls.Add(this.imageResTypeComboBox, 1, 4);
         this.tableLayoutPanel3.Controls.Add(this.image16ResComboBox, 1, 5);
         this.tableLayoutPanel3.Controls.Add(this.image24ResComboBox, 1, 6);
         this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel3.Location = new System.Drawing.Point(5, 16);
         this.tableLayoutPanel3.Name = "tableLayoutPanel3";
         this.tableLayoutPanel3.RowCount = 7;
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
         this.tableLayoutPanel3.Size = new System.Drawing.Size(471, 175);
         this.tableLayoutPanel3.TabIndex = 2;
         // 
         // image16FileLbl
         // 
         this.image16FileLbl.AutoSize = true;
         this.image16FileLbl.Location = new System.Drawing.Point(3, 27);
         this.image16FileLbl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
         this.image16FileLbl.Name = "image16FileLbl";
         this.image16FileLbl.Size = new System.Drawing.Size(36, 13);
         this.image16FileLbl.TabIndex = 8;
         this.image16FileLbl.Text = "16x16";
         // 
         // image16ResLbl
         // 
         this.image16ResLbl.AutoSize = true;
         this.image16ResLbl.Location = new System.Drawing.Point(3, 131);
         this.image16ResLbl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
         this.image16ResLbl.Name = "image16ResLbl";
         this.image16ResLbl.Size = new System.Drawing.Size(36, 13);
         this.image16ResLbl.TabIndex = 6;
         this.image16ResLbl.Text = "16x16";
         // 
         // imageResTypeLbl
         // 
         this.imageResTypeLbl.AutoSize = true;
         this.imageResTypeLbl.Location = new System.Drawing.Point(3, 106);
         this.imageResTypeLbl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
         this.imageResTypeLbl.Name = "imageResTypeLbl";
         this.imageResTypeLbl.Size = new System.Drawing.Size(31, 13);
         this.imageResTypeLbl.TabIndex = 5;
         this.imageResTypeLbl.Text = "Type";
         // 
         // imgResRadioButton
         // 
         this.imgResRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.imgResRadioButton.AutoSize = true;
         this.tableLayoutPanel3.SetColumnSpan(this.imgResRadioButton, 2);
         this.imgResRadioButton.Location = new System.Drawing.Point(3, 81);
         this.imgResRadioButton.Name = "imgResRadioButton";
         this.imgResRadioButton.Size = new System.Drawing.Size(125, 17);
         this.imgResRadioButton.TabIndex = 1;
         this.imgResRadioButton.TabStop = true;
         this.imgResRadioButton.Text = "Embedded Resource";
         this.imgResRadioButton.UseVisualStyleBackColor = true;
         this.imgResRadioButton.CheckedChanged += new System.EventHandler(this.imgResRadioButton_CheckedChanged);
         // 
         // imgFileRadioButton
         // 
         this.imgFileRadioButton.AutoSize = true;
         this.tableLayoutPanel3.SetColumnSpan(this.imgFileRadioButton, 2);
         this.imgFileRadioButton.Location = new System.Drawing.Point(3, 3);
         this.imgFileRadioButton.Name = "imgFileRadioButton";
         this.imgFileRadioButton.Size = new System.Drawing.Size(70, 16);
         this.imgFileRadioButton.TabIndex = 0;
         this.imgFileRadioButton.TabStop = true;
         this.imgFileRadioButton.Text = "Local File";
         this.imgFileRadioButton.UseVisualStyleBackColor = true;
         this.imgFileRadioButton.CheckedChanged += new System.EventHandler(this.imgFileRadioButton_CheckedChanged);
         // 
         // image16FileBrowseBtn
         // 
         this.image16FileBrowseBtn.Dock = System.Windows.Forms.DockStyle.Fill;
         this.image16FileBrowseBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.image16FileBrowseBtn.Location = new System.Drawing.Point(399, 23);
         this.image16FileBrowseBtn.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
         this.image16FileBrowseBtn.Name = "image16FileBrowseBtn";
         this.image16FileBrowseBtn.Size = new System.Drawing.Size(69, 23);
         this.image16FileBrowseBtn.TabIndex = 3;
         this.image16FileBrowseBtn.Text = "Browse";
         this.image16FileBrowseBtn.UseVisualStyleBackColor = true;
         this.image16FileBrowseBtn.Click += new System.EventHandler(this.image16FileBrowseBtn_Click);
         // 
         // image16FileTextBox
         // 
         this.image16FileTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.presetBindingSource, "Image16Name", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.image16FileTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.image16FileTextBox.Location = new System.Drawing.Point(88, 25);
         this.image16FileTextBox.Name = "image16FileTextBox";
         this.image16FileTextBox.Size = new System.Drawing.Size(305, 20);
         this.image16FileTextBox.TabIndex = 9;
         // 
         // presetBindingSource
         // 
         this.presetBindingSource.DataSource = typeof(Outliner.Presets.OutlinerPreset);
         this.presetBindingSource.BindingComplete += new System.Windows.Forms.BindingCompleteEventHandler(this.presetBindingSource_BindingComplete);
         // 
         // image24FileLbl
         // 
         this.image24FileLbl.AutoSize = true;
         this.image24FileLbl.Location = new System.Drawing.Point(3, 54);
         this.image24FileLbl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
         this.image24FileLbl.Name = "image24FileLbl";
         this.image24FileLbl.Size = new System.Drawing.Size(36, 13);
         this.image24FileLbl.TabIndex = 12;
         this.image24FileLbl.Text = "24x24";
         // 
         // image24FileBrowseBtn
         // 
         this.image24FileBrowseBtn.Dock = System.Windows.Forms.DockStyle.Fill;
         this.image24FileBrowseBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.image24FileBrowseBtn.Location = new System.Drawing.Point(399, 50);
         this.image24FileBrowseBtn.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
         this.image24FileBrowseBtn.Name = "image24FileBrowseBtn";
         this.image24FileBrowseBtn.Size = new System.Drawing.Size(69, 23);
         this.image24FileBrowseBtn.TabIndex = 13;
         this.image24FileBrowseBtn.Text = "Browse";
         this.image24FileBrowseBtn.UseVisualStyleBackColor = true;
         this.image24FileBrowseBtn.Click += new System.EventHandler(this.image24FileBrowseBtn_Click);
         // 
         // image24FileTextBox
         // 
         this.image24FileTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.presetBindingSource, "Image24Name", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.image24FileTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.image24FileTextBox.Location = new System.Drawing.Point(88, 52);
         this.image24FileTextBox.Name = "image24FileTextBox";
         this.image24FileTextBox.Size = new System.Drawing.Size(305, 20);
         this.image24FileTextBox.TabIndex = 14;
         // 
         // image24ResLbl
         // 
         this.image24ResLbl.AutoSize = true;
         this.image24ResLbl.Location = new System.Drawing.Point(3, 156);
         this.image24ResLbl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
         this.image24ResLbl.Name = "image24ResLbl";
         this.image24ResLbl.Size = new System.Drawing.Size(36, 13);
         this.image24ResLbl.TabIndex = 15;
         this.image24ResLbl.Text = "24x24";
         // 
         // imageResTypeComboBox
         // 
         this.imageResTypeComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.resTypeBindingSource, "ImageResourceTypeName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.imageResTypeComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.imageResTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.imageResTypeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.imageResTypeComboBox.FormattingEnabled = true;
         this.imageResTypeComboBox.Location = new System.Drawing.Point(88, 104);
         this.imageResTypeComboBox.Name = "imageResTypeComboBox";
         this.imageResTypeComboBox.Size = new System.Drawing.Size(305, 21);
         this.imageResTypeComboBox.TabIndex = 17;
         // 
         // resTypeBindingSource
         // 
         this.resTypeBindingSource.DataSource = typeof(Outliner.Presets.OutlinerPreset);
         this.resTypeBindingSource.BindingComplete += new System.Windows.Forms.BindingCompleteEventHandler(this.resTypeBindingSource_BindingComplete);
         // 
         // image16ResComboBox
         // 
         this.image16ResComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.presetBindingSource, "Image16Name", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.image16ResComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.image16ResComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.image16ResComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.image16ResComboBox.FormattingEnabled = true;
         this.image16ResComboBox.Location = new System.Drawing.Point(88, 129);
         this.image16ResComboBox.Name = "image16ResComboBox";
         this.image16ResComboBox.Size = new System.Drawing.Size(305, 21);
         this.image16ResComboBox.TabIndex = 18;
         // 
         // image24ResComboBox
         // 
         this.image24ResComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedItem", this.presetBindingSource, "Image24Name", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.image24ResComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.image24ResComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.image24ResComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.image24ResComboBox.FormattingEnabled = true;
         this.image24ResComboBox.Location = new System.Drawing.Point(88, 154);
         this.image24ResComboBox.Name = "image24ResComboBox";
         this.image24ResComboBox.Size = new System.Drawing.Size(305, 21);
         this.image24ResComboBox.TabIndex = 19;
         // 
         // outlinerGroupBox1
         // 
         this.outlinerGroupBox1.BorderColor = System.Drawing.Color.Black;
         this.outlinerGroupBox1.Controls.Add(this.tableLayoutPanel1);
         this.outlinerGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
         this.outlinerGroupBox1.Location = new System.Drawing.Point(0, 0);
         this.outlinerGroupBox1.Name = "outlinerGroupBox1";
         this.outlinerGroupBox1.Padding = new System.Windows.Forms.Padding(5, 3, 5, 5);
         this.outlinerGroupBox1.Size = new System.Drawing.Size(481, 135);
         this.outlinerGroupBox1.TabIndex = 2;
         this.outlinerGroupBox1.TabStop = false;
         this.outlinerGroupBox1.Text = "Properties";
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 3;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
         this.tableLayoutPanel1.Controls.Add(this.contextMenuBrowseBtn, 2, 3);
         this.tableLayoutPanel1.Controls.Add(this.contextMenuTextBox, 1, 3);
         this.tableLayoutPanel1.Controls.Add(this.label1, 0, 3);
         this.tableLayoutPanel1.Controls.Add(this.checkBox1, 2, 2);
         this.tableLayoutPanel1.Controls.Add(this.sorterComboBox, 1, 2);
         this.tableLayoutPanel1.Controls.Add(this.label6, 0, 2);
         this.tableLayoutPanel1.Controls.Add(this.modeComboBox, 1, 1);
         this.tableLayoutPanel1.Controls.Add(this.label10, 0, 1);
         this.tableLayoutPanel1.Controls.Add(this.label9, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.nameTextBox, 1, 0);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(5, 16);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 4;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(471, 114);
         this.tableLayoutPanel1.TabIndex = 2;
         // 
         // contextMenuBrowseBtn
         // 
         this.contextMenuBrowseBtn.Dock = System.Windows.Forms.DockStyle.Top;
         this.contextMenuBrowseBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.contextMenuBrowseBtn.Location = new System.Drawing.Point(399, 87);
         this.contextMenuBrowseBtn.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
         this.contextMenuBrowseBtn.Name = "contextMenuBrowseBtn";
         this.contextMenuBrowseBtn.Size = new System.Drawing.Size(69, 24);
         this.contextMenuBrowseBtn.TabIndex = 10;
         this.contextMenuBrowseBtn.Text = "Browse";
         this.contextMenuBrowseBtn.UseVisualStyleBackColor = true;
         this.contextMenuBrowseBtn.Click += new System.EventHandler(this.contextMenuBrowseBtn_Click);
         // 
         // contextMenuTextBox
         // 
         this.contextMenuTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.presetBindingSource, "ContextMenuFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.contextMenuTextBox.Dock = System.Windows.Forms.DockStyle.Top;
         this.contextMenuTextBox.Location = new System.Drawing.Point(90, 89);
         this.contextMenuTextBox.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
         this.contextMenuTextBox.Name = "contextMenuTextBox";
         this.contextMenuTextBox.Size = new System.Drawing.Size(301, 20);
         this.contextMenuTextBox.TabIndex = 9;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(3, 92);
         this.label1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 0);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(73, 13);
         this.label1.TabIndex = 8;
         this.label1.Text = "Context-Menu";
         // 
         // checkBox1
         // 
         this.checkBox1.AutoSize = true;
         this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.nodeSorterBindingSource, "Descending", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.checkBox1.Location = new System.Drawing.Point(400, 64);
         this.checkBox1.Margin = new System.Windows.Forms.Padding(4, 6, 3, 3);
         this.checkBox1.Name = "checkBox1";
         this.checkBox1.Size = new System.Drawing.Size(66, 17);
         this.checkBox1.TabIndex = 7;
         this.checkBox1.Text = "Reverse";
         this.checkBox1.UseVisualStyleBackColor = true;
         // 
         // nodeSorterBindingSource
         // 
         this.nodeSorterBindingSource.DataSource = typeof(Outliner.NodeSorters.NodeSorter);
         this.nodeSorterBindingSource.BindingComplete += new System.Windows.Forms.BindingCompleteEventHandler(this.nodeSorterBindingSource_BindingComplete);
         // 
         // sorterComboBox
         // 
         this.sorterComboBox.Dock = System.Windows.Forms.DockStyle.Top;
         this.sorterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.sorterComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.sorterComboBox.FormattingEnabled = true;
         this.sorterComboBox.Location = new System.Drawing.Point(88, 61);
         this.sorterComboBox.Name = "sorterComboBox";
         this.sorterComboBox.Size = new System.Drawing.Size(305, 21);
         this.sorterComboBox.TabIndex = 6;
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Dock = System.Windows.Forms.DockStyle.Top;
         this.label6.Location = new System.Drawing.Point(3, 63);
         this.label6.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(79, 13);
         this.label6.TabIndex = 5;
         this.label6.Text = "Sorting";
         // 
         // modeComboBox
         // 
         this.modeComboBox.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.presetBindingSource, "TreeModeType", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.modeComboBox.DataSource = this.presetBindingSource;
         this.modeComboBox.Dock = System.Windows.Forms.DockStyle.Top;
         this.modeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.modeComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.modeComboBox.FormattingEnabled = true;
         this.modeComboBox.Location = new System.Drawing.Point(88, 33);
         this.modeComboBox.Name = "modeComboBox";
         this.modeComboBox.Size = new System.Drawing.Size(305, 21);
         this.modeComboBox.TabIndex = 4;
         // 
         // label10
         // 
         this.label10.AutoSize = true;
         this.label10.Dock = System.Windows.Forms.DockStyle.Top;
         this.label10.Location = new System.Drawing.Point(3, 35);
         this.label10.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
         this.label10.Name = "label10";
         this.label10.Size = new System.Drawing.Size(79, 13);
         this.label10.TabIndex = 3;
         this.label10.Text = "Mode";
         // 
         // label9
         // 
         this.label9.AutoSize = true;
         this.label9.Dock = System.Windows.Forms.DockStyle.Top;
         this.label9.Location = new System.Drawing.Point(3, 5);
         this.label9.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
         this.label9.Name = "label9";
         this.label9.Size = new System.Drawing.Size(79, 13);
         this.label9.TabIndex = 0;
         this.label9.Text = "Name";
         // 
         // nameTextBox
         // 
         this.nameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.presetBindingSource, "Name", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
         this.nameTextBox.Dock = System.Windows.Forms.DockStyle.Top;
         this.nameTextBox.Location = new System.Drawing.Point(90, 3);
         this.nameTextBox.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
         this.nameTextBox.Name = "nameTextBox";
         this.nameTextBox.Size = new System.Drawing.Size(301, 20);
         this.nameTextBox.TabIndex = 1;
         // 
         // openContextMenuFileDialog
         // 
         this.openContextMenuFileDialog.Filter = "XML files|*.xml|All files|*.*";
         this.openContextMenuFileDialog.Title = "Open context-menu file";
         // 
         // PresetPropertiesEditor
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.outlinerGroupBox2);
         this.Controls.Add(this.outlinerGroupBox1);
         this.Name = "PresetPropertiesEditor";
         this.Size = new System.Drawing.Size(481, 340);
         this.outlinerGroupBox2.ResumeLayout(false);
         this.tableLayoutPanel3.ResumeLayout(false);
         this.tableLayoutPanel3.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.presetBindingSource)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.resTypeBindingSource)).EndInit();
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
      private System.Windows.Forms.Label label9;
      private OutlinerGroupBox outlinerGroupBox2;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
      private System.Windows.Forms.Label image16FileLbl;
      private System.Windows.Forms.Label image16ResLbl;
      private System.Windows.Forms.Label imageResTypeLbl;
      private System.Windows.Forms.RadioButton imgResRadioButton;
      private System.Windows.Forms.RadioButton imgFileRadioButton;
      private System.Windows.Forms.Button image16FileBrowseBtn;
      private System.Windows.Forms.TextBox image16FileTextBox;
      private System.Windows.Forms.Label image24FileLbl;
      private System.Windows.Forms.Button image24FileBrowseBtn;
      private System.Windows.Forms.TextBox image24FileTextBox;
      private System.Windows.Forms.Label image24ResLbl;
      private System.Windows.Forms.ComboBox imageResTypeComboBox;
      private System.Windows.Forms.ComboBox image16ResComboBox;
      private System.Windows.Forms.ComboBox image24ResComboBox;
      private System.Windows.Forms.BindingSource resTypeBindingSource;
      private System.Windows.Forms.TextBox nameTextBox;
      private System.Windows.Forms.OpenFileDialog openImageFileDialog;
      private System.Windows.Forms.Label label10;
      private System.Windows.Forms.ComboBox modeComboBox;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.ComboBox sorterComboBox;
      private System.Windows.Forms.CheckBox checkBox1;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox contextMenuTextBox;
      private System.Windows.Forms.Button contextMenuBrowseBtn;
      private System.Windows.Forms.BindingSource nodeSorterBindingSource;
      private System.Windows.Forms.OpenFileDialog openContextMenuFileDialog;
   }
}
