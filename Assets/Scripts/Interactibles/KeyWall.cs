using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyWall : MonoBehaviour
{
    [SerializeField] GameObject key;

    [SerializeField] GameObject outline;

    [SerializeField] GameObject keyAlphas;

    [SerializeField] Color keyColor;

    bool used = false;

    public void DestroyWall(){
        StartCoroutine(DestroyObject());
        used = true;
        AudioSource ad = GetComponent<AudioSource>();
        ad.Play();
    }



    IEnumerator DestroyObject(){
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        SpriteRenderer sprOutline = outline.GetComponent<SpriteRenderer>();
        float alpha = 1f;

        for (int i = 0; i < 8; i++){
            GameObject obj = Instantiate(keyAlphas);
            obj.GetComponent<SpriteRenderer>().color = keyColor;
            float speed = 15f;
            switch(i){
                case 0:
                    obj.GetComponent<Rigidbody2D>().velocity = new Vector2(1f, 0f) * speed;
                    break;
                case 1:
                    obj.GetComponent<Rigidbody2D>().velocity = new Vector2(1f, 1f) * speed;
                    break;
                case 2:
                    obj.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 1f) * speed;
                    break;
                case 3:
                    obj.GetComponent<Rigidbody2D>().velocity = new Vector2(-1f, 1f) * speed;
                    break;
                case 4:
                    obj.GetComponent<Rigidbody2D>().velocity = new Vector2(-1f, 0f) * speed;
                    break;
                case 5:
                    obj.GetComponent<Rigidbody2D>().velocity = new Vector2(-1f, -1f) * speed;
                    break;
                case 6:
                    obj.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -1f) * speed;
                    break;
                case 7:
                    obj.GetComponent<Rigidbody2D>().velocity = new Vector2(1f, -1f) * speed;
                    break;
                break;
            }
            obj.transform.parent = null;
            obj.transform.position = transform.position;
        }
        while (spr.color.a >= 0.1f){
            spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, alpha);
            sprOutline.color = new Color(spr.color.r, spr.color.g, spr.color.b, alpha);
            alpha -= 0.03f;
            yield return null;
        }
        yield return null;
        key.GetComponent<MagneticObject>().DestroyTempBoxCollider();
        Destroy(key);
        Destroy(gameObject);
    }
}
