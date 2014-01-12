struct NestedLayers
(
    public function getParent layer =
    (
        local NLClass = dotnetClass "PJanssen.Outliner.LayerTools.NestedLayersMXS";
        getAnimByHandle (NLClass.GetParent (getHandleByAnim layer.layerAsRefTarg));
    ),
   
    public function setParent layer parent =
    (
        local NLClass = dotnetClass "PJanssen.Outliner.LayerTools.NestedLayersMXS";
        local parentHandle;
        if (parent == undefined) then
            parentHandle = NLClass.RootHandle;
        else
            parentHandle = getHandleByAnim parent.layerAsRefTarg;

        NLClass.SetParent (getHandleByAnim layer.layerAsRefTarg) parentHandle;
        OK;
    ),

    public function clearScene =
    (
        local NLClass = dotnetClass "PJanssen.Outliner.LayerTools.NestedLayersMXS";
        NLClass.ClearScene();
        OK;
    )
)