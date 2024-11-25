using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : Singleton<PlayerManager>
{
    public GameObject player;

    [Header("Health")]
    public int currentHealth;
    public int maxHealth;
    public int pvPerSecond;
    public int armor;

    [Header("XP")]
    public int level;
    public float currentXP;
    public float XPToLevelUp;
    private float baseXp;
    public float requiredXpMultiplier;
    public int remainingLevel = 0;

    [Header("BaseStats")]
    public float baseDamage = 10f;
    public float moveSpeed = 5f;

    [Header("Boosts")]
    public float cooldownReduction;
    public float damageBoost;
    public float collectRange;
    public float speedBoost;
    public float xpBoost;

    public GameObject dieScreen;

    void Start()
    {
        currentHealth = maxHealth;
        level = 0;
        baseXp = XPToLevelUp;
        UIManager.Instance.UpdateHealthIndicator();
        StartCoroutine(HealthRegen());
        GameManager.Instance.sceneLoaded += Reload;
    }

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void GainXP(float amount)
    {
        amount *= 1 + (xpBoost/100);
        currentXP += amount;
        while(currentXP >= XPToLevelUp)
        {
            LevelUpPlayer();
        }
        UIManager.Instance.UpdateXPBar();
    }

    public int CalculateDamage(float damage = float.NaN, float multiplier = float.NaN)
    {
        if (float.IsNaN(damage))
        {
            damage = baseDamage;
        }

        if(float.IsNaN(multiplier))
        {
            multiplier = 1.0f;
        }

        return Mathf.CeilToInt(damage * multiplier * (1 + (damageBoost / 100)));
    }

    public void LevelUpPlayer()
    {
        currentXP -= XPToLevelUp;
        level++;
        remainingLevel++;
        UIManager.Instance.DisplayUpgradeMenu();
        XPToLevelUp = baseXp * Mathf.Pow(requiredXpMultiplier, level - 1);
    }

    public void IncreaseMaxHealth(int percentage)
    {
        int healthDifference = Mathf.CeilToInt(maxHealth * (percentage / 100.0f));
        maxHealth += healthDifference;
        currentHealth += healthDifference;
        UIManager.Instance.UpdateHealthIndicator();
    }

    public void Heal(int amount)
    {
        if(currentHealth+amount <= maxHealth)
        {
            currentHealth += amount;
        }
        else
        {
            currentHealth = maxHealth;
        }

        UIManager.Instance.UpdateHealthIndicator();
    }

    public IEnumerator HealthRegen()
    {
        while (true)
        {
            Heal(pvPerSecond);
            yield return new WaitForSeconds(1);
        }
    }

    public void TakeDamage(int amount)
    {
        amount = Mathf.FloorToInt(amount * ( 1 - ((float)armor/100.0f)));
        currentHealth -= amount;
        UIManager.Instance.UpdateHealthIndicator();
        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        dieScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public override void Reload()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
