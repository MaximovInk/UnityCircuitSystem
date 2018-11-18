using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;


public class CircuitManager : MonoBehaviour
{
    public InputField InputField;

    private CircuitElement selected;

    public static Tool circuit_tool = Tool.Circuit;

    public GameObject And;
    public GameObject Or;
    public GameObject Not;
    public GameObject Bit_display;
    public GameObject EditableOut;
    public GameObject Splitter;
    public GameObject Button;
    public GameObject Nand;
    public GameObject Nor;
    public GameObject Equal;
    public GameObject Repeater;
    public GameObject Xor;
    public GameObject RS;

    public Transform elements_parent;
    public Text play_text;

    private bool loading = false;

    public float mouseSensitivity = 1.0f;
    private Vector3 lastPosition;

    private MonoBehaviour selected_t;

    private Camera cam;

    public void Save()
    {
        List<SElement> elements = new List<SElement>();



        for (int i = 0; i < elements_parent.childCount; i++)
        {
            var ce = elements_parent.GetChild(i).GetComponent<CircuitElement>();
            //Debug.Log(ce);
            string type = ce is EditableOut ? "editableout" :
                ce is OrLogicalElement ? "or" :
                ce is AndLogicalElement ? "and" :
                ce is NotLogicalElement ? "not" :
                ce is ButtonLogicalElement ? "button" :
                ce is BitDisplay ? "bitdisplay" :
                ce is SplitterLogicalElement ? "splitter" :
                ce is NandLogicalElement ? "nand" :
                ce is RepeaterLogicalElement ? "repeater" :
                ce is EqualLogicalElement ? "equal" :
                ce is XorLogicalElement ? "xor" :
                ce is NorLogicalElement ? "nor" :
                ce is RSTriggerLogicalElement ? "rs"
                : "error";
            SElement se = new SElement(type, ce.transform.position, i);
            elements.Add(se);
        }
        for (int i = 0; i < elements.Count; i++)
        {
            var ce = elements_parent.GetChild(i).GetComponent<CircuitElement>();

            LogicalElement le = ce as LogicalElement;

            for (int out_id = 0; out_id < le.outPoints.Length; out_id++)
            {
                if (le.outPoints[out_id].Out != null)
                {
                    elements[i].out_ids.Add(new EndNode(le.outPoints[out_id].Out.transform.parent.GetSiblingIndex(), le.outPoints[out_id].Out.transform.GetSiblingIndex()));
                }
                else
                {
                    elements[i].out_ids.Add(new EndNode());
                }
            }


        }
        var file_name = Path.Combine(Application.persistentDataPath, "circuit.dat");
        FileStream fs = new FileStream(file_name, FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();
        try
        {
            formatter.Serialize(fs, elements);
        }
        catch (SerializationException e)
        {
            throw new ArgumentException("Failed to serialize " + file_name, e.Message);
        }
        finally
        {
            fs.Close();
        }
    }

    public void Clear()
    {
        for (int i = 0; i < elements_parent.childCount; i++)
        {
            Destroy(elements_parent.GetChild(i).gameObject);

        }
    }

    public void Load()
    {
        if (!loading)
        {
            loading = true;
            List<SElement> elements;

            var file_name = Path.Combine(Application.persistentDataPath, "circuit.dat");
            if (!File.Exists(file_name))
            {
                Save();
            }
            FileStream fs = new FileStream(file_name, FileMode.Open);
            try
            {

                Clear();


                BinaryFormatter formatter = new BinaryFormatter();
                elements = (List<SElement>)formatter.Deserialize(fs);
                List<LogicalElement> objects = new List<LogicalElement>();



                if (elements != null && elements.Count > 0)
                {
                    for (int i = 0; i < elements.Count; i++)
                    {
                        GameObject go = null;
                        switch (elements[i].type)
                        {
                            case "editableout":
                                go = Spawn(0, false);
                                break;
                            case "or":
                                go = Spawn(2, false);
                                break;
                            case "and":
                                go = Spawn(1, false);
                                break;
                            case "not":
                                go = Spawn(3, false);
                                break;
                            case "button":
                                go = Spawn(6, false);
                                break;
                            case "bitdisplay":
                                go = Spawn(5, false);
                                break;
                            case "splitter":
                                go = Spawn(4, false);
                                break;
                            case "nand":
                                go = Spawn(7, false);
                                break;
                            case "nor":
                                go = Spawn(8, false);
                                break;
                            case "repeater":
                                go = Spawn(9, false);
                                break;
                            case "xor":
                                go = Spawn(11, false);
                                break;
                            case "equal":
                                go = Spawn(10, false);
                                break;
                            case "rs":
                                go = Spawn(12, false);
                                break;
                            default:
                                break;
                        }
                        go.transform.position = new Vector2(elements[i].x_position, elements[i].y_position);
                        objects.Add(go.GetComponent<LogicalElement>());
                    }
                }



                for (int i = 0; i < objects.Count; i++)
                {
                    for (int o = 0; o < elements[i].out_ids.Count; o++)
                    {
                        if (elements[i].out_ids[o].id != -1 && elements[i].out_ids[o].child != -1)
                        {
                            objects[i].outPoints[o].Out
                                = elements_parent.GetChild(elements[i].out_ids[o].id).GetComponent<LogicalElement>().inPoints[elements[i].out_ids[o].child].GetComponent<InPoint>();
                            elements_parent.GetChild(elements[i].out_ids[o].id).GetComponent<LogicalElement>().inPoints[elements[i].out_ids[o].child].GetComponent<InPoint>().In
                                = objects[i].outPoints[o];
                        }
                    }
                }

                foreach (var item in objects)
                {
                    item.PositionChanged();

                }




            }
            catch (SerializationException e)
            {
                throw new ArgumentException("Failed to deserialize " + file_name, e.Message);
            }
            finally
            {
                fs.Close();
            }
            loading = false;
        }



    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {


        if (Input.GetMouseButton(0))
        {
            if (circuit_tool == Tool.Translate)
            {
                if (selected_t != null)
                {
                    selected_t.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
                    if (selected_t is LogicalElement)
                        (selected_t as LogicalElement).PositionChanged();
                    else
                        (selected_t as EditableOut).PositionChanged();
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (circuit_tool == Tool.Translate)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && (hit.collider.GetComponent<LogicalElement>() || hit.collider.GetComponent<EditableOut>()))
                {
                    selected_t = hit.collider.GetComponent<MonoBehaviour>();
                }

            }

            Select();
        }
        if (Input.GetMouseButtonUp(0))
        {
            selected_t = null;
            lastPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        }

    }

    public void Select()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider == null || (hit.collider != null && !hit.collider.GetComponent<EditableOut>()))
        {
            InputField.text = string.Empty;
        }

        if (hit.collider != null && hit.collider.GetComponent<CircuitElement>())
        {
            selected = hit.collider.GetComponent<CircuitElement>();
            if (selected is EditableOut)
                InputField.text = (selected as EditableOut).value.ToString();
        }
    }

