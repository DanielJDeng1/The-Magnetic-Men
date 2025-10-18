using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class IceEffect : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D coll){
        if (coll.gameObject.GetComponent<PlayerController>() != null){
            coll.gameObject.GetComponent<PlayerController>().IceEnable(true);
        }
    }

    void OnCollisionExit2D(Collision2D coll){
        if (coll.gameObject.GetComponent<PlayerController>() != null){
            coll.gameObject.GetComponent<PlayerController>().IceEnable(false);
        }
    }
}
