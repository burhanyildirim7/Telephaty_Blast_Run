using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DusmanTetikleyici : MonoBehaviour
{
    [Header("DusmanCikarici")]
    private EnemySpawn enemySpawn;
    [SerializeField] private int dusmanSayisi;

    void Start()
    {
        enemySpawn = transform.parent.GetComponent<EnemySpawn>();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemySpawn.DusmanOlustur(dusmanSayisi);
        }
    }
}
