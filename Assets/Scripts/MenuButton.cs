using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{

    void Awake()
    {
       if(Application.isEditor == false)
        {
            Debug.unityLogger.logEnabled = false;
        }
    }
    void Start()
    {

    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
