using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarakterPaketiMovement : MonoBehaviour
{
    [Header("KarakterHiz")]
    [SerializeField] private float _speed;

    [Header("KarakterRotasyon")]
    private Transform donecekObje;
    private Transform donulecekObje;
    public bool donme = false;
    private Quaternion donusYonu;


    void Start()
    {
        donecekObje = transform.GetChild(0).transform;
    }


    void FixedUpdate()
    {
        if (GameController.instance.isContinue == true)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * _speed);
        }

       
        if(donme)
        {
            donusYonu = Quaternion.LookRotation(Vector3.right * donulecekObje.position.x + Vector3.forward * donulecekObje.position.z - transform.position);
            donecekObje.transform.rotation = Quaternion.RotateTowards(transform.rotation, donusYonu, 50000 * Time.deltaTime);

        }
        else if(donecekObje.transform.rotation.eulerAngles.y != 0)
        {
            donusYonu = Quaternion.LookRotation(Vector3.zero);
            donecekObje.transform.rotation = Quaternion.RotateTowards(transform.rotation, donusYonu, 50 * Time.deltaTime);
        }
        
    }

    public void DonulmeAktiflestir(Transform hedefObje)
    {
        donme = true;
        donulecekObje = hedefObje;
    }

    public void DonulmePasiflestir()
    {
        donme = false;
    }

}
