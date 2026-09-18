using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    private AudioSource SFXSource;

    [SerializeField] private List<AudioClip> pickingShapeAudioClips;
    [SerializeField] private List<AudioClip> puttingShapeAudioClips;
    [SerializeField] private List<AudioClip> gettingPointAudioClips;

    private void Awake()
    {
        instance = this;
        SFXSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        GameEvent.BeginDragShape += OnBeginDragShape;
        GameEvent.PlaceShapeOnBoard += OnPlaceShapeOnBoard;
        GameEvent.GettingPoints += OnScoreAdded;
    }

    private void OnDestroy()
    {
        GameEvent.BeginDragShape -= OnBeginDragShape;
        GameEvent.PlaceShapeOnBoard -= OnPlaceShapeOnBoard;
    }
    private void OnScoreAdded(){
        PlayRandomClip(gettingPointAudioClips);
    }
    private void OnPlaceShapeOnBoard()
    {
        PlayRandomClip(puttingShapeAudioClips);
    }

    private void OnBeginDragShape()
    {
        PlayRandomClip(pickingShapeAudioClips);
    }

    private void PlayRandomClip(List<AudioClip> clips)
    {
        if (SFXSource == null || clips == null || clips.Count == 0)
        {
            return;
        }

        for (int attempt = 0; attempt < clips.Count; attempt++)
        {
            AudioClip clip = clips[UnityEngine.Random.Range(0, clips.Count)];
            if (clip != null)
            {
                SFXSource.PlayOneShot(clip);
                return;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
