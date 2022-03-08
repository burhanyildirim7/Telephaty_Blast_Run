using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finish : MonoBehaviour
{
    [Header("KapiAnimasyonuIcin")]
    [SerializeField] private GameObject[] kapilar;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(KapilariKapat());
        }
    }

    IEnumerator KapilariKapat()
    {
        yield return new WaitForSeconds(.75f);
        for (int i = 0; i < kapilar.Length; i++)
        {
            kapilar[i].GetComponent<Animation>().Play();
        }
    }
}
