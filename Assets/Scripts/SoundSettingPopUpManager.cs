using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSettingPopUpManager : MonoBehaviour
{
   
    void Start()
    {
        this.gameObject.SetActive(false);
    }
    public void OpenPopUp()
    {
        this.gameObject.SetActive(true);
    }
    public void ClosePopUp()
    {
        this.gameObject.SetActive(false);
    }
}
