using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    
    private GameObject Player;

    [Header("DogrultuIslemleri")]
    private float eksenX, eksenY, eksenZ; 
    private float rotasyonY = 180;

    void Start()
    {
        BaslangicDegerleri();
    }

    public void BaslangicDegerleri()
    {

        Player = GameObject.FindGameObjectWithTag("Player");


        rotasyonY = 180;
        eksenX = 0;
        eksenY = 14.275f;
        eksenZ = 12;

        transform.position = Vector3.up * 13.275f + Vector3.forward * 11;
        transform.rotation = Quaternion.Euler(Vector3.up * 180 + Vector3.right * 45);

        

        
    }


    void Update()
    {

        transform.position = Vector3.Lerp(transform.position, new Vector3(Player.transform.position.x + eksenX, Player.transform.position.y + eksenY, Player.transform.position.z + eksenZ), Time.deltaTime * 5f);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(Vector3.right* 45 + Vector3.up * rotasyonY) , Time.deltaTime * 3);
    }

    public void KameraDoðrultuDegistir(int x, int y, int z, int rotY)
    {
        eksenX += x;
        eksenY += y;
        eksenZ += z;
        rotasyonY += rotY;
    }

}
