using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Presets;
using Outliner.MaxUtils;
using Autodesk.Max;
using Outliner.Plugins;
using System.Reflection;
using System.Resources;
using Outliner.NodeSorters;

namespace Outliner.Controls.Options
{
public partial class PresetPropertiesEditor : OutlinerUserControl
{
   private OutlinerPreset preset;
   private Action updateAction;

   public PresetPropertiesEditor()
   {
      InitializeComponent();
   }

   public PresetPropertiesEditor(OutlinerPreset preset, Action updateAction) : this()
   {
      this.preset = preset;
      this.updateAction = updateAction;
   }

   protected override void OnLoad(EventArgs e)
   {
 	   this.imgFileRadioButton.Checked = String.IsNullOrEmpty(preset.ResourceTypeName);
      this.imgResRadioButton.Checked = !String.IsNullOrEmpty(preset.ResourceTypeName);
      this.SetEnabledStates();

      //Mode combobox data source.
      IEnumerable<OutlinerPluginData> modes = OutlinerPlugins.GetPlugins(OutlinerPluginType.TreeMode);
      this.modeComboBox.DataSource = modes.ToList();
      this.modeComboBox.ValueMember = "Type";
      this.modeComboBox.DisplayMember = "DisplayName";


      //Sorter combobox data source.
      IEnumerable<OutlinerPluginData> sorters = OutlinerPlugins.GetPlugins(OutlinerPluginType.NodeSorter);
      this.sorterComboBox.DataSource = sorters.ToList();
      this.sorterComboBox.ValueMember = "Type";
      this.sorterComboBox.DisplayMember = "DisplayName";
      this.sorterComboBox.SelectedValue = this.preset.NodeSorter.GetType();
      this.sorterComboBox.SelectedValueChanged += this.sorterComboBox_SelectedValueChanged;

      //Fill image resource types list.
      List<String> resTypes = new List<string>();
      foreach (Assembly assembly in OutlinerPlugins.PluginAssemblies)
      {
         foreach (Type t in assembly.GetTypes())
         {
            PropertyInfo p = t.GetProperty("ResourceManager", BindingFlags.Static | BindingFlags.NonPublic);
            if (p != null)
            {
               if (p.PropertyType.Equals(typeof(ResourceManager)))
               {
                  this.imageResTypeComboBox.Items.Add(t.FullName);
                  break;
               }
            }
         }
      }
      
      this.FillImageResNameComboBoxes();
      this.presetBindingSource.DataSource = preset;
      this.resTypeBindingSource.DataSource = preset;
      this.nodeSorterBindingSource.DataSource = preset.NodeSorter;

      base.OnLoad(e);
   }


   private void SetControlColor(Control c, Color backColor, Color foreColor)
   {
      c.BackColor = backColor;
      c.ForeColor = foreColor;
   }

   private void SetEnabledStates()
   {
      if (this.preset == null)
         return;

      Boolean local = String.IsNullOrEmpty(this.preset.ResourceTypeName);
      Boolean resource = !String.IsNullOrEmpty(this.preset.ResourceTypeName);

      this.image16FileLbl.Enabled = local;
      this.image24FileLbl.Enabled = local;
      this.image24FileTextBox.Enabled = local;
      this.image16FileTextBox.Enabled = local;
      this.image16FileBrowseBtn.Enabled = local;
      this.image24FileBrowseBtn.Enabled = local;

      this.imageResTypeLbl.Enabled = resource;
      this.image16ResLbl.Enabled = resource;
      this.image24ResLbl.Enabled = resource;
      this.imageResTypeComboBox.Enabled = resource;
      this.image16ResComboBox.Enabled = resource;
      this.image24ResComboBox.Enabled = resource;
   }

   private void FillImageResNameComboBoxes()
   {
      this.image16ResComboBox.Items.Clear();
      this.image24ResComboBox.Items.Clear();

      Type imageType = typeof(Image);
      Type resourceType = this.preset.ResourceType;
      if (resourceType == null)
         return;

      PropertyInfo[] props = resourceType.GetProperties(BindingFlags.Static | BindingFlags.NonPublic);
      foreach (PropertyInfo prop in props)
      {
         if (prop.PropertyType.Equals(imageType) || prop.PropertyType.IsSubclassOf(imageType))
         {
            Image img = prop.GetValue(null, null) as Image;
            if (img != null)
            {
               if (img.Size == new Size(16, 16))
                  this.image16ResComboBox.Items.Add(prop.Name);
               else if (img.Size == new Size(24, 24))
                  this.image24ResComboBox.Items.Add(prop.Name);
            }
         }
      }
   }


