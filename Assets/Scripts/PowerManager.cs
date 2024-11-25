using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : Singleton<PowerManager>
{

    public GameObject powerContainer;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (powerContainer == null && PlayerManager.Instance.player)
        {
            powerContainer = PlayerManager.Instance.player.transform.Find("PowerContainer").gameObject;
        }
    }

    public List<Power> GetNonMaxedPower()
    {
        List<Power> nonMaxedPowers = new List<Power>();

        foreach (Transform child in powerContainer.transform)
        {
            Power power = child.GetComponent<Power>();
            if (power.level < power.maxLevel)
            {
                nonMaxedPowers.Add(power);
            }
        }

        return nonMaxedPowers;
    }

    public override void Reload()
    {
    }
}
