using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().volume = 0f;
        StartCoroutine(fadeMusic());
    }

    public void FadeMusicOut(){
        StartCoroutine(fadeMusicOut());
    }

    // Update is called once per frame
    IEnumerator fadeMusic(){
        while (GetComponent<AudioSource>().volume < 0.8f){
            GetComponent<AudioSource>().volume = GetComponent<AudioSource>().volume + 0.005f;
            yield return null;
        }
    }

    IEnumerator fadeMusicOut(){
        while (GetComponent<AudioSource>().volume > 0f){
            GetComponent<AudioSource>().volume = GetComponent<AudioSource>().volume - 0.005f;
            yield return null;
        }
    }
}
