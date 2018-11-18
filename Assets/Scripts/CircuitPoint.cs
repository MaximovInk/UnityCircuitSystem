using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class CircuitPoint : MonoBehaviour {
    public CircuitPoint In;
    public CircuitPoint Out;
    private LineRenderer lineRenderer;


    private bool drag = false;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        Draw();
    }

    public void Draw()
    {
        if (!drag)
        {
            if (Out == null)
            {
                lineRenderer.SetPositions(new Vector3[] { transform.position, transform.position });
            }

            else
            {
                lineRenderer.SetPositions(new Vector3[] { transform.position, Out.transform.position });
            }

        }

    }

    public void OnMouseDown()
    {
        drag = true;
    }

    public void OnMouseDrag()
    {
        lineRenderer.SetPositions(new Vector3[] { transform.position, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f)) });
    }

    public void OnMouseUp()
    {
        drag = false;

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject != gameObject && hit.collider.GetComponent<CircuitPoint>())
        {
            CircuitPoint c = hit.collider.GetComponent<CircuitPoint>();

            if (c.In == this)
            {
                c.In = null;
                Out = null;
            }else if (c.Out != this)
            {
                if (c.In == null)
                {
                    if (Out != null && Out != c)
                    {
                        Out.In = null;
                        Out = null;
                    }
                    c.In = this;
                    Out = c;
                }
            }
            


            /*if (c.Out != this)
            {
                if (Out == c)
                    Out = null;
                else
                    Out = c;
            }
            else if(c.Out == this)
            {
                c.Out = null;
                Out = c;
            }*/
        }
    }
}