   private void presetBindingSource_BindingComplete(object sender, BindingCompleteEventArgs e)
   {
      if (e.BindingCompleteContext == BindingCompleteContext.DataSourceUpdate)
      {
         if (this.updateAction != null)
            this.updateAction();
      }
   }

   private void nodeSorterBindingSource_BindingComplete(object sender, BindingCompleteEventArgs e)
   {
      if (e.BindingCompleteContext == BindingCompleteContext.DataSourceUpdate)
      {
         if (this.updateAction != null)
            this.updateAction();
      }
   }

   private void resTypeBindingSource_BindingComplete(object sender, BindingCompleteEventArgs e)
   {
      if (e.BindingCompleteContext == BindingCompleteContext.DataSourceUpdate)
         this.FillImageResNameComboBoxes();
   }

   private void sorterComboBox_SelectedValueChanged(object sender, EventArgs e)
   {
      Boolean ascending = this.preset.NodeSorter.Ascending;
      Type selectedType = ((OutlinerPluginData)this.sorterComboBox.SelectedItem).Type;

      this.preset.NodeSorter = Activator.CreateInstance(selectedType) as NodeSorter;
      this.preset.NodeSorter.Ascending = ascending;

      if (this.updateAction != null)
         this.updateAction();
   }

   private String browseXmlFile(String filename)
   {
      openContextMenuFileDialog.InitialDirectory = OutlinerPaths.ContextMenusDir;
      openContextMenuFileDialog.FileName = filename;
      if (openContextMenuFileDialog.ShowDialog() == DialogResult.OK)
      {
         Uri localFile = new Uri(openContextMenuFileDialog.FileName);
         Uri presetsPath = new Uri(OutlinerPaths.ContextMenusDir);
         Uri relativePath = presetsPath.MakeRelativeUri(localFile);
         return Uri.UnescapeDataString(relativePath.ToString());
      }
      else
         return null;
   }

   private void contextMenuBrowseBtn_Click(object sender, EventArgs e)
   {
      String browseResult = browseXmlFile(this.preset.ContextMenuFile);
      if (!String.IsNullOrEmpty(browseResult))
      {
         this.preset.ContextMenuFile = browseResult;
         this.presetBindingSource.ResetCurrentItem();
      }
   }

   private void imgFileRadioButton_CheckedChanged(object sender, EventArgs e)
   {
      this.preset.ResourceTypeName = String.Empty;
      resTypeBindingSource.ResetCurrentItem();
      this.SetEnabledStates();
   }

   private void imgResRadioButton_CheckedChanged(object sender, EventArgs e)
   {
      this.preset.ResourceTypeName = typeof(OutlinerResources).FullName;
      resTypeBindingSource.ResetCurrentItem();
      this.FillImageResNameComboBoxes();
      this.SetEnabledStates();
   }

   private String browseImage(String filename)
   {
      this.openImageFileDialog.InitialDirectory = OutlinerPaths.PresetsDir;
      this.openImageFileDialog.FileName = filename;
      if (this.openImageFileDialog.ShowDialog() == DialogResult.OK)
      {
         Uri localFile = new Uri(this.openImageFileDialog.FileName);
         Uri presetsPath = new Uri(OutlinerPaths.PresetsDir);
         Uri relativePath = presetsPath.MakeRelativeUri(localFile);
         return Uri.UnescapeDataString(relativePath.ToString());
      }
      else
         return null;
   }

   private void image16FileBrowseBtn_Click(object sender, EventArgs e)
   {
      String browseResult = browseImage(this.preset.Image16Res);
      if (!String.IsNullOrEmpty(browseResult))
      {
         this.preset.Image16Res = browseResult;
         this.presetBindingSource.ResetCurrentItem();
      }
   }

   private void image24FileBrowseBtn_Click(object sender, EventArgs e)
   {
      String browseResult = browseImage(this.preset.Image16Res);
      if (!String.IsNullOrEmpty(browseResult))
      {
         this.preset.Image16Res = browseResult;
         this.presetBindingSource.ResetCurrentItem();
      }
   }

   


}
}
