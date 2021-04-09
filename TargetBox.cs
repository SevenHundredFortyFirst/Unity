using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TargetBox : MonoBehaviour, IPointerClickHandler
{
    public GameObject tb;
    [SerializeField] private Text m_TextDistance;
    [SerializeField] private Text m_TextHitpoints;
    [SerializeField] private Text m_TextName;
    public Destructible Target { get; set; }

    private Vector3 m_LocalScale;

    private void Start()
    {
        m_LocalScale = transform.localScale;
    }

    public void Update()
    {
      /*  if (Input.GetKey(KeyCode.Tab))
        {
            tb.SetActive(false);
        }
        else
        {
            tb.SetActive(true);
        }
        */
        m_TextHitpoints.text = ((int)Target.HitPoints).ToString();
        m_TextName.text = ((string)Target.name).ToString();
        Vector3 targetPosition = Target.transform.position;

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetPosition);

        if (screenPosition.z > 0)
        {
            screenPosition.z = 0;
            transform.position = screenPosition;
        }
        else
        {
            transform.position = Vector3.one * 10000.0f;
        }

        transform.localScale = (TargetHudController.Instance.PlayerShip.SelectedTarget == Target) ?
            m_LocalScale : m_LocalScale * 0.7f;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            TargetHudController.Instance.SetTarget(Target);
    }
   
}
