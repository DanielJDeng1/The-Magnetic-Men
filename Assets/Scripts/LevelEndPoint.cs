using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class LevelEndPoint : MonoBehaviour
{

    bool taken = false;
    public SceneChanger sceneChanger;

    [SerializeField] string levelChange;

    [SerializeField] AudioSource audio;

    public Animator anim;

    void OnTriggerEnter2D(Collider2D c){
        if (c.gameObject.GetComponent<PlayerController>() != null && !taken){
            taken = true;
            sceneChanger.ChangeScene(levelChange);
            anim.SetTrigger("Done");
            AudioSource ad = GetComponent<AudioSource>();
            ad.Play();
            StartCoroutine(fadeMusic());
        }
    }

    IEnumerator fadeMusic(){
        while (audio.volume >= 0.05f){
            audio.volume = audio.volume - 0.005f;
            yield return null;
        }
    }

}
