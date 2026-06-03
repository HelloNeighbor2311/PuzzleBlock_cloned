using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Button settingBtn;


    private void Awake()
    {
        playBtn.onClick.AddListener(OnPlayClicked);
    }

    private void OnPlayClicked()
    {
        StartCoroutine(LoadGamePlayAfterDelay());
    }

    private IEnumerator LoadGamePlayAfterDelay()
    {
        yield return new WaitForSeconds(0.2f);
        SceneLoader.LoadScene(SceneLoader.Scene.GamePlay);
    }


}
