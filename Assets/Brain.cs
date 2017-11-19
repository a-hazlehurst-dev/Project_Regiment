using System;
using Assets.Code.StateMachine;
using Assets.Code.World;
using System.Collections.Generic;
using Assets.Code.Builders;
using Assets.Code.StateMachine.States;
using UnityEngine;
using UnityEngine.UI;
using Assets.Code.Services.Helper;

public class Brain : MonoBehaviour {

    private BattleStateMachine _battleStateMachine;
    private BaseCharacterBuilder _characterBuilder;
    public CharacterCanvasView MyCanvas;
    public BaseCharacter Character { get; set; }
    public GameObject root;
    public Rigidbody2D rigidBody;
    public GameObject target;
   
    public FacingHelper _facingHelper;
    public bool IsFleeing;
    public bool HasEscaped;
    public Animator knifeAttack;

    private string facing = "oblivion";


    public bool IsDead;
    public float speed;

    public void OnVictory()
    {
        _battleStateMachine.ChangeState(new CelebrationState(root));
        Debug.Log(root.gameObject.name + " has won.");
    }

    void Start()
    {
        _battleStateMachine = new BattleStateMachine();
        _characterBuilder = new BaseCharacterBuilder();
        _facingHelper = new FacingHelper(ref facing);

        Character = _characterBuilder.Build(); 
       

        Debug.Log(root.gameObject.name + "reach: " + Character.Reach);
     
        _battleStateMachine.ChangeState(new FindTargetState(root, this, 10,  "Battle", OnNewTargetFound));
       
    }

    public void ExitReached()
    {
        Debug.Log("Exit was reached.");
        HasEscaped = true;
    }

    void Update()
    {

        

        _facingHelper.SetFacing(root, target, "loop");

        _battleStateMachine.ExecuteUpdate();

        //MyCanvas.state = _battleStateMachine.ActiveState;


        if (IsDead)
        {
            _battleStateMachine.ChangeState(new FindTargetState(root, this, 20  , "Battle", OnNewTargetFound));
            return;
        }

       

      
        if(target == null|| target.GetComponentInChildren<Brain>().HasEscaped)
        {
            _battleStateMachine.ChangeState(new FindTargetState(root, this, 20, "Battle", OnNewTargetFound));
        }
    }

    public void Die()
    {
        IsDead = true;
        Debug.Log(root.gameObject.name + " has died.");
        _battleStateMachine.ChangeState(new DyingState(root ));
    }

    private void OnTargetDisappeared()
    {
        Debug.Log(root.name + " target dissapeared finding new target");
        _battleStateMachine.ChangeState(new FindTargetState(root, this, 20, "Battle", OnNewTargetFound));
    }

    private void OnExitFound(GameObject newTarget)
    {
        target = newTarget;
        IsFleeing = true;
        Debug.Log(root.gameObject.name + " has picked exit point: " + target.name);
        _battleStateMachine.ChangeState(new FleeState(root, target, Character.Speed, ExitReached, _facingHelper));
    }
    private void OnTargetReached()
    {
          _battleStateMachine.ChangeState(new AttackState(root, target,Character.AttackSpeed,Character.Reach, OnTargetDisappeared, knifeAttack, _facingHelper) );
    }

    public void OnHit(int x)
    {
        if (IsDead|| HasEscaped) { return; }
     
        Character.HitPoints -= x;
        Debug.Log(root.name +" has "+ Character.HitPoints + " left.");

        if (Character.HitPoints <= 0)
        {
            Die();
            return;
        }

        float hpPer = (float)   Character.HitPoints / Character.MaxHitPoints *100.0f;

        if (hpPer <= 30 && !IsFleeing)
        {
            _battleStateMachine.ChangeState(new FindTargetState(root, this, 20, "Exit", OnExitFound));
        }

        
    }

    private void OnNewTargetFound(GameObject newTarget)
    {
        target = newTarget;
        Debug.Log(root.name + " has found " + target.name);
        _battleStateMachine.ChangeState(new EngageTarget(root, target, Character.Speed, OnTargetReached, Character.Reach, _facingHelper));
    }
    

}
