using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float movingSpeed;

    void FixedUpdate()
    {
        transform.position += Vector3.right * Time.deltaTime * movingSpeed;
    }
}
