using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowController : MonoBehaviour
{
    [Header("ErisilecekObbjeler")]
    private GameObject player;

    [Header("FirlatilanObjeIleIlgili")]
    private AtilanObje atilanObje;
    private GameObject throwingObj;
    [SerializeField] private float objeUzaklikMenzili;

    [Header("DokunmaAyarlari")]
    private RaycastHit hit;
    private Touch touch;

    [Header("Efektler")]
    [SerializeField] private ParticleSystem efekt;

    [Header("AnimasyonVeKarakterDonusuIcin")]
    private PlayerController playerController;
    private KarakterPaketiMovement karakterPaketiMovement;


    [SerializeField] Text text;
    private Vector3 deltaTouchPosition;

    [Header("FirlatmaYonKontrol")]
    public bool karakterSagaGidiyor;
    public bool karakterSolaGidiyor;

    float result;


    void Start()
    {
        BaslangicDegerleri();
    }

    public void BaslangicDegerleri()
    {
        player = GameObject.FindWithTag("Player");
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        karakterPaketiMovement = GameObject.FindWithTag("KarakterPaketi").GetComponent<KarakterPaketiMovement>();

        karakterSolaGidiyor = false;
        karakterSagaGidiyor = false;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                DokunmayaBasla();
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved && throwingObj != null)
            {
                Dokunma();
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended && throwingObj != null)
            {
                DokunmayiBitir();
            }
        }
    }

    //Dokunma kýsýmlarý
    private void DokunmayaBasla()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        if (Physics.Raycast(ray, out hit))
        {
            uzaklikAlgilayici1();

            if (hit.transform.CompareTag("FirlatilabilirNesne") && result >= objeUzaklikMenzili)
            {
                throwingObj = hit.transform.gameObject;
                atilanObje = throwingObj.GetComponent<AtilanObje>();
                atilanObje.ObjeSec();
                atilanObje.tag = "FirlatmaNesnesi";

                karakterPaketiMovement.DonulmeAktiflestir(throwingObj.transform);
                efekt.Play();
                playerController.ObjeleriKontrolEtBasla();
            }
        }
    }

    private void Dokunma()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

        if (Physics.Raycast(ray, out hit))
        {
            throwingObj.transform.position = Vector3.Lerp(throwingObj.transform.position, hit.point + Vector3.up * .75f, Time.deltaTime * 20);
        }

        uzaklikAlgilayici2();
        if (result >= objeUzaklikMenzili)
        {
            DokunmayiBitir();
        }
    }

    private void DokunmayiBitir()
    {
        deltaTouchPosition = touch.deltaPosition;
        text.text = (deltaTouchPosition).ToString();

        if (deltaTouchPosition.magnitude >= 2)
        {
            ObjeyiFirlat();
        }
        else
        {
            atilanObje.Birak();
        }

        karakterPaketiMovement.DonulmePasiflestir();
        efekt.Stop();
        throwingObj = null;
        atilanObje = null;

        playerController.ObjeleriKontrolEtBitir();
    }




    //Firlatma kisimlari
    private void ObjeyiFirlat()
    {
        if (!karakterSagaGidiyor && !karakterSolaGidiyor)
        {
            atilanObje.Firlat(-deltaTouchPosition.normalized.x * Vector3.right * 6 - deltaTouchPosition.normalized.y * Vector3.forward * 6 + Vector3.up * 4);
        }
        else if (karakterSagaGidiyor)
        {
            atilanObje.Firlat(-deltaTouchPosition.normalized.y * Vector3.right * 6 + deltaTouchPosition.normalized.x * Vector3.forward * 6 + Vector3.up * 4);
        }
        else if (karakterSolaGidiyor)
        {
            atilanObje.Firlat(deltaTouchPosition.normalized.y * Vector3.right * 6 - deltaTouchPosition.normalized.x * Vector3.forward * 6 + Vector3.up * 4);
        }

        playerController.ObjeleriKontrolEtBitir();
    }


    //Firlatmada yön belirtmek icin olan kisimlar
    public void SagaDon()
    {
        if (karakterSolaGidiyor)
        {
            karakterSolaGidiyor = false;
        }
        else if (!karakterSolaGidiyor)
        {
            karakterSagaGidiyor = true;
        }
    }

    public void SolaDon()
    {
        if (karakterSagaGidiyor)
        {
            karakterSagaGidiyor = false;
        }
        else if (!karakterSagaGidiyor)
        {
            karakterSolaGidiyor = true;
        }
    }

    private float uzaklikAlgilayici1()
    {

        if (!karakterSagaGidiyor && !karakterSolaGidiyor)
        {
            result = hit.point.z - player.transform.position.z;
        }
        else if (karakterSolaGidiyor)
        {
            result = player.transform.position.x - hit.point.x;
        }
        else if (karakterSagaGidiyor)
        {
            result = -player.transform.position.x + hit.point.x;
        }

        return result;
    }

    private float uzaklikAlgilayici2()
    {

        if (!karakterSagaGidiyor && !karakterSolaGidiyor)
        {
            result = -throwingObj.transform.position.z + player.transform.position.z;
        }
        else if (karakterSolaGidiyor)
        {
            result = -player.transform.position.x + throwingObj.transform.position.x;
        }
        else if (karakterSagaGidiyor)
        {
            result = player.transform.position.x - throwingObj.transform.position.x;
        }

        return result;
    }
}

