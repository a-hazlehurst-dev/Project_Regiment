using Assets.Code.StateMachine;
using Assets.Code.World;
using Assets.Code.Builders;
using Assets.Code.StateMachine.States;
using UnityEngine;
using Assets.Code.Services.Helper;
using Assets.Code.StateMachine.States.BattleStates;
using Assets.Code.StateMachine.States.MovementState;
using Assets.Code.StateMachine.States.SearchState;
using Assets.Code.StateMachine.States.Other;
using System;

public class Brain : MonoBehaviour {

    private BattleStateMachine _battleStateMachine;
    private BaseCharacterBuilder _characterBuilder;
    public CharacterCanvasView MyCanvas;
    public BaseCharacter Character { get; set; }
    public GameObject root;
    public Rigidbody2D rigidBody;
    public GameObject target;
    public FacingHelper _facingHelper;
    public Animator knifeAttack;
    private string facing = "oblivion";
    public string baseName = "";

  
    public float speed;


    void Start()
    {
        baseName = root.gameObject.name;
        _battleStateMachine = new BattleStateMachine();
        _characterBuilder = new BaseCharacterBuilder();
        _facingHelper = new FacingHelper(ref facing);

        Character = _characterBuilder.Build();
        _battleStateMachine.AddState(new FindTargetState(root, this, 10, "Battle", OnNewTargetFound));
        _battleStateMachine.AddState(new NonBattleState(root));
        _battleStateMachine.AddState(new NoMoveState(root));
    }

    void Update()
    {
        _battleStateMachine.ExecuteUpdate();

        if (!Character.IsActive())
        {
            target = null;
            return;
        }
       

        if (target == null)
        {
            _battleStateMachine.AddState(new FindTargetState(root, this, 10, "Battle", OnNewTargetFound));
            return;
        }

        _facingHelper.SetFacing(root, target, "loop");
    }

    public void TargetInRange()
    {
        _battleStateMachine.AddState(new StayInRangeState(root,target, Character, OnOutOfRange));
        _battleStateMachine.AddState(new AttackState(root, target, Character, Dissapeared,OnRest, knifeAttack));
    }

    public void OnOutOfRange(GameObject go)
    {
        _battleStateMachine.AddState(new MoveToState(root, target, Character, TargetInRange));
    }
    public void Dissapeared()
    {

    }

    public void OnRest()
    {
        _battleStateMachine.AddState(new BattleRestState(root, OnRested));
    }

    public void OnRested()
    {
        _battleStateMachine.AddState(new AttackState(root, target, Character, Dissapeared, OnRest, knifeAttack));
    }
    public void OnNewTargetFound(GameObject go)
    {
        if ( this.target != go)
        {
            this.target = go;
            _battleStateMachine.AddState(new IdleSearchState(root));
            _battleStateMachine.AddState(new MoveToState(root, target, Character, TargetInRange));
        }
    }

    public void OnFinishedMyAttack()
    {
        if (target != null && !target.GetComponentInChildren<Brain>().Character.IsDead())
        {
            _battleStateMachine.AddState(new AttackState(root, target, Character, Dissapeared, OnRest, knifeAttack));
        }
    }

    public void OnHit(int attackPower)
    {
        Character.TakeDamage(attackPower);

        if (Character.IsDead())
        {
            _battleStateMachine.AddState(new NoMoveState(root));
            _battleStateMachine.AddState(new NonBattleState(root));
            _battleStateMachine.AddState(new DyingState(root, OnDead));
            knifeAttack.SetBool("OnAttack", false);
        }

    }

    public void OnDead()
    {

    }
}
