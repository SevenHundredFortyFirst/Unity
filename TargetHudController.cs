using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHudController : MonoBehaviour
{
    [SerializeField] private TargetBox m_TargetBoxPrefab;

    [SerializeField] private SpaceShip m_PlayerShip;
    public SpaceShip PlayerShip => m_PlayerShip;

    public void SetPlayerShip(SpaceShip ship)
    {
        m_PlayerShip = ship;
    }

    [SerializeField] private LeadBox m_LeadBox;

    public static TargetHudController Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_LeadBox.gameObject.SetActive(false);
    }

    public void SetTarget(Destructible newTarget)
    {
        m_PlayerShip.SelectedTarget = newTarget;

        m_LeadBox.gameObject.SetActive(true);
    }

    public TargetBox SpawnTargetBox(Destructible destructibleObject)
    {
        var e = Instantiate(m_TargetBoxPrefab.gameObject);

        e.transform.SetParent(transform);

        TargetBox t = e.GetComponent<TargetBox>();
        t.Target = destructibleObject;
        return t;
    }

    private void Update()
    {
        if (m_PlayerShip == null)
            return;
       
        if (m_PlayerShip.SelectedTarget != null && m_LeadBox.IsLock)
            m_PlayerShip.WorldAimPoint = m_LeadBox.LeadPosition;
        else
            m_PlayerShip.WorldAimPoint = DefaultMouseAimPoint;
    }

    private const float DefaultAimDistance = 5000.0f;

    private Vector3 DefaultMouseAimPoint
    {
        get
        {
            // empty space
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 aimPoint = mouseRay.GetPoint(DefaultAimDistance);

            // collider
            RaycastHit hit;

            if(Physics.Raycast(mouseRay, out hit, DefaultAimDistance))
            {
                // we dont point at player ship
                SpaceShip potentialPlayerShip = hit.collider.transform.root.GetComponent<SpaceShip>();

                if (PlayerShip != null && PlayerShip == potentialPlayerShip)
                {
                    return aimPoint;
                }

                return hit.point;
            }

            return aimPoint;
        }
    }
}
