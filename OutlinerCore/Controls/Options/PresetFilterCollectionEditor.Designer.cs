namespace Outliner.Controls.Options
{
   partial class PresetFilterCollectionEditor
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
         this.outlinerGroupBox1 = new Outliner.Controls.OutlinerGroupBox();
         this.filterEditor = new Outliner.Controls.Options.FilterCollectionEditor();
         this.outlinerGroupBox1.SuspendLayout();
         this.SuspendLayout();
         // 
         // outlinerGroupBox1
         // 
         this.outlinerGroupBox1.BorderColor = System.Drawing.Color.Black;
         this.outlinerGroupBox1.Controls.Add(this.filterEditor);
         this.outlinerGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.outlinerGroupBox1.Location = new System.Drawing.Point(0, 0);
         this.outlinerGroupBox1.Name = "outlinerGroupBox1";
         this.outlinerGroupBox1.Padding = new System.Windows.Forms.Padding(5);
         this.outlinerGroupBox1.Size = new System.Drawing.Size(355, 362);
         this.outlinerGroupBox1.TabIndex = 3;
         this.outlinerGroupBox1.TabStop = false;
         this.outlinerGroupBox1.Text = "Filters";
         // 
         // filterEditor
         // 
         this.filterEditor.Dock = System.Windows.Forms.DockStyle.Fill;
         this.filterEditor.Location = new System.Drawing.Point(5, 18);
         this.filterEditor.Name = "filterEditor";
         this.filterEditor.Size = new System.Drawing.Size(345, 339);
         this.filterEditor.TabIndex = 0;
         // 
         // PresetFilterCollectionEditor
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.outlinerGroupBox1);
         this.Name = "PresetFilterCollectionEditor";
         this.Size = new System.Drawing.Size(355, 362);
         this.outlinerGroupBox1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private OutlinerGroupBox outlinerGroupBox1;
      private FilterCollectionEditor filterEditor;
   }
}
