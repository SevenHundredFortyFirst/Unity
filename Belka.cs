using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belka : MonoBehaviour
{
    public bool agressive;
    [SerializeField] private MovePlayer m_Player;
    [Range(0f, 2500f)]
    [SerializeField] private float m_VisionRadius = 10;
    [Range(0f, 20f)]
    [SerializeField] private float m_Speed;
    

    // Update is called once per frame
    void Update()
    {
        if (m_Player == null)
            FindObjectOfType<MovePlayer>();
        if (m_Player == null)
            return;

        if (agressive)
            AgressiveChase();
        else
            NotAgressiveChase();
    }

    private void AgressiveChase()
    {
        if ((m_Player.transform.position - transform.position).sqrMagnitude < m_VisionRadius * m_VisionRadius)
        {
            transform.position +=  Time.deltaTime * (m_Player.transform.position - transform.position).normalized * m_Speed ;
        }
        else
        {
            
        }
    }

    private void NotAgressiveChase()
    {
        if ((m_Player.transform.position - transform.position).sqrMagnitude < m_VisionRadius * m_VisionRadius)
        {
            
            transform.position += Time.deltaTime * (m_Player.transform.position - transform.position).normalized * m_Speed;
        }
        else
        {
            
        }
    }
}

