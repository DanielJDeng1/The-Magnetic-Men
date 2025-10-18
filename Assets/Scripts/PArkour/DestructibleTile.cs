using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleTile : MonoBehaviour
{

    int _fixedFrame;

    [SerializeField] private SpriteRenderer spr;

    Color color;
    private Collider2D cd;

    public ParticleSystem destroyParticles;

    private bool isDestroying = false;
    private int frameDestroyed;

    private bool isRecovering = false;
    private int frameRegen;

    [SerializeField] private int framesToDestroy;
    [SerializeField] private int framesToRecover;

    

    void Awake(){
        cd = gameObject.GetComponent<Collider2D>();
        _fixedFrame = 0;
        color = spr.color;
    }

    void FixedUpdate(){
        _fixedFrame++;

        if (isDestroying){
            if (_fixedFrame >= frameDestroyed){
                DestroyBlock();
            }
            else{
                spr.gameObject.transform.localPosition = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0f);
                if (frameDestroyed - _fixedFrame <= 50){
                    spr.color = new Color(color.r, color.g, color.b, 0f + (frameDestroyed - _fixedFrame)/10f);
                    if (!destroyParticles.isPlaying){
                        destroyParticles.Play();
                    }
                }
            }
        }
        else if (isRecovering){
            if (frameRegen <= _fixedFrame){
                RegenBlock();
            }
            else{
                spr.gameObject.transform.localPosition = new Vector3(0f, 0f, 0f);
                spr.color = new Color(color.r, color.g, color.b, 1f - (frameRegen - _fixedFrame)/10f);
            }
        }
    }

    void OnCollisionEnter2D(){
        if (!isDestroying){
            isDestroying = true;
            frameDestroyed = _fixedFrame + framesToDestroy;
            isRecovering = false;
        }
    }

    void DestroyBlock(){
        cd.enabled = false;
        spr.color = new Color(color.r, color.g, color.b, 0f);
        destroyParticles.Stop();
        isDestroying = false;
        isRecovering = true;
        frameRegen = _fixedFrame + framesToRecover;
    }
    
    void RegenBlock(){
        cd.enabled = true;
        spr.color = new Color(color.r, color.g, color.b, 1f);
        isRecovering = false;
    }

}