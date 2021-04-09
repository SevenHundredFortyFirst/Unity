using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCamera : MonoBehaviour
{
    [SerializeField] private Transform m_Camera;

    private void Update()
    {
        transform.rotation = m_Camera.rotation;
    }
}
