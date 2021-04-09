using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawn : MonoBehaviour
{
    [SerializeField] private float m_Radius;

    [SerializeField] private SpaceShip m_SpaceShipPrefab;

    [SerializeField] private int m_NumBots;

    [SerializeField] private AIPointPatrol m_PatrolArea;

    [SerializeField] private int m_TeamId;

    [SerializeField] private bool m_AutoSpawnOnStart;

    private void Start()
    {
        if (m_AutoSpawnOnStart)
            SpawnBots();
    }

    public void SpawnBots()
    {
        for (int i = 0; i < m_NumBots; i++)
        {
            var bot = Instantiate(m_SpaceShipPrefab.gameObject);

            bot.transform.position = transform.position + UnityEngine.Random.insideUnitSphere * m_Radius;

            bot.GetComponent<SpaceShip>().SetTeamId(m_TeamId);

            bot.GetComponent<SpaceShipAIController>().SetPatrolBehaviour(m_PatrolArea);
        }
    }
}
