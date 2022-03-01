using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("KosuHizAyarlari")]
    [SerializeField] private float hiz;
    [SerializeField] private float odaklanmaMesafesiPlayer;

    [Header("KosuGenelAyarlari")]
    private bool karaktereKosu = false;
    Quaternion kosuYonu;

    [Header("AnimasyonAyarlari")]
    private Animator anim;
    private bool OyunBasladiMi = false;

    private GameObject player;

    private WaitForSeconds beklemeSuresi1 = new WaitForSeconds(.2f);
    private WaitForSeconds beklemeSuresi2 = new WaitForSeconds(.5f);
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");


        StartCoroutine(OyunBasladiMiKontrol());
        StartCoroutine(KaraktereMesafe());
    }

    void Update()
    {
        if(!karaktereKosu && GameController.instance.isContinue)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * hiz);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, kosuYonu, 1500 * Time.deltaTime);
        }
        else if(karaktereKosu && GameController.instance.isContinue)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * hiz);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, kosuYonu, 1000 * Time.deltaTime); 
        }

    }

    IEnumerator KaraktereMesafe()
    {
        while(true)
        {
            if(Vector3.Distance(transform.position, player.transform.position) <= odaklanmaMesafesiPlayer)
            {
                kosuYonu = Quaternion.LookRotation(player.transform.position - transform.position);
                karaktereKosu = true;
            }
            else
            {
                kosuYonu = Quaternion.Euler(Vector3.zero);
                karaktereKosu = false;
            }
            yield return beklemeSuresi1;
        }
    }

    IEnumerator OyunBasladiMiKontrol()
    {
        while(!OyunBasladiMi)
        {
            if(GameController.instance.isContinue)
            {
                anim.SetBool("KosmaP", true);
                OyunBasladiMi = true;
                StartCoroutine(OyunBittiMiKontrol());
            }
            yield return beklemeSuresi1;
        }
    }

    IEnumerator OyunBittiMiKontrol()
    {
        while (anim.GetBool("KosmaP"))
        {
            if (!GameController.instance.isContinue)
            {
                anim.SetBool("KosmaP", false);
            }
            yield return beklemeSuresi1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
