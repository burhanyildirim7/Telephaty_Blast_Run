using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowController : MonoBehaviour
{


    [Header("FirlatilanObjeIleIlgili")]
    private AtilanObje atilanObje;
    private GameObject throwingObj;

    [Header("Raycast")]
    private RaycastHit hit;

    [Header("Efektler")]
    [SerializeField] private ParticleSystem efekt;


    [SerializeField] Text text;
    private Vector3 deltaTouchPosition;



    void FixedUpdate()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);


            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.CompareTag("FirlatmaNesnesi"))
                    {
                        throwingObj = hit.transform.gameObject;
                        atilanObje = throwingObj.GetComponent<AtilanObje>();
                        atilanObje.ObjeSec();
                        efekt.Play();
                    }
                }
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved && throwingObj != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

                if (Physics.Raycast(ray, out hit))
                {
                    throwingObj.transform.position = Vector3.Lerp(throwingObj.transform.position, hit.point + Vector3.up * throwingObj.transform.localScale.y * .5f, Time.deltaTime * 20);
                }
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended && throwingObj != null)
            {
                deltaTouchPosition = touch.deltaPosition;
                text.text = (deltaTouchPosition).ToString();

                if (deltaTouchPosition.magnitude >= 5)
                {
                    atilanObje.Firlat(deltaTouchPosition.normalized.x * Vector3.right * 4 + deltaTouchPosition.normalized.y * Vector3.forward * 4 + Vector3.up * 4);
                }
                else
                {
                    atilanObje.Birak();
                }

                efekt.Stop();
                throwingObj = null;
                atilanObje = null;
            }
        }
    }
}

