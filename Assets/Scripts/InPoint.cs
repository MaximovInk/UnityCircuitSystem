
public class InPoint : CircuitElement
{
    public OutPoint In;
    public LogicalElement element;

    public void CircuitChanged()
    {
        element = GetComponentInParent<LogicalElement>();
        element.CircuitChanged();
    }
}

