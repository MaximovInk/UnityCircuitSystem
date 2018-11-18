public class RepeaterLogicalElement : LogicalElement
{
    protected override void ValueSet()
    {
        base.ValueSet();
        outPoints[0].value = inPoints[0].In == null ? (byte)0 : inPoints[0].In.value;
    }
}
