
using System.Collections.Generic;
using Assets.Code.StateMachine;

namespace Assets.CodeV2.Item
{
    public interface  IItem
    {
        List<IState> States { get; set; }
        ItemStateMachine StateMachine { get;}
    }

    
}
