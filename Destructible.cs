﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Destructible : MonoBehaviour
{
    [Header("Destructible")]
    [SerializeField] public float m_MaxHitPoints;

    [SerializeField] private ImpactEffect m_ExplosionPrefab;

    public float m_HitPoints;

    public float HitPoints => m_HitPoints;

    [SerializeField] private int m_TeamId;
    public int TeamId => m_TeamId;

    public void SetTeamId(int team) => m_TeamId = team;

    public const int TeamIdNeutral = 0;

    #region All destructibles

    private static HashSet<Destructible> m_AllDestructibles;

    public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructibles;

    protected virtual void OnEnable()
    {
        if (m_AllDestructibles == null)
            m_AllDestructibles = new HashSet<Destructible>();

        m_AllDestructibles.Add(this);
    }

    protected virtual void OnDisable()
    {
        if(m_AllDestructibles != null)
        {
            m_AllDestructibles.Remove(this);
        }


    }

    #endregion

    protected virtual void Start()
    {
        m_HitPoints = m_MaxHitPoints;

        SpawnTargetBox();
    }

    private TargetBox m_TargetBox;

    private void SpawnTargetBox()
    {
        m_TargetBox = TargetHudController.Instance.SpawnTargetBox(this);
    }

    private void UnspawnTargetBox()
    {
        Destroy(m_TargetBox.gameObject);
    }

    public void ApplyDamage(float damage)
    {
        m_HitPoints -= damage;

        if(m_HitPoints < 0)
        {
            Explode();
        }
    }

    protected virtual void Explode()
    {
        UnspawnTargetBox();

        if (m_ExplosionPrefab != null)
        {
            var exp = PoolManager.Instance.Spawn(m_ExplosionPrefab.gameObject);
            exp.transform.position = transform.position;
        }

        OnDestroyed?.Invoke();

        Destroy(gameObject);
    }

    [SerializeField] private UnityEvent m_OnDestroyed;
    public UnityEvent OnDestroyed => m_OnDestroyed;

    public virtual Vector3 LinearVelocity => Vector3.zero;
    public virtual Vector3 AverageTurretLaunchPosition => transform.position;
    public virtual float AverageTurretLaunchVelocity => 0;
}
