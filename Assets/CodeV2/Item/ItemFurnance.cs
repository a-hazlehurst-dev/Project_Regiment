using System.Collections.Generic;
using Assets.Code.StateMachine;
using Assets.CodeV2.States;

namespace Assets.CodeV2.Item
{
    public class ItemFurnance : IItem
    {
        public List<IState> States { get; set; }
        public ItemStateMachine StateMachine { get; private set; }



        public ItemFurnance()
        {
            States = new List<IState>();
            States.Add(new ItemOffState());
            States.Add(new ItemOnState());

            

            StateMachine = new ItemStateMachine();
            StateMachine.ChangeState(new ItemOffState());
        }

        public void Update(float deltaTime)
        {
            StateMachine.Execute();
        }
    }
}
