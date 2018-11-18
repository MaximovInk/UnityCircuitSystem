public class EncoderLogicalElement : LogicalElement
{
    protected override void ValueSet()
    {
        string value = string.Empty;
        for (int i = 0; i < inPoints.Length; i++)
        {
            value += inPoints[i].In != null ? (inPoints[i].In is CipherOut ? (inPoints[i].In as CipherOut).cipherValue : inPoints[i].In.value.ToString()) : "0";
        }
        outPoints[0].value = byte.Parse(value[0].ToString());
        (outPoints[0] as CipherOut).cipherValue = value;
    }
}
