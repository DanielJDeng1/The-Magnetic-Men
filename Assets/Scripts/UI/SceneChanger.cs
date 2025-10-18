using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChanger : MonoBehaviour
{
    public GameObject image;

    public void ChangeScene(string name){
        Animator anim = image.GetComponent<Animator>();
        anim.SetTrigger("Start");
        StartCoroutine(PlayAnimation(name));
    }

    public void PlayAnimation(){
        StartCoroutine(Anim());
    }

    IEnumerator Anim(){
        Animator anim = image.GetComponent<Animator>();
        anim.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("FadeOut");
        
    }

    IEnumerator PlayAnimation(string str){
        yield return new WaitForSeconds(1.1f);
        SceneManager.LoadScene(str);
    }
}
