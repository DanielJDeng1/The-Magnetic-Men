using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectButton : MonoBehaviour
{

    public string levelName;

    public void ChangeScene(){
        GameObject sceneChanger = GameObject.Find("SceneManager");
        sceneChanger.GetComponent<SceneChanger>().ChangeScene(levelName);
        mp.FadeMusicOut();
    }

    [SerializeField] MusicPlayer mp;

}
