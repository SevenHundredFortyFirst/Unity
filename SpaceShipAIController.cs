using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpaceShip))]
public class SpaceShipAIController : MonoBehaviour
{
    [SerializeField] private bool m_EnableAI;

    public enum AIBehaviour
    {
        Null,
        Patrol
    }

    [SerializeField] private AIBehaviour m_AIBehaviour;

    [Range(0.0f, 1.0f)]
    [SerializeField] private float m_NavigationLinear;

    [Range(0.0f, 1.0f)]
    [SerializeField] private float m_NavigationAngular;

    [SerializeField] private AIPointPatrol m_PatrolPoint;

    private SpaceShip m_SpaceShip;

    private void Start()
    {
        m_SpaceShip = GetComponent<SpaceShip>();

        InitActionTimers();
    }


    private void Update()
    {
        UpdateAI();

        UpdateActionTimers();
    }

    private void UpdateAI()
    {
        if (!m_EnableAI)
            return;

        switch (m_AIBehaviour)
        {
            case AIBehaviour.Null:
                break;

            case AIBehaviour.Patrol:
                {
                    ActionFindNewMovePosition();
                    ActionMoveToPosition();
                    ActionFindNewAttackTarget();
                    ActionFire();
                }
                break;
        }
    }

    private Vector3 m_MovePosition;

    private void ActionMoveToPosition()
    {
        m_SpaceShip.ControlThrust = Vector3.forward * m_NavigationLinear;
        m_SpaceShip.ControlTorque = ComputeAlignTorqueNormalized(m_MovePosition, transform) * m_NavigationAngular;
    }

    private const float MaxAngle = 45.0f;

    private static Vector3 ComputeAlignTorqueNormalized(Vector3 targetPosition, Transform source)
    {
        Vector3 torque = Vector3.zero;

        Vector3 localTargetPosition = source.InverseTransformPoint(targetPosition);

        // Left & right
        {
            var lp = localTargetPosition;
            lp.y = 0;
            float angle = Vector3.SignedAngle(lp, Vector3.forward, Vector3.up);
            angle = Mathf.Clamp(angle, -MaxAngle, MaxAngle) / MaxAngle;
            torque -= Vector3.up * angle;
        }

        // Up & down
        {
            var lp = localTargetPosition;
            lp.x = 0;
            float angle = Vector3.SignedAngle(lp, Vector3.forward, Vector3.right);
            angle = Mathf.Clamp(angle, -MaxAngle, MaxAngle) / MaxAngle;
            torque -= Vector3.right * angle;
        }

        return torque;
    }

    private float m_AlignTimer;

    [SerializeField] private float m_RandomizeAlignTime;

    private void ActionFindNewMovePosition()
    {
        if (m_AIBehaviour == AIBehaviour.Patrol)
        {
            if(m_SpaceShip.SelectedTarget != null)
            {
                m_MovePosition = m_SpaceShip.SelectedTarget.transform.position;
            }
            else
            if (m_PatrolPoint != null)
            {
                bool isInsidePatrolZone = (m_PatrolPoint.transform.position - transform.position).sqrMagnitude < m_PatrolPoint.Radius * m_PatrolPoint.Radius;

                if (isInsidePatrolZone)
                {
                    if (IsActionTimerFinished(ActionTimerType.RandomizeDirection))
                    {
                        Vector3 newPoint = UnityEngine.Random.onUnitSphere * m_PatrolPoint.Radius + m_PatrolPoint.transform.position;
                        m_MovePosition = newPoint;

                        SetActionTimer(ActionTimerType.RandomizeDirection, m_RandomizeAlignTime);
                    }   
                }
                else
                {
                    m_MovePosition = m_PatrolPoint.transform.position;
                }
            }

        }

    }

    private enum ActionTimerType
    {
        Null,

        RandomizeDirection,
        Fire,
        FindNewTarget,

        MaxValues
    }

    private float[] m_ActionTimers;

    private void InitActionTimers()
    {
        m_ActionTimers = new float[(int)ActionTimerType.MaxValues];
    }

    private void UpdateActionTimers()
    {
        for(int i =0; i< m_ActionTimers.Length; i++)
        {
            if (m_ActionTimers[i] > 0)
                m_ActionTimers[i] -= Time.deltaTime;
        }
    }

    private void SetActionTimer(ActionTimerType e, float time)
    {
        m_ActionTimers[(int)e] = time;
    }

    private bool IsActionTimerFinished(ActionTimerType e)
    {
        return m_ActionTimers[(int)e] <= 0;
    }

    private void ActionFindNewAttackTarget()
    {
        m_SpaceShip.SelectedTarget = FindNearestDestructibleTarget();
    }

