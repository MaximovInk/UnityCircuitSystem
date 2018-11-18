public class SplitterLogicalElement : LogicalElement
{
    protected override void ValueSet()
    {
        byte value = inPoints[0].In == null ? (byte)0 : inPoints[0].In.value;

        for (int i = 0; i < outPoints.Length; i++)
        {
            outPoints[i].value = value;
        }
    }

}
