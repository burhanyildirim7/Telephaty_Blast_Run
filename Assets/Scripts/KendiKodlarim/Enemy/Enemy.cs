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
    private Quaternion kosuYonu;
    private float donusHizi = 1000;
    private float engellerdenKacmaAyari;

    [Header("AnimasyonAyarlari")]
    private Animator anim;

    [Header("Efektler")]
    [SerializeField] private ParticleSystem olumEfekti;

    [Header("RotasyonAyari")]
    RaycastHit hit;   //Rotasyon ayari 
    RaycastHit hit1;// Engelden kacma ayari
    RaycastHit hit2;// Engelden kacma ayari

    [Header("KosmayaBaslamaAyari")]
    private bool oyunBasladiMi;
    private float oncekiMesafe;
    private float simdikiMesafe;

    private GameObject player;

    private WaitForSeconds beklemeSuresi1 = new WaitForSeconds(.2f);
    private WaitForSeconds beklemeSuresi2 = new WaitForSeconds(.5f);
    private WaitForSeconds beklemeSuresi3 = new WaitForSeconds(2f);


    void Start()
    {
        BaslangicDegerleri();
        StartCoroutine(EngelYanindanGec());
    }

    private void BaslangicDegerleri()
    {
        oyunBasladiMi = false;
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        kosuYonu = Quaternion.Euler(Vector3.up * transform.rotation.eulerAngles.y);

        StartCoroutine(KarakterPesindenKosmaAyari());
    }


    //Rotasyon ve haraket islemleri
    void Update()
    {
        if (!karaktereKosu && oyunBasladiMi)
        {
            transform.Translate((Vector3.forward + Vector3.right * engellerdenKacmaAyari) * Time.deltaTime * hiz);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, mevcutKosuYonu, donusHizi * Time.deltaTime);
        }
        else if (karaktereKosu && oyunBasladiMi)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * hiz);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, mevcutKosuYonu, 1000 * Time.deltaTime);
        }
    }


    IEnumerator EngelYanindanGec()
    {
        while(true)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit1, 10))//Layer kullanýlýyorsa sontrafa virgül koyulup layerMask yazýlmaýlýdr.
            {
                if(hit1.transform.CompareTag("FirlatilabilirNesne") || hit1.transform.CompareTag("Nesne"))
                {
                    if(Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up * 2), out hit2, Mathf.Infinity))
                    {
                        if(hit2.transform.CompareTag("Zemin"))
                        {
                            if(hit2.transform.position.x > transform.position.x)
                            {
                                engellerdenKacmaAyari = .4f;
                            }
                            else
                            {
                                engellerdenKacmaAyari = -.4f;
                            }
                        }
                    }
                }
            }
            else
            {
                engellerdenKacmaAyari = 0;
            }

            yield return beklemeSuresi1;
        }
    }

    IEnumerator KarakterPesindenKosmaAyari()
    {
        yield return beklemeSuresi2;

        while (!oyunBasladiMi)
        {
            oncekiMesafe = Vector3.Distance(transform.position, player.transform.position);
            yield return beklemeSuresi2;
            simdikiMesafe = Vector3.Distance(transform.position, player.transform.position);

            if (Vector3.Distance(transform.position, player.transform.position) <= 40)
            {
                if((simdikiMesafe - .75f) > oncekiMesafe)
                {
                    oyunBasladiMi = true;
                    anim.SetBool("KosmaP", true);
                    StartCoroutine(KaraktereMesafe());
                    StartCoroutine(OyunBittiMiKontrol());
                }
            }
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

   

  

    //Donus ayarlari
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


    //Carpisma ayarlari
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
            Instantiate(olumEfekti, transform.position + Vector3.up * .28f, Quaternion.identity).Play();
            Destroy(gameObject);
        }
    }
}
