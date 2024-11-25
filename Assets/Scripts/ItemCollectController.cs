using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectController : MonoBehaviour
{
    private SphereCollider range;

    public float attractSpeed = 3f;
    public float collectDistance = 0.5f;

    void Start()
    {
        range = gameObject.GetComponent<SphereCollider>();
        StartCoroutine(UpdateRange());
    }

    public IEnumerator UpdateRange()
    {
        range.radius = PlayerManager.Instance.collectRange;
        yield return new WaitForSeconds(1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("XpOrb"))
        {
            StartCoroutine(AttractOrb(other.gameObject));
        }
    }

    private IEnumerator AttractOrb(GameObject orb)
    {
        XpOrbController orbController = orb.GetComponent<XpOrbController>();

        while (Vector3.Distance(orb.transform.position, transform.position) > collectDistance)
        {
            orb.transform.position = Vector3.MoveTowards(orb.transform.position, transform.position, attractSpeed * Time.deltaTime);
            yield return null;
        }

        PlayerManager.Instance.GainXP(orbController.xpValue);

        Destroy(orb);
    }
}
