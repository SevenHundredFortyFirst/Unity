using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float m_Velocity;
    public float Velocity => m_Velocity;

    [SerializeField] private float m_Lifetime;

    [SerializeField] private float m_Damage;

    [SerializeField] private ImpactEffect m_ImpactEffectPrefab;
    [SerializeField] private float m_ExplosionRadius;

    private float m_LifeTimer;

    private void OnEnable()
    {
        m_LifeTimer = 0;
        AimTarget = null;
    }

    private void Update()
    {
        if (m_LifeTimer > m_Lifetime)
            return;

        AutoAim();

        float len = m_Velocity * Time.deltaTime;
        Vector3 step = transform.forward * len;
        

       


            RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, len))
        {
            m_LifeTimer = m_Lifetime;
            transform.position = hit.point;

            if (m_ImpactEffectPrefab != null)
            {

                var effect = PoolManager.Instance.Spawn(m_ImpactEffectPrefab.gameObject);
                effect.transform.position = hit.point;
            }
            if (m_ExplosionRadius > 0)
            {
                var colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius);
                if (colliders != null || colliders.Length != 0)
                {
                    foreach (var v in colliders)
                    {
                        var destructible = v.transform.root.GetComponent<Destructible>();

                        if (destructible)
                        {
                            destructible.ApplyDamage(m_Damage);
                        }
                    }
                }
                
            }
            else
            {
                var destructible = hit.collider.transform.root.GetComponent<Destructible>();

                if (destructible)
                {
                    destructible.ApplyDamage(m_Damage);
                }

                OnLifetimeEnd();

                return;
            }
        }
        transform.position += step;

        m_LifeTimer += Time.deltaTime;

        if (m_LifeTimer > m_Lifetime)
            OnLifetimeEnd();
    }

    private void OnLifetimeEnd()
    {
        PoolManager.Instance.Unspawn(gameObject);
    }

    public Destructible AimTarget { get; set; }

    [SerializeField] private float m_AimSpeed; // radians/s

    private void AutoAim()
    {
        if (AimTarget == null)
            return;

        if (m_AimSpeed > 0)
        {
            Vector3 dir = (AimTarget.transform.position - transform.position).normalized;

            transform.forward = Vector3.RotateTowards(transform.forward, dir, m_AimSpeed * Time.deltaTime, 1);
        }
    }
}
