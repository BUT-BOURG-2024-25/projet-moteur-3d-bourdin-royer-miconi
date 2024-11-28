using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : Power
{
    public float satelliteRadius = 3f;
    public float satelliteSpeed = 10f;
    public float damageIncrement = 1.1f;
    public float speedIncrement = 0.1f;

    private List<GameObject> satellites = new List<GameObject>();

    void Start()
    {
        
    }

    void Update()
    {
        UpdateSatellites();
    }

    [ContextMenu("LevelUp()")]
    public override void LevelUp()
    {
        List<PowerUpgradeButton> upgrades = new List<PowerUpgradeButton>();
        base.LevelUp();
        switch (level)
        {
            case 1:
                CreateNewSatellite();
                damageMultiplier = 1f;
                satelliteSpeed = 1.5f;
                UIManager.Instance.shouldShowNextMenu = true;
                break;

            case var n when n % 4 == 0:
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
                        text = "Speed +"+ speedIncrement*100 +"% ("+satelliteSpeed+"→"+satelliteSpeed*(1 + speedIncrement)+")",
                        callback = () =>
                        {
                            satelliteSpeed *= (float)(1 + speedIncrement);
                        }
                    },
                    new PowerUpgradeButton
                    {
                        text = "New Satellite ("+satellites.Count+"→"+ (satellites.Count+1) +")",
                        callback = () =>
                        {
                            CreateNewSatellite();
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
                        text = "Speed +"+ speedIncrement*100 +"% ("+satelliteSpeed+"→"+satelliteSpeed*(1 + speedIncrement)+")",
                        callback = () =>
                        {
                            satelliteSpeed *= (float)(1 + speedIncrement);
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

    void CreateNewSatellite()
    {
        GameObject satellite = Instantiate(prefab, castSource.transform.position, Quaternion.identity);
        satellite.AddComponent<SatelliteBehaviour>();
        satellite.transform.parent = castSource.transform;
        satellites.Add(satellite);
    }

    void UpdateSatellites()
    {
        for (int i = 0; i < satellites.Count; i++)
        {
            float angle = satelliteSpeed * Time.time + (i * Mathf.PI * 2 / satellites.Count);
            float x = Mathf.Cos(angle) * satelliteRadius;
            float z = Mathf.Sin(angle) * satelliteRadius;
            satellites[i].transform.localPosition = new Vector3(x, 0, z);
        }
    }
}

public class SatelliteBehaviour : MonoBehaviour
{
    private Satellite parentSatellite;

    void Start()
    {
        parentSatellite = GetComponentInParent<Satellite>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null && parentSatellite != null)
            {
                enemy.Damage(PlayerManager.Instance.CalculateDamage(multiplier: parentSatellite.damageMultiplier));
            }
        }
    }
}
