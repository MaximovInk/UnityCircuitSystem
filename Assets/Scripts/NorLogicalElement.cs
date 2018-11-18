using UnityEngine;
using System.Collections;

public class NorLogicalElement : LogicalElement
{
    protected override void ValueSet()
    {
        byte end = 0;

        for (int i = 0; i < inPoints.Length; i++)
        {
            end += inPoints[i].In != null ? inPoints[i].In.value : (byte)0;
        }
        end = end > 0 ? (byte)0 : (byte)1;

        for (int i = 0; i < outPoints.Length; i++)
        {
            outPoints[i].value = end;
        }
    }
}
