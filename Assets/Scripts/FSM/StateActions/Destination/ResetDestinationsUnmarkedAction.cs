using UnityEngine;
public class Destination
{
    private Transform transform;
    private BoolWrapper marked;
    public Transform Transform { get { return transform; } }
    public BoolWrapper Marked { get { return marked; } }
    public Destination(Transform destination, BoolWrapper marked)
    {
        this.transform = destination;
        this.marked = marked;
    }
    public void MarkDestination(bool value)
    {
        marked.Value = value;
    }

}
namespace FSM
{
    public class ResetDestinationUnmarkedAction: StateAction
    {
        Destination[] destinations;

        /// <summary>
        /// FSM Action: Checked if all destinations are marked yet and, if is true, unmarks all
        /// </summary>
        /// <param name="destinations">All possible destinations</param>
        public ResetDestinationUnmarkedAction(Destination[] destinations)
        {
            this.destinations = destinations;
        }

        public override void OnEnter()
        {
            if(!AllDestinationAreDiscovered()) return; 
            ResetAllDestinations();
        }

        private bool AllDestinationAreDiscovered()
        {
            foreach (var dest in destinations)
            {
                if (dest.Marked.Value == false)
                {
                    return false;
                }
            }
            return true;
        }

        private void ResetAllDestinations()
        {
            foreach (var dest in destinations)
            {
                dest.Marked.Value = false;
            }
        }
    }

}