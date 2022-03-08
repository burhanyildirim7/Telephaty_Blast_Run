using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("KosuHizAyarlari")]
    [SerializeField] private float hiz;
    [SerializeField] private float donusHizi;
    private float baslangicHizi;
    [SerializeField] private float odaklanmaMesafesiPlayer;

    [Header("KosuGenelAyarlari")]
    private bool karaktereKosu = false;
    private Quaternion mevcutKosuYonu;
    private Quaternion kosuYonu;
    private float donusAcisalHizi = 1000;

    [Header("KosarkenKacmaAyarlari")]
    private float engellerdenKacmaAyari;
    private KarakterPaketiMovement karakterPaketiMovement;
    private bool karakterSagaGidiyor = false;
    private bool karakterSolaGidiyor = false;

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
    private bool kosmayiSonlandir = false;
    private EnemySpawn enemySpawn;

    private GameObject player;

    private WaitForSeconds beklemeSuresi0 = new WaitForSeconds(.1f);
    private WaitForSeconds beklemeSuresi1 = new WaitForSeconds(.2f);
    private WaitForSeconds beklemeSuresi2 = new WaitForSeconds(.5f);
    private WaitForSeconds beklemeSuresi3 = new WaitForSeconds(2f);


    void Start()
    {
        BaslangicDegerleri();
        StartCoroutine(EngelYanindanGec());
        StartCoroutine(DusmanlarinKosuAyarlari());
        baslangicHizi = hiz;
    }



    private void BaslangicDegerleri()
    {
        oyunBasladiMi = false;
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player");
        kosuYonu = Quaternion.Euler(Vector3.up * transform.rotation.eulerAngles.y);
        enemySpawn = transform.parent.GetComponent<EnemySpawn>();


        if (transform.rotation.eulerAngles.y == 270)
        {
            karakterSolaGidiyor = true;
        }
        else if (transform.rotation.eulerAngles.y == 90)
        {
            karakterSagaGidiyor = true;
        }
    }


    //Rotasyon ve haraket islemleri
    void Update()
    {
        if (!karaktereKosu && oyunBasladiMi)
        {
            transform.Translate((Vector3.forward + Vector3.right * engellerdenKacmaAyari) * Time.deltaTime * hiz);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, mevcutKosuYonu, donusAcisalHizi * Time.deltaTime);
        }
        else if (karaktereKosu && oyunBasladiMi)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * hiz);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, mevcutKosuYonu, 1000 * Time.deltaTime);
        }
    }


    IEnumerator EngelYanindanGec()
    {
        
        while (true)
        {
            if (Physics.Raycast(transform.position + Vector3.up * .25f, transform.TransformDirection(Vector3.forward), out hit1, 50))
            {

                if (hit1.transform.CompareTag("FirlatilabilirNesne") || hit1.transform.CompareTag("Nesne"))
                {
                    if (Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up * 2), out hit2, 3))
                    {
                       
                        if (!karakterSagaGidiyor && !karakterSolaGidiyor)
                        {
                            
                            if (hit2.transform.CompareTag("Zemin"))
                            {
                              
                                if (hit2.transform.position.x > transform.position.x)
                                {
                                    
                                    engellerdenKacmaAyari = .3f + (hit2.transform.position.x - transform.position.x) / 20;
                                }
                                else
                                {
                                    
                                    engellerdenKacmaAyari = -.3f + (hit2.transform.position.x - transform.position.x) / 20;
                                }
                            }
                        }
                        else if (karakterSolaGidiyor)
                        {
                            if (hit2.transform.position.z > transform.position.z)
                            {
                                engellerdenKacmaAyari = .3f + (hit2.transform.position.z - transform.position.z) / 15;
                            }
                            else
                            {
                                engellerdenKacmaAyari = -.3f + (hit2.transform.position.z - transform.position.z) / 15;
                            }
                        }
                        else if (karakterSagaGidiyor)
                        {
                            if (hit2.transform.position.z > transform.position.z)
                            {
                                engellerdenKacmaAyari = -.3f + (hit2.transform.position.z - transform.position.z) / 15;
                            }
                            else
                            {
                                engellerdenKacmaAyari = +.3f + (hit2.transform.position.z - transform.position.z) / 15;
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


    IEnumerator DusmanlarinKosuAyarlari()
    {
        if (enemySpawn.DusmanlarKossunMu)
        {
            oyunBasladiMi = true;
            anim.SetBool("KosmaP", true);
            StartCoroutine(KaraktereMesafe());
            StartCoroutine(OyunBittiMiKontrol());
        }


        while (!oyunBasladiMi)
        {
            if (enemySpawn.DusmanlarKossunMu)
            {
                oyunBasladiMi = true;
                anim.SetBool("KosmaP", true);
                StartCoroutine(KaraktereMesafe());
                StartCoroutine(OyunBittiMiKontrol());
            }
            yield return beklemeSuresi1;
        }
    }


    //Dusman karakterden uzaklastigi zamanda yapacak rotasyonu burdadir
    IEnumerator KaraktereMesafe()
    {
        while (!kosmayiSonlandir)
        {
            if (Vector3.Distance(transform.position, player.transform.position) >= 35)
            {
                donusAcisalHizi += Random.Range(20, 50);
                hiz = 15;
            }
            else if (Vector3.Distance(transform.position, player.transform.position) >= 25)
            {
                donusAcisalHizi += Random.Range(10, 25);
                hiz = 10;
            }
            else if (Vector3.Distance(transform.position, player.transform.position) >= 15)
            {
                donusAcisalHizi += Random.Range(5, 10);
                hiz = 8;
            }
            else
            {
                hiz = 6f;
            }

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
            yield return beklemeSuresi0;
        }
    }


    IEnumerator OyunBittiMiKontrol()
    {
        while (anim.GetBool("KosmaP"))
        {
            if (!GameController.instance.isContinue)
            {
                kosmayiSonlandir = true;
                hiz = 0;
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
                karakterSagaGidiyor = true;
                kosuYonu = Quaternion.Euler(Vector3.up * (transform.rotation.eulerAngles.y + 90));
                uzaklik = Vector3.Distance(transform.position, hit.point);
                StartCoroutine(DonusHiziBelirle(uzaklik));
            }
        }

        if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(-Vector3.right), out hit, Mathf.Infinity, layerMask)) //SolaDonus
        {
            if (hit.transform.CompareTag("DonusAyarlatici"))
            {
                karakterSolaGidiyor = true;
                kosuYonu = Quaternion.Euler(Vector3.up * (transform.rotation.eulerAngles.y - 90));
                uzaklik = Vector3.Distance(transform.position, hit.point);
                StartCoroutine(DonusHiziBelirle(uzaklik));
            }
        }
    }

    IEnumerator DonusHiziBelirle(float uzaklikDegeri)
    {

        donusAcisalHizi = 45 + 15 * (8 - uzaklikDegeri);
        if ((8 - uzaklikDegeri) <= 1)
        {
            donusAcisalHizi += 60;
        }
        if ((8 - uzaklikDegeri) <= 2)
        {
            donusAcisalHizi += 35;
        }



        yield return beklemeSuresi3;
        donusAcisalHizi = 1500;
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
        else if (other.CompareTag("DonusHizAzaltici"))
        {
            DonusAyarlayici();
        }
    }

    IEnumerator HizAzaltici()
    {
        hiz = donusHizi;
        yield return new WaitForSeconds(3);
        hiz = baslangicHizi;

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("FirlatmaNesnesi"))
        {
            GameController.instance.SetScore(1);
            Instantiate(olumEfekti, transform.position + Vector3.up * .28f, Quaternion.identity).Play();
            Destroy(gameObject);
        }
    }
}
