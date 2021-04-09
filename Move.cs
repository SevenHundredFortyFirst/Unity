using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] int MoveSpeed;
    [SerializeField] private Rigidbody rigid;
    [SerializeField] public bool m_IsWorking = true;
    bool isThrusting;
    void Update()
    {
        if (!m_IsWorking)
            return;
        if (Input.GetKey(KeyCode.W))
        {

            
            /* rigid.AddForce(new Vector3(1, 0, 0) * MoveSpeed);
            isThrusting = true;*/
            rigid.AddForce(transform.right * MoveSpeed * Time.deltaTime);
            isThrusting = true;
        }
        if (Input.GetKey(KeyCode.S))
        {


            rigid.AddForce(transform.right * -MoveSpeed * Time.deltaTime);
            isThrusting = true;
        }
        if (Input.GetKey(KeyCode.A))
        {

            transform.Rotate(new Vector3(0, -3, 0), MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {

            transform.Rotate(new Vector3(0, 3, 0), MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightAlt))
        {
            rigid.AddForce(transform.up * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.C))
        {
            rigid.AddForce(transform.up * -MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rigid.AddForce(transform.right * MoveSpeed* 2 * Time.deltaTime);
            isThrusting = true;
        }
    }

}
