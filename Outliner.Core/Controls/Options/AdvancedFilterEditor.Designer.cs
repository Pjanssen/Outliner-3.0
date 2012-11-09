namespace Outliner.Controls.Options
{
   partial class AdvancedFilterEditor
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
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.closeBtn = new System.Windows.Forms.Button();
         this.filterCollectionEditor1 = new Outliner.Controls.Options.FilterCollectionEditor();
         this.tableLayoutPanel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 1;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.Controls.Add(this.filterCollectionEditor1, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.closeBtn, 0, 1);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 2;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(366, 356);
         this.tableLayoutPanel1.TabIndex = 1;
         // 
         // closeBtn
         // 
         this.closeBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
         this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.closeBtn.Location = new System.Drawing.Point(133, 322);
         this.closeBtn.Name = "closeBtn";
         this.closeBtn.Size = new System.Drawing.Size(100, 30);
         this.closeBtn.TabIndex = 1;
         this.closeBtn.Text = "Close";
         this.closeBtn.UseVisualStyleBackColor = true;
         this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
         // 
         // filterCollectionEditor1
         // 
         this.filterCollectionEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.filterCollectionEditor1.Location = new System.Drawing.Point(3, 3);
         this.filterCollectionEditor1.Name = "filterCollectionEditor1";
         this.filterCollectionEditor1.Size = new System.Drawing.Size(360, 312);
         this.filterCollectionEditor1.TabIndex = 0;
         // 
         // AdvancedFilterEditor
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(366, 356);
         this.Controls.Add(this.tableLayoutPanel1);
         this.MaximizeBox = false;
         this.Name = "AdvancedFilterEditor";
         this.ShowIcon = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "Advanced Filter";
         this.tableLayoutPanel1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private FilterCollectionEditor filterCollectionEditor1;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.Button closeBtn;
   }
}