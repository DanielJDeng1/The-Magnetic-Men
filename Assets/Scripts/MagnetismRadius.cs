using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class MagnetismRadius : MonoBehaviour
{
    public GameObject other = null;

    public List<GameObject> inRadius;

    void OnTriggerEnter2D(Collider2D coll){
        if (coll.gameObject.GetComponent<PlayerController>() != null){
            other = coll.gameObject;
        }
        else if (coll.gameObject.GetComponent<MagneticObject>() != null){
            inRadius.Add(coll.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D coll){
        if (coll.gameObject.GetComponent<PlayerController>() != null){
            other = null;
        }
        else if (coll.gameObject.GetComponent<MagneticObject>() != null){
            inRadius.Remove(coll.gameObject);
        }
    }

}
