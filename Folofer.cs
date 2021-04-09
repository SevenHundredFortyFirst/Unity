using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Folofer : MonoBehaviour
{
    [SerializeField] private Transform followedObject;
    [SerializeField] private Vector3 offset;

    static public void DoSomething() { }
    void Update()
    {
        if (followedObject != null)
            transform.position = followedObject.position + offset;
    }
}
