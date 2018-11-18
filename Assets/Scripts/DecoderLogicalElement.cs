using UnityEngine;

public class DecoderLogicalElement : LogicalElement
{
    protected override void ValueSet()
    {
        string ciphervalue = inPoints[0].In != null ? (inPoints[0].In as CipherOut).cipherValue : "0";
        try
        {
            var firstHalfLength = (ciphervalue.Length / 2);
            var secondHalfLength = ciphervalue.Length - firstHalfLength;
            var split = new[]
            {
                ciphervalue.Substring(0, firstHalfLength),
                ciphervalue.Substring(firstHalfLength, secondHalfLength)
            };
            Debug.Log(split[0] + " " + split[1]);
            if (outPoints[0].Out != null)
            {
                if (outPoints[0].Out.element is DecoderLogicalElement)
                {
                    string decode = split[0];

                    ciphervalue = split[1];
                    (outPoints[0] as CipherOut).cipherValue = decode;
                }
                else
                {
                    outPoints[0].value = byte.Parse(ciphervalue.Substring(0, 1));
                    ciphervalue = ciphervalue.Substring(1, ciphervalue.Length - 1);
                }
            }
            if (outPoints[1].Out != null)
            {
                if (outPoints[1].Out.element is DecoderLogicalElement)
                {
                    string decode = split[1];
                    (outPoints[1] as CipherOut).cipherValue = decode;
                }
                else
                {
                    outPoints[1].value = byte.Parse(ciphervalue.ToCharArray()[ciphervalue.ToCharArray().Length - 1].ToString());
                    Debug.Log(ciphervalue.Substring(0, ciphervalue.Length - 1) + "     " + ciphervalue.ToCharArray()[ciphervalue.ToCharArray().Length - 1].ToString());
                }
            }
        }
        catch
        {

        }
        

        /*if (outPoints[0].Out != null && outPoints[1].Out != null)
        {
            if (outPoints[0].Out.element is DecoderLogicalElement && outPoints[1].Out.element is DecoderLogicalElement)
            {
                string b = ciphervalue.Substring(0, ciphervalue.Length / 2);
                string c = ciphervalue.Substring(ciphervalue.Length / 2, ciphervalue.Length - ciphervalue.Length / 2);
                (outPoints[0] as CipherOut).cipherValue = b;
                (outPoints[1] as CipherOut).cipherValue = c;
                return;
            }
            else if (outPoints[0].Out.element is DecoderLogicalElement && !(outPoints[1].Out.element is DecoderLogicalElement))
            {
                (outPoints[0] as CipherOut).cipherValue = ciphervalue.Substring(0, ciphervalue.Length - 1);
                outPoints[1].value = byte.Parse(ciphervalue.Substring(ciphervalue.Length - 2, 1));
            }
            else if (outPoints[1].Out.element is DecoderLogicalElement && !(outPoints[0].Out.element is DecoderLogicalElement))
            {
                (outPoints[1] as CipherOut).cipherValue = ciphervalue.Substring(1, ciphervalue.Length - 1);
                outPoints[0].value = byte.Parse(ciphervalue.Substring(0, 1));
            }
            else
            {
                char[] c_a = ciphervalue.ToCharArray();
                outPoints[1].value = byte.Parse(c_a[c_a.Length-1].ToString());
                outPoints[0].value = byte.Parse(c_a[0].ToString());
            }
        }
        else if (outPoints[0].Out != null)
        {
            if (outPoints[0].Out.element is DecoderLogicalElement)
            {
                (outPoints[0] as CipherOut).cipherValue = ciphervalue.Substring(0, ciphervalue.Length - 1);
            }
            else
            {
                outPoints[0].value = byte.Parse(ciphervalue.Substring(0, 1));
            }
        }
        else if(outPoints[1].Out != null)
        {
            if (outPoints[1].Out.element is DecoderLogicalElement)
            {
                (outPoints[1] as CipherOut).cipherValue = ciphervalue.Substring(1, ciphervalue.Length - 1);
            }
            else
            {
                outPoints[1].value = byte.Parse(ciphervalue.Substring(ciphervalue.Length - 2, 1));
            }
        }*/
    }
}
