namespace Outliner.Controls
{
   partial class TestControl
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
         Outliner.Controls.TreeViewColors treeViewColors1 = new Outliner.Controls.TreeViewColors();
         Outliner.Controls.Layout.TreeNodeLayout treeNodeLayout1 = new Outliner.Controls.Layout.TreeNodeLayout();
         this.treeView1 = new Outliner.Controls.TreeView();
         this.panel1 = new System.Windows.Forms.Panel();
         this.panel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // treeView1
         // 
         this.treeView1.AllowDrop = true;
         this.treeView1.AutoScroll = true;
         treeViewColors1.BackColor = System.Drawing.SystemColors.Window;
         treeViewColors1.FrozenBackColor = System.Drawing.SystemColors.Window;
         treeViewColors1.FrozenForeColor = System.Drawing.SystemColors.GrayText;
         treeViewColors1.HiddenBackColor = System.Drawing.SystemColors.Window;
         treeViewColors1.HiddenForeColor = System.Drawing.SystemColors.GrayText;
         treeViewColors1.LayerBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
         treeViewColors1.LayerForeColor = System.Drawing.SystemColors.WindowText;
         treeViewColors1.LineColor = System.Drawing.SystemColors.ControlText;
         treeViewColors1.LinkBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(177)))), ((int)(((byte)(177)))));
         treeViewColors1.LinkForeColor = System.Drawing.SystemColors.WindowText;
         treeViewColors1.NodeBackColor = System.Drawing.SystemColors.Window;
         treeViewColors1.NodeForeColor = System.Drawing.SystemColors.WindowText;
         treeViewColors1.ParentBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(255)))), ((int)(((byte)(177)))));
         treeViewColors1.ParentForeColor = System.Drawing.SystemColors.WindowText;
         treeViewColors1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
         treeViewColors1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
         this.treeView1.Colors = treeViewColors1;
         this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.treeView1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.treeView1.Location = new System.Drawing.Point(4, 4);
         this.treeView1.Name = "treeView1";
         this.treeView1.NodeSorter = null;
         this.treeView1.Size = new System.Drawing.Size(201, 229);
         this.treeView1.TabIndex = 0;
         treeNodeLayout1.FullRowSelect = true;
         treeNodeLayout1.ItemHeight = 20;
         treeNodeLayout1.TreeView = this.treeView1;
         this.treeView1.TreeNodeLayout = treeNodeLayout1;
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.treeView1);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Name = "panel1";
         this.panel1.Padding = new System.Windows.Forms.Padding(4);
         this.panel1.Size = new System.Drawing.Size(209, 237);
         this.panel1.TabIndex = 1;
         // 
         // TestControl
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.panel1);
         this.MinimumSize = new System.Drawing.Size(100, 150);
         this.Name = "TestControl";
         this.Size = new System.Drawing.Size(209, 237);
         this.panel1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      public TreeView treeView1;
      private System.Windows.Forms.Panel panel1;


   }
}
