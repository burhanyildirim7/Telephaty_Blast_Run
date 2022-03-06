using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtilanObje : MonoBehaviour
{
    [Header("MateryalIcinGereklidir")]
    [SerializeField] Material[] material;
    private MeshRenderer renderer;

    [Header("FirlatmaIcinGereklidir")]
    private Rigidbody fizik;




    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        fizik = GetComponent<Rigidbody>();
    }


    public void ObjeSec()
    {
        //renderer.material = material[1];
        fizik.velocity = Vector3.zero;
        gameObject.layer = 2;
    }

    public void Firlat(Vector3 yon)
    {
        //renderer.material = material[0];
        fizik.velocity = yon;
        gameObject.layer = 0;
    }

    public void Birak()
    {
        //renderer.material = material[0];
        fizik.velocity = Vector3.zero;
        gameObject.layer = 0;
    }
}
