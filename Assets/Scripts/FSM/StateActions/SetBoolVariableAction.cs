
public class BoolWrapper
{
    public bool Value { get; set; }
    public BoolWrapper(bool value)
    {
        this.Value = value;
    }
}

namespace FSM
{
    public class SetBoolVariableAction : StateAction
    {
        private BoolWrapper variable;
        private bool value;

        public SetBoolVariableAction(BoolWrapper variable, bool value)
        {
            this.variable = variable;
            this.value = value;
        }
        public override void OnExit()
        {
            variable.Value = value;
        }
    }
}