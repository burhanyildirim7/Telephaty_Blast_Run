using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtilanObje : MonoBehaviour
{
    [Header("FirlatmaIcinGereklidir")]
    private Rigidbody fizik;
    private bool tutuluyorMu = false;
    private BoxCollider collider;

    [Header("EfektIcinGereklidir")]
    private KarakterPaketiMovement karakterPaketiMovement;
    public ParticleSystem efekt;
    private Outline outline;

    [Header("PatlamaEfektiKontrol")] // BaslangicIcin
    [SerializeField] private bool patlamaEfektiVarMi;
    [SerializeField] private ParticleSystem patlamaEfekti;

    [Header("PatlamaEfektSon")]
    [SerializeField] private bool sonPatlamaEfektVarMi;
    [SerializeField] private ParticleSystem sonPatlamaEfekti;

    [Header("YereCarpmaEfekt")]
    [SerializeField] private ParticleSystem yereCarpmaEfekt;

    private float eksenX, eksenY, eksenZ;

    private WaitForSeconds beklemeSuresi1 = new WaitForSeconds(.25f);
    RaycastHit hit;

    void Start()
    {
        efekt = Instantiate(efekt, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        efekt.transform.parent = transform;

        collider = GetComponent<BoxCollider>();
        fizik = GetComponent<Rigidbody>();
        karakterPaketiMovement = GameObject.FindWithTag("KarakterPaketi").GetComponent<KarakterPaketiMovement>();
        outline = GetComponent<Outline>();

        StartCoroutine(TutunabilirligiKontrolEt());
        outline.Secilebilir();
    }

    IEnumerator bekeleme()
    {
        yield return new WaitForSeconds(1);
    }

    void Update()
    {
        if (tutuluyorMu)
        {
            transform.Rotate((Vector3.up * eksenY + Vector3.forward * eksenZ + Vector3.right * eksenX) * Time.deltaTime * 20);
        }
    }

    IEnumerator TutunabilirligiKontrolEt()
    {
        while (true)
        {
            if (!karakterPaketiMovement.karakterSagaGidiyor && !karakterPaketiMovement.karakterSolaGidiyor)
            {
                if (karakterPaketiMovement.transform.position.z - transform.position.z >= 5f)
                {
                    efekt.Stop();
                    outline.Secilemez();
                }
            }
            else if (karakterPaketiMovement.karakterSagaGidiyor)
            {
                if (karakterPaketiMovement.transform.position.x - transform.position.x >= 5f)
                {
                    efekt.Stop();
                    outline.Secilemez();
                }
            }
            else if (karakterPaketiMovement.karakterSolaGidiyor)
            {
                if (-karakterPaketiMovement.transform.position.x + transform.position.x >= 5f)
                {
                    efekt.Stop();
                    outline.Secilemez();
                }
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

        if (patlamaEfektiVarMi)
        {
            patlamaEfekti.Play();
        }

        collider.enabled = false;
        fizik.isKinematic = false;
        fizik.useGravity = false;
    }

    public void Firlat(Vector3 yon)
    {
        tutuluyorMu = false;

        fizik.velocity = yon;
        gameObject.layer = 0;
        StartCoroutine(TagDegistir());

        collider.enabled = true;
        fizik.useGravity = true;
    }

    public void Birak()
    {
        tutuluyorMu = false;
        fizik.velocity = Vector3.zero;
        gameObject.layer = 0;
        StartCoroutine(TagDegistir());

        collider.enabled = true;
        fizik.useGravity = true;
    }

    IEnumerator TagDegistir() //Geriden gelen dusmanların tekrar carpmamasi icin
    {
        yield return new WaitForSeconds(2.25f);
        gameObject.tag = "FirlatilabilirNesne";
        collider.size *= .42f;
        if (Physics.Raycast(transform.position + transform.up * .25f, transform.TransformDirection(-transform.up), out hit, 5))
        {
            if (hit.transform.gameObject.CompareTag("Zemin"))
            {
                fizik.isKinematic = true;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.gameObject.CompareTag("Zemin"))
        {
             if (Physics.Raycast(transform.position, -Vector3.up, out hit, 10) && yereCarpmaEfekt != null)
             {
                 ParticleSystem efekt = Instantiate(yereCarpmaEfekt, transform.position, Quaternion.identity);
                 efekt.transform.position = hit.point + Vector3.up * .25f;
                 efekt.Play();
             }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DonusYap"))
        {
            gameObject.layer = 2;
            BoxCollider collider = GetComponent<BoxCollider>();
            collider.enabled = false;
        }

    }
}
