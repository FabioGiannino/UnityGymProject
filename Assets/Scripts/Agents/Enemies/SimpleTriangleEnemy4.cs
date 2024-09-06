using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using Codice.Client.BaseCommands.Merge;
using System;
public class SimpleTriangleEnemy4 : MonoBehaviour
{
    [SerializeField]
    private Transform[] points;
    [SerializeField]
    private Vector2 velocity;
    [SerializeField]
    private float timeToWait;


    private Destination[] destinations;
    private Rigidbody2D rb;
    private Destination currentDestination;

    private Func<Destination> destinationFunc;

    public State SetupIdleState()
    {
        State idle = new State("Idle");
        StateAction stopMove = new SetVelocity2DAction(rb, Vector2.zero);
        StateAction resetDestinations = new ResetDestinationUnmarkedAction(destinations);
        idle.SetUpMe(new StateAction[] {stopMove, resetDestinations});
        return idle;
    }

    public State SetupMoveState()
    {
        State move = new State("Move");
        StateAction setSpeed = new SetVelocity2DAction(rb, velocity,true);
        StateAction goToRandomDest = new GoToRandomPosition2DAction(gameObject, destinations);
        destinationFunc = (goToRandomDest as GoToRandomPosition2DAction).destinationFunc;
        move.SetUpMe(new StateAction[] {setSpeed, goToRandomDest});
        return move;
    }

    public Transition IdleToMove(State prev, State next)
    {
        Transition transition = new Transition();
        Condition waitCondition = new ExitTimeCondition(timeToWait);
        transition.SetUpMe(prev,next, new Condition[] {waitCondition});
        return transition;
    }

    public Transition MoveToIdle(State prev, State next)
    {
        Transition transition = new Transition();
        Condition checkDistance = new CheckDestinationDistanceCondition(transform, destinationFunc, 0.5f, COMPARISON.LESSEQUAL);
        transition.SetUpMe(prev, next, new Condition[] {checkDistance});
        return transition; 
    }

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StateMachine machine = GetComponent<StateMachine>();
        destinations = new Destination[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            destinations[i] = new Destination(points[i], new BoolWrapper(false));
        }
        currentDestination = destinations[0];
        State idle = SetupIdleState();
        State move = SetupMoveState();

        Transition idleToMove = IdleToMove(idle, move);
        Transition moveToIdle = MoveToIdle(move, idle);

        idle.SetUpMe(new Transition[] { idleToMove });
        move.SetUpMe(new Transition[] { moveToIdle });

        machine.Init(new State[] {idle,move},idle);
    }


    private void OnDestinationChange(Destination dest)
    {
        currentDestination = dest;
    }
}
