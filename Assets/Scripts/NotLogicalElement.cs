using System.Collections;
using UnityEngine;

public class NotLogicalElement : LogicalElement
{
    protected override void ValueSet()
    {
        base.ValueSet();
        byte val = 0;
        for (int i = 0; i < inPoints.Length; i++)
        {
            val += inPoints[i].In != null ? inPoints[i].In.value : (byte)0;
        }
        
        val = (byte)Mathf.Clamp01(val > 0 ? 0 : 1);

        for (int i = 0; i < outPoints.Length; i++)
        {
            outPoints[i].value = val;
        }
    }
}