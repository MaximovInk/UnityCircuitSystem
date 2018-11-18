using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OutPoint : CircuitElement
{

    public InPoint Out { get { return _out;  } set { _out = value; CircuitChanged(); Draw(); } }
    protected InPoint _out;
    protected LineRenderer lineRenderer
    {
        get { return _lineRenderer; }
        set {
            _lineRenderer = value; Draw();
        }
    }
    protected LineRenderer _lineRenderer;


    public byte value { get { return _value;  } set { _value = value; CircuitChanged(); } }
    protected byte _value;


    protected bool drag = false;

    protected void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.startWidth = 0.2f;
        lineRenderer.material = GetComponent<SpriteRenderer>().material;
        lineRenderer.sortingOrder = -1;
    }

    public void CircuitChanged()
    {
        if (Out != null)
        {
            Out.CircuitChanged();
        }
    }

    public virtual void Draw()
    {
        if (!drag)
        {
            if (_out == null)
            {
                lineRenderer.SetPositions(new Vector3[] { transform.position, transform.position });
            }

            else
            {
                lineRenderer.SetPositions(new Vector3[] { transform.position, _out.transform.position });
            }

        }

    }

    protected virtual void OnMouseDown()
    {
        drag = true;
    }

    protected virtual void OnMouseDrag()
    {
        if (CircuitManager.circuit_tool == CircuitManager.Tool.Circuit)
        {
            lineRenderer.SetPositions(new Vector3[] { transform.position, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f)) });
        }   
    }

    protected virtual void OnMouseUp()
    {
        drag = false;

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (CircuitManager.circuit_tool == CircuitManager.Tool.Circuit)
        {
            if (hit.collider != null && hit.collider.gameObject != gameObject && hit.collider.GetComponent<InPoint>())
            {
                InPoint end_point = hit.collider.GetComponent<InPoint>();
                if (end_point.In != this)
                {
                    if (Out != null)
                    {
                        Out.In = null;
                        Out = null;
                    }

                    if (end_point.In != null)
                    {
                        end_point.In.Out = null;
                    }
                    end_point.In = this;
                    Out = end_point;
                }
                else if (end_point.In != null)
                {
                    end_point.In = null;
                    Out = null;
                }
            }
            Draw();
        }
       
    }
}
