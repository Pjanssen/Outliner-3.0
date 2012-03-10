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
         this.components = new System.ComponentModel.Container();
         this.treeView1 = new Outliner.Controls.TreeView();
         this.SuspendLayout();
         // 
         // treeView1
         // 
         this.treeView1.AllowDrop = true;
         this.treeView1.CheckBoxes = true;
         this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.treeView1.Indent = 12;
         this.treeView1.Location = new System.Drawing.Point(0, 0);
         this.treeView1.Name = "treeView1";
         this.treeView1.Size = new System.Drawing.Size(260, 259);
         this.treeView1.TabIndex = 0;
         // 
         // MainWindow
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(260, 259);
         this.Controls.Add(this.treeView1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
         this.Name = "MainWindow";
         this.Text = "MainWindow";
         this.ResumeLayout(false);

      }

      #endregion

      public TreeView treeView1;



   }
}