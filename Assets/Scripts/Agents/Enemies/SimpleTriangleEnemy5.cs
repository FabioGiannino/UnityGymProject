using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using System;
public class SimpleTriangleEnemy5 : MonoBehaviour
{
    [SerializeField]
    private Vector2 moveSpeed;
    [SerializeField]
    private Transform[] patrolPoints;
    private State SetUpMoveState(Rigidbody2D rigidbody)
    {
        State move = new State("PatrolState");
        SetVelocity2DAction setVelocity = new SetVelocity2DAction(rigidbody, moveSpeed, true);
        PatrolDestination2DAction patrol = new PatrolDestination2DAction(gameObject, patrolPoints);
        move.SetUpMe(new StateAction[] { setVelocity, patrol });
        return move;
    }

    private void Start()
    {
        StateMachine machine = GetComponent<StateMachine>();
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        State move = SetUpMoveState(rigidbody);
        move.SetUpMe(new Transition[] { });
        machine.Init(new State[] { move }, move);
    }

}
