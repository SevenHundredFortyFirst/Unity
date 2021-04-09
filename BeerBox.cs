using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerBox : MonoBehaviour
{
    [SerializeField] private GameObject m_StarQuadPrefab;

    [SerializeField] private int m_NumStars;
    [SerializeField] private float m_Radius;

    [SerializeField] private Transform m_Center;

    [SerializeField] private float m_Scale;
    private void Start()
    {
        for (int i = 0; i < m_NumStars; i++)
        {
            var quad = Instantiate(m_StarQuadPrefab);

            quad.transform.position = UnityEngine.Random.onUnitSphere * m_Radius + m_Center.position;
            quad.transform.localScale = Vector3.one * m_Scale;
            quad.transform.LookAt(m_Center.position);
            quad.transform.Rotate(0, 0, UnityEngine.Random.Range(0.0f, 180.0f), Space.Self);

            quad.transform.SetParent(transform);
        }
    }
}
