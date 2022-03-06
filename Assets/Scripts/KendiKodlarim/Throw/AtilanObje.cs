using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtilanObje : MonoBehaviour
{
    [Header("MateryalIcinGereklidir")]
    private Material material;
    private MeshRenderer renderer;

    [Header("FirlatmaIcinGereklidir")]
    private Rigidbody fizik;

   


    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        fizik = GetComponent<Rigidbody>();

    //    material = GetComponent<MeshRenderer>().sharedMaterial;

    }

  /*  void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            material.SetFloat("_OutlineWidth", 15);
            material.SetColor("_OutlineColor", Color.green);
        }

        if(Input.GetMouseButtonUp(0))
        {
            material.SetFloat("_OutlineWidth", 0);
            material.SetColor("_OutlineColor", Color.green);
        }
    }*/


    public void ObjeSec()
    {
        fizik.velocity = Vector3.zero;
        gameObject.layer = 2;
    }

    public void Firlat(Vector3 yon)
    {
        fizik.velocity = yon;
        gameObject.layer = 0;
    }

    public void Birak()
    {
        fizik.velocity = Vector3.zero;
        gameObject.layer = 0;
    }
}
