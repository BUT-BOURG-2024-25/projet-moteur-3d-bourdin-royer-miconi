using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveByVelocity : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 200f;
    public float jumpCooldown = 1f;
    private bool canJump = true;
    public Joystick joystick;
    public bool useJoystick;
    public float rotationSpeed = 5f;

    [Header("Physics")]
    public Rigidbody rb;

    [Header("Animation")]
    public Animator animator;

    void Start()
    {
        gameObject.AddComponent<Rigidbody>();
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        InputManager.Instance.RegisterOnJumpAction(Jump, true);
    }

    void Update()
    {
        Vector3 move = InputManager.Instance.movementInput.normalized;
        Vector3 direction = Vector3.zero;

        if (useJoystick)
        {
            direction = new Vector3(joystick.Direction.x, 0, joystick.Direction.y);
        }
        else
        {
            direction = new Vector3(move.x, 0, move.z);
        }

        if (direction != Vector3.zero)
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;

            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 relativeDirection = (cameraForward * direction.z + cameraRight * direction.x).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(relativeDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            Vector3 movement = relativeDirection * moveSpeed;
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        animator.SetBool("IsGrounded", rb.velocity.y == 0);
        animator.SetFloat("Speed", rb.velocity.magnitude);
    }


    private void OnDestroy()
    {
        InputManager.Instance.RegisterOnJumpAction(Jump, false);
    }

    private IEnumerator JumpCooldown() {
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    private void Jump(InputAction.CallbackContext callbackContext)
    { 
        if (canJump)
        {
            canJump = false;
            rb.AddForce(Vector3.up * jumpForce);
            StartCoroutine(JumpCooldown());
        }
    }

}
