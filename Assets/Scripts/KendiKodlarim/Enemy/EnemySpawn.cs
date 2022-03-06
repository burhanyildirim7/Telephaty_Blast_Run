using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("DusmanOlusturma")]
    [SerializeField] private GameObject dusman;
    [SerializeField] private int dusmanSayisi;
    [SerializeField] private int dusmanCikisAcisi;

    public bool DusmanlarKossunMu = false;


    void Start()
    {
        GameObject obje;
        for (int i = 0; i < dusmanSayisi; i++)
        {
            obje = Instantiate(dusman, transform.position + Vector3.right * Random.Range(-3.5f, 3.5f) + Vector3.forward * (Random.Range(-3.8f, 3.8f)), Quaternion.Euler(Vector3.up * dusmanCikisAcisi));
            obje.transform.parent = transform;
        }
    }

    public void DusmanOlustur(int sayi)
    {
        DusmanlarKossunMu = true;
        GameObject obje;
        for (int i = 0; i < sayi; i++)
        {
            obje = Instantiate(dusman, transform.position + Vector3.right * Random.Range(-3.5f, 3.5f) + Vector3.forward * (Random.Range(-3.8f, 3.8f)), Quaternion.Euler(Vector3.up * dusmanCikisAcisi));
            obje.transform.parent = transform;
        }
    }
    

}
