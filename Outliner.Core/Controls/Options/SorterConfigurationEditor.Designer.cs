namespace Outliner.Controls.Options
{
   partial class SorterConfigurationEditor
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
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.downButton = new System.Windows.Forms.Button();
         this.upButton = new System.Windows.Forms.Button();
         this.sorterPropertyGrid = new System.Windows.Forms.PropertyGrid();
         this.sortersTree = new Outliner.Controls.Tree.TreeView();
         this.deleteButton = new System.Windows.Forms.Button();
         this.sorterTypesComboBox = new System.Windows.Forms.ComboBox();
         this.addButton = new System.Windows.Forms.Button();
         this.tableLayoutPanel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 2;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
         this.tableLayoutPanel1.Controls.Add(this.downButton, 1, 3);
         this.tableLayoutPanel1.Controls.Add(this.upButton, 1, 2);
         this.tableLayoutPanel1.Controls.Add(this.sorterPropertyGrid, 0, 4);
         this.tableLayoutPanel1.Controls.Add(this.sortersTree, 0, 1);
         this.tableLayoutPanel1.Controls.Add(this.deleteButton, 1, 1);
         this.tableLayoutPanel1.Controls.Add(this.sorterTypesComboBox, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.addButton, 1, 0);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 5;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(513, 479);
         this.tableLayoutPanel1.TabIndex = 0;
         // 
         // downButton
         // 
         this.downButton.Dock = System.Windows.Forms.DockStyle.Top;
         this.downButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.downButton.Location = new System.Drawing.Point(435, 88);
         this.downButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
         this.downButton.Name = "downButton";
         this.downButton.Size = new System.Drawing.Size(75, 23);
         this.downButton.TabIndex = 7;
         this.downButton.Text = "Down";
         this.downButton.UseVisualStyleBackColor = true;
         this.downButton.Click += new System.EventHandler(this.downButton_Click);
         // 
         // upButton
         // 
         this.upButton.Dock = System.Windows.Forms.DockStyle.Top;
         this.upButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.upButton.Location = new System.Drawing.Point(435, 60);
         this.upButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
         this.upButton.Name = "upButton";
         this.upButton.Size = new System.Drawing.Size(75, 23);
         this.upButton.TabIndex = 6;
         this.upButton.Text = "Up";
         this.upButton.UseVisualStyleBackColor = true;
         this.upButton.Click += new System.EventHandler(this.upButton_Click);
         // 
         // sorterPropertyGrid
         // 
         this.sorterPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
         this.sorterPropertyGrid.HelpVisible = false;
         this.sorterPropertyGrid.Location = new System.Drawing.Point(3, 364);
         this.sorterPropertyGrid.Name = "sorterPropertyGrid";
         this.sorterPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
         this.sorterPropertyGrid.Size = new System.Drawing.Size(426, 112);
         this.sorterPropertyGrid.TabIndex = 5;
         this.sorterPropertyGrid.ToolbarVisible = false;
         this.sorterPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.sorterPropertyGrid_PropertyValueChanged);
         // 
         // sortersTree
         // 
         this.sortersTree.AllowDrop = true;
         this.sortersTree.AutoScroll = true;
         this.sortersTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.sortersTree.Dock = System.Windows.Forms.DockStyle.Fill;
         this.sortersTree.Location = new System.Drawing.Point(3, 33);
         this.sortersTree.Name = "sortersTree";
         this.tableLayoutPanel1.SetRowSpan(this.sortersTree, 3);
         this.sortersTree.Size = new System.Drawing.Size(426, 325);
         this.sortersTree.TabIndex = 3;
         this.sortersTree.Text = "sorterTree";
         this.sortersTree.SelectionChanged += new System.EventHandler<Outliner.Controls.Tree.SelectionChangedEventArgs>(this.sortersTree_SelectionChanged);
         // 
         // deleteButton
         // 
         this.deleteButton.Dock = System.Windows.Forms.DockStyle.Top;
         this.deleteButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.deleteButton.Location = new System.Drawing.Point(435, 32);
         this.deleteButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
         this.deleteButton.Name = "deleteButton";
         this.deleteButton.Size = new System.Drawing.Size(75, 23);
         this.deleteButton.TabIndex = 2;
         this.deleteButton.Text = "Delete";
         this.deleteButton.UseVisualStyleBackColor = true;
         this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
         // 
         // sorterTypesComboBox
         // 
         this.sorterTypesComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.sorterTypesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.sorterTypesComboBox.FormattingEnabled = true;
         this.sorterTypesComboBox.Location = new System.Drawing.Point(3, 3);
         this.sorterTypesComboBox.Name = "sorterTypesComboBox";
         this.sorterTypesComboBox.Size = new System.Drawing.Size(426, 21);
         this.sorterTypesComboBox.TabIndex = 0;
         // 
         // addButton
         // 
         this.addButton.Dock = System.Windows.Forms.DockStyle.Top;
         this.addButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.addButton.Location = new System.Drawing.Point(435, 2);
         this.addButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 3);
         this.addButton.Name = "addButton";
         this.addButton.Size = new System.Drawing.Size(75, 23);
         this.addButton.TabIndex = 1;
         this.addButton.Text = "Add";
         this.addButton.UseVisualStyleBackColor = true;
         this.addButton.Click += new System.EventHandler(this.addButton_Click);
         // 
         // SorterConfigurationEditor
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.tableLayoutPanel1);
         this.Name = "SorterConfigurationEditor";
         this.Size = new System.Drawing.Size(513, 479);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.ComboBox sorterTypesComboBox;
      private System.Windows.Forms.Button addButton;
      private System.Windows.Forms.Button deleteButton;
      private Tree.TreeView sortersTree;
      private System.Windows.Forms.PropertyGrid sorterPropertyGrid;
      private System.Windows.Forms.Button downButton;
      private System.Windows.Forms.Button upButton;


   }
}
