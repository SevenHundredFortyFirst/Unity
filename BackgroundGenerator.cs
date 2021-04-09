using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_FonPrefab;

    [SerializeField] private int m_NumInstances;
    [SerializeField] private float m_Radius;

    [SerializeField] private Vector2 m_Scale;
    [SerializeField] private Transform m_Center;

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        for(int i = 0; i< m_NumInstances; i++)
        {
            var quad = Instantiate(m_FonPrefab[Random.Range(0, m_FonPrefab.Count)], m_Center);
            Material material = quad.GetComponent<Renderer>().material;
            material.color = new Color(Random.value, Random.value, Random.value, 1);
            quad.transform.localScale = Vector3.one * UnityEngine.Random.Range(m_Scale.x, m_Scale.y);
            quad.transform.position = m_Center.position + UnityEngine.Random.onUnitSphere * m_Radius;
            quad.transform.LookAt(m_Center);
            quad.transform.forward = -quad.transform.forward;

            quad.transform.Rotate(Vector3.forward * UnityEngine.Random.Range(-180, 180), Space.Self);

            quad.transform.SetParent(m_Center);
        }
    }
}
