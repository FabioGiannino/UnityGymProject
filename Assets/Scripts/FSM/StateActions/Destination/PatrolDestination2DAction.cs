
using UnityEngine;

namespace FSM
{
    public class PatrolDestination2DAction : StateAction
    {

        private GameObject agent;
        private Transform[] points;

        private Transform pointChosen;
        private int indexPointChosen;
        private Rigidbody2D rigidbody;


        public PatrolDestination2DAction(GameObject agent, Transform[] destinations)
        {
            this.agent = agent;
            this.points = destinations;
            indexPointChosen = -1;
            rigidbody = agent.GetComponent<Rigidbody2D>();
        }

        public override void OnEnter()
        {
            InternalChooseDestination();
            InternalSetVelocity();
        }

        public override void OnUpdate()
        {
            InternalSetVelocity();
            Vector3 positionToReachLocal = agent.transform.InverseTransformPoint(pointChosen.position);
            if (positionToReachLocal.x < 0)
            {
                InternalChooseDestination();
            }
        }

        private void InternalChooseDestination()
        {
            if (points.Length<2)  return; 
            int randIndex;
            do
            {
                randIndex = Random.Range(0, points.Length);
            } while (randIndex == indexPointChosen);
            indexPointChosen = randIndex;
            pointChosen = points[indexPointChosen];
            Vector3 positionToReachLocal = agent.transform.InverseTransformPoint(pointChosen.position);
            if (positionToReachLocal.x< agent.transform.position.x)
            {
                agent.transform.Rotate(0, 180, 0);
            }
        }
        private void InternalSetVelocity()
        {
            Vector2 direction = agent.transform.position.x > pointChosen.position.x ?
                Vector2.left : Vector2.right;
            rigidbody.velocity = direction * Mathf.Abs(rigidbody.velocity.x);
        }
    }
}
