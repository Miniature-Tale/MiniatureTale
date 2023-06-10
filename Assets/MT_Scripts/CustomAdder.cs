using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAdder : MonoBehaviour
{
    public int result;
    // Start is called before the first frame update
    void Start()
    {
        result = MyAdderMethod(1,2);
    }

    public int MyAdderMethod(int a, int b)
    {
        return a+b;
    }
}
