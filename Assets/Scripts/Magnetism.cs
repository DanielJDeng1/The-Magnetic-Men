using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class Magnetism : MonoBehaviour
{
    public bool positivePolarity = true;

    CircleCollider2D collider;

    List<GameObject> objects;

    private LineRenderer lineRenderer;
    public GameObject drawingPrefab;
    public Material material;
    private Color randomColor;

    GameObject drawing;

    void Start(){
        collider = GetComponent<CircleCollider2D>();

        objects = new List<GameObject>();

        drawing = Instantiate(drawingPrefab);
        lineRenderer = drawing.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.15f;
        lineRenderer.endWidth = 0.15f;
        Randomize();
        lineRenderer.startColor = randomColor;
        lineRenderer.endColor = randomColor;
    }

    void Update(){
        if (objects.Count > 0 && (objects[0].GetComponent<Magnetism>().positivePolarity && !positivePolarity)){
            drawing.SetActive(true);
                
            lineRenderer.SetPosition(0, gameObject.transform.position);
            lineRenderer.SetPosition(1, objects[0].gameObject.transform.position);
            
            Vector2 mid = FindMidpoint(transform.position, objects[0].gameObject.transform.position);

            

            gameObject.GetComponent<PlayerController>().IncreaseVelocity((mid - new Vector2(transform.position.x, transform.position.y)) * 0.2f);

        }
        else if (objects.Count > 0 && !(objects[0].GetComponent<Magnetism>().positivePolarity && positivePolarity)){
            drawing.SetActive(true);
                
            lineRenderer.SetPosition(0, gameObject.transform.position);
            lineRenderer.SetPosition(1, objects[0].gameObject.transform.position);
            
            Vector2 mid = FindMidpoint(transform.position, objects[0].gameObject.transform.position);

            gameObject.GetComponent<PlayerController>().IncreaseVelocity((mid - new Vector2(transform.position.x, transform.position.y)) * 0.2f);
        }
        else{
            drawing.SetActive(false);
        }

    }

    void OnTriggerEnter2D(Collider2D coll){
        if (coll.gameObject.GetComponent<Magnetism>() != null){
            objects.Add(coll.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D coll){
        if (coll.gameObject.GetComponent<Magnetism>() != null){
            objects.Remove(coll.gameObject);
        }
    }

    void Randomize()
    {
        int randomInt = Random.Range(1, 5); 

        switch (randomInt)
        {
            case 4:
                material.SetFloat("width", 0.5f); 
                material.SetFloat("heigth", 0.1f); 
                randomColor = Color.yellow;
                break;
            case 3:
                material.SetFloat("width", 0.5f); 
                material.SetFloat("heigth", 1f); 
                randomColor = Color.cyan;
                break;
            case 2:
                material.SetFloat("width", 0.75f); 
                material.SetFloat("heigth", 0.1f); 
                randomColor = Color.green;
                break;      
            case 1:
                material.SetFloat("width", 0.4f); 
                material.SetFloat("heigth", 0.8f); 
                randomColor = Color.red;
                break;
        }
     
    }

    Vector2 FindMidpoint(Vector2 v1, Vector2 v2){
        return new Vector2((v1.x + v2.x)/2, (v1.y + v2.y)/2);
    }

}
