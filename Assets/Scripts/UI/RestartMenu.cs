using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartMenu : MonoBehaviour
{

    // Update is called once per frame

    void Awake(){

        pausePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            pausePanel.SetActive(!pausePanel.active);
        }
    }

    [SerializeField] GameObject pausePanel;
}
