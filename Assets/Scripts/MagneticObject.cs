using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class MagneticObject : MonoBehaviour
{
    public bool isStaticObject = false;

    public bool isPositive = false;

    public bool canStick = false;

    bool isSticking = false;

    BoxCollider2D tempCollider;

    PlayerController tempPlayer;

    //Vector2 velocityIncrease = Vector2.zero;

    public void DestroyTempBoxCollider(){
        if (tempPlayer != null)
            Destroy(tempPlayer.GetComponent<BoxCollider2D>());
    }

    void OnCollisionStay2D(Collision2D coll){
        PlayerController pc = coll.gameObject.GetComponent<PlayerController>();

        if (pc == null || !pc.isActive || pc.isPositive == isPositive){
            return;
        }

        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

        if (isStaticObject){
            if (pc.isPositive != isPositive){
                coll.gameObject.GetComponent<Rigidbody2D>().velocity = (GetComponent<Rigidbody2D>().velocity);
            }
        }
        else{
            if (canStick){
                //Debug.Log("sticking");
                if (!isSticking){
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero;

                    transform.parent = pc.gameObject.transform;
                    BoxCollider2D bc = pc.gameObject.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
                    bc.size = GetComponent<BoxCollider2D>().size * (Vector2)transform.localScale;
                    bc.offset = transform.position - coll.gameObject.transform.position;

                    GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                    GetComponent<Collider2D>().isTrigger = true;
                    isSticking = true;

                    tempPlayer = pc;
                    StartCoroutine(Stick());
                }
            }
            
        }
    }

    IEnumerator Stick(){
        while (!(!tempPlayer != null && isSticking && !tempPlayer.isActive)){
            yield return null;
        }
        yield return null;
        Destroy(tempPlayer.GetComponent<BoxCollider2D>());
        transform.parent = null;
        isSticking = false;

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        GetComponent<Collider2D>().isTrigger = false;

        tempPlayer = null;
    }

    public void SwapPolarity(){
        isPositive = !isPositive;

        if (isPositive)
            GetComponent<SpriteRenderer>().material = redmat;
        else
            GetComponent<SpriteRenderer>().material = bluemat;
    }

    [SerializeField] Material bluemat;

    [SerializeField] Material redmat;

    /*public void IncreaseVelocity(Vector2 increase){
        GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity + increase;
    }*/



}
