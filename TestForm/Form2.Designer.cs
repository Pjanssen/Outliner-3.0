namespace TestForm
{
   partial class Form2
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
         this.treeView1 = new Outliner.Controls.Tree.TreeView();
         this.button1 = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // treeView1
         // 
         this.treeView1.AllowDrop = true;
         this.treeView1.AutoScroll = true;
         this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.treeView1.Dock = System.Windows.Forms.DockStyle.Top;
         this.treeView1.Location = new System.Drawing.Point(3, 3);
         this.treeView1.Name = "treeView1";
         this.treeView1.Size = new System.Drawing.Size(307, 256);
         this.treeView1.TabIndex = 0;
         this.treeView1.Text = "treeView1";
         // 
         // button1
         // 
         this.button1.Dock = System.Windows.Forms.DockStyle.Top;
         this.button1.Location = new System.Drawing.Point(3, 259);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(307, 23);
         this.button1.TabIndex = 1;
         this.button1.Text = "button1";
         this.button1.UseVisualStyleBackColor = true;
         this.button1.Click += new System.EventHandler(this.button1_Click);
         // 
         // Form2
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(313, 316);
         this.Controls.Add(this.button1);
         this.Controls.Add(this.treeView1);
         this.Name = "Form2";
         this.Padding = new System.Windows.Forms.Padding(3);
         this.Text = "Form2";
         this.ResumeLayout(false);

      }

      #endregion

      private Outliner.Controls.Tree.TreeView treeView1;
      private System.Windows.Forms.Button button1;
   }
}