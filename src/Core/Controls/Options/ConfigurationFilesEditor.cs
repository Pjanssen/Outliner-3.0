using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Autodesk.Max;
using PJanssen.Outliner.MaxUtils;
using PJanssen.Outliner.Configuration;
using PJanssen.Outliner.Controls.Tree.Layout;

namespace PJanssen.Outliner.Controls.Options
{
public partial class ConfigFilesEditor<T> : Form where T : class, new()
{
   protected String directory;
   protected Type editorType;
   protected IDictionary<String, T> files;
   protected ICollection<String> newFiles;
   protected IDictionary<String, String> renamedFiles;
   protected String editingFile;

   public ConfigFilesEditor(String directory, Type editorType)
   {
      Throw.IfNull(directory, "directory");
      Throw.IfNull(editorType, "editorType");

      InitializeComponent();

      this.directory = directory;
      this.editorType = editorType;

      this.files = new Dictionary<String, T>();
      this.newFiles = new List<String>();
      this.renamedFiles = new Dictionary<String, String>();
      this.ShowUIProperties = true;
   }

   public Boolean ShowUIProperties { get; set; }

   public Action UpdateTreeAction { get; set; }

   protected override void OnLoad(EventArgs e)
   {
      base.OnLoad(e);

      Color backColor = Colors.FromMaxGuiColor(GuiColors.Background);
      Color foreColor = Colors.FromMaxGuiColor(GuiColors.Text);
      Color windowColor = Colors.FromMaxGuiColor(GuiColors.Window);
      Color windowTextColor = Colors.FromMaxGuiColor(GuiColors.WindowText);

      this.BackColor = backColor;
      this.ForeColor = foreColor;
      Tree.TreeViewColorScheme treeColors = new Tree.TreeViewColorScheme();
      treeColors.Background = new SerializableColor(windowColor);
      treeColors.ForegroundLight = new SerializableColor(windowTextColor);
      treeColors.ParentBackground = new SerializableColor(windowColor);
      treeColors.ParentForeground = new SerializableColor(windowTextColor);
      treeColors.AlternateBackground = false;
      this.filesTree.Colors = treeColors;
      this.filesTree.Settings.MultiSelect = false;

      TreeNodeLayout layout = new TreeNodeLayout();
      layout.LayoutItems.Add(new TreeNodeText());
      layout.LayoutItems.Add(new EmptySpace());
      layout.FullRowSelect = true;
      this.filesTree.TreeNodeLayout = layout;
      this.filesTree.NodeSorter = new Outliner.Controls.Tree.TreeNodeTextSorter();

      this.RefreshUI();
   }

   protected virtual void RefreshUI()
   {
      this.filesTree.Nodes.Clear();
      this.files.Clear();

      foreach (KeyValuePair<String, T> file in Configurations.GetConfigurationFiles<T>(this.directory))
      {
         this.AddFileToTree(file.Key, file.Value, this.filesTree.Nodes);
      }

      this.filesTree.OnSelectionChanged();
   }

   protected virtual Tree.TreeNode AddFileToTree(String file, T config, Tree.TreeNodeCollection parentCollection)
   {
      Tree.TreeNode tn = new Tree.TreeNode(Path.GetFileName(file));
      tn.Tag = file;
      parentCollection.Add(tn);
      this.files.Add(file, config);
      return tn;
   }

   protected virtual String GetEditingFile(Tree.TreeNode tn)
   {
      Throw.IfNull(tn, "tn");

      return tn.Tag as String;
   }

   protected virtual T GetEditingConfiguration(Tree.TreeNode tn)
   {
      Throw.IfNull(tn, "tn");

      String editingFile = GetEditingFile(tn);
      T editingConfiguration = null;

      if (editingFile != null)
         this.files.TryGetValue(editingFile, out editingConfiguration);

      return editingConfiguration;
   }

   protected virtual void filesTree_BeforeNodeTextEdit(object sender, Tree.BeforeNodeTextEditEventArgs e)
   {
      String editingFile = this.GetEditingFile(e.TreeNode);
      if (String.IsNullOrEmpty(editingFile))
         e.Cancel = true;
      else
         e.EditText = Path.GetFileNameWithoutExtension(editingFile);
   }

