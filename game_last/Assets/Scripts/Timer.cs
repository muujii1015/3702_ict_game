using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour



{
    public WinLose winlose;
    float currentTime = 200f;
    float startingTime =200f;

    [SerializeField] Text countdownText; 

    // Start is called before the first frame update
    void Start()
    {
        currentTime = startingTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        print(currentTime);
        countdownText.text = currentTime.ToString("0");
        if(currentTime <= 0)
        {
            winlose.Lose();
        }
    }
}
