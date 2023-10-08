using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLose : MonoBehaviour
{

    private bool gameEnded;
    // Start is called before the first frame update
    public void Win()
    {
        if (!gameEnded)
        {
            Debug.Log("You Win");

            SwitchToScene("Win");
            Debug.Log("Scene Load Attempted");
        }
    }

    public void Lose()
    {

        if (!gameEnded)
        {
            Debug.Log("LOSer");
            SwitchToScene("Lose");
        }
        
    }
    public void SwitchToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
