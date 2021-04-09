using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class HealthPlayer : MonoBehaviour {
    
    public Slider slider;
    public float health;
    void Start() {

    }


    void Update() {
        slider.value = health;
        if (health >= 100)
        {
            health = 100;
        }

        if (health <= 0)
        {
            health = 0;
            DestroyObject(gameObject);
        }
       
    }
    
}
