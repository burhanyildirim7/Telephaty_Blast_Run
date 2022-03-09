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


    void Start()
    {
        baslangicHizi = _speed;
        BaslangicDegerleri();
       
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
