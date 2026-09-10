using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    void Awake()
    {
       if(Application.isEditor == false)
        {
            Debug.unityLogger.logEnabled = false;
        }
    }
    void Start()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            LoadScene("MainMenu");
        });
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
