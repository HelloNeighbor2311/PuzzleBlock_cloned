using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
   public static MusicManager instance;
   [SerializeField] private List<AudioClip> themeClips;
   private AudioSource musicSource;
   private static float musicTime;
   private bool isChoseMusic = false;
   private static int musicIndex;
    void Awake()
    {
        instance = this;
        musicSource = GetComponent<AudioSource>();
        musicIndex = UnityEngine.Random.Range(0, themeClips.Count); 
        musicSource.time = musicTime;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        musicSource.clip = themeClips[musicIndex];
        musicSource.loop = true;
        musicSource.Play();
    }
    private void Update()
    {
        musicTime = musicSource.time;
    }
}
