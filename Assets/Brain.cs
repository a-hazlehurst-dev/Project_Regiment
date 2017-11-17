using System;
using Assets.Code.StateMachine;
using Assets.Code.World;
using System.Collections.Generic;
using Assets.Code.Builders;
using Assets.Code.StateMachine.States;
using UnityEngine;
using UnityEngine.UI;

public class Brain : MonoBehaviour {

    private BattleStateMachine _battleStateMachine;
    private BaseCharacterBuilder _characterBuilder;
    public CharacterCanvasView MyCanvas;
    public BaseCharacter Character { get; set; }
    public GameObject root;
    public Rigidbody2D rigidBody;
    public GameObject target;
 

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

        Character = _characterBuilder.Build(); 
       

        Debug.Log(root.gameObject.name + "reach: " + Character.Reach);
     
        _battleStateMachine.ChangeState(new FindTargetState(root, this, 10,  "Battle", OnNewTargetFound));
       
    }

    public void ExitReached()
    {
        
    }

    void Update()
    {
        _battleStateMachine.ExecuteUpdate();

        MyCanvas.state = _battleStateMachine.ActiveState;


        if (IsDead)
        {
            return;
        }

        var hpPer = (Character.HitPoints * 100) / Character.MaxHitPoints;

        if (hpPer <= 50)
        {
            _battleStateMachine.ChangeState(new FindTargetState(root, this, 10, "Exit", OnExitFound));
        }



        if (target != null && Vector2.Distance(root.transform.position, target.transform.position) > 1)
        {
            _battleStateMachine.ChangeState(new EngageTarget(root, target, Character.Speed, OnTargetReached, Character.Reach));
        }

        if(target == null)
        {
            _battleStateMachine.ChangeState(new FindTargetState(root, this, 10, "Battle", OnNewTargetFound));
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
        _battleStateMachine.ChangeState(new FindTargetState(root, this, 10, "Battle", OnNewTargetFound));
    }

    private void OnExitFound(GameObject newTarget)
    {
        target = newTarget;
        _battleStateMachine.ChangeState(new FleeState(root, target, Character.Speed, ExitReached));
    }
    private void OnTargetReached()
    {
          _battleStateMachine.ChangeState(new AttackState(root, target,Character.AttackSpeed,Character.Reach, OnTargetDisappeared) );
    }

    public void OnHit(int x)
    {
        Character.HitPoints -= x;
        Debug.Log(root.name +" has "+ Character.HitPoints + " left.");
        if (Character.HitPoints <= 0)
        {
            Die();
        }
    }

    private void OnNewTargetFound(GameObject newTarget)
    {
        target = newTarget;
        Debug.Log(root.name + " has found " + target.name);
        _battleStateMachine.ChangeState(new EngageTarget(root, target, Character.Speed, OnTargetReached, Character.Reach));
    }
    

}
