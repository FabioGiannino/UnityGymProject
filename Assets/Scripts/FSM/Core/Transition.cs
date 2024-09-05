
namespace FSM
{
    /*Ha lo stato di partenza e lo stato di destinazione
     * 1
     */
    public class Transition
    {

        private State fromState;
        private State toState;

        public State FromState { get { return fromState; } }
        public State ToState { get { return toState; } }

        private Condition[] conditions;


        //Constructor
        public void SetUpMe(State fromState, State toState, Condition[] conditions)
        {
            this.fromState = fromState;
            this.toState = toState;
            this.conditions = conditions;
        }

        //Quando entra nella transizione. Effettua l'OnEnter di tutte le condizioni della transizione
        public void OnEnter()
        {
            foreach (Condition condition in conditions)
            {
                condition.OnEnter();
            }
        }
        //Quando esce dalla transizione. Effettua l'OnExit di tutte le condizioni 
        public void OnExit()
        {
            foreach (Condition condition in conditions)
            {
                condition.OnExit();
            }
        }

        //per ogni condizione della transizione, va a validarla. Se tutte sono true, ritorna true
        public bool Validate()
        {
            foreach (Condition condition in conditions)
            {
                if (!condition.Validate()) return false;
            }
            return true;
        }


    }
}