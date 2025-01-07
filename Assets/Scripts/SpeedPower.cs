using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class SpeedPower : Power
{

    [Header("Power Specific")]
    public int currentSpeedBoost = 0;
    [Range(0, 100), Tooltip("How many percent to increase speed boost each level")]
    public int speedIncrease;

    public override void LevelUp()
    {
        base.LevelUp();
        switch (level)
        {
            case > 0:
                currentSpeedBoost += speedIncrease;
                UIManager.Instance.shouldShowNextMenu = true;
                break;


        }
        PlayerManager.Instance.speedBoost = currentSpeedBoost;
    }
}
