using System;
using System.Linq;
using UnityEngine;

namespace FSM
{
    public class GoToRandomPosition2DAction : StateAction
    {
        private GameObject agent;
        private Destination[] destinations;
        private Rigidbody2D rigidbody;
        private Destination destinationChosen;

        public Func<Destination> destinationFunc;

        public GoToRandomPosition2DAction(GameObject agent, Destination[] destinations)
        {
            this.agent = agent;
            this.destinations = destinations; 
            destinationFunc = new Func<Destination>(InternalGetDestinationChosen);
            rigidbody = agent.GetComponent<Rigidbody2D>();
        }
        
        public override void OnEnter()
        {
            destinationChosen = InternalChooseDestination();
        }
        
        public override void OnUpdate()
        {
            InternalSetVelocity();
            Vector3 positionToReachLocal = agent.transform.InverseTransformPoint(destinationChosen.Transform.position);
            if (positionToReachLocal.x < 0)
            {
                InternalSwitch();
            }
        }

        public override void OnExit()
        {
            destinationChosen.MarkDestination(true);
        }



        private Destination InternalGetDestinationChosen()
        {
            return destinationChosen;
        }

        private Destination InternalChooseDestination()
        {
            Destination[] unmarkedDestinations = destinations.Where(dest => dest.Marked.Value == false).ToArray();
            int randIndex = UnityEngine.Random.Range(0, unmarkedDestinations.Length);
            return unmarkedDestinations[randIndex];
        }        

        private void InternalSetVelocity()
        {
            Vector2 direction = destinationChosen.Transform.position.x > agent.transform.position.x ? Vector2.right : Vector2.left;
            rigidbody.velocity = direction * Mathf.Abs(rigidbody.velocity.x);
        }

        private void InternalSwitch()
        {
            agent.transform.Rotate(0, 180, 0);
        }
    }
}
