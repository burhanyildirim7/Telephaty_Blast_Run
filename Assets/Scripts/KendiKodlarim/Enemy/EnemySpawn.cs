using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("DusmanOlusturma")]
    [SerializeField] private GameObject dusman;
    [SerializeField] private int dusmanSayisi;

    private WaitForSeconds beklemeSuresi1 = new WaitForSeconds(.015f);


    void Start()
    {
        StartCoroutine(DusmanOlustur());
        
    }


    IEnumerator DusmanOlustur()
    {
        for (int i = 0; i < dusmanSayisi; i++)
        {
            Instantiate(dusman, transform.position + Vector3.right * Random.Range(-3, 3) + Vector3.forward * (Random.Range(-3, 3)), Quaternion.identity);
            yield return beklemeSuresi1;
        }
    }


    void Update()
    {


    }
}