   protected virtual void filesTree_AfterNodeTextEdit(object sender, Tree.AfterNodeTextEditEventArgs e)
   {
      if (e.Canceled)
         return;

      String editingFile = this.GetEditingFile(e.TreeNode);
      if (!String.IsNullOrEmpty(editingFile))
      {
         String oldname = editingFile;
         String filename = String.Join("", e.NewText, Configurations.ConfigurationFileExtension);
         String fullFileName = Path.Combine(this.directory, filename);
         if (Configurations.IsValidNewFileName(fullFileName))
         {
            Configurations.RenameConfigurationFile(oldname, fullFileName);

            String originalname = null;
            if (this.renamedFiles.TryGetValue(oldname, out originalname))
               this.renamedFiles.Remove(oldname);
            else
               originalname = oldname;
            this.renamedFiles.Add(fullFileName, originalname);

            this.RefreshUI();
         }
         else
         {
            e.TreeNode.Text = e.OldText;

            if (fullFileName != editingFile)
            {
               //TODO: make nice messagebox.
               MessageBox.Show("Invalid file name");
               e.TreeNode.TreeView.BeginNodeTextEdit(e.TreeNode);
            }
         }
      }
   }

   protected virtual void filesTree_SelectionChanged(object sender, Tree.SelectionChangedEventArgs e)
   {
      this.editorPanel.Controls.Clear();

      Tree.TreeNode selNode = e.Nodes.FirstOrDefault();
      if (selNode == null)
         return;
      
      this.editorPanel.SuspendLayout();
      this.editorPanel.Controls.Clear();

      Control editor = this.GetEditorFor(selNode);
      if (editor != null)
      {
         editor.Dock = DockStyle.Fill;

         this.editorPanel.Controls.Add(editor);
      }

      this.editorPanel.ResumeLayout();
   }

   protected virtual Control GetEditorFor(Tree.TreeNode tn)
   {
      T configuration = this.GetEditingConfiguration(tn);

      Panel panel = new Panel();

      Control detailsEditor = (Control)Activator.CreateInstance(this.editorType, new object[] { configuration });
      detailsEditor.Dock = DockStyle.Fill;
      panel.Controls.Add(detailsEditor);

      if (this.ShowUIProperties)
      {
         OutlinerGroupBox uiProperties = new OutlinerGroupBox();
         uiProperties.Text = "UI Properties";
         uiProperties.Height = 125;
         uiProperties.Dock = DockStyle.Top;
         panel.Controls.Add(uiProperties);
         ConfigurationFilePropertiesEditor configProperties = new ConfigurationFilePropertiesEditor();
         configProperties.Configuration = configuration as ConfigurationFile;
         configProperties.Dock = DockStyle.Fill;
         uiProperties.Controls.Add(configProperties);
      }

      return panel;
   }

   protected virtual void addFileBtn_Click(object sender, EventArgs e)
   {
      Tuple<String, T> newFile = Configurations.NewConfigurationFile<T>(this.directory, "newfile");
      this.newFiles.Add(newFile.Item1);

      Tree.TreeNode tn = this.AddFileToTree(newFile.Item1, newFile.Item2, this.filesTree.Nodes);
      this.filesTree.Sort();

      tn.TreeView.SelectNode(tn, true);
      tn.TreeView.OnSelectionChanged();

      tn.TreeView.BeginNodeTextEdit(tn);
   }

   protected virtual void deleteFileBtn_Click(object sender, EventArgs e)
   {
      Tree.TreeNode selNode = this.filesTree.SelectedNodes.FirstOrDefault();
      if (selNode == null)
         return;

      String file = selNode.Tag as String;
      if (file == null)
         return;

      if (MessageBox.Show( String.Format(OutlinerResources.Warning_DeleteConfig, Path.GetFileName(file))
                         , OutlinerResources.Warning_DeleteConfigTitle
                         , MessageBoxButtons.YesNo
                         , MessageBoxIcon.Warning
                         , MessageBoxDefaultButton.Button2
                         , ControlHelpers.CreateLocalizedMessageBoxOptions()) 
            == System.Windows.Forms.DialogResult.Yes)
      {
         Configurations.DeleteConfigurationFile(file);

         this.files.Remove(file);
         selNode.Remove();
         this.editorPanel.Controls.Clear();
      }
   }

   private Boolean okClicked = false;
   protected override void OnClosing(CancelEventArgs e)
   {
      if (!this.okClicked)
         this.Rollback();

      base.OnClosing(e);
   }
   private void cancelBtn_Click(object sender, EventArgs e)
   {
      this.Close();
   }

   private void okBtn_Click(object sender, EventArgs e)
   {
      this.Commit();

      this.okClicked = true;
      this.Close();
   }

   protected virtual void Rollback()
   {
      foreach (KeyValuePair<String, String> renamedFile in this.renamedFiles)
      {
         Configurations.RenameConfigurationFile(renamedFile.Key, renamedFile.Value);
      }

      foreach (String file in this.newFiles)
      {
         Configurations.DeleteConfigurationFile(file);
      }
   }

   protected virtual void Commit()
   {
      foreach (KeyValuePair<String, T> configFile in this.files)
      {
         XmlSerialization.Serialize<T>(configFile.Key, configFile.Value);
      }
   }
}
}
