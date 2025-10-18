using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerBelt : MonoBehaviour
{
    [SerializeField] float dir;
    [SerializeField] float power;

    /*public void OnCollisionStay2D(Collision2D collider){
        Rigidbody2D rb = collider.gameObject.GetComponent<Rigidbody2D>();
        MagneticObject mo = collider.gameObject.GetComponent<MagneticObject>();
        if (rb == null || mo == null)
            return;
        if (!(rb.bodyType == RigidbodyType2D.Dynamic))
            return;

        //mo.IncreaseVelocity(new Vector2(power * dir, 0f));
        //rb.velocity = rb.velocity + new Vector2(power * dir, 0f);
        //Debug.Log("here");
    }*/
}
