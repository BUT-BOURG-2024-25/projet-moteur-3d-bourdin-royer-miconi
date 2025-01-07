using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownReduction : Power
{
    [Header("Power Specific")]
    public float cooldownPercentage = 0f;
    public float cooldownIncrement;

    void Start()
    {
    }

    void Update()
    {
        
    }

    public override void LevelUp()
    {
        base.LevelUp();
        switch (level)
        {
            case > 0 :
                cooldownPercentage += cooldownIncrement;
                UIManager.Instance.shouldShowNextMenu = true;
                break;


        }
        PlayerManager.Instance.cooldownReduction = cooldownPercentage;
    }
}
