﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Outliner.Controls.ContextMenu;
using Outliner.Controls.Tree.Layout;
using Outliner.NodeSorters;
using Outliner.Presets;

namespace Outliner.Controls.Options
{
public class PresetsEditor : ConfigFilesEditor<OutlinerPreset>
{
   private Tree.TreeNode defaultPresetsTn;
   private Tree.TreeNode customPresetsTn;

   private enum EditorType
   {
      Layout,
      Sorter,
      ContextMenu,
      Filters
   }

   public PresetsEditor(String directory)
      : base(directory, typeof(PresetPropertiesEditor))
   { }

   protected override void OnLoad(EventArgs e)
   {
      base.OnLoad(e);
         
      TreeNodeLayout layout = new TreeNodeLayout();
      layout.LayoutItems.Add(new TreeNodeIndent() { UseVisualStyles = false });
      layout.LayoutItems.Add(new TreeNodeText());
      layout.LayoutItems.Add(new EmptySpace());
      layout.FullRowSelect = true;
      this.filesTree.TreeNodeLayout = layout;
   }

   protected override void RefreshUI()
   {
      this.defaultPresetsTn = new Tree.TreeNode(OutlinerResources.Str_PresetsDefault);
      this.defaultPresetsTn.IsExpanded = true;
      this.customPresetsTn = new Tree.TreeNode(OutlinerResources.Str_PresetsCustom);
      this.customPresetsTn.IsExpanded = true;

      base.RefreshUI();

      this.filesTree.Nodes.Add(defaultPresetsTn);
      this.filesTree.Nodes.Add(customPresetsTn);
   }

   protected override Tree.TreeNode AddFileToTree(string file, OutlinerPreset config, Tree.TreeNodeCollection parentCollection)
   {
      if (config.IsDefaultPreset)
         parentCollection = this.defaultPresetsTn.Nodes;
      else
         parentCollection = this.customPresetsTn.Nodes;

      Tree.TreeNode tn = base.AddFileToTree(file, config, parentCollection);

      Tree.TreeNode layoutTn = new Tree.TreeNode(OutlinerResources.Str_PresetLayout);
      layoutTn.Tag = EditorType.Layout;
      tn.Nodes.Add(layoutTn);

      Tree.TreeNode sorterTn = new Tree.TreeNode(OutlinerResources.Str_PresetSorter);
      sorterTn.Tag = EditorType.Sorter;
      tn.Nodes.Add(sorterTn);

      Tree.TreeNode filterTn = new Tree.TreeNode(OutlinerResources.Str_PresetFilter);
      filterTn.Tag = EditorType.Filters;
      tn.Nodes.Add(filterTn);

      Tree.TreeNode contextMenuTn = new Tree.TreeNode(OutlinerResources.Str_PresetContextMenu);
      contextMenuTn.Tag = EditorType.ContextMenu;
      tn.Nodes.Add(contextMenuTn);

      return tn;
   }

   protected override string GetEditingFile(Tree.TreeNode tn)
   {
      Object tag = tn.Tag;
      if (tag is EditorType)
         return base.GetEditingFile(tn.Parent);
      else
         return base.GetEditingFile(tn);
   }

   protected override Control GetEditorFor(Tree.TreeNode tn)
   {
      if (tn.Tag is EditorType)
      {
         EditorType editorType = (EditorType)tn.Tag;
         OutlinerPreset preset = this.GetEditingConfiguration(tn);
         if (editorType == EditorType.Sorter)
            return new SorterConfigurationEditor();
         else if (editorType == EditorType.Filters)
            return new FilterCollectionEditor(preset.Filters);
         else if (editorType == EditorType.Layout)
            return new TreeNodeLayoutEditor(preset.TreeNodeLayout);
         else if (editorType == EditorType.ContextMenu)
            return new ContextMenuModelEditor(preset.ContextMenu);
         else
            return null;
      }

      return base.GetEditorFor(tn);
   }

   protected override void Commit()
   {
      foreach (KeyValuePair<String, OutlinerPreset> configFile in this.files)
      {
         XmlSerializationHelpers.Serialize<TreeNodeLayout>(Path.Combine(OutlinerPaths.LayoutsDir, configFile.Value.LayoutFile), configFile.Value.TreeNodeLayout);
         XmlSerializationHelpers.Serialize<ContextMenuModel>(Path.Combine(OutlinerPaths.ContextMenusDir, configFile.Value.ContextMenuFile), configFile.Value.ContextMenu);
      }

      base.Commit();
   }
}
}