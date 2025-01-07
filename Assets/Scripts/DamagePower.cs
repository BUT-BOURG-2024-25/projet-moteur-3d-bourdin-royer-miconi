using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePower : Power
{
    [Header("Power Specific")]
    public int currentDamageBoost = 0;
    [Range(0, 100), Tooltip("How many percent to increase damage boost each level")]
    public int damageIncrease;

    public override void LevelUp()
    {
        base.LevelUp();
        switch (level)
        {
            case > 0:
                currentDamageBoost += damageIncrease;
                UIManager.Instance.shouldShowNextMenu = true;
                break;

        }
        PlayerManager.Instance.damageBoost = currentDamageBoost;
    }
}
