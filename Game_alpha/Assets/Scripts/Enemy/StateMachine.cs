using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    public BaseState activeState;

    public Petrol patrolState;

    // Start is called before the first frame update

    public void Initialise()
    {
        //default
        patrolState = new Petrol();
        changeState(patrolState);
    }


    void Start()
    {
        if(activeState != null)
        {
            activeState.Perform();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void changeState(BaseState newState)
    {
        if (activeState != null)
        {
            activeState.Exit();
        }
        activeState = newState;
        if (activeState != null)
        {
            activeState.stateMachine = null;
            activeState.enemy = GetComponent<Enemy>();
            activeState.Enter();
        }

    }
}
