using Assets.Code.StateMachine;
using Assets.Code.World;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brain : MonoBehaviour {

    private BattleStateMachine _battleStateMachine;
    public BaseCharacter Character { get; set; }
    public GameObject root;
    public Rigidbody2D rigidBody;
    public GameObject target;
    public float speed = 1f;
    public float reach;

    void Start()
    {
        _battleStateMachine = new BattleStateMachine();
     
        _battleStateMachine.ChangeState(new FindTargetState(root, this, 10,  LayerMask.GetMask("Battle")));
       
    }

    void Update() {

        _battleStateMachine.ExecuteUpdate();
        
        if(target != null && Vector2.Distance(root.transform.position, target.transform.position) > 1)
        {
            _battleStateMachine.ChangeState(new EngageTarget(root, target, speed, OnTargetReached, reach));
        }

        if(target == null)
        {
            _battleStateMachine.ChangeState(new FindTargetState(root, this, 10, LayerMask.GetMask("Battle")));
        }
    }

    private void OnTargetReached()
    {
          _battleStateMachine.ChangeState(new AttackState( target));
    }
    

}
