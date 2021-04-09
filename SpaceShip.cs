using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpaceShip : Destructible
{
    [Header("Space ship")]
    [SerializeField] private float m_ThrustForce;
    [SerializeField] private float m_TorqueForce;

    [SerializeField] private float m_MaxLinearVelocity;
    [SerializeField] private float m_MaxAngularVelocity;

    [SerializeField] private SpaceShipParameters m_SpaceShipParameters;

    [SerializeField] private Turret[] m_Turrets;

    [SerializeField] private Transform m_ThirdPersonCameraPoint;
    public Transform ThirdPersonCameraPoint => m_ThirdPersonCameraPoint;

    public Rigidbody m_Rigid;

    public Vector3 ControlThrust { get; set; }
    public Vector3 ControlTorque { get; set; }

    public Destructible SelectedTarget { get; set; }

    public Vector3 WorldAimPoint
    {
        set
        {
            foreach (var v in m_Turrets)
                v.WorldAimPoint = value;
        }
    }

    override protected void Start()
    {
        base.Start();

        m_Rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        UpdateRigidbody();
    }

    public void FireAllTurrets()
    {
        foreach (var t in m_Turrets)
            t.Fire();
    }

    private void UpdateRigidbody()
    {
        m_Rigid.AddRelativeForce(Time.fixedDeltaTime * m_ThrustForce * ControlThrust, ForceMode.Force);

        float DragCoeff = m_ThrustForce / m_MaxLinearVelocity;
        m_Rigid.AddForce(-m_Rigid.velocity * DragCoeff * Time.fixedDeltaTime, ForceMode.Force);

        m_Rigid.AddRelativeTorque(Time.fixedDeltaTime * m_TorqueForce * ControlTorque, ForceMode.Force);

        // angular velocity limit
        var W = m_Rigid.angularVelocity;

        W.x = Mathf.Clamp(W.x, -m_MaxAngularVelocity, m_MaxAngularVelocity);
        W.y = Mathf.Clamp(W.y, -m_MaxAngularVelocity, m_MaxAngularVelocity);
        W.z = Mathf.Clamp(W.z, -m_MaxAngularVelocity, m_MaxAngularVelocity);

        m_Rigid.angularVelocity = W;
    }

    

    public override Vector3 LinearVelocity => m_Rigid.velocity;

    public override Vector3 AverageTurretLaunchPosition
    {
        get
        {
            if (m_Turrets == null || m_Turrets.Length == 0)
                return transform.position;

            Vector3 pos = Vector3.zero;

            foreach(var v in m_Turrets)
            {
                pos += v.LaunchPosition;
            }

            return pos / m_Turrets.Length;
        }
    }

    public override float AverageTurretLaunchVelocity
    {
        get
        {
            if (m_Turrets == null || m_Turrets.Length == 0)
                return 0;

            float vel = 0;

            foreach (var v in m_Turrets)
            {
                vel += v.ProjectilePrefab.Velocity;
            }

            return vel / m_Turrets.Length;
        }
    }
}
