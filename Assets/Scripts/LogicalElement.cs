using System.Collections;
using UnityEngine;

public class LogicalElement : CircuitElement
{
    public OutPoint[] outPoints;
    public InPoint[] inPoints;
    private string type;

    public int tick_wait = 1;

    protected virtual void Awake()
    {

        outPoints = GetComponentsInChildren<OutPoint>();
        inPoints = GetComponentsInChildren<InPoint>();

    }

    protected void Start()
    {
        CircuitChanged();
    }


    public void CircuitChanged()
    {
        try
        {
            StartCoroutine(Wait());
        }
        catch
        {

        }
    }

    IEnumerator Wait()
    {
        yield return StartCoroutine(WaitFor.Frames(tick_wait));
        ValueSet();
        yield return StartCoroutine(WaitFor.Frames(tick_wait));
    }
    
   protected virtual void ValueSet()
    {

    }

    public void PositionChanged()
    {

        for (int i = 0; i < inPoints.Length; i++)
        {
            if (inPoints[i].In != null)
                inPoints[i].In.Draw();
        }

        for (int i = 0; i < outPoints.Length; i++)
        {
           outPoints[i].Draw();
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < outPoints.Length; i++)
        {
            if (outPoints[i].Out != null)
            {
                outPoints[i].Out.In = null;
                outPoints[i].Out = null;
            }
        }
        for (int i = 0; i < inPoints.Length; i++)
        {
            if (inPoints[i].In != null)
            {
                inPoints[i].In.Out = null;
                inPoints[i].In = null;
            }
        }
    }

}

public static class WaitFor
{
    public static IEnumerator Frames(int frameCount)
    {
        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }
    }
}
