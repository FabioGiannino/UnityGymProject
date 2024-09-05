using UnityEngine;
using FSM;
using System.Linq;

public class DestinationMarks
{
    private Transform transform;
    private BoolWrapper marked;
    public Transform Transform {  get { return transform; } }
    public BoolWrapper Marked { get { return marked; } }
    public DestinationMarks(Transform destination, BoolWrapper marked )
    {
        this.transform = destination;
        this.marked = marked;
    }
    public void CheckDestination()
    {
        marked.Value = true;
    }
    public void UncheckDestination()
    {
        marked.Value = false;
    }
}

public class SimpleTriangleEnemy2 : MonoBehaviour
{
    [SerializeField]
    private Transform[] points;
    [SerializeField]
    private Vector2 velocity;
    [SerializeField]
    private float timeWait;

    private DestinationMarks[] destinations;

    private State IdleStateSetup(Rigidbody2D rigidbody)
    {
        State idle = new State("IdleState");
        StateAction setVelocity = new SetVelocity2DAction(rigidbody, Vector2.zero);        
        idle.SetUpMe(new StateAction[] { setVelocity });    
        return idle;
    }
    private State MoveStateSetup(Rigidbody2D rigidbody, DestinationMarks destination, int i)
    {
        State move = new State("MoveState_"+i);
        StateAction setVelocity = new SetVelocity2DAction(rigidbody, velocity,true);
        StateAction moveToDestination = new GoToPosition2DAction(gameObject, destination.Transform);
        StateAction setIsMarked = new SetBoolVariableAction(destination.Marked, true);
        move.SetUpMe(new StateAction[] { setVelocity , setIsMarked, moveToDestination  });
        return move;
    }

    private Transition IdleToMove(State prev, State next, DestinationMarks destination)
    {
        Transition transition = new Transition();
        Condition timeCondition = new ExitTimeCondition(timeWait);
        Condition checkIsNotMarked = new CheckBoolCondition(destination.Marked, false);
        transition.SetUpMe(prev,next,new Condition[] { timeCondition, checkIsNotMarked });
        return transition;
    }
        

    private Transition MoveToIdle(State prev, State next, Transform destination)
    {
        Transition transition = new Transition();
        Condition checkDistanceCondition = new CheckDistanceCondition(transform, destination, 0.5f, COMPARISON.LESSEQUAL);
        transition.SetUpMe(prev, next, new Condition[] { checkDistanceCondition });
        return transition;
    }

    private void Start()
    {
        StateMachine machine = GetComponent<StateMachine>();
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

        destinations = new DestinationMarks[points.Length];
        State[] moveTo = new State[points.Length];
        Transition[] movesToIdle = new Transition[destinations.Length];
        Transition[] idleToMove = new Transition[destinations.Length];
        

        //setup idle state
        State idle = IdleStateSetup(rigidbody);
        for (int i = 0; i < points.Length; i++)
        {
            destinations[i] = new DestinationMarks(points[i], new BoolWrapper(false));
            //Setup move states
            moveTo[i] = MoveStateSetup(rigidbody, destinations[i], i);
            
            //set transitions for each move states to idle and idle to move
            movesToIdle[i]= MoveToIdle(moveTo[i], idle, destinations[i].Transform);
            idleToMove[i] = IdleToMove(idle, moveTo[i], destinations[i]);

            //set the specific transition to idle for each move state
            moveTo[i].SetUpMe(new Transition[] { movesToIdle[i] });
        }
        //set all possible transitions to move for the only idle state
        idle.SetUpMe(idleToMove);
        State[] states = moveTo.Concat(new State[] { idle }).ToArray();        
        machine.Init(states, idle);

    }

    private bool AllDestinationAreDiscovered()
    {
        foreach (var dest in destinations)
        {
            if (dest.Marked.Value == false)
            {
                return false;
            }
            
        }
        return true;        
    }
    private void ResetAllDestinations()
    {
        foreach (var dest in destinations)
        {
            dest.Marked.Value = false;
        }
    }

}
