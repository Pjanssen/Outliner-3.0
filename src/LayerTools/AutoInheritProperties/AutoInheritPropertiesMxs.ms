struct AutoInheritProperties
(
	public function getAutoInherit layer =
	(
		layer = layer.layerAsRefTarg;
		local AutoInheritClass = dotnetClass "PJanssen.Outliner.LayerTools.AutoInheritPropertiesMXS";
		AutoInheritClass.GetAutoInherit (getHandleByAnim layer);
	),
   
	public function setAutoInherit layer prop inherit =
	(
		layer = layer.layerAsRefTarg;
		local AutoInheritClass = dotnetClass "PJanssen.Outliner.LayerTools.AutoInheritPropertiesMXS";
		AutoInheritClass.SetAutoInherit (getHandleByAnim layer) prop inherit;
		OK;
	),

	public function clearScene =
	(
		local AutoInheritClass = dotnetClass "PJanssen.Outliner.LayerTools.AutoInheritPropertiesMXS";
		AutoInheritClass.ClearScene();
		OK;
	)
)