using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictoryController : MonoBehaviour
{
    public Transform victoryStats;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        majStats();
    }

    public void majStats()
    {
        victoryStats.Find("NbConfused").GetComponent<TextMeshProUGUI>().text = ""+GameController.Instance.dog.nbOfConfusedTimes;
    }
}
