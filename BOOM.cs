using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOOM : MonoBehaviour
{
   
    [SerializeField] private MovePlayer Submarine;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "HP" )
        {
            if (collision.gameObject.tag != "Player")
            {
                Submarine.m_IsWorking = false;

                GameObject.Destroy(gameObject);
            }
            
        }
        
        else
        {
            
        }
    }
}

