using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material.color = Color.yellow;
    }
}