    private void ActionFire()
    {
        if (m_SpaceShip.SelectedTarget != null)
        {
            Vector3 launchPoint = TargetHudController.Instance.PlayerShip.AverageTurretLaunchPosition;
            float launchVelocity = TargetHudController.Instance.PlayerShip.AverageTurretLaunchVelocity;
            Vector3 targetPos = m_SpaceShip.SelectedTarget.transform.position;
            Vector3 targetVelocity = m_SpaceShip.SelectedTarget.LinearVelocity;
            Vector3 playerVelocity = TargetHudController.Instance.PlayerShip.LinearVelocity;

            m_SpaceShip.WorldAimPoint = LeadBox.MakeLead(launchPoint, playerVelocity + (targetPos - launchPoint).normalized * launchVelocity,
                targetPos,
                targetVelocity);

            if(IsActionTimerFinished(ActionTimerType.Fire))
            {
                m_SpaceShip.FireAllTurrets();

                SetActionTimer(ActionTimerType.Fire, UnityEngine.Random.Range(0.0f, 2.0f));
            }
            
        }
    }

    private Destructible FindNearestDestructibleTarget()
    {
        float dist2 = -1;

        Destructible potentialTarget = null;

        foreach(var v in Destructible.AllDestructibles)
        {
            if (v.GetComponent<SpaceShip>() == m_SpaceShip)
                continue;

            if (Destructible.TeamIdNeutral == v.TeamId)
                continue;

            if (m_SpaceShip.TeamId == v.TeamId)
                continue;

            float d2 = (m_SpaceShip.transform.position - v.transform.position).sqrMagnitude;

            if (dist2 < 0 || d2 < dist2)
            {
                potentialTarget = v;
                dist2 = d2;
            }
        }

        return potentialTarget;
    }

    public void SetPatrolBehaviour(AIPointPatrol point)
    {
        m_AIBehaviour = AIBehaviour.Patrol;
        m_PatrolPoint = point;

        
    }

    /*
     * Snippet
    #region Action timers for randomization

    private enum Action
    {
        Null,

        RandomizeDirection,
        FindNewAttackTarget,
        Fire,

        MaxValues
    }

    private float[] m_ActionTimers;

    private void InitActionTimers()
    {
        m_ActionTimers = new float[(int)Action.MaxValues];
    }

    private void UpdateActionTimers()
    {
        for (int i = 0; i < m_ActionTimers.Length; i++)
        {
            if (m_ActionTimers[i] > 0)
                m_ActionTimers[i] -= Time.deltaTime;
        }
    }

    private bool IsActionFinished(Action e)
    {
        return m_ActionTimers[(int)e] < 0;
    }

    private void SetActionTimer(Action e, float time)
    {
        m_ActionTimers[(int)e] = time;
    }

    private bool RandomChance(int max)
    {
        return UnityEngine.Random.Range(0, max) == 0;
    }

    #endregion

    #region Attack

    private IEnumerable<Destructible> AllDestructibles => FindObjectsOfType<Destructible>();

    private int ShipTeamId => 0;

    private void ActionFindNewTarget()
    {
        m_SpaceShip.SelectedTarget = GetNearestValidTarget();
    }

    private Destructible GetNearestValidTarget()
    {
        float dist2 = -1;

        Destructible potentialNewTarget = null;

        foreach (var v in AllDestructibles)
        {
            if (v.GetComponent<SpaceShip>() == m_SpaceShip)
                continue;
            //if (v.TeamId == Destructible.TeamIdNeutral)
            //    continue;

            //if (v.TeamId == m_SpaceShip.TeamId)
            //    continue;

            float d2 = (v.transform.position - m_SpaceShip.transform.position).sqrMagnitude;

            if (dist2 < 0 || d2 < dist2)
            {
                potentialNewTarget = v;
                dist2 = d2;
            }
        }

        return potentialNewTarget;
    }

    public static Vector3 ComputeLead(SpaceShip ship, Destructible target)
    {
        Vector3 launchPoint = TargetHudController.Instance.PlayerShip.AverageTurretLaunchPosition;
        float launchVelocity = TargetHudController.Instance.PlayerShip.AverageTurretLaunchVelocity;
        Vector3 targetPos = target.transform.position;
        Vector3 targetVelocity = target.LinearVelocity;
        Vector3 playerVelocity = TargetHudController.Instance.PlayerShip.LinearVelocity;

        return LeadBox.MakeLead(launchPoint, playerVelocity + (targetPos - launchPoint).normalized * launchVelocity,
            targetPos,
            targetVelocity);
    }

    private void ActionAttack()
    {
        if (m_SpaceShip.SelectedTarget != null)
        {
            m_SpaceShip.WorldAimPoint = ComputeLead(m_SpaceShip, m_SpaceShip.SelectedTarget);

            m_SpaceShip.FireAllTurrets();
        }
    }

    #endregion
    */
}
