using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    public List<GameObject> bonusList;

    private void Start()
    {
        GameEvent.ShowBonus += OnShowBonus;
    }
    void OnDisable()
    {
         GameEvent.ShowBonus -= OnShowBonus;
    }
    private void OnShowBonus(Config.SquareColor color)
    {
        GameObject obj = null;
        foreach(var i in bonusList)
        {
            var bonusComp = i.GetComponent<Bonus>();
            if(bonusComp.color == color)
            {
                obj = i;
                i.SetActive(true);
                break;
            }
        }   
        StartCoroutine(DeActivateBonus(obj));
    }
    private IEnumerator DeActivateBonus(GameObject obj)
    {
        yield return new WaitForSeconds(1.5f);
        obj.SetActive(false);
    }
}
