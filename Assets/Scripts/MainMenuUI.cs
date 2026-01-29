using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Button settingBtn;


    private void Awake()
    {
        playBtn.onClick.AddListener(() =>
        {
            SceneLoader.LoadScene(SceneLoader.Scene.GamePlay);
        });
    }
    private void Start()
    {
        playBtn.Select();
    }


}
