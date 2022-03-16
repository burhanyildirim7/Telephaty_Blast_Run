using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarakterPaketiMovement : MonoBehaviour
{
    [Header("KarakterHiz")]
    public float _speed;
    private float baslangicHizi;

    [Header("PlayerRotasyon")] //ObjeKontrolEderken
    private Transform donecekObje;
    private Transform donulecekObje;
    public bool donme = false;
    private Quaternion donusYonu;

    [Header("KarakterRotasyonIcin")]
    RaycastHit hit;
    private Quaternion hedefRotasyon;

    [Header("KameraRotasyonu")]
    private CameraMovement cameraMovement;

    [Header("AtýsAyarlamakIcindir")]
    private ThrowController throwController;


    [Header("YonBilgisiIcindir")]   //Throw controller ve camera ayari icin gereklidir
    public bool karakterSagaGidiyor;
    public bool karakterSolaGidiyor;

    [SerializeField] private GameObject[] karakterler;

    [Header("OgreticiModIcinGerekli")]
    [SerializeField] private GameObject ogreticiCanvasi;



    void Start()
    {
        baslangicHizi = _speed;
        BaslangicDegerleri();
    }

    IEnumerator OgreticiMod()
    {
        yield return new WaitForSeconds(.25f);
        GameObject obje1 = GameObject.Find("Ogretici1");
        GameObject obje2 = GameObject.Find("Ogretici2");
        UIController uIController = GameObject.FindObjectOfType<UIController>();


        while (transform.position.z < (obje2.transform.position.z + 9))
        {
            if (Vector3.Distance(transform.position, obje1.transform.position) <= 4.75f)
            {
                if(!ogreticiCanvasi.activeSelf)
                {
                    ogreticiCanvasi.SetActive(true);
                    StartCoroutine(uIController.EliGonder(obje1.transform));
                }
                else
                {
                    Time.timeScale = Mathf.Lerp(Time.timeScale, .5f, Time.deltaTime * 3);
                }
            }
            else if(Vector3.Distance(transform.position, obje1.transform.position) <= 7)
            {
                Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, Time.deltaTime * 10);

                if (ogreticiCanvasi.activeSelf)
                {
                    ogreticiCanvasi.SetActive(false);
                }
            }



            if(Vector3.Distance(transform.position, obje2.transform.position) <= 4.75f)
            {
                if (!ogreticiCanvasi.activeSelf)
                {
                    ogreticiCanvasi.SetActive(true);
                    StartCoroutine(uIController.EliGonder(obje2.transform));
                }
                else
                {
                    Time.timeScale = Mathf.Lerp(Time.timeScale, .5f, Time.deltaTime * 3);
                }
            }
            else if (Vector3.Distance(transform.position, obje2.transform.position) <= 7)
            {
                Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, Time.deltaTime * 10);

                if (ogreticiCanvasi.activeSelf)
                {
                    ogreticiCanvasi.SetActive(false);
                }
            }

            yield return null;
        }
    }

    public void KarakterAktiflestir(int deger)
    {
        for (int i = 0; i < 3; i++)
        {
            karakterler[i].SetActive(false);
        }
        karakterler[deger].SetActive(true);
    }

    public void BaslangicDegerleri() //UIController
    {
        transform.GetChild(0).transform.gameObject.SetActive(true);
        _speed = baslangicHizi; //Oyun sonlarinda surekli olarak karakterin hizi artirildigi icin boyle bir seye ihtiyac duyulmustur
        throwController = GameObject.FindWithTag("ThrowController").GetComponent<ThrowController>();
        cameraMovement = GameObject.FindWithTag("MainCamera").GetComponent<CameraMovement>();
        donecekObje = transform.GetChild(0).transform;

        karakterSolaGidiyor = false;
        karakterSagaGidiyor = false;

        hedefRotasyon = Quaternion.Euler(Vector3.zero);

        if (PlayerPrefs.GetInt("level") == 0)
        {
            StartCoroutine(OgreticiMod());
        }
    }


    void FixedUpdate()
    {
        if (GameController.instance.isContinue == true)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * _speed);
            transform.rotation = Quaternion.Slerp(transform.rotation, hedefRotasyon, Time.deltaTime * 1.4865f);
        }


        if (donme)  //Zihin kontrolü yaparken gecerlidir
        {
            donusYonu = Quaternion.LookRotation(Vector3.right * donulecekObje.position.x + Vector3.forward * donulecekObje.position.z - transform.position);
            donecekObje.transform.rotation = Quaternion.RotateTowards(transform.rotation, donusYonu, 1000 * Time.deltaTime);

        }
        else if (donecekObje.transform.rotation.eulerAngles.y != 0)
        {
            donusYonu = Quaternion.LookRotation(Vector3.zero);
            donecekObje.transform.rotation = Quaternion.RotateTowards(transform.rotation, donusYonu, 50 * Time.deltaTime);
        }

    }

    public void DonulmeAktiflestir(Transform hedefObje) //Buyu yaparken gecerlidir
    {
        donme = true;
        donulecekObje = hedefObje;
    }

    public void DonulmePasiflestir() //Buyu yaparken gecerlidir
    {
        donme = false;
    }

    public void KaraktereDonusYaptir()
    {
        if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(Vector3.right), out hit, Mathf.Infinity)) //Saga Donus
        {
            if (hit.transform.CompareTag("DonusAyarlatici"))
            {
                if (karakterSolaGidiyor)
                {
                    throwController.SagaDon();
                    hedefRotasyon = Quaternion.Euler(Vector3.up * (transform.rotation.eulerAngles.y + 90));
                    cameraMovement.KameraDogrultuDegistir(12, 0, 12, 90);

                    karakterSagaGidiyor = false;
                    karakterSolaGidiyor = false;
                }
                else if (!karakterSolaGidiyor && !karakterSagaGidiyor)
                {
                    throwController.SagaDon();
                    hedefRotasyon = Quaternion.Euler(Vector3.up * (transform.rotation.eulerAngles.y + 90));
                    cameraMovement.KameraDogrultuDegistir(12, 0, -12, 90);

                    karakterSolaGidiyor = false;
                    karakterSagaGidiyor = true;
                }
            }
        }

        if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(-Vector3.right), out hit, Mathf.Infinity)) //Sola Donus
        {
            if (hit.transform.CompareTag("DonusAyarlatici"))
            {
                if (karakterSagaGidiyor)
                {
                    throwController.SolaDon();
                    hedefRotasyon = Quaternion.Euler(Vector3.up * (transform.rotation.eulerAngles.y - 90));
                    cameraMovement.KameraDogrultuDegistir(-12, 0, 12, -90);

                    karakterSagaGidiyor = false;
                    karakterSolaGidiyor = false;
                }
                else if (!karakterSolaGidiyor && !karakterSagaGidiyor)
                {
                    throwController.SolaDon();
                    hedefRotasyon = Quaternion.Euler(Vector3.up * (transform.rotation.eulerAngles.y - 90));
                    cameraMovement.KameraDogrultuDegistir(-12, 0, -12, -90);

                    karakterSagaGidiyor = false;
                    karakterSolaGidiyor = true;
                }
            }
        }
    }
}
