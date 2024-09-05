using UnityEngine;

namespace FSM
{

    /*La StateMachine ha un insieme di stati e il riferimento allo stato attuale.
     * Chiamando L'init (da dove?), si andranno a settare tutti gli stati.
     * 
     * Lo Swap state serve per cambiare stato della stateMachine (richiamando l'OnExit dello stato di uscita e l'OnEnter dello stato di entrata)
     * 
     * Ad Ogni Update (del gameLoop), viene eseguito l'OnUpdate dell'attuale stato
     */
    public class StateMachine : MonoBehaviour
    {
        private State[] states;
        private State activeState;

        //La init da dove viene chiamata?
        public void Init(State[] states, State firstState)
        {
            this.states = states;
            foreach (var state in states)
            {
                state.Init(this);
            }
            SwapState(firstState);
        }

        public void SwapState(State onState)
        {
            if (activeState != null)
            {
                Debug.Log("State " + activeState.Name + " Exited");
                activeState.OnExit();
            }
            activeState = onState;
            Debug.Log("State " + activeState.Name + " Entered");
            activeState.OnEnter();
        }

        public void Update()
        {
            if (activeState == null)
            {
                return;
            }
            activeState.OnUpdate();
        }


    }
}