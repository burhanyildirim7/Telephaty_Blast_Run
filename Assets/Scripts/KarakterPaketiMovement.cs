using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarakterPaketiMovement : MonoBehaviour
{
    [Header("KarakterHiz")]
    [SerializeField] private float _speed;

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


    void Start()
    {
        BaslangicDegerleri();
    }

    public void BaslangicDegerleri() //UIController
    {
        throwController = GameObject.FindWithTag("ThrowController").GetComponent<ThrowController>();
        cameraMovement = GameObject.FindWithTag("MainCamera").GetComponent<CameraMovement>();
        donecekObje = transform.GetChild(0).transform;

        hedefRotasyon = Quaternion.Euler(Vector3.zero);
    }


    void FixedUpdate()
    {
        if (GameController.instance.isContinue == true)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * _speed);
            transform.rotation = Quaternion.Slerp(transform.rotation, hedefRotasyon, Time.deltaTime * 1.4865f);
        }

       
        if(donme)  //Zihin kontrolü yaparken gecerlidir
        {
            donusYonu = Quaternion.LookRotation(Vector3.right * donulecekObje.position.x + Vector3.forward * donulecekObje.position.z - transform.position);
            donecekObje.transform.rotation = Quaternion.RotateTowards(transform.rotation, donusYonu, 1000 * Time.deltaTime);

        }
        else if(donecekObje.transform.rotation.eulerAngles.y != 0)
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
        if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(Vector3.right), out hit, Mathf.Infinity)) //SagaDonus
        {
            if (hit.transform.CompareTag("DonusAyarlatici"))
            {
                throwController.SagaDon();
                hedefRotasyon = Quaternion.Euler(Vector3.up * (transform.rotation.eulerAngles.y + 90));
                cameraMovement.KameraDoðrultuDegistir(11, 0, -11, 90);
            }
        }

        if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(-Vector3.right), out hit, Mathf.Infinity)) //SolaDonus
        {
            if (hit.transform.CompareTag("DonusAyarlatici"))
            {
                throwController.SolaDon();
                hedefRotasyon = Quaternion.Euler(Vector3.up * (transform.rotation.eulerAngles.y - 90));
                cameraMovement.KameraDoðrultuDegistir(-11, 0, -11, -90);
            }
        }
    }

}
