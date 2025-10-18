using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    [SerializeField] GameObject drop;

    public void Drop(){
        GameObject d = Instantiate(drop, transform.position, transform.rotation);
    }
}
