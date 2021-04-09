using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private string m_TargetScene;
    [SerializeField] private string m_TargetGate;

    [SerializeField] private float m_GateRadius;
    public float GateRadius => m_GateRadius;

    private void Update()
    {
        if(Player.Instance != null && Player.Instance.PlayerShip != null)
        {
            if((Player.Instance.PlayerShip.transform.position - transform.position).sqrMagnitude < m_GateRadius* m_GateRadius)
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        SceneSwitchController.SwitchSystem(m_TargetScene, m_TargetGate);
    }

    private static readonly Color GizmoColor = new Color(1, 0, 0, 0.3f);

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = GizmoColor;

        Gizmos.DrawSphere(transform.position, m_GateRadius);
    }
}
