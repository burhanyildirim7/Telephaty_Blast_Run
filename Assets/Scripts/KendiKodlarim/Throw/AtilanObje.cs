using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtilanObje : MonoBehaviour
{
    [Header("FirlatmaIcinGereklidir")]
    private Rigidbody fizik;
    private bool tutuluyorMu = false;

    [Header("EfektIcinGereklidir")]
    private KarakterPaketiMovement karakterPaketiMovement;
    private ParticleSystem efekt;

    private float eksenX, eksenY, eksenZ;

    private WaitForSeconds beklemeSuresi1 = new WaitForSeconds(.25f);

    void Start()
    {
        efekt = transform.GetChild(0).GetComponent<ParticleSystem>();
        fizik = GetComponent<Rigidbody>();
        karakterPaketiMovement = GameObject.FindWithTag("KarakterPaketi").GetComponent<KarakterPaketiMovement>();

        StartCoroutine(TutunabilirligiKontrolEt());
    }


    void Update()
    {
        if(tutuluyorMu)
        {
            transform.Rotate((Vector3.up * eksenY + Vector3.forward * eksenZ + Vector3.right * eksenX) * Time.deltaTime * 20);
        }
    }

    IEnumerator TutunabilirligiKontrolEt()
    {
        while(true)
        {
            if(!karakterPaketiMovement.karakterSagaGidiyor && !karakterPaketiMovement.karakterSolaGidiyor)
            {
                if(karakterPaketiMovement.transform.position.z - transform.position.z >= -.2f)
                {
                    efekt.Stop();
                }
            }
            else if(karakterPaketiMovement.karakterSagaGidiyor)
            {

            }
            else if(karakterPaketiMovement.karakterSolaGidiyor)
            {

            }
            yield return beklemeSuresi1;
        }
    }


    public void ObjeSec()
    {
        eksenX = Random.Range(-.5f, .5f);
        eksenY = Random.Range(-10f, 10f);
        eksenZ = Random.Range(-.5f, .5f);

        tutuluyorMu = true;

        fizik.velocity = Vector3.zero;
        gameObject.layer = 2;
    }

    public void Firlat(Vector3 yon)
    {
        tutuluyorMu = false;

        fizik.velocity = yon;
        gameObject.layer = 0;
        StartCoroutine(TagDegistir());
    }

    public void Birak()
    {
        tutuluyorMu = false;
        fizik.velocity = Vector3.zero;
        gameObject.layer = 0;
        StartCoroutine(TagDegistir());
    }

    IEnumerator TagDegistir() //Geriden gelen dusmanlarýn tekrar carpmamasi icin
    {
        yield return new WaitForSeconds(3);
        gameObject.tag = "FirlatilabilirNesne";
    }
}
