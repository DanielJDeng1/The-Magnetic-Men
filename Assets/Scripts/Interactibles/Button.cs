using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class Button : MonoBehaviour
{

    Animator anim;

    bool isPressed = false;

    [SerializeField] GameObject triggerGameObject;

    List<GameObject> list;

    void Awake(){
        anim = GetComponent<Animator>();
        list = new List<GameObject>();
        
    }

    void OnTriggerEnter2D(Collider2D coll){
        if (coll.gameObject.GetComponent<PlayerController>() != null || coll.gameObject.GetComponent<MagneticObject>() != null){
            anim.SetBool("Click", true);
            AudioSource ad = GetComponent<AudioSource>();
            if (triggerGameObject.GetComponent<WallTrigger>() != null){
                triggerGameObject.GetComponent<WallTrigger>().TriggerObject(true);
                ad.Play();
            }
            else if (triggerGameObject.GetComponent<MagneticObject>() != null){
                triggerGameObject.GetComponent<MagneticObject>().SwapPolarity();
                ad.Play();
            }
            else if (triggerGameObject.GetComponent<Dropper>() != null){
                triggerGameObject.GetComponent<Dropper>().Drop();
                ad.Play();
            }
            list.Add(coll.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D coll){
        if (coll.gameObject == null)
            return;
        if (coll.gameObject.GetComponent<PlayerController>() != null || coll.gameObject.GetComponent<MagneticObject>() != null){
            list.Remove(coll.gameObject);
            if (list.Count > 0)
                return;
            anim.SetBool("Click", false);
            if (triggerGameObject != null && triggerGameObject.GetComponent<WallTrigger>() != null){
                triggerGameObject.GetComponent<WallTrigger>().TriggerObject(false);
            }
            else if (triggerGameObject != null && triggerGameObject.GetComponent<MagneticObject>() != null){
                triggerGameObject.GetComponent<MagneticObject>().SwapPolarity();
            }
        }
    }



}
