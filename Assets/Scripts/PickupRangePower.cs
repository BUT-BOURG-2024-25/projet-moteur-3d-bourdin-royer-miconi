using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRangePower : Power
{ 
    [Header("Power Specific")]
    public float pickupRange = 2f;
    [Range(0f, 100f)]
    public float rangeMultiplier;

    // Start is called before the first frame update
    void Start()
    {
    }


    public override void LevelUp()
    {
        base.LevelUp();
        switch (level)
        {
            case > 0:
                pickupRange *= (1 + (rangeMultiplier/100));
                UIManager.Instance.shouldShowNextMenu = true;
                break;


        }
        PlayerManager.Instance.collectRange = pickupRange;
    }
}
