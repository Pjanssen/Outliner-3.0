using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Outliner.MaxUtils;
using Autodesk.Max;
using Outliner.Configuration;
using Outliner.Controls.Tree.Layout;

namespace Outliner.Controls.Options
{
public partial class ConfigFilesEditor<T> : Form where T : class, new()
{
   private String directory;
   private Type editorType;
   private IDictionary<String, T> files;
   private ICollection<String> newFiles;
   private Tuple<String, T> editingFile;
   private IDictionary<String, String> renamedFiles;

   public ConfigFilesEditor(String directory, Type editorType, String title)
   {
      Throw.IfArgumentIsNull(directory, "directory");
      Throw.IfArgumentIsNull(editorType, "editorType");
      Throw.IfArgumentIsNull(title, "title");

      InitializeComponent();

      this.directory = directory;
      this.editorType = editorType;
      this.Text = title;

      this.newFiles = new List<String>();
      this.renamedFiles = new Dictionary<String, String>();
   }

   protected override void OnLoad(EventArgs e)
   {
      base.OnLoad(e);

      Color backColor = ColorHelpers.FromMaxGuiColor(GuiColors.Background);
      Color foreColor = ColorHelpers.FromMaxGuiColor(GuiColors.Text);
      Color windowColor = ColorHelpers.FromMaxGuiColor(GuiColors.Window);
      Color windowTextColor = ColorHelpers.FromMaxGuiColor(GuiColors.WindowText);

      this.BackColor = backColor;
      this.ForeColor = foreColor;
      Tree.TreeViewColorScheme treeColors = new Tree.TreeViewColorScheme();
      treeColors.Background = new SerializableColor(windowColor);
      treeColors.ForegroundLight = new SerializableColor(windowTextColor);
      treeColors.ParentBackground = new SerializableColor(windowColor);
      treeColors.ParentForeground = new SerializableColor(windowTextColor);
      treeColors.AlternateBackground = false;
      this.filesTree.Colors = treeColors;

      TreeNodeLayout layout = new TreeNodeLayout();
      layout.LayoutItems.Add(new TreeNodeText());
      layout.LayoutItems.Add(new EmptySpace());
      layout.FullRowSelect = true;
      this.filesTree.TreeNodeLayout = layout;
      this.filesTree.NodeSorter = new Outliner.Controls.Tree.TreeNodeTextSorter();

      this.RefreshUI();
   }

   private void RefreshUI()
   {
      this.files = ConfigurationHelpers.GetConfigurationFiles<T>(this.directory);
      this.FillFilesTree();
      this.UpdateEditorsUI();
   }

   private void FillFilesTree()
   {
      if (this.files == null)
         return;

      this.filesTree.Nodes.Clear();

      foreach (KeyValuePair<String, T> file in this.files)
      {
         this.AddFileToTree(file.Key, file.Value);
      }
   }

   protected virtual Tree.TreeNode AddFileToTree(String file, T config)
   {
      Tree.TreeNode tn = new Tree.TreeNode(Path.GetFileName(file));
      tn.Tag = new Tuple<String, T>(file, config);
      this.filesTree.Nodes.Add(tn);
      return tn;
   }

   protected virtual void filesTree_SelectionChanged(object sender, Tree.SelectionChangedEventArgs e)
   {
      Tree.TreeNode selNode = e.Nodes.FirstOrDefault();
      if (selNode != null)
         this.editingFile = selNode.Tag as Tuple<String, T>;
      else
         this.editingFile = null;

      this.UpdateEditorsUI();
   }

   protected virtual void filesTree_BeforeNodeTextEdit(object sender, Tree.BeforeNodeTextEditEventArgs e)
   {
      Tuple<String, T> tag = e.TreeNode.Tag as Tuple<String, T>;
      if (tag == null)
         e.Cancel = true;
      else
         e.EditText = Path.GetFileNameWithoutExtension(tag.Item1);
   }

   protected virtual void filesTree_AfterNodeTextEdit(object sender, Tree.AfterNodeTextEditEventArgs e)
   {
      if (e.Canceled)
         return;

      Tuple<String, T> tag = e.TreeNode.Tag as Tuple<String, T>;
      if (tag != null)
      {
         String oldname = tag.Item1;
         String filename = String.Join("", e.NewText, ConfigurationHelpers.ConfigurationFileExtension);
         String fullFileName = Path.Combine(this.directory, filename);
         if (ConfigurationHelpers.IsValidFileName(fullFileName))
         {
            ConfigurationHelpers.RenameConfigurationFile(oldname, fullFileName);

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

            if (fullFileName != tag.Item1)
            {
               //TODO: make nice messagebox.
               MessageBox.Show("Invalid file name");
               e.TreeNode.TreeView.BeginNodeTextEdit(e.TreeNode);
            }
         }
      }
   }

   protected virtual void UpdateEditorsUI()
   {
      Boolean show = this.editingFile != null;
      this.uiPropertiesGroupBox.Visible = show;
      this.configPropertiesGroupBox.Visible = show;

      if (show)
      {
         this.configurationFileEditor.Configuration = this.editingFile.Item2 as ConfigurationFile;

         Control editor = (Control)Activator.CreateInstance(this.editorType, new object[] { this.editingFile.Item2 });

         editor.Dock = DockStyle.Fill;

         this.SuspendLayout();
         this.editorPanel.Controls.Clear();
         this.editorPanel.Controls.Add(editor);
         this.ResumeLayout();
      }
   }

   protected virtual void addFileBtn_Click(object sender, EventArgs e)
   {
      Tuple<String, T> newFile = ConfigurationHelpers.NewConfigurationFile<T>(this.directory, "newfile");
      this.newFiles.Add(newFile.Item1);

      Tree.TreeNode tn = this.AddFileToTree(newFile.Item1, newFile.Item2);
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

      if (!(selNode.Tag is Tuple<String, T>))
         return;

      Tuple<String, T> tag = (Tuple<String, T>)selNode.Tag;
      String file = tag.Item1;

      if (MessageBox.Show( String.Format(OutlinerResources.Str_DeleteConfigWarning, Path.GetFileName(file))
                         , OutlinerResources.Str_DeleteConfigWarningTitle
                         , MessageBoxButtons.YesNo
                         , MessageBoxIcon.Warning
                         , MessageBoxDefaultButton.Button2
                         , ControlHelpers.GetLocalizedMessageBoxOptions()) 
            == System.Windows.Forms.DialogResult.Yes)
      {
         ConfigurationHelpers.DeleteConfigurationFile(file);

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
         ConfigurationHelpers.RenameConfigurationFile(renamedFile.Key, renamedFile.Value);
      }

      foreach (String file in this.newFiles)
      {
         ConfigurationHelpers.DeleteConfigurationFile(file);
      }
   }

   protected virtual void Commit()
   {
      foreach (KeyValuePair<String, T> configFile in this.files)
      {
         XmlSerializationHelpers.Serialize<T>(configFile.Key, configFile.Value);
      }
   }
}
}
