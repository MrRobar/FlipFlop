using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    void Update()
    {
        transform.Translate(Vector3.left * 20 * Time.deltaTime);
    }
}
