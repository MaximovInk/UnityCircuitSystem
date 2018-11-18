public class ButtonLogicalElement : LogicalElement
{
    private bool pressed = false;

    private void OnMouseDown()
    {
        if (!pressed)
        {
            for (int i = 0; i < outPoints.Length; i++)
            {
                outPoints[i].value = 1;
            }
            pressed = true;
        }
        
    }

    private void OnMouseUp()
    {        
        for (int i = 0; i < outPoints.Length; i++)
        {
            outPoints[i].value = 0;
        }
        pressed = false;
    }
}
