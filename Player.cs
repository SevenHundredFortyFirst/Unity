using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private string m_Nickname;

    [SerializeField] private SpaceShip m_PlayerShip;
    public SpaceShip PlayerShip => m_PlayerShip;

    [SerializeField] private SpaceShip m_PlayerShipPrefab;


    [SerializeField] private ThirdPersonCamera m_ThirdPersonCamera;

    private void Start()
    {
        Instance = this;

        if(m_PlayerShip == null)
        {
            m_PlayerShip = Instantiate(m_PlayerShipPrefab.gameObject).GetComponent<SpaceShip>();

            TargetHudController.Instance.SetPlayerShip(m_PlayerShip);

            if(SceneSwitchController.TargetGate != null)
            {
                var allGates = FindObjectsOfType<Gate>();

                foreach(var v in allGates)
                {
                    if(v.name == SceneSwitchController.TargetGate)
                    {
                        m_PlayerShip.transform.position = v.transform.position + (v.GateRadius + 100.0f) * UnityEngine.Random.onUnitSphere;
                    }
                }
            }
        }

        m_ThirdPersonCamera.SetTargets(m_PlayerShip.ThirdPersonCameraPoint, m_PlayerShip.transform);
    }

    private void Update()
    {
        UpdateSpaceControls();
    }

    private void UpdateSpaceControls()
    {
        if (m_PlayerShip == null)
            return;

        Vector3 ControlThrust = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            ControlThrust += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            ControlThrust -= Vector3.forward;
        }

        if (Input.GetKey(KeyCode.D))
        {
            ControlThrust += Vector3.right;
        }

        if (Input.GetKey(KeyCode.A))
        {
            ControlThrust -= Vector3.right;
        }

        m_PlayerShip.ControlThrust = ControlThrust;

        Vector3 ControlTorque = Vector3.zero;

        if (Input.GetKey(KeyCode.Q))
        {
            ControlTorque += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.E))
        {
            ControlTorque -= Vector3.forward;
        }

        ControlTorque += NormalizedMousePosition;

        m_PlayerShip.ControlTorque = ControlTorque;

        ///
        if (Input.GetMouseButton(1))
        {
            m_PlayerShip.FireAllTurrets();
        }
    }

    public static Vector3 NormalizedMousePosition
    {
        get
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0;

            Vector3 halfScreen = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);

            mousePos -= halfScreen;

            mousePos.x /= halfScreen.x;
            mousePos.y /= halfScreen.y;

            return new Vector3(-mousePos.y, mousePos.x, 0);
        }
    }
}
