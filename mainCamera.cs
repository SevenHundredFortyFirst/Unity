using UnityEngine;
using System.Collections;

public class mainCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    public float rotSpeed = 1.5f; //скорость вращения
    public float zoomSpeed = 5.0f; //скорость приближения камеры
    private float _rotY, _rotX; //углы по x и y
    private Vector3 _offset; //смещение камеры относительно объекта


    // Use this for initialization
    void Start()
    {
        _rotY = transform.eulerAngles.y;
        _rotX = transform.eulerAngles.x;
        _offset = target.position - transform.position; //получает начальное смещение
    }

    void LateUpdate()
    {
        float input = Input.GetAxis("Mouse ScrollWheel");
        if (input != 0) //если крутится колесико мыши
        {
            Camera.main.fieldOfView += input * zoomSpeed; //зум

            //сперва хотел менять смещение, что то криво вышло
            //_offset.x += 10 * input;
            //_offset.y += 10 * input;
        }

        _rotY -= Input.GetAxis("Mouse X") * -rotSpeed; //поворот камеры вокруг объекта
        _rotX += Input.GetAxis("Mouse Y") * -rotSpeed;

        if (_rotY != 0 || _rotX != 0)
        {
            Quaternion rotation = Quaternion.Euler(_rotX, _rotY, 0); //задает вращение камеры
            transform.position = target.position - (rotation * _offset);
            transform.LookAt(target);
        }
    }
}