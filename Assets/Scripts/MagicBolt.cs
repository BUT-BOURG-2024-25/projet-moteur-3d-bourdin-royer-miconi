using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MagicBolt : Power
{
    public float speed = 5f;
    public float damageIncrement = 1.1f;
    public float cooldownDecrement = 0.05f;

    void Start()
    {
        LevelUp();
    }

    void Update()
    {
        
    }

    public override IEnumerator Cast()
    {
        GameObject[] targets;
        while (true)
        {
            targets = GetClosestEnemy(targetsNumber);

            foreach (var target in targets)
            {
                GameObject projectile = Instantiate(prefab, castSource.transform.position, Quaternion.identity);
                Vector3 direction = (target.transform.position - castSource.transform.position).normalized;
                projectile.GetComponent<Rigidbody>().velocity = direction * speed;

                projectile.transform.LookAt(target.transform.position);
                projectile.transform.Rotate(90, 0, 0);

                projectile.AddComponent<ProjectileBehaviour>().Init(PlayerManager.Instance.CalculateDamage(multiplier: damageMultiplier));
                Destroy(projectile,lifetime);
            }
            yield return new WaitForSeconds(cooldown * ( 1 - (PlayerManager.Instance.cooldownReduction/100)));
        }
        
    }

    [ContextMenu("LevelUp()")]
    public override void LevelUp()
    {
        List<PowerUpgradeButton> upgrades = new List<PowerUpgradeButton>();
        base.LevelUp();
        switch (level)
        {
            case 1:
                damageMultiplier = 1f;
                StartCoroutine(Cast());
                UIManager.Instance.shouldShowNextMenu = true;
                break;

            case var n when n % 5 == 0:
                upgrades = new List<PowerUpgradeButton>
                {
                    new PowerUpgradeButton
                    {
                        text = "Damage x"+damageIncrement+" ("+PlayerManager.Instance.baseDamage*damageMultiplier+"→"+PlayerManager.Instance.baseDamage*(damageMultiplier*damageIncrement)+")",
                        callback = () =>
                        {
                            damageMultiplier *= damageIncrement;
                        }
                    },
                    new PowerUpgradeButton
                    {
                        text = "Cooldown -5% ("+cooldown+"→"+cooldown*(1-cooldownDecrement)+")",
                        callback = () =>
                        {
                            cooldown *= (float)(1 - cooldownDecrement);
                        }
                    },
                    new PowerUpgradeButton
                    {
                        text = "Number of projectiles : 2",
                        callback = () =>
                        {
                            targetsNumber = 2;
                        }
                    }
                };
                break;

            case > 1:
                upgrades = new List<PowerUpgradeButton>
                {
                    new PowerUpgradeButton
                    {
                        text = "Damage x"+damageIncrement+" ("+PlayerManager.Instance.baseDamage*damageMultiplier+"→"+PlayerManager.Instance.baseDamage*(damageMultiplier*damageIncrement)+")",
                        callback = () =>
                        {
                            damageMultiplier *= damageIncrement;
                        }
                    },
                    new PowerUpgradeButton
                    {
                        text = "Cooldown -5% ("+cooldown+"→"+cooldown*(1-cooldownDecrement)+")",
                        callback = () =>
                        {
                            cooldown *= (float)(1 - cooldownDecrement);
                        }
                    }
                };
                break;
        }

        if(level != 1)
        {
            UIManager.Instance.DisplayUpgradePowerMenu(this, upgrades);
        }
    }
}

public class ProjectileBehaviour : MonoBehaviour
{
    private float damage;
    public void Init(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().Damage(damage);
            Destroy(gameObject);
        }
    }
}
