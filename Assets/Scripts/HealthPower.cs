using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class HealthPower : Power
{
    [Header("Power Specific")]
    [Range(0, 100), Tooltip("How many percent to increase health each level")]
    public int healthIncrease;

    public override void LevelUp()
    {
        base.LevelUp();
        switch (level)
        {
            case > 0:
                PlayerManager.Instance.IncreaseMaxHealth(healthIncrease);
                UIManager.Instance.shouldShowNextMenu = true;
                break;


        }
    }
}
