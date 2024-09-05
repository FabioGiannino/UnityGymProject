using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FSM
{
    public class SetVelocity2DAction : StateAction
    {
        private Rigidbody2D rigidbody;
        private Vector2 velocityToSet;
        private bool everyFrame;

        public SetVelocity2DAction(Rigidbody2D rigidbody, Vector2 velocityToSet, bool everyFrame = false)
        {
            this.rigidbody = rigidbody;
            this.velocityToSet = velocityToSet;
            this.everyFrame = everyFrame;
        }

        public override void OnEnter()
        {
            InternalSetVelocity();
        }

        public override void OnUpdate()
        {
            if (!everyFrame) return;
            InternalSetVelocity();
        }

        private void InternalSetVelocity()
        {
            rigidbody.velocity = velocityToSet;
        }
    }
}
