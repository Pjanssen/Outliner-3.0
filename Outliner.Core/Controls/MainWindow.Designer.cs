namespace Outliner.Controls
{
   partial class MainWindow
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
         this.outlinerSplitContainer1 = new Outliner.Controls.OutlinerSplitContainer();
         this.treeView1 = new Outliner.Controls.Tree.TreeView();
         this.treeView2 = new Outliner.Controls.Tree.TreeView();
         ((System.ComponentModel.ISupportInitialize)(this.outlinerSplitContainer1)).BeginInit();
         this.outlinerSplitContainer1.Panel1.SuspendLayout();
         this.outlinerSplitContainer1.Panel2.SuspendLayout();
         this.outlinerSplitContainer1.SuspendLayout();
         this.SuspendLayout();
         // 
         // outlinerSplitContainer1
         // 
         this.outlinerSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.outlinerSplitContainer1.Location = new System.Drawing.Point(0, 0);
         this.outlinerSplitContainer1.Name = "outlinerSplitContainer1";
         this.outlinerSplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
         // 
         // outlinerSplitContainer1.Panel1
         // 
         this.outlinerSplitContainer1.Panel1.Controls.Add(this.treeView1);
         // 
         // outlinerSplitContainer1.Panel2
         // 
         this.outlinerSplitContainer1.Panel2.Controls.Add(this.treeView2);
         this.outlinerSplitContainer1.Panel2Collapsed = true;
         this.outlinerSplitContainer1.Size = new System.Drawing.Size(254, 320);
         this.outlinerSplitContainer1.SplitterDistance = 94;
         this.outlinerSplitContainer1.TabIndex = 0;
         // 
         // treeView1
         // 
         this.treeView1.AllowDrop = true;
         this.treeView1.AutoScroll = true;
         this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.treeView1.Location = new System.Drawing.Point(0, 0);
         this.treeView1.Name = "treeView1";
         this.treeView1.Size = new System.Drawing.Size(254, 320);
         this.treeView1.TabIndex = 0;
         this.treeView1.Text = "treeView1";
         // 
         // treeView2
         // 
         this.treeView2.AllowDrop = true;
         this.treeView2.AutoScroll = true;
         this.treeView2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.treeView2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.treeView2.Location = new System.Drawing.Point(0, 0);
         this.treeView2.Name = "treeView2";
         this.treeView2.Size = new System.Drawing.Size(150, 46);
         this.treeView2.TabIndex = 0;
         this.treeView2.Text = "treeView2";
         // 
         // TestForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(254, 320);
         this.Controls.Add(this.outlinerSplitContainer1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
         this.Name = "TestForm";
         this.Text = "Outliner";
         this.outlinerSplitContainer1.Panel1.ResumeLayout(false);
         this.outlinerSplitContainer1.Panel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.outlinerSplitContainer1)).EndInit();
         this.outlinerSplitContainer1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      internal OutlinerSplitContainer outlinerSplitContainer1;
      internal Tree.TreeView treeView1;
      internal Tree.TreeView treeView2;

   }
}