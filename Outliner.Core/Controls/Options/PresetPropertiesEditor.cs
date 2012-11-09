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

namespace Outliner.Controls.Options
{
public partial class PresetPropertiesEditor : UserControl
{
   private PresetEditor owningEditor;
   private OutlinerPreset preset;

   public PresetPropertiesEditor()
   {
      InitializeComponent();
   }

   public PresetPropertiesEditor(PresetEditor owningEditor, OutlinerPreset preset) : this()
   {
      this.owningEditor = owningEditor;
      this.preset = preset;

      this.radioButton1.Checked = String.IsNullOrEmpty(preset.ImageResourceTypeName);
      this.radioButton2.Checked = !String.IsNullOrEmpty(preset.ImageResourceTypeName);
      this.SetEnabledStates();

      Color windowColor = ColorHelpers.FromMaxGuiColor(GuiColors.Window);
      Color windowTextColor = ColorHelpers.FromMaxGuiColor(GuiColors.WindowText);

      this.SetControlColor(this.nameTextBox, windowColor, windowTextColor);
      this.SetControlColor(this.image16FileTextBox, windowColor, windowTextColor);
      this.SetControlColor(this.image24FileTextBox, windowColor, windowTextColor);
      this.SetControlColor(this.image16ResComboBox, windowColor, windowTextColor);
      this.SetControlColor(this.image24ResComboBox, windowColor, windowTextColor);
      this.SetControlColor(this.imageResTypeComboBox, windowColor, windowTextColor);
      this.SetControlColor(this.modeComboBox, windowColor, windowTextColor);
      this.SetControlColor(this.sorterComboBox, windowColor, windowTextColor);

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

      IEnumerable<OutlinerPluginData> modes = OutlinerPlugins.GetPluginsByType(OutlinerPluginType.TreeMode);
      foreach (OutlinerPluginData mode in modes)
      {
         //this.modeComboBox.Items.Add(new Tuple<String, String>(mode.DisplayName, mode.Type.FullName));
         this.modeComboBox.Items.Add(mode.Type.FullName);
      }
      //this.modeComboBox.ValueMember = "Item2";
      //this.modeComboBox.DisplayMember = "Item1";
      Binding modesBinding = this.modeComboBox.DataBindings.Add( "SelectedItem"
                                                               , this.preset
                                                               , "TreeModeTypeName");


      IEnumerable<OutlinerPluginData> sorters = OutlinerPlugins.GetPluginsByType(OutlinerPluginType.NodeSorter);
      foreach (OutlinerPluginData sorter in sorters)
      {
         this.sorterComboBox.Items.Add(sorter.Type.FullName);
      }
      //this.sorterComboBox.DisplayMember = "DisplayName";
      //this.sorterComboBox.ValueMember = "Type";
      Binding sorterBinding = this.sorterComboBox.DataBindings.Add( "SelectedValue"
                                                                  , this.preset
                                                                  , "NodeSorter");
      
      this.FillImageResNameComboBoxes();
      this.presetBindingSource.DataSource = preset;
      this.resTypeBindingSource.DataSource = preset;
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

      Boolean local = String.IsNullOrEmpty(this.preset.ImageResourceTypeName);
      Boolean resource = !String.IsNullOrEmpty(this.preset.ImageResourceTypeName);

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
      Type resourceType = this.preset.ImageResourceType;
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


   void presetBindingSource_BindingComplete(object sender, BindingCompleteEventArgs e)
   {
      if (e.BindingCompleteContext == BindingCompleteContext.DataSourceUpdate)
         this.owningEditor.UpdatePreviewTree();
   }

   void resTypeBindingSource_BindingComplete(object sender, BindingCompleteEventArgs e)
   {
      if (e.BindingCompleteContext == BindingCompleteContext.DataSourceUpdate)
         this.FillImageResNameComboBoxes();
   }
}
}
