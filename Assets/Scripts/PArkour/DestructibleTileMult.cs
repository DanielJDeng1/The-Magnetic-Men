using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

public class DestructibleTileMult : MonoBehaviour
{

    Collider2D collider;

    List<GameObject> tiles;

    List<Vector2> tilePositions;

    private bool isDestroying = false;
    private int frameDestroyed;

    private bool isRecovering = false;
    private int frameRegen;

    private int _fixedFrame = 0;

    private Color color;

    [SerializeField] private int framesToDestroy;
    [SerializeField] private int framesToRecover;


    AudioSource audio;

    void Awake(){
        audio = GetComponent<AudioSource>();
        collider = GetComponent<Collider2D>();
        tiles = new List<GameObject>();
        tilePositions = new List<Vector2>();
        foreach (Transform child in transform){
            tiles.Add(child.gameObject);
        }
        foreach(GameObject obj in tiles){
            tilePositions.Add(obj.transform.localPosition);
        }
        color = tiles[0].GetComponent<SpriteRenderer>().color;
    }

    void FixedUpdate(){
        _fixedFrame++;

        if (isDestroying){
            if (_fixedFrame >= frameDestroyed){
                DestroyBlock();
            }
            else{
                RandomizePosition();
                if (frameDestroyed - _fixedFrame <= 50){
                    SetColor(new Color(color.r, color.g, color.b, 0f + (frameDestroyed - _fixedFrame)/10f));
                    if (!IsPlaying())
                        PlayParticles(true);
                }
            }
        }
        else if (isRecovering){
            if (frameRegen <= _fixedFrame){
                RegenBlock();
            }
            else{
                NormalizePosition();
                SetColor(new Color(color.r, color.g, color.b, 1f - (frameRegen - _fixedFrame)/10f));
            }
        }
    }

    bool IsPlaying(){
        return tiles[0].transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying;
    }

    void PlayParticles(bool b){
        if (b)
            foreach (GameObject t in tiles)
                t.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        else
            foreach (GameObject t in tiles)
                t.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
    }

    void SetColor(Color c){
        foreach (GameObject t in tiles){
            t.GetComponent<SpriteRenderer>().color = c;
            Color spr = t.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color;
            t.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = new Color(spr.r, spr.g, spr.b, c.a);
        }
    }

    void NormalizePosition(){
        for (int i = 0; i < tiles.Count; i++)
            tiles[i].transform.localPosition = tilePositions[i];
    }

    void RandomizePosition(){
        for (int i = 0; i < tiles.Count; i++)
            tiles[i].transform.localPosition = new Vector3(tilePositions[i].x + Random.Range(-0.15f, 0.15f), tilePositions[i].y + Random.Range(-0.15f, 0.15f), 0f);
    }

    void OnCollisionEnter2D(){
        if (!isDestroying){
            isDestroying = true;
            frameDestroyed = _fixedFrame + framesToDestroy;
            isRecovering = false;
            //audio.PlayOneShot(rumble);
        }
    }

    void DestroyBlock(){
        AudioSource ad = GetComponent<AudioSource>();

        ad.Play();
        //audio.PlayOneShot(destroy);
        collider.enabled = false;
        SetColor(new Color(color.r, color.g, color.b, 0f));
        PlayParticles(false);
        isDestroying = false;
        isRecovering = true;
        frameRegen = _fixedFrame + framesToRecover;
    }
    
    void RegenBlock(){
        collider.enabled = true;
        SetColor(new Color(color.r, color.g, color.b, 1f));
        isRecovering = false;
    }

}
