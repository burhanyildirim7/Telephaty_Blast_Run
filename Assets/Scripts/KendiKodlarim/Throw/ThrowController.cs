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


   void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        karakterPaketiMovement = GameObject.FindWithTag("KarakterPaketi").GetComponent<KarakterPaketiMovement>();
    }

    void FixedUpdate()
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

    private void DokunmayaBasla()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("FirlatmaNesnesi") &&  (player.transform.position.z - hit.transform.position.z) <= 1)
            {
                throwingObj = hit.transform.gameObject;
                atilanObje = throwingObj.GetComponent<AtilanObje>();
                atilanObje.ObjeSec();

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
            throwingObj.transform.position = Vector3.Lerp(throwingObj.transform.position, hit.point + Vector3.up * throwingObj.transform.localScale.y * .5f, Time.deltaTime * 20);
        }

        if((player.transform.position.z - throwingObj.transform.position.z) >= 1)
        {
            DokunmayiBitir();
        }
    }

    private void DokunmayiBitir()
    {
        deltaTouchPosition = touch.deltaPosition;
        text.text = (deltaTouchPosition).ToString();

        if (deltaTouchPosition.magnitude >= 3)
        {
            atilanObje.Firlat(-deltaTouchPosition.normalized.x * Vector3.right * 6 - deltaTouchPosition.normalized.y * Vector3.forward * 6 + Vector3.up * 4);
        }
        else
        {
            atilanObje.Birak();
        }

        karakterPaketiMovement.DonulmePasiflestir();
        playerController.ObjeleriKontrolEtBitir();
        efekt.Stop();
        throwingObj = null;
        atilanObje = null;
    }
}

