using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerBOOm : MonoBehaviour
{
    [SerializeField] private BEER Beesr;
   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "HP")
        {

            
            GameObject.Destroy(gameObject);
        }
        
    }
}
