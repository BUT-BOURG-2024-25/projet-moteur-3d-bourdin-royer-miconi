using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : Power
{
    public float damageIncrement = 1.1f;
    public float cooldownDecrement = 0.05f;
    public float sizeMultiplier = 1f;

    void Start()
    {
    }

    void Update()
    {

    }

    public override IEnumerator Cast()
    {
        GameObject[] targets;
        while (true)
        {
            targets = GetClosestEnemy(1);

            foreach (var target in targets)
            {
                GameObject projectile = Instantiate(prefab,new Vector3(target.transform.position.x, target.transform.position.y + 10, target.transform.position.z), Quaternion.identity);

                projectile.AddComponent<MeteorBehaviour>().Init(PlayerManager.Instance.CalculateDamage(multiplier: damageMultiplier));
                projectile.transform.localScale = projectile.transform.localScale * sizeMultiplier;
                Destroy(projectile, lifetime);
            }
            yield return new WaitForSeconds(cooldown * (1 - (PlayerManager.Instance.cooldownReduction / 100)));
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
                    },
                    new PowerUpgradeButton
                    {
                        text = "Size of Metor +10%",
                        callback = () =>
                        {
                            sizeMultiplier *= 1.1f; ;
                        }
                    }
                };
                break;
        }

        if (level != 1)
        {
            UIManager.Instance.DisplayUpgradePowerMenu(this, upgrades);
        }
    }
}

public class MeteorBehaviour : MonoBehaviour
{
    private float damage;
    public void Init(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().Damage(damage);
        }
        Destroy(gameObject, 2f);
    }
}