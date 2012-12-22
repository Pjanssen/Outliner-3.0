namespace Outliner.Controls.Options
{
   partial class ConfigFilesEditor<T>
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
         this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
         this.cancelBtn = new System.Windows.Forms.Button();
         this.okBtn = new System.Windows.Forms.Button();
         this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
         this.addFileBtn = new System.Windows.Forms.Button();
         this.deleteFileBtn = new System.Windows.Forms.Button();
         this.editorPanel = new System.Windows.Forms.Panel();
         this.filesTree = new Outliner.Controls.Tree.TreeView();
         this.tableLayoutPanel1.SuspendLayout();
         this.tableLayoutPanel2.SuspendLayout();
         this.tableLayoutPanel3.SuspendLayout();
         this.SuspendLayout();
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 2;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.Controls.Add(this.filesTree, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 2);
         this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 1);
         this.tableLayoutPanel1.Controls.Add(this.editorPanel, 1, 0);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
         this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 3;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(622, 490);
         this.tableLayoutPanel1.TabIndex = 0;
         // 
         // tableLayoutPanel2
         // 
         this.tableLayoutPanel2.ColumnCount = 2;
         this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
         this.tableLayoutPanel2.Controls.Add(this.cancelBtn, 1, 0);
         this.tableLayoutPanel2.Controls.Add(this.okBtn, 0, 0);
         this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel2.Location = new System.Drawing.Point(200, 455);
         this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
         this.tableLayoutPanel2.Name = "tableLayoutPanel2";
         this.tableLayoutPanel2.RowCount = 1;
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel2.Size = new System.Drawing.Size(422, 35);
         this.tableLayoutPanel2.TabIndex = 2;
         // 
         // cancelBtn
         // 
         this.cancelBtn.Dock = System.Windows.Forms.DockStyle.Right;
         this.cancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.cancelBtn.Location = new System.Drawing.Point(339, 3);
         this.cancelBtn.Name = "cancelBtn";
         this.cancelBtn.Size = new System.Drawing.Size(80, 29);
         this.cancelBtn.TabIndex = 1;
         this.cancelBtn.Text = "Cancel";
         this.cancelBtn.UseVisualStyleBackColor = true;
         this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
         // 
         // okBtn
         // 
         this.okBtn.Dock = System.Windows.Forms.DockStyle.Right;
         this.okBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.okBtn.Location = new System.Drawing.Point(249, 3);
         this.okBtn.Name = "okBtn";
         this.okBtn.Size = new System.Drawing.Size(80, 29);
         this.okBtn.TabIndex = 0;
         this.okBtn.Text = "OK";
         this.okBtn.UseVisualStyleBackColor = true;
         this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
         // 
         // tableLayoutPanel3
         // 
         this.tableLayoutPanel3.ColumnCount = 2;
         this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel3.Controls.Add(this.addFileBtn, 0, 0);
         this.tableLayoutPanel3.Controls.Add(this.deleteFileBtn, 1, 0);
         this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 420);
         this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
         this.tableLayoutPanel3.Name = "tableLayoutPanel3";
         this.tableLayoutPanel3.RowCount = 1;
         this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel3.Size = new System.Drawing.Size(200, 35);
         this.tableLayoutPanel3.TabIndex = 3;
         // 
         // addFileBtn
         // 
         this.addFileBtn.Dock = System.Windows.Forms.DockStyle.Fill;
         this.addFileBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.addFileBtn.Location = new System.Drawing.Point(3, 3);
         this.addFileBtn.Name = "addFileBtn";
         this.addFileBtn.Size = new System.Drawing.Size(94, 29);
         this.addFileBtn.TabIndex = 0;
         this.addFileBtn.Text = "Add";
         this.addFileBtn.UseVisualStyleBackColor = true;
         this.addFileBtn.Click += new System.EventHandler(this.addFileBtn_Click);
         // 
         // deleteFileBtn
         // 
         this.deleteFileBtn.Dock = System.Windows.Forms.DockStyle.Fill;
         this.deleteFileBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.deleteFileBtn.Location = new System.Drawing.Point(103, 3);
         this.deleteFileBtn.Name = "deleteFileBtn";
         this.deleteFileBtn.Size = new System.Drawing.Size(94, 29);
         this.deleteFileBtn.TabIndex = 1;
         this.deleteFileBtn.Text = "Delete";
         this.deleteFileBtn.UseVisualStyleBackColor = true;
         this.deleteFileBtn.Click += new System.EventHandler(this.deleteFileBtn_Click);
         // 
         // editorPanel
         // 
         this.editorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.editorPanel.Location = new System.Drawing.Point(203, 3);
         this.editorPanel.Name = "editorPanel";
         this.editorPanel.Size = new System.Drawing.Size(416, 414);
         this.editorPanel.TabIndex = 5;
         // 
         // filesTree
         // 
         this.filesTree.AllowDrop = true;
         this.filesTree.AutoScroll = true;
         this.filesTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.filesTree.Dock = System.Windows.Forms.DockStyle.Fill;
         this.filesTree.Location = new System.Drawing.Point(3, 3);
         this.filesTree.Name = "filesTree";
         this.filesTree.Size = new System.Drawing.Size(194, 414);
         this.filesTree.TabIndex = 1;
         this.filesTree.Text = "treeView1";
         this.filesTree.SelectionChanged += new System.EventHandler<Outliner.Controls.Tree.SelectionChangedEventArgs>(this.filesTree_SelectionChanged);
         this.filesTree.BeforeNodeTextEdit += new System.EventHandler<Outliner.Controls.Tree.BeforeNodeTextEditEventArgs>(this.filesTree_BeforeNodeTextEdit);
         this.filesTree.AfterNodeTextEdit += new System.EventHandler<Outliner.Controls.Tree.AfterNodeTextEditEventArgs>(this.filesTree_AfterNodeTextEdit);
         // 
         // ConfigFilesEditor
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(628, 496);
         this.Controls.Add(this.tableLayoutPanel1);
         this.MaximizeBox = false;
         this.Name = "ConfigFilesEditor";
         this.Padding = new System.Windows.Forms.Padding(3);
         this.ShowIcon = false;
         this.ShowInTaskbar = false;
         this.Text = "UserFilesEditor";
         this.tableLayoutPanel1.ResumeLayout(false);
         this.tableLayoutPanel2.ResumeLayout(false);
         this.tableLayoutPanel3.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
      private System.Windows.Forms.Button cancelBtn;
      private System.Windows.Forms.Button okBtn;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
      private System.Windows.Forms.Button addFileBtn;
      private System.Windows.Forms.Button deleteFileBtn;
      protected Tree.TreeView filesTree;
      private System.Windows.Forms.Panel editorPanel;
   }
}