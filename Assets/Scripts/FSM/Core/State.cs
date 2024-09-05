
namespace FSM
{
    /* Lo stato avrà tra i suoi attributi il riferimento alla statemachine a cui appartiene, una lista di actions e una lista di transizioni
     * Quando la stateMachine entra in uno stato, lo stato esegue tutti i metodi iscritti agli OnEnter delle sue Action e delle sue Transition.
     * Allo stesso modo, quando la state machine esce da uno stato, lo stato chiama tutti gli OnExit delle actions e transitions.
     * Poi, ad ogni Update (della statemachine), verranno eseguiti gli update delle action dello stato in cui è la FSM e, allo stesso tempo, 
     * vengono verificate le condizioni dentro le transizioni:
     *      - Se anche una sola condizione di transizione è verificata, verrà fatto uno Swap di stato
     */
    public class State
    {
        private StateMachine owner;
        private StateAction[] actions;
        private Transition[] transitions;
        private string name;
        public string Name { get { return name; } }

        public State(string name)
        {
            this.name = name;
        }
        
        public void Init(StateMachine owner)
        {
            this.owner = owner;
        }

        public void SetUpMe(StateAction[] actions)  //costruttore delle stateactions
        {
            this.actions = actions;
        }

        public void SetUpMe(Transition[] transitions)   //costruttore delle transitions
        {
            this.transitions = transitions;
        }

        public void OnEnter()
        {
            foreach (var action in actions)
            {
                action.OnEnter();
            }
            foreach (var transition in transitions)
            {
                transition.OnEnter();
            }
        }

        public void OnExit()
        {
            foreach (var action in actions)
            {
                action.OnExit();
            }
            foreach (var transition in transitions)
            {
                transition.OnExit();
            }
        }

        public void OnUpdate()
        {
            foreach (var action in actions)
            {
                action.OnUpdate();
            }
            foreach (var transition in transitions)
            {
                if (transition.Validate())
                {
                    owner.SwapState(transition.ToState);
                    return;
                }
            }
        }

        public string GetName()
        {
            return name;
        }
    }

}