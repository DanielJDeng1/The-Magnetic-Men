using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAlert : MonoBehaviour
{

    public GameObject key;

    Collider2D boxColl;

    [SerializeField] KeyWall k;

    bool isUsed = false;

    void OnTriggerEnter2D(Collider2D coll){
        if (coll.gameObject.name == "Key" && !isUsed){
            k.DestroyWall();
            isUsed = true;
        }
    }
}
