using UnityEngine;

public class AndLogicalElement : LogicalElement
{
    protected override void ValueSet()
    {
        byte end = 1;

        for (int i = 0; i < inPoints.Length; i++)
        {
            end *= inPoints[i].In != null ? inPoints[i].In.value : (byte)0;
        }
        end = (byte)Mathf.Clamp01(end);

        for (int i = 0; i < outPoints.Length; i++)
        {
            outPoints[i].value = end;
        }
    }
}
