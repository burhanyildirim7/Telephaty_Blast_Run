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
    private Quaternion mevcutKosuYonu;
    private Quaternion kosuYonu = Quaternion.Euler(Vector3.zero);
    private float donusHizi = 1000;

    [Header("AnimasyonAyarlari")]
    private Animator anim;
    private bool OyunBasladiMi = false;

    [Header("Efektler")]
    [SerializeField] private ParticleSystem olumEfekti;

    [Header("RotasyonAyari")]
    RaycastHit hit;

    private GameObject player;

    private WaitForSeconds beklemeSuresi1 = new WaitForSeconds(.2f);
    private WaitForSeconds beklemeSuresi2 = new WaitForSeconds(.5f);
    private WaitForSeconds beklemeSuresi3 = new WaitForSeconds(2f);


    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");

        StartCoroutine(OyunBasladiMiKontrol());
        StartCoroutine(KaraktereMesafe());

    }

    void Update()
    {
        if (!karaktereKosu && GameController.instance.isContinue)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * hiz);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, mevcutKosuYonu, donusHizi * Time.deltaTime);
        }
        else if (karaktereKosu && GameController.instance.isContinue)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * hiz);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, mevcutKosuYonu, 1000 * Time.deltaTime);
        }

    }


    //Dusman karakterden uzaklastigi zamanda yapacak rotasyonu burdadir
    IEnumerator KaraktereMesafe()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= odaklanmaMesafesiPlayer)
            {
                mevcutKosuYonu = Quaternion.LookRotation(player.transform.position - transform.position);
                karaktereKosu = true;
            }
            else
            {
                mevcutKosuYonu = kosuYonu;
                karaktereKosu = false;
            }
            yield return beklemeSuresi1;
        }
    }

    IEnumerator OyunBasladiMiKontrol()
    {
        while (!OyunBasladiMi)
        {
            if (GameController.instance.isContinue)
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
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("DonusYap"))
        {
            DonusAyarlayici();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("FirlatmaNesnesi"))
        {
            GameController.instance.SetScore(5);
            Instantiate(olumEfekti, transform.position, Quaternion.identity).Play();
            Destroy(gameObject);
        }

    }


    private void DonusAyarlayici()
    {
        float uzaklik;

        int layerMask = 1 << 2;

        layerMask = ~layerMask;

        if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(Vector3.right), out hit, Mathf.Infinity)) //SagaDonus
        {
            if (hit.transform.CompareTag("DonusAyarlatici"))
            {
                kosuYonu = Quaternion.Euler(Vector3.up * (transform.rotation.eulerAngles.y + 90));
                uzaklik = Vector3.Distance(transform.position, hit.point);
                StartCoroutine(DonusHiziBelirle(uzaklik));
            }
        }

        if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(-Vector3.right), out hit, Mathf.Infinity, layerMask)) //SolaDonus
        {
            if (hit.transform.CompareTag("DonusAyarlatici"))
            {
                kosuYonu = Quaternion.Euler(Vector3.up * (transform.rotation.eulerAngles.y - 90));
                uzaklik = Vector3.Distance(transform.position, hit.point);
                StartCoroutine(DonusHiziBelirle(uzaklik));
            }
        }
    }




    IEnumerator DonusHiziBelirle(float uzaklikDegeri)
    {

        donusHizi = 45 + 15 * (8 - uzaklikDegeri);
        if((8 - uzaklikDegeri) <= 1)
        {
            donusHizi += 60;
        }
        if ((8 - uzaklikDegeri) <= 2)
        {
            donusHizi += 35;
        }



        yield return beklemeSuresi3;
        donusHizi = 1500;
    }
}
