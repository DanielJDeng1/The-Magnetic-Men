using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class WindEffect : MonoBehaviour
{

    [SerializeField] float dir;
    [SerializeField] float power;

    public bool isVert = false;

    List<GameObject> playerList = new List<GameObject>();

    GameObject player;

    void Awake(){
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Collider2D>().enabled = true;
    }

    void OnTriggerEnter2D(Collider2D coll){
        GameObject p = coll.gameObject;
        if (p.GetComponent<PlayerController>() != null){
            playerList.Add(p);
        }
    }

    void OnTriggerExit2D(Collider2D coll){
        GameObject p = coll.gameObject;
        if (p.GetComponent<PlayerController>() != null){
            PlayerController player = p.GetComponent<PlayerController>();
            player.SetVelocity(player.GetFrameVelocity() + new Vector2(power * dir, 0f));
            playerList.Remove(p);
        }
    }

    void FixedUpdate(){
        if (playerList.Count > 0){
            for (int i = 0; i < playerList.Count; i++){
                Vector2 playerVel = playerList[i].GetComponent<Rigidbody2D>().velocity;
            
                //playerList[i].GetComponent<PlayerController>().SetVelocity(new Vector2(Mathf.Min(maxSpeed, playerVel.x + power * dir), playerVel.y));
                if (isVert)
                    playerList[i].GetComponent<PlayerController>().IncreaseVelocity(new Vector2(0f, power * dir));
                else
                    playerList[i].GetComponent<PlayerController>().IncreaseVelocity(new Vector2(power * dir, 0f));
                
                //Debug.Log(playerList[i].GetComponent<Rigidbody2D>().velocity);
            }
            //player.GetComponent<Rigidbody2D>().velocity = new Vector2(playerVel.x + power * dir, playerVel.y);
        }
    }
}
