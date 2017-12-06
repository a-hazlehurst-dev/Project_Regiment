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
    public float TickCoolDown = 1;


    public bool IsDefending { get; set; }
    public bool IsFleeing;
    public bool HasEscaped;
    public bool HasWon;
    public bool IsDead;
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
        TickCoolDown -= Time.deltaTime;
        _facingHelper.SetFacing(root, target, "loop");
        if (TickCoolDown > 0)
        {
            return; 
            
        }
        if (Character.Stamina < 10)
        {
            _battleStateMachine.AddState(new BattleRestState(root));
        }

        _battleStateMachine.ExecuteUpdate();

        if (IsDead || HasEscaped || HasWon)
        {
            return;
        }

        if (target == null || target.GetComponentInChildren<Brain>().HasEscaped || target.GetComponentInChildren<Brain>().IsDead)
        {
            _battleStateMachine.AddState(new FindTargetState(root, this, 20, "Battle", OnNewTargetFound));
        }
        
    }

    public void OnBeingAttacked()
    {
        //im being attacked shall i turn on defensive mode
        if (UnityEngine.Random.Range(1, 100) < 50 && Character.Stamina <10)
        {
            _battleStateMachine.ExecuteUpdate();
            _battleStateMachine.AddState(new DefendState(root, knifeAttack));
        }
    }

    public void OnVictory()
    {
        _battleStateMachine.AddState(new CelebrationState(root));
        _battleStateMachine.AddState(new IdleSearchState(root));
        _battleStateMachine.AddState(new NonBattleState(root));
        _battleStateMachine.AddState(new NoMoveState(root));

        Debug.Log(baseName + " has won.");
        HasWon = true;
    }

    public void ExitReached()
    {
        Debug.Log("Exit was reached.");
        HasEscaped = true;
    }

    public void Die()
    {
       
        IsFleeing = false;
        Debug.Log(root.gameObject.name + " has died.");
        _battleStateMachine.AddState( new DyingState(root, OnDead ));
       
    }

  
    public void OnDead()
    {
        _battleStateMachine.AddState(new IdleSearchState(root));
        _battleStateMachine.AddState(new NonBattleState(root));
        _battleStateMachine.AddState(new NoMoveState(root));
        IsDead = true;
    }

    private void OnTargetDisappeared()
    {
        _battleStateMachine.AddState( new FindTargetState(root, this, 20, "Battle", OnNewTargetFound));
    }

    private void OnExitFound(GameObject newTarget)
    {
        target = newTarget;
        IsFleeing = true;

        _battleStateMachine.AddState(new FleeState(root, target, Character.Speed, ExitReached));
        _battleStateMachine.AddState(new IdleSearchState(root));
        _battleStateMachine.AddState(new NonBattleState(root));
    }
    private void OnTargetReached()
    {
        _battleStateMachine.AddState(new StayInRangeState(root, target, Character, OnTargetOutOfRange));
      
        if (Character.Stamina > 10)
        {
            IsDefending = false;
            _battleStateMachine.AddState(new AttackState(root, target, Character, OnTargetDisappeared, knifeAttack));
        }
    }

    public void OnTargetOutOfRange(GameObject go)
    {
        _battleStateMachine.AddState(new MoveToState(root, target, Character.Speed, OnTargetReached, Character.Reach));
        _battleStateMachine.AddState(new NonBattleState(root));
    }

    public void OnHit(int x)
    {
        if (IsDead|| HasEscaped) { return; }

        if (IsDefending)
        {
            var t  = (float)x *.50f;
            Debug.Log(baseName + " has defended reducing damage (" + t+ ", " + x );
            x = Mathf.CeilToInt(t);
            //reduce damage by 50

            IsDefending = false;
            _battleStateMachine.AddState(new AttackState(root, target, Character, OnTargetDisappeared, knifeAttack));
        }
        
        Character.HitPoints -= x  + UnityEngine.Random.Range(1,3);

        if (Character.HitPoints <= 0)
        {
            Die();
            return;
        }

        float hpPer = (float)   Character.HitPoints / Character.MaxHitPoints *100.0f;

        if (hpPer <= 30 && !IsFleeing)
        {
            _battleStateMachine.AddState(new FindTargetState(root, this, 20, "Exit", OnExitFound));
        }

        
    }

    private void OnNewTargetFound(GameObject newTarget)
    {
        target = newTarget;
        
        _battleStateMachine.AddState(new IdleSearchState(root));
        _battleStateMachine.AddState( new MoveToState(root, target, Character.Speed, OnTargetReached, Character.Reach));
    }

   
    /// <summary>
    /// occurs when an opponent has completed their attack on me manouvre.
    /// </summary>
    public void OnFinishedBeingAttacked()
    {

        _battleStateMachine.AddState(new AttackState(root, target, Character, OnTargetDisappeared, knifeAttack));
    }

    /// <summary>
    /// occures when I have completed an attack Manouvre on an enemy.
    /// </summary>
    public void OnFinishedMyAttack()
    {
        _battleStateMachine.AddState(new AttackState(root, target, Character, OnTargetDisappeared, knifeAttack));
    }

    public void StopAttackAnimation()
    {
        knifeAttack.SetBool("OnAttack", false);
        _battleStateMachine.AddState(new BattleRestState(root));
    }
}
