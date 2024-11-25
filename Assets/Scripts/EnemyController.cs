using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Stats")]
    public float health = 100f;
    public float xpValue = 10f;
    public float damage = 5f;
    public float damageCooldown = 1f;
    private bool canDamage = true;

    [Header("Follow")]
    public Transform target;
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    private Rigidbody rb;

    void Start()
    {
        EnemyManager.Instance.AddEnemyToList(gameObject);
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindWithTag("Player").transform;
    }

    private void OnDestroy()
    {
        EnemyManager.Instance.RemoveEnemyToList(gameObject);
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        Vector3 direction = (targetPosition - transform.position).normalized;

        rb.velocity = direction * moveSpeed;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && canDamage)
        {
            PlayerManager.Instance.TakeDamage(Mathf.FloorToInt(damage));
            StartCoroutine(DamageCooldown());
        }
    }

    private IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }

    public void Damage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        EnemyManager.Instance.DropXpOrb(gameObject.transform);
        EnemyManager.Instance.killedEnemies++;
        Destroy(gameObject);
    }

}
