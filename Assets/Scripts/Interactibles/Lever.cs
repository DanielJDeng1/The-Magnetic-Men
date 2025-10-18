using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class Lever : MonoBehaviour
{

    Animator anim;

    bool isPressed = false;

    [SerializeField] GameObject triggerGameObject;

    bool canBeInteracted = true;

    //List<GameObject> list;

    void Awake(){
        anim = GetComponent<Animator>();
        //list = new List<GameObject>();
        
    }

    void OnCollisionEnter2D(Collision2D coll){
        if (canBeInteracted && coll.gameObject.GetComponent<PlayerController>() != null || coll.gameObject.GetComponent<MagneticObject>() != null){
            isPressed = !isPressed;
            anim.SetBool("isPressed", isPressed);
            if (triggerGameObject.GetComponent<WallTrigger>() != null){
                triggerGameObject.GetComponent<WallTrigger>().TriggerObject(isPressed);
            }
            AudioSource ad = GetComponent<AudioSource>();

            ad.Play();
            //list.Add(coll.gameObject);
            canBeInteracted = false;
            //StartCoroutine(coolDown());
        }
    }

    IEnumerator coolDown(){
        canBeInteracted = false;
        yield return new WaitForSeconds(2f);
        //canBeInteracted = true;

    }

    /*void OnTriggerExit2D(Collider2D coll){
        if (coll.gameObject.GetComponent<PlayerController>() != null || coll.gameObject.GetComponent<MagneticObject>() != null){
            list.Remove(coll.gameObject);
            if (list.Count > 0)
                return;
            anim.SetBool("Click", false);
            if (triggerGameObject.GetComponent<WallTrigger>() != null){
                triggerGameObject.GetComponent<WallTrigger>().TriggerObject(false);
            }
        }
    }*/



}
