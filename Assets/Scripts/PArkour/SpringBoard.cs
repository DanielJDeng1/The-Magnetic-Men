using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class BouncyObject : MonoBehaviour
{
    Animator anim;
    public float bounciness = 32f;

    [SerializeField] Vector2 bounceDir = new Vector2(0, 1);

    AudioSource audio;

    void Awake(){
        audio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Transform tra = coll.gameObject.transform;
        PlayerController pc = coll.gameObject.GetComponent<PlayerController>();
        if (pc != null) //+ transform.GetComponent<Renderer>().bounds.size.y)
        {
            Vector2 vell = pc.GetFrameVelocity();
            //pc.SetVelocity(new Vector2(vell.x, bounciness));
            if (bounceDir.y > 0){
                pc.BouncyJump(bounciness);
            }
            else {
                pc.SetVelocity(new Vector2(bounciness * bounceDir.x, pc.GetFrameVelocity().y));
                pc.BouncyJump(bounciness/2);
            }
            //audio.PlayOneShot(bounceSound);
            anim.SetTrigger("Bounce");
            audio.Play();
        }
    }
}
