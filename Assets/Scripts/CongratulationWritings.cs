using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongratulationWritings : MonoBehaviour
{
    public List<GameObject> writings;
    private void Start()
    {
        GameEvent.ShowCongratulationWritings += OnShowCongratulationWritings;
    }

    private void OnShowCongratulationWritings()
    {
       var index = UnityEngine.Random.Range(0, writings.Count);
       writings[index].SetActive(true);
    }

    private void OnDisable()
    {
         GameEvent.ShowCongratulationWritings -= OnShowCongratulationWritings;
    }
    private void ShowCongratulationWritings()
    {
        
    }
}
