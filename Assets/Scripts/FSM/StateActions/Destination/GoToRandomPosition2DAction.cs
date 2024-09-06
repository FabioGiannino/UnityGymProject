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
        private Destination selectedDestination;

        public Action<Destination> OnDestinationChange;

        public Func<Destination> destinationFunc;
        
        public Destination SelectedDestination() { return selectedDestination;  }


        public GoToRandomPosition2DAction(GameObject agent, Destination[] destinations)
        {
            this.agent = agent;
            this.destinations = destinations; 
            destinationFunc = new Func<Destination>(SelectedDestination);
            rigidbody = agent.GetComponent<Rigidbody2D>();
        }
        
        public override void OnEnter()
        {
            
           // selectedDestination = SetCurrentDestination();
           // OnDestinationChange?.Invoke(selectedDestination);
            selectedDestination = SetCurrentDestination();
        }
        
        private Destination SetCurrentDestination()
        {
            Destination[] unmarkedDestinations = destinations.Where(dest => dest.Marked.Value = false).ToArray();
            int randIndex = UnityEngine.Random.Range(0, destinations.Length );
            return destinations[randIndex];
        }
        public override void OnUpdate()
        {
            InternalSetVelocity();
            Vector3 positionToReachLocal = agent.transform.InverseTransformPoint(selectedDestination.Transform.position);
            if (positionToReachLocal.x < 0)
            {
                InternalSwitch();
            }
        }

        public override void OnExit()
        {
            selectedDestination.MarkDestination(true);
        }

        private void InternalSetVelocity()
        {
            Vector2 direction = selectedDestination.Transform.position.x > agent.transform.position.x ? Vector2.right : Vector2.left;
            rigidbody.velocity = direction * Mathf.Abs(rigidbody.velocity.x);
        }

        private void InternalSwitch()
        {
            agent.transform.Rotate(0, 180, 0);
        }
    }
}
