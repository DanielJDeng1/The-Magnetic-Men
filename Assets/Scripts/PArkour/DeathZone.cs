using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class DeathZone : MonoBehaviour
{
    [SerializeField] SceneChanger sceneChanger;

    [SerializeField] List<Checkpoint> checkpoints;

    bool isRestarting = false;

    AudioSource ad;

    void OnTriggerEnter2D(Collider2D coll){
        if (coll.GetComponent<PlayerController>() != null && !isRestarting){
            isRestarting = true;
            sceneChanger.PlayAnimation();
            Checkpoint cp = checkpoints[0];
            for (int i = checkpoints.Count - 1; i >= 0; i--){
                if (checkpoints[i].taken){
                    cp = checkpoints[i];
                    break;
                }
            }
            ad = GetComponent<AudioSource>();
            ad.Play();
            StartCoroutine(Wait(cp));
        }
    }

    IEnumerator Wait(Checkpoint cp){
        yield return new WaitForSeconds(2);
        cp.RespawnPlayers();
        isRestarting = false;
    }
}
