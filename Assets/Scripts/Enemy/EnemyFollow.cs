using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyFollow : MonoBehaviour
{

    public Transform target;
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    public int difficulty;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindWithTag("Player").transform;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            
        }
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);

        Vector3 pos = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        rb.MovePosition(pos);

        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime));
    }
}
