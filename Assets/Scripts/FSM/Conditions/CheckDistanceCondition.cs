
using UnityEngine;

namespace FSM
{
    /// <summary>
    /// FSM Condition: Give 2 point transforms, check if the distance between is EQUAL,LESS or GREATER than an input distance
    /// </summary>
    public class CheckDistanceCondition : Condition
    {
        private Transform from;
        private Transform to;
        private float distanceToCheck;
        private COMPARISON comparison;

        /// <summary>
        /// FSM Condition: Give 2 point transforms, check if the distance between is EQUAL,LESS or GREATER than an input distance
        /// </summary>
        /// <param name="from">First point transform</param>
        /// <param name="to">Second point transform</param>
        /// <param name="distance">Distance to check</param>
        /// <param name="comparison">Type of Comparison. Could be EQUAL,LESS,GREATER,LESSEQUAL,GREATEREQUAL</param>
        public CheckDistanceCondition(Transform from, Transform to, float distance, COMPARISON comparison)
        {
            this.from = from;
            this.to = to;
            this.distanceToCheck = distance * distance;
            this.comparison = comparison;
        }
        public override bool Validate()
        {
            return InternalDistanceCompare();
        }

        public bool InternalDistanceCompare()
        {
            float distanceSquared = (from.position - to.position).sqrMagnitude;
            switch (comparison)
            {
                case COMPARISON.EQUAL:
                    return distanceSquared == distanceToCheck;
                case COMPARISON.LESS:
                    return distanceSquared < distanceToCheck;
                case COMPARISON.GREATER:
                    return distanceSquared > distanceToCheck;
                case COMPARISON.LESSEQUAL:
                    return distanceSquared <= distanceToCheck;
                case COMPARISON.GREATEREQUAL:
                    return distanceSquared >= distanceToCheck;
                default:
                    return false;
            }
        }
    }
}
