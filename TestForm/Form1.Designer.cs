namespace TestForm
{
   partial class Form1
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
         this.toolStrip1 = new System.Windows.Forms.ToolStrip();
         this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
         this.singleViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.splitHorizontallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.splitVerticallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
         this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.toolStrip1.SuspendLayout();
         this.SuspendLayout();
         // 
         // toolStrip1
         // 
         this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripSplitButton1});
         this.toolStrip1.Location = new System.Drawing.Point(0, 0);
         this.toolStrip1.Name = "toolStrip1";
         this.toolStrip1.Size = new System.Drawing.Size(306, 25);
         this.toolStrip1.TabIndex = 0;
         this.toolStrip1.Text = "toolStrip1";
         // 
         // toolStripDropDownButton1
         // 
         this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.singleViewToolStripMenuItem,
            this.splitHorizontallyToolStripMenuItem,
            this.splitVerticallyToolStripMenuItem});
         this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
         this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
         this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
         this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
         this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
         // 
         // singleViewToolStripMenuItem
         // 
         this.singleViewToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlText;
         this.singleViewToolStripMenuItem.Name = "singleViewToolStripMenuItem";
         this.singleViewToolStripMenuItem.ShowShortcutKeys = false;
         this.singleViewToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
         this.singleViewToolStripMenuItem.Text = "Single view";
         // 
         // splitHorizontallyToolStripMenuItem
         // 
         this.splitHorizontallyToolStripMenuItem.Name = "splitHorizontallyToolStripMenuItem";
         this.splitHorizontallyToolStripMenuItem.ShowShortcutKeys = false;
         this.splitHorizontallyToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
         this.splitHorizontallyToolStripMenuItem.Text = "Split horizontally";
         // 
         // splitVerticallyToolStripMenuItem
         // 
         this.splitVerticallyToolStripMenuItem.Name = "splitVerticallyToolStripMenuItem";
         this.splitVerticallyToolStripMenuItem.ShowShortcutKeys = false;
         this.splitVerticallyToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
         this.splitVerticallyToolStripMenuItem.Text = "Split vertically";
         // 
         // toolStripSplitButton1
         // 
         this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
         this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem});
         this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
         this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
         this.toolStripSplitButton1.Name = "toolStripSplitButton1";
         this.toolStripSplitButton1.Size = new System.Drawing.Size(32, 22);
         this.toolStripSplitButton1.Text = "toolStripSplitButton1";
         // 
         // testToolStripMenuItem
         // 
         this.testToolStripMenuItem.Name = "testToolStripMenuItem";
         this.testToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
         this.testToolStripMenuItem.Text = "test";
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(306, 64);
         this.ControlBox = false;
         this.Controls.Add(this.toolStrip1);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
         this.Name = "Form1";
         this.ShowIcon = false;
         this.ShowInTaskbar = false;
         this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
         this.Text = "Form1";
         this.TopMost = true;
         this.toolStrip1.ResumeLayout(false);
         this.toolStrip1.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.ToolStrip toolStrip1;
      private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
      private System.Windows.Forms.ToolStripMenuItem singleViewToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem splitHorizontallyToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem splitVerticallyToolStripMenuItem;
      private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
      private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
   }
}