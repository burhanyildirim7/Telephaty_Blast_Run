using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("DusmanOlusturma")]
    [SerializeField] private GameObject dusman;
    [SerializeField] private int dusmanSayisi;
    [SerializeField] private int dusmanCýkýsAcisi;

    private WaitForSeconds beklemeSuresi1 = new WaitForSeconds(.015f);


    void Start()
    {
        StartCoroutine(DusmanOlustur());
    }


    IEnumerator DusmanOlustur()
    {
        for (int i = 0; i < dusmanSayisi; i++)
        {
            Instantiate(dusman, transform.position + Vector3.right * Random.Range(-3.0f, 3.0f) + Vector3.forward * (Random.Range(-3.0f, 3.0f)), Quaternion.Euler(Vector3.up * dusmanCýkýsAcisi));
            yield return beklemeSuresi1;
        }
    }

}
