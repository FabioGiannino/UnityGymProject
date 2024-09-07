using UnityEngine;
using System;

namespace FSM
{
    
    public class CheckDestinationDistanceCondition : Condition
    {
        private Transform from;
        private Func<Destination> destination;
        private float distanceToCheck;
        private COMPARISON comparison;
        
        public CheckDestinationDistanceCondition(Transform from, Func<Destination> destination, float distance, COMPARISON comparison)
        {
            this.from = from;
            this.destination = destination;
            this.distanceToCheck = distance * distance;
            this.comparison = comparison;
        }
        public override bool Validate()
        {
            return InternalDistanceCompare();
        }

        public bool InternalDistanceCompare()
        {

            float distanceSquared = (from.position - destination.Invoke().Transform.position).sqrMagnitude;
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
