namespace Outliner.Controls.Options
{
   partial class PresetEditor
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
         this.tableLayout = new System.Windows.Forms.TableLayoutPanel();
         this.addDeletePanel = new System.Windows.Forms.TableLayoutPanel();
         this.deleteBtn = new System.Windows.Forms.Button();
         this.addBtn = new System.Windows.Forms.Button();
         this.okCancelPanel = new System.Windows.Forms.TableLayoutPanel();
         this.cancelBtn = new System.Windows.Forms.Button();
         this.okBtn = new System.Windows.Forms.Button();
         this.propertiesPanel = new System.Windows.Forms.Panel();
         this.presetsTree = new Outliner.Controls.Tree.TreeView();
         this.previewGroupBox = new Outliner.Controls.OutlinerGroupBox();
         this.previewTree = new Outliner.Controls.Tree.TreeView();
         this.tableLayout.SuspendLayout();
         this.addDeletePanel.SuspendLayout();
         this.okCancelPanel.SuspendLayout();
         this.previewGroupBox.SuspendLayout();
         this.SuspendLayout();
         // 
         // tableLayout
         // 
         this.tableLayout.ColumnCount = 2;
         this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
         this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayout.Controls.Add(this.addDeletePanel, 0, 2);
         this.tableLayout.Controls.Add(this.presetsTree, 0, 0);
         this.tableLayout.Controls.Add(this.okCancelPanel, 1, 3);
         this.tableLayout.Controls.Add(this.propertiesPanel, 1, 0);
         this.tableLayout.Controls.Add(this.previewGroupBox, 1, 1);
         this.tableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayout.Location = new System.Drawing.Point(0, 0);
         this.tableLayout.Name = "tableLayout";
         this.tableLayout.Padding = new System.Windows.Forms.Padding(7);
         this.tableLayout.RowCount = 4;
         this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 171F));
         this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
         this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
         this.tableLayout.Size = new System.Drawing.Size(647, 582);
         this.tableLayout.TabIndex = 9;
         // 
         // addDeletePanel
         // 
         this.addDeletePanel.ColumnCount = 2;
         this.addDeletePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.addDeletePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.addDeletePanel.Controls.Add(this.deleteBtn, 1, 0);
         this.addDeletePanel.Controls.Add(this.addBtn, 0, 0);
         this.addDeletePanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.addDeletePanel.Location = new System.Drawing.Point(7, 503);
         this.addDeletePanel.Margin = new System.Windows.Forms.Padding(0);
         this.addDeletePanel.Name = "addDeletePanel";
         this.addDeletePanel.RowCount = 1;
         this.addDeletePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.addDeletePanel.Size = new System.Drawing.Size(200, 36);
         this.addDeletePanel.TabIndex = 16;
         // 
         // deleteBtn
         // 
         this.deleteBtn.Dock = System.Windows.Forms.DockStyle.Fill;
         this.deleteBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.deleteBtn.Location = new System.Drawing.Point(103, 3);
         this.deleteBtn.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
         this.deleteBtn.Name = "deleteBtn";
         this.deleteBtn.Size = new System.Drawing.Size(93, 30);
         this.deleteBtn.TabIndex = 1;
         this.deleteBtn.Text = "Delete";
         this.deleteBtn.UseVisualStyleBackColor = true;
         // 
         // addBtn
         // 
         this.addBtn.Dock = System.Windows.Forms.DockStyle.Fill;
         this.addBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.addBtn.Location = new System.Drawing.Point(6, 3);
         this.addBtn.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
         this.addBtn.Name = "addBtn";
         this.addBtn.Size = new System.Drawing.Size(91, 30);
         this.addBtn.TabIndex = 0;
         this.addBtn.Text = "Add";
         this.addBtn.UseVisualStyleBackColor = true;
         // 
         // okCancelPanel
         // 
         this.okCancelPanel.ColumnCount = 2;
         this.okCancelPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.okCancelPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
         this.okCancelPanel.Controls.Add(this.cancelBtn, 0, 0);
         this.okCancelPanel.Controls.Add(this.okBtn, 0, 0);
         this.okCancelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.okCancelPanel.Location = new System.Drawing.Point(207, 539);
         this.okCancelPanel.Margin = new System.Windows.Forms.Padding(0);
         this.okCancelPanel.Name = "okCancelPanel";
         this.okCancelPanel.RowCount = 1;
         this.okCancelPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.okCancelPanel.Size = new System.Drawing.Size(433, 36);
         this.okCancelPanel.TabIndex = 18;
         // 
         // cancelBtn
         // 
         this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.cancelBtn.Dock = System.Windows.Forms.DockStyle.Right;
         this.cancelBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.cancelBtn.Location = new System.Drawing.Point(336, 3);
         this.cancelBtn.Name = "cancelBtn";
         this.cancelBtn.Size = new System.Drawing.Size(94, 30);
         this.cancelBtn.TabIndex = 19;
         this.cancelBtn.Text = "Cancel";
         this.cancelBtn.UseVisualStyleBackColor = true;
         this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
         // 
         // okBtn
         // 
         this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
         this.okBtn.Dock = System.Windows.Forms.DockStyle.Right;
         this.okBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.okBtn.Location = new System.Drawing.Point(236, 3);
         this.okBtn.Name = "okBtn";
         this.okBtn.Size = new System.Drawing.Size(94, 30);
         this.okBtn.TabIndex = 18;
         this.okBtn.Text = "OK";
         this.okBtn.UseVisualStyleBackColor = true;
         // 
         // propertiesPanel
         // 
         this.propertiesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.propertiesPanel.Location = new System.Drawing.Point(210, 10);
         this.propertiesPanel.Name = "propertiesPanel";
         this.propertiesPanel.Size = new System.Drawing.Size(427, 319);
         this.propertiesPanel.TabIndex = 19;
         // 
         // presetsTree
         // 
         this.presetsTree.AllowDrop = true;
         this.presetsTree.AutoScroll = true;
         this.presetsTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.presetsTree.Dock = System.Windows.Forms.DockStyle.Fill;
         this.presetsTree.Location = new System.Drawing.Point(10, 10);
         this.presetsTree.Name = "presetsTree";
         this.tableLayout.SetRowSpan(this.presetsTree, 2);
         this.presetsTree.Size = new System.Drawing.Size(194, 490);
         this.presetsTree.TabIndex = 12;
         this.presetsTree.Text = "treeView1";
         this.presetsTree.SelectionChanged += new System.EventHandler<Outliner.Controls.Tree.SelectionChangedEventArgs>(this.presetsTree_SelectionChanged);
         this.presetsTree.BeforeNodeTextEdit += new System.EventHandler<Outliner.Controls.Tree.BeforeNodeTextEditEventArgs>(this.presetsTree_BeforeNodeTextEdit);
         // 
         // previewGroupBox
         // 
         this.previewGroupBox.BorderColor = System.Drawing.Color.Black;
         this.previewGroupBox.Controls.Add(this.previewTree);
         this.previewGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.previewGroupBox.Location = new System.Drawing.Point(210, 335);
         this.previewGroupBox.Name = "previewGroupBox";
         this.previewGroupBox.Padding = new System.Windows.Forms.Padding(5);
         this.previewGroupBox.Size = new System.Drawing.Size(427, 165);
         this.previewGroupBox.TabIndex = 20;
         this.previewGroupBox.TabStop = false;
         this.previewGroupBox.Text = "Preview";
         // 
         // previewTree
         // 
         this.previewTree.AllowDrop = true;
         this.previewTree.AutoScroll = true;
         this.previewTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.previewTree.Dock = System.Windows.Forms.DockStyle.Fill;
         this.previewTree.Enabled = false;
         this.previewTree.Location = new System.Drawing.Point(5, 18);
         this.previewTree.Name = "previewTree";
         this.previewTree.Size = new System.Drawing.Size(417, 142);
         this.previewTree.TabIndex = 2;
         this.previewTree.Text = "treeView1";
         // 
         // PresetEditor
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(647, 582);
         this.Controls.Add(this.tableLayout);
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.MinimumSize = new System.Drawing.Size(500, 300);
         this.Name = "PresetEditor";
         this.ShowIcon = false;
         this.Text = "Preset Editor";
         this.tableLayout.ResumeLayout(false);
         this.addDeletePanel.ResumeLayout(false);
         this.okCancelPanel.ResumeLayout(false);
         this.previewGroupBox.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TableLayoutPanel tableLayout;
      private Tree.TreeView presetsTree;
      private System.Windows.Forms.TableLayoutPanel addDeletePanel;
      private System.Windows.Forms.Button deleteBtn;
      private System.Windows.Forms.Button addBtn;
      private System.Windows.Forms.TableLayoutPanel okCancelPanel;
      private System.Windows.Forms.Button cancelBtn;
      private System.Windows.Forms.Button okBtn;
      private System.Windows.Forms.Panel propertiesPanel;
      private OutlinerGroupBox previewGroupBox;
      private Tree.TreeView previewTree;
   }
}