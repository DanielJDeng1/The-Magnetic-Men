using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class Checkpoint : MonoBehaviour
{

    public bool taken = false;

    [SerializeField] Sprite spr;

    [SerializeField] GameObject player1;

    [SerializeField] GameObject player2;

    [SerializeField] GameObject flagObj;

    AudioSource audio;

    void Awake(){
        audio = GetComponent<AudioSource>();
    }
    public void RespawnPlayers(){
        player1.transform.position = transform.position + new Vector3(2f, 1f, 0f);
        player2.transform.position = transform.position + new Vector3(-2f, 1f, 0f);
    }

    void OnTriggerEnter2D(Collider2D c){
        if (c.gameObject.GetComponent<PlayerController>() != null && !taken){
            taken = true;
            GetComponent<SpriteRenderer>().sprite = spr;
            SpawnParticles();
            audio.Play();
        }
    }

    void SpawnParticles(){
        for (int i = 0; i < 8; i++){
            GameObject obj = Instantiate(flagObj);
            //obj.GetComponent<SpriteRenderer>().color = keyColor;
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
    }

}
