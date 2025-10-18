using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class MovingPlatform : MonoBehaviour
{


    [SerializeField] List<GameObject> positions;

    Vector2 equalThreshold = new Vector2(0.2f, 0.2f);

    GameObject target;

    public int pos = 0;

    void Start(){
        target = positions[pos];
    }

    void OnCollisionStay2D(Collision2D coll){
        if (coll.gameObject.GetComponent<PlayerController>() != null){
            coll.gameObject.GetComponent<PlayerController>().IncreaseVelocity(gameObject.GetComponent<Rigidbody2D>().velocity);
        }
    }

    /*void OnCollisionExit2D(Collision2D coll){
        if (coll.gameObject.GetComponent<PlayerController>() != null){
            
        }
    }*/

    [SerializeField] float speed = 1.0f;

    void Update(){
        if (Equal(transform.position, (Vector2) target.transform.position, equalThreshold)){
            //Debug.Log("Equal");
            pos = (pos + 1) % positions.Count;
            target = positions[pos];
        }
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float totalDistance = Vector2.Distance(target.transform.position, positions[(positions.Count + pos - 1) % positions.Count].transform.position);
        float dist = Vector2.Distance(target.transform.position, transform.position);
        rb.velocity = Vector2.Lerp(rb.velocity, target.transform.position - transform.position, dist/totalDistance * Time.deltaTime * 0.5f * speed);
        
        //Debug.Log(target.gameObject.name);
    }

    static bool Equal(Vector2 _v1, Vector2 _v2, Vector2 _e)
    {
        return System.Math.Abs(_v1.x - _v2.x) <= _e.x &&
               System.Math.Abs(_v1.y - _v2.y) <= _e.y;
    }
}