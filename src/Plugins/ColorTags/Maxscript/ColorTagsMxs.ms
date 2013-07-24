struct ColorTags
(
    public function getHandle node =
    (
        if ((classof node) == MixinInterface) do
            node = node.layerAsRefTarg;

        getHandleByAnim node;
    ),

    public function getNumTags =
    (
        local ColorTagClass = dotnetClass "Outliner.ColorTags.ColorTags";
        ColorTagClass.GetNumTags();
    ),

    public function getTagColor tag =
    (
        local ColorTagClass = dotnetClass "Outliner.ColorTags.ColorTags";
        local col = ColorTagClass.GetColor tag;
        color col.R col.G col.B col.A;
    ),

    public function hasTag node =
    (
        local ColorTagClass = dotnetClass "Outliner.ColorTags.ColorTagsMXS";
        ColorTagClass.HasTag (ColorTags.getHandle node);
    ),
    
    public function getTag node =
    (
        local ColorTagClass = dotnetClass "Outliner.ColorTags.ColorTagsMXS";
        ColorTagClass.GetTag (ColorTags.getHandle node);
    ),

    public function getColor node =
    (
        local ColorTagClass = dotnetClass "Outliner.ColorTags.ColorTagsMXS";
        local tag = ColorTagClass.GetColor (ColorTags.getHandle node);
        color tag.R tag.G tag.B tag.A;
    ),

    public mapped function setTag node tag =
    (
        local ColorTagClass = dotnetClass "Outliner.ColorTags.ColorTagsMXS";
        ColorTagClass.SetTag (ColorTags.getHandle node) tag;
        OK;
    ),

    public mapped function setWireColorTag node =
    (
        ColorTags.setTag node (dotnetClass "Outliner.ColorTags.ColorTag").WireColor;
    ),

    public mapped function removeTag node =
    (
        local ColorTagClass = dotnetClass "Outliner.ColorTags.ColorTagsMXS";
        ColorTagClass.RemoveTag (ColorTags.getHandle node);
        OK;
    )
)