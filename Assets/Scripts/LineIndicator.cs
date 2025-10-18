
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineIndicator: MonoBehaviour
{
    private LineRenderer lineRenderer;
    public GameObject drawingPrefab;
    public Material material;
    private Color randomColor;

    [SerializeField] GameObject player1;

    [SerializeField] GameObject player2;

    void Start(){
        GameObject drawing = Instantiate(drawingPrefab);
        lineRenderer = drawing.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.15f;
        lineRenderer.endWidth = 0.15f;
        Randomize();
        lineRenderer.startColor = randomColor;
        lineRenderer.endColor = randomColor;
    }

    void Update()
    {
        FreeDraw();

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

    void FreeDraw()
    {

        //Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
        //lineRenderer.positionCount++;
        lineRenderer.SetPosition(0, player1.transform.position);
        lineRenderer.SetPosition(1, player2.transform.position);
        
    }
}