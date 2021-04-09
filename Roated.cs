using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roated : MonoBehaviour
{
    [SerializeField] int SpeedRot;
    void Update()
    {
        transform.Rotate(0, 0 + SpeedRot, 0 );

    }
    
}
