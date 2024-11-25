using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePositionByAxis : MonoBehaviour
{
    public float speed = 0.02f;

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(horizontal, 0, vertical);
        gameObject.transform.position += move * speed;
    }
}
