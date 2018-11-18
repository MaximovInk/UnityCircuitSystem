using UnityEngine;

public class BitDisplay : LogicalElement
{
    private TextMesh text;

    protected override void Awake()
    {
        text = GetComponentInChildren<TextMesh>();
        base.Awake();
        
    }

    protected override void ValueSet()
    {
        base.ValueSet();
        text.text = (inPoints[0].In == null ? 0 : inPoints[0].In.value).ToString();
    }
}
