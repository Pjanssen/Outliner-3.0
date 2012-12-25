namespace Outliner.Controls
{
   partial class AboutBox
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
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
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
         this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
         this.logoPictureBox = new System.Windows.Forms.PictureBox();
         this.labelProductName = new System.Windows.Forms.Label();
         this.labelVersion = new System.Windows.Forms.Label();
         this.labelCopyright = new System.Windows.Forms.Label();
         this.labelLoadedPlugins = new System.Windows.Forms.Label();
         this.textBoxPlugins = new System.Windows.Forms.TextBox();
         this.okButton = new System.Windows.Forms.Button();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.label1 = new System.Windows.Forms.Label();
         this.linkLabel1 = new System.Windows.Forms.LinkLabel();
         this.tableLayoutPanel.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
         this.tableLayoutPanel1.SuspendLayout();
         this.SuspendLayout();
         // 
         // tableLayoutPanel
         // 
         this.tableLayoutPanel.ColumnCount = 2;
         this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 71F));
         this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
         this.tableLayoutPanel.Controls.Add(this.logoPictureBox, 0, 0);
         this.tableLayoutPanel.Controls.Add(this.labelProductName, 1, 0);
         this.tableLayoutPanel.Controls.Add(this.labelVersion, 1, 1);
         this.tableLayoutPanel.Controls.Add(this.labelCopyright, 1, 2);
         this.tableLayoutPanel.Controls.Add(this.labelLoadedPlugins, 1, 4);
         this.tableLayoutPanel.Controls.Add(this.textBoxPlugins, 1, 5);
         this.tableLayoutPanel.Controls.Add(this.okButton, 1, 6);
         this.tableLayoutPanel.Controls.Add(this.tableLayoutPanel1, 1, 3);
         this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel.Location = new System.Drawing.Point(9, 9);
         this.tableLayoutPanel.Name = "tableLayoutPanel";
         this.tableLayoutPanel.RowCount = 7;
         this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
         this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
         this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
         this.tableLayoutPanel.Size = new System.Drawing.Size(382, 329);
         this.tableLayoutPanel.TabIndex = 0;
         // 
         // logoPictureBox
         // 
         this.logoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
         this.logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.Image")));
         this.logoPictureBox.Location = new System.Drawing.Point(0, 0);
         this.logoPictureBox.Margin = new System.Windows.Forms.Padding(0);
         this.logoPictureBox.Name = "logoPictureBox";
         this.tableLayoutPanel.SetRowSpan(this.logoPictureBox, 3);
         this.logoPictureBox.Size = new System.Drawing.Size(71, 60);
         this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
         this.logoPictureBox.TabIndex = 12;
         this.logoPictureBox.TabStop = false;
         // 
         // labelProductName
         // 
         this.labelProductName.Dock = System.Windows.Forms.DockStyle.Fill;
         this.labelProductName.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.labelProductName.Location = new System.Drawing.Point(74, 0);
         this.labelProductName.MaximumSize = new System.Drawing.Size(0, 17);
         this.labelProductName.Name = "labelProductName";
         this.labelProductName.Size = new System.Drawing.Size(305, 17);
         this.labelProductName.TabIndex = 19;
         this.labelProductName.Text = "Outliner";
         this.labelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // labelVersion
         // 
         this.labelVersion.Dock = System.Windows.Forms.DockStyle.Fill;
         this.labelVersion.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.labelVersion.Location = new System.Drawing.Point(74, 20);
         this.labelVersion.MaximumSize = new System.Drawing.Size(0, 17);
         this.labelVersion.Name = "labelVersion";
         this.labelVersion.Size = new System.Drawing.Size(305, 17);
         this.labelVersion.TabIndex = 0;
         this.labelVersion.Text = "Core version:";
         this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // labelCopyright
         // 
         this.labelCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
         this.labelCopyright.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.labelCopyright.Location = new System.Drawing.Point(74, 40);
         this.labelCopyright.MaximumSize = new System.Drawing.Size(0, 17);
         this.labelCopyright.Name = "labelCopyright";
         this.labelCopyright.Size = new System.Drawing.Size(305, 17);
         this.labelCopyright.TabIndex = 21;
         this.labelCopyright.Text = "Copyright";
         this.labelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // labelLoadedPlugins
         // 
         this.labelLoadedPlugins.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.labelLoadedPlugins.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.labelLoadedPlugins.Location = new System.Drawing.Point(74, 88);
         this.labelLoadedPlugins.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
         this.labelLoadedPlugins.MaximumSize = new System.Drawing.Size(0, 17);
         this.labelLoadedPlugins.Name = "labelLoadedPlugins";
         this.labelLoadedPlugins.Size = new System.Drawing.Size(305, 17);
         this.labelLoadedPlugins.TabIndex = 22;
         this.labelLoadedPlugins.Text = "Loaded plugins:";
         this.labelLoadedPlugins.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // textBoxPlugins
         // 
         this.textBoxPlugins.Dock = System.Windows.Forms.DockStyle.Fill;
         this.textBoxPlugins.Location = new System.Drawing.Point(74, 108);
         this.textBoxPlugins.Multiline = true;
         this.textBoxPlugins.Name = "textBoxPlugins";
         this.textBoxPlugins.ReadOnly = true;
         this.textBoxPlugins.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
         this.textBoxPlugins.Size = new System.Drawing.Size(305, 188);
         this.textBoxPlugins.TabIndex = 23;
         this.textBoxPlugins.TabStop = false;
         this.textBoxPlugins.Text = "plugins";
         // 
         // okButton
         // 
         this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.okButton.Location = new System.Drawing.Point(304, 303);
         this.okButton.Name = "okButton";
         this.okButton.Size = new System.Drawing.Size(75, 23);
         this.okButton.TabIndex = 24;
         this.okButton.Text = "&OK";
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 2;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.58199F));
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.41801F));
         this.tableLayoutPanel1.Controls.Add(this.linkLabel1, 1, 0);
         this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(71, 60);
         this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 1;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel1.Size = new System.Drawing.Size(311, 20);
         this.tableLayoutPanel1.TabIndex = 30;
         // 
         // label1
         // 
         this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
         this.label1.Location = new System.Drawing.Point(3, 0);
         this.label1.MaximumSize = new System.Drawing.Size(0, 17);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(85, 17);
         this.label1.TabIndex = 30;
         this.label1.Text = "Report issues at:";
         this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // linkLabel1
         // 
         this.linkLabel1.ActiveLinkColor = System.Drawing.Color.SteelBlue;
         this.linkLabel1.AutoSize = true;
         this.linkLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.linkLabel1.LinkColor = System.Drawing.Color.LightSkyBlue;
         this.linkLabel1.Location = new System.Drawing.Point(94, 0);
         this.linkLabel1.Name = "linkLabel1";
         this.linkLabel1.Size = new System.Drawing.Size(214, 20);
         this.linkLabel1.TabIndex = 31;
         this.linkLabel1.TabStop = true;
         this.linkLabel1.Text = "http://outliner.pjanssen.nl/issuetracker";
         this.linkLabel1.VisitedLinkColor = System.Drawing.Color.LightSkyBlue;
         // 
         // AboutBox
         // 
         this.AcceptButton = this.okButton;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(400, 347);
         this.Controls.Add(this.tableLayoutPanel);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "AboutBox";
         this.Padding = new System.Windows.Forms.Padding(9);
         this.ShowIcon = false;
         this.ShowInTaskbar = false;
         this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
         this.Text = "About";
         this.tableLayoutPanel.ResumeLayout(false);
         this.tableLayoutPanel.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
         this.tableLayoutPanel1.ResumeLayout(false);
         this.tableLayoutPanel1.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
      private System.Windows.Forms.PictureBox logoPictureBox;
      private System.Windows.Forms.Label labelProductName;
      private System.Windows.Forms.Label labelVersion;
      private System.Windows.Forms.Label labelCopyright;
      private System.Windows.Forms.Label labelLoadedPlugins;
      private System.Windows.Forms.TextBox textBoxPlugins;
      private System.Windows.Forms.Button okButton;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.LinkLabel linkLabel1;
      private System.Windows.Forms.Label label1;
   }
}
