using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityStandardAssets.Characters.FirstPerson;

public class npcsystem2: MonoBehaviour

{
    bool player_detection = false;
    public GameObject d_template;
    public GameObject canva;

    // Update is called once per frame
    void Update()
    {
        if (player_detection && Input.GetKeyDown(KeyCode.F) && !FirstPersonController.dialogue)
        {
            canva.SetActive(true);
            FirstPersonController.dialogue = true;
            newDialogue("Hey Soldier");
            newDialogue("Our country need your help");
            newDialogue("There is time Bomb");
            newDialogue("You have to collect 7 coins");
            newDialogue("Under 3 Min");
            
            canva.transform.GetChild(1).gameObject.SetActive(true);

        }
    }
    void newDialogue(string text)
    {
        GameObject template_clone = Instantiate(d_template, d_template.transform);
        template_clone.transform.parent = canva.transform;
        template_clone.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayerBody")
        {
            player_detection = true;

            Debug.Log("hello");
        }
    }

    private void OnTriggerExit(Collider other)
    {

        player_detection = false;

    }
}
