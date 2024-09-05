using UnityEngine;

namespace FSM
{
    public class ExitTimeCondition : Condition
    {
        private float timeToWait;
        private float onEnterTime;

        public ExitTimeCondition(float timeToWait)
        {
            this.timeToWait = timeToWait;
        }

        public override void OnEnter()
        {
            onEnterTime = Time.time;
        }
        public override bool Validate()
        {
            return Time.time - onEnterTime > timeToWait;
        }
    }
}