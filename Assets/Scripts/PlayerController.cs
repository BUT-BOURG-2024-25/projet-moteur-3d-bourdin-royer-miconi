using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public Joystick joystick;
    public float rotationSpeed = 5f;

    [Header("Physics")]
    public Rigidbody rb;

    void Start()
    {
        moveSpeed = PlayerManager.Instance.moveSpeed;
        rb = GetComponent<Rigidbody>();
        rb.constraints =  RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        joystick = UIManager.Instance.joystick;
    }

    void Update()
    {
        Vector3 move = InputManager.Instance.movementInput.normalized;
        Vector3 joystickDirection = Vector3.zero;
        Vector3 inputDirection = Vector3.zero;

        joystickDirection = new Vector3(joystick.Direction.x, 0, joystick.Direction.y);
        inputDirection = new Vector3(move.x, 0, move.z);

        if (inputDirection != Vector3.zero)
        {
            Vector3 movement = inputDirection * (moveSpeed * (1 + (PlayerManager.Instance.speedBoost / 100)) );
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        }
        else if(joystickDirection != Vector3.zero)
        {
            Vector3 movement = joystickDirection * (moveSpeed * (1 + (PlayerManager.Instance.speedBoost / 100)));
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

}
