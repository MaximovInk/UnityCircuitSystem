public class CipherOut : OutPoint
{
    public string cipherValue { get { return c_value;  }
set { c_value = value; CircuitChanged(); } }
    private string c_value;
}