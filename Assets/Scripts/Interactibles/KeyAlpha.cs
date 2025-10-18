using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyAlpha : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("hello");
        StartCoroutine(DestroyObject());
    }

    IEnumerator DestroyObject(){
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        float alpha = 1f;
        while (spr.color.a >= 0.05f){
            spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, alpha);
            alpha -= 0.005f;
            yield return null;
        }
        yield return null;
        
        Destroy(gameObject);
    }
}