    public void OnInputField(string value)
    {
        byte v = 0;
        byte.TryParse(value, out v);
        v = (byte)Mathf.Clamp01(v);
        InputField.text = v.ToString();

        if (selected is EditableOut)
        {
            (selected as EditableOut).value = v;
        }
    }

    public void Delete()
    {
        if (selected != null && selected is LogicalElement)
            Destroy(selected.gameObject);
    }

    public void Switch()
    {
        int i = (int)circuit_tool + 1;
        if (i > 2)
            i = 0;

        Switch(i);
    }

    public void Spawn(int type)
    {
        Spawn(type, false);
    }

    public GameObject Spawn(int type, bool FIXBUGUNITYSUKA)
    {
        GameObject go = null;
        switch (type)
        {
            case 0:
                go = Instantiate(EditableOut, elements_parent);
                break;
            case 1:
                go = Instantiate(And, elements_parent);
                break;
            case 2:
                go = Instantiate(Or, elements_parent);
                break;
            case 3:
                go = Instantiate(Not, elements_parent);
                break;
            case 4:
                go = Instantiate(Splitter, elements_parent);
                break;
            case 5:
                go = Instantiate(Bit_display, elements_parent);
                break;
            case 6:
                go = Instantiate(Button, elements_parent);
                break;
            case 7:
                go = Instantiate(Nand, elements_parent);
                break;
            case 8:
                go = Instantiate(Nor, elements_parent);
                break;
            case 9:
                go = Instantiate(Repeater, elements_parent);
                break;
            case 10:
                go = Instantiate(Equal, elements_parent);
                break;
            case 11:
                go = Instantiate(Xor, elements_parent);
                break;
            case 12:
                go = Instantiate(RS, elements_parent);
                break;
            default:
                go = Instantiate(EditableOut, elements_parent);
                break;
        }
        go.transform.position = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2));
        go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, 0);
        return go;
    }

    public enum Tool
    {
        Circuit,
        Translate,
        Move
    }

    public void Switch(int type)
    {
        circuit_tool = (Tool)type;
    }
    [Serializable]
    public class SElement
    {
        public string type = string.Empty;
        public float x_position = 0;
        public float y_position = 0;
        public int id = -1;
        public List<EndNode> out_ids = new List<EndNode>();

        public SElement(string type, Vector2 position, int id)
        {
            this.type = type;
            x_position = position.x;
            y_position = position.y;
            this.id = id;
        }
    }
    [Serializable]
    public class EndNode
    {
        public int id;
        public int child;

        public EndNode()
        {
            id = -1;
            child = -1;
        }

        public EndNode(int id, int child)
        {
            this.id = id;
            this.child = child;
        }
    }
}
