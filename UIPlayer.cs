using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    [SerializeField] private Destructible m_destructible;
    [SerializeField] private Text m_TextSpeed;
    [SerializeField] public Rigidbody Rigid;
    [SerializeField] private SpaceShip m_PlayerShipPrefab;
    [SerializeField] private Player m_player;
    void Start()
    {
        slider.minValue = 0;
        slider.maxValue = m_destructible.m_MaxHitPoints;

       
    }

    // Update is called once per frame
    void Update()
    {
        if (Rigid == null)
        {
            Destroy(GetComponent<UIPlayer>());
        }
        else
        {
            m_destructible = m_player.PlayerShip;
            Rigid = m_player.PlayerShip.m_Rigid;
            slider.value = m_destructible.m_HitPoints;
            var speed = Rigid.velocity.magnitude;
            m_TextSpeed.text = "Speed " + ((int)speed).ToString();
        }



        
    }
}
