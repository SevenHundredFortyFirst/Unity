using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform m_LaunchPoint;
    public Vector3 LaunchPosition => m_LaunchPoint.position;

    [SerializeField] private Projectile m_ProjectilePrefab;
    public Projectile ProjectilePrefab => m_ProjectilePrefab;

    [SerializeField] private float m_RateOfFire;

    private float m_RefireTime;

    public Vector3 WorldAimPoint { get; set; }

    public void Fire()
    {
        if (m_RefireTime > 0)
            return;

        m_RefireTime = m_RateOfFire;

        LaunchProjectile();
    }

    private void LaunchProjectile()
    {
        var projectile = PoolManager.Instance.Spawn(m_ProjectilePrefab.gameObject);

        projectile.transform.position = m_LaunchPoint.position;

        projectile.transform.forward = (WorldAimPoint - m_LaunchPoint.position).normalized;
        //projectile.transform.rotation = m_LaunchPoint.rotation;

        projectile.GetComponent<Projectile>().AimTarget = transform.root.GetComponent<SpaceShip>().SelectedTarget;

        // Add SFX
        // Add muzzle
        // Reduce ammo
    }

    private void Update()
    {
        if (m_RefireTime > 0)
            m_RefireTime -= Time.deltaTime;
    }
}
