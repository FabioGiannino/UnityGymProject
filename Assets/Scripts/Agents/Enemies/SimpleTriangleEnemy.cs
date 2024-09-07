using FSM;
using UnityEngine;

public class SimpleTriangleEnemy : MonoBehaviour
{
    [SerializeField]
    private Vector2 moveSpeed;
    [SerializeField]
    private Transform patrolPoint1;
    [SerializeField]
    private Transform patrolPoint2;
    private State SetUpMoveState( Rigidbody2D rigidbody)
    {
        State move = new State("PatrolState");
        SetVelocity2DAction setVelocity = new SetVelocity2DAction(rigidbody, moveSpeed, true);
        Simple2DPatrolAction patrol = new Simple2DPatrolAction(gameObject, patrolPoint1, patrolPoint2);
        move.SetUpMe(new StateAction[] { setVelocity,patrol });
        return move;
    }
    
    private void Start()
    {
        StateMachine machine = GetComponent<StateMachine>();
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        State move = SetUpMoveState(rigidbody);
        move.SetUpMe(new Transition[] {});
        machine.Init(new State[] {  move }, move);
    }

}