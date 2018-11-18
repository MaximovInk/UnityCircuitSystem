using UnityEngine;
using System;

public class Binarytest : MonoBehaviour
{
    public int value;

    void Start()
    {
        string BinaryCode = Convert.ToString(value, 2).PadLeft(4, '0');
        Debug.Log(BinaryCode);
        int new_value = Convert.ToInt32(BinaryCode, 2);
        Debug.Log(new_value);
    }

}
