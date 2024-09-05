using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    /* Azione: setto due punti target (sinstra e destra) e poi setto la velocità dell'agent
     *   inserisco un controllo per vedere se sono uscito dalle posizioni
     */
    public class Simple2DPatrolAction : StateAction
    {
        private GameObject patroller;
        private Transform leftPositionPoint;
        private Transform rightPositionPoint;

        private Transform currentTransformToReach;
        private Rigidbody2D rigidbody;

        public Simple2DPatrolAction(GameObject patroller, Transform leftPositionPoint, Transform rightPositionPoint)
        {
            this.patroller = patroller;
            this.leftPositionPoint = leftPositionPoint;
            this.rightPositionPoint = rightPositionPoint;
            rigidbody = patroller.GetComponent<Rigidbody2D>();
        }

        /* Il primo punto di destinazione sarà quello su cui è orientato l'agent */
        public override void OnEnter()
        {
            currentTransformToReach = patroller.transform.right.x > 0 ? rightPositionPoint : leftPositionPoint;
            InternalSetVelocity();
        }

        /* Controllo se siamo andati oltre l'ultimo punto:
        *  Trovo le coordinate locali rispetto all'agent dell'attuale target point. Se la x di queste coordinate è minore di 0
        *  significa che è alle spalle dell'agent e quindi che l'ho superato
        */
        public override void OnUpdate()
        {
            InternalSetVelocity();            
            Vector3 positionToReachLocal = patroller.transform.InverseTransformPoint(currentTransformToReach.position);
            if (positionToReachLocal.x < 0)
            {
                InternalSwitch();
            }
        }

        private void InternalSetVelocity()
        {
            Vector2 direction = currentTransformToReach == rightPositionPoint ? Vector2.right : Vector2.left;
            rigidbody.velocity = direction * Mathf.Abs(rigidbody.velocity.x);
        }

        /*Cambia l'attuale punto target e ruota l'agent. questo metodo viene chiamato quando l'agent supera il target*/
        private void InternalSwitch()
        {
            currentTransformToReach = currentTransformToReach == leftPositionPoint ? rightPositionPoint : leftPositionPoint;
            patroller.transform.Rotate(0, 180, 0);
        }
    }
}
