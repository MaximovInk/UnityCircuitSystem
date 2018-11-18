using UnityEngine;

public class NandLogicalElement : LogicalElement
{
    protected override void ValueSet()
    {

        byte end = (byte)((inPoints[0].In != null ? inPoints[0].In.value : 0) == 1 && (inPoints[1].In != null ? inPoints[1].In.value : 0) == 1 ? 0 : 1);

        outPoints[0].value = (byte)(end == 1 ? 0 : 1);

    }
}
