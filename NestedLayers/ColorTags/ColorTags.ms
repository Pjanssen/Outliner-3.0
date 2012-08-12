struct ColorTags
(
    public function getHandle node =
    (
        if ((classof node) == MixinInterface) do
            node = node.layerAsRefTarg;

        getHandleByAnim node;
    ),

    public function hasTag node =
    (
        local ColorTagClass = dotnetClass "Outliner.LayerTools.ColorTagsMXS";
        ColorTagClass.HasTag (ColorTags.getHandle node);
    ),
    
    public function getTag node =
    (
        local ColorTagClass = dotnetClass "Outliner.LayerTools.ColorTagsMXS";
        ColorTagClass.GetTag (ColorTags.getHandle node);
    ),

    public function getColor node =
    (
        local ColorTagClass = dotnetClass "Outliner.LayerTools.ColorTagsMXS";
        local tag = ColorTagClass.GetColor (ColorTags.getHandle node);
        color tag.R tag.G tag.B tag.A;
    ),

    public mapped function setTag node tag =
    (
        local ColorTagClass = dotnetClass "Outliner.LayerTools.ColorTagsMXS";
        ColorTagClass.SetTag (ColorTags.getHandle node) tag;
        OK;
    ),

    public mapped function setWireColorTag node =
    (
        ColorTags.setTag node (dotnetClass "Outliner.LayerTools.ColorTags").UseWireColor;
    ),

    public mapped function removeTag node =
    (
        local ColorTagClass = dotnetClass "Outliner.LayerTools.ColorTagsMXS";
        ColorTagClass.RemoveTag (ColorTags.getHandle node);
    )
)