
namespace FSM
{
    public class CheckBoolCondition: Condition
    {
        BoolWrapper variable;
        bool value;
        public CheckBoolCondition(BoolWrapper variable, bool value)
        {
            this.variable = variable;
            this.value = value;
        }
        public override bool Validate()
        {
            return variable.Value == value;
        }
    }

}
