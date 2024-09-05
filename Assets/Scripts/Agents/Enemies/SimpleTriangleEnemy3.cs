using UnityEngine;
using FSM;
using System.Linq;


public class SimpleTriangleEnemy3 : MonoBehaviour
{
    [SerializeField]
    private Transform[] points;
    [SerializeField]
    private Vector2 velocity;
    [SerializeField]
    private float timeWait;

    private Destination[] destinations;

    private State IdleStateSetup(Rigidbody2D rigidbody)
    {
        State idle = new State("IdleState");
        StateAction setVelocity = new SetVelocity2DAction(rigidbody, Vector2.zero);
        StateAction resetAllDestination = new ResetDestinationUnmarkedAction(destinations);
        idle.SetUpMe(new StateAction[] { setVelocity , resetAllDestination });
        return idle;
    }
    private State MoveStateSetup(Rigidbody2D rigidbody, Destination destination, int i)
    {
        State move = new State("MoveState_" + i);
        StateAction setVelocity = new SetVelocity2DAction(rigidbody, velocity, true);
        StateAction moveToDestination = new GoToPosition2DAction(gameObject, destination.Transform);
        StateAction setIsMarked = new SetBoolVariableAction(destination.Marked, true);
        move.SetUpMe(new StateAction[] { setVelocity, setIsMarked, moveToDestination });
        return move;
    }

    private Transition IdleToMove(State prev, State next, Destination destination)
    {
        Transition transition = new Transition();
        Condition timeCondition = new ExitTimeCondition(timeWait);
        Condition checkIsNotMarked = new CheckBoolCondition(destination.Marked, false);
        transition.SetUpMe(prev, next, new Condition[] { timeCondition, checkIsNotMarked });
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

        destinations = new Destination[points.Length];
        State[] moveTo = new State[points.Length];
        Transition[] movesToIdle = new Transition[destinations.Length];
        Transition[] idleToMove = new Transition[destinations.Length];
        for (int i = 0; i < points.Length; i++)
        {
            destinations[i] = new Destination(points[i], new BoolWrapper(false));
        }

        //setup idle state
        State idle = IdleStateSetup(rigidbody);
        for (int i = 0; i < points.Length; i++)
        {
            //Setup move states
            moveTo[i] = MoveStateSetup(rigidbody, destinations[i], i);

            //set transitions for each move states to idle and idle to move
            movesToIdle[i] = MoveToIdle(moveTo[i], idle, destinations[i].Transform);
            idleToMove[i] = IdleToMove(idle, moveTo[i], destinations[i]);

            //set the specific transition to idle for each move state
            moveTo[i].SetUpMe(new Transition[] { movesToIdle[i] });
        }
        //set all possible transitions to move for the only idle state
        idle.SetUpMe(idleToMove);
        State[] states = moveTo.Concat(new State[] { idle }).ToArray();
        machine.Init(states, idle);

    }

}
