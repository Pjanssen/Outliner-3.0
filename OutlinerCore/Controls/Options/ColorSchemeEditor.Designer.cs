namespace Outliner.Controls.Options
{
   partial class ColorSchemeEditor
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
         this.comboBox1 = new System.Windows.Forms.ComboBox();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.colorsTree = new Outliner.Controls.Tree.TreeView();
         this.tableLayoutPanel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // comboBox1
         // 
         this.comboBox1.Dock = System.Windows.Forms.DockStyle.Top;
         this.comboBox1.FormattingEnabled = true;
         this.comboBox1.Location = new System.Drawing.Point(3, 3);
         this.comboBox1.Name = "comboBox1";
         this.comboBox1.Size = new System.Drawing.Size(208, 21);
         this.comboBox1.TabIndex = 0;
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 3;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
         this.tableLayoutPanel1.Controls.Add(this.comboBox1, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.colorsTree, 0, 1);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 2;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.210526F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.78947F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(274, 299);
         this.tableLayoutPanel1.TabIndex = 1;
         // 
         // colorsTree
         // 
         this.colorsTree.AllowDrop = true;
         this.colorsTree.AutoScroll = true;
         this.colorsTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.colorsTree.Dock = System.Windows.Forms.DockStyle.Fill;
         this.colorsTree.Location = new System.Drawing.Point(3, 30);
         this.colorsTree.Name = "colorsTree";
         this.colorsTree.Size = new System.Drawing.Size(208, 266);
         this.colorsTree.TabIndex = 1;
         // 
         // ColorSchemeEditor
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.tableLayoutPanel1);
         this.Name = "ColorSchemeEditor";
         this.Size = new System.Drawing.Size(274, 299);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.ComboBox comboBox1;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private Tree.TreeView colorsTree;
   }
}
