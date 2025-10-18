using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class WallTrigger : MonoBehaviour
{

    public GameObject pos1;

    public GameObject pos2;

    GameObject target;

    Vector2 equalThreshold = new Vector2(0.2f, 0.2f);

    Rigidbody2D rb;

    void Awake(){
        target = pos1;
        rb = GetComponent<Rigidbody2D>();
    }

    IEnumerator MoveTo(){
        while (!Equal(transform.position, target.transform.position, equalThreshold)){
            float totalDistance = Vector2.Distance(pos2.transform.position, pos1.transform.position);
            float dist = Vector2.Distance(target.transform.position, transform.position);
            rb.velocity = Vector2.Lerp(rb.velocity, target.transform.position - transform.position, dist/totalDistance * Time.deltaTime * 5f);
            yield return null;
        }
        rb.velocity = Vector2.zero;
    }

    void OnCollisionStay2D(Collision2D coll){
        if (coll.gameObject.GetComponent<PlayerController>() != null){
            coll.gameObject.GetComponent<PlayerController>().IncreaseVelocity(gameObject.GetComponent<Rigidbody2D>().velocity);
        }
    }
    
    public void TriggerObject(bool b){
        if (b)
            target = pos2;
        else
            target = pos1;
        StartCoroutine(MoveTo());

    }

    static bool Equal(Vector2 _v1, Vector2 _v2, Vector2 _e)
    {
        return System.Math.Abs(_v1.x - _v2.x) <= _e.x &&
               System.Math.Abs(_v1.y - _v2.y) <= _e.y;
    }
}
