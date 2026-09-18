using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour
{
    public Button settingBtn;
    public Animator animator;
    private bool isOpened = false;

    public void SettingsOpened()
    {
        settingBtn.gameObject.SetActive(false);
        
    }
    private void Start()
    {
        settingBtn.onClick.AddListener(() =>
        {
            if(isOpened == false){
                animator.Play("OnOpen");
                isOpened = true;
            }
            else
            {
                animator.Play("OnClose");
                isOpened = false;
            }
        });
    }

}
