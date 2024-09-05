
namespace FSM
{
    //La condizione da soddisfare tra la transizione da uno stato all'altro. 
    // Ha un metodo che restituisce se la condizione è avverata oppure no. Di Default è true
    public class Condition : ExecutableNode
    {
        public virtual bool Validate()
        {
            return true;
        }
    }
}