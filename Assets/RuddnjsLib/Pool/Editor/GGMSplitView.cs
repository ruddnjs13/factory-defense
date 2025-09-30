using System;
using UnityEngine.UIElements;

public class GGMSplitView : TwoPaneSplitView
{
    [Obsolete("Obsolete")]
    public new class UxmlFactory : UxmlFactory<GGMSplitView, UxmlTraits> { }
    [Obsolete("Obsolete")]
    public new class UxmlTraits : TwoPaneSplitView.UxmlTraits { }
}