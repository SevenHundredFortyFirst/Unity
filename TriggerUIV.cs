using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TriggerUIV : MonoBehaviour
{

    public int valueDown ;
    public int valueUp ;
   
    void OnTriggerStay2D(Collider2D col)
    {
        
        if (col.tag == "Player")
        {
            col.GetComponent<HealthPlayer>().health -= valueDown * Time.deltaTime;
           
            col.GetComponent<HealthPlayer>().health += valueUp * Time.deltaTime;
        }
        

        } 
    }