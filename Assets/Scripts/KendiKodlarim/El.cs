using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class El : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Kucult());
    }


    private IEnumerator Kucult()
    {
       while(true)
        {
            if(transform.localScale.y >= .4f)
            {
                transform.localScale *= .97f;
            }
            else
            {
                Destroy(gameObject);
            }
            yield return null;
        }
    }
}
