using UnityEngine;

namespace FSM
{
    public class  GoToPosition2DAction: StateAction
    {
        private GameObject agent;
        private Transform positionPoint;
        private Rigidbody2D rigidbody;
        
        public GoToPosition2DAction(GameObject agent, Transform positionPoint)
        {
            this.agent = agent;
            this.positionPoint = positionPoint;
            rigidbody = agent.GetComponent<Rigidbody2D>();
        }
        public override void OnUpdate()
        {
            InternalSetVelocity();           
            Vector3 positionToReachLocal = agent.transform.InverseTransformPoint(positionPoint.position);
            if (positionToReachLocal.x < 0)
            {
                InternalSwitch();
            }
        }

        private void InternalSetVelocity()
        {
            Vector2 direction = positionPoint.position.x > agent.transform.position.x ? Vector2.right : Vector2.left;
            rigidbody.velocity = direction * Mathf.Abs(rigidbody.velocity.x);
        }

        private void InternalSwitch()
        {            
            agent.transform.Rotate(0, 180, 0);
        }
    }
}
