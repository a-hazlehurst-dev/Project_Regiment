using Assets.Code.StateMachine;
using Assets.Code.World;
using Assets.Code.Builders;
using Assets.Code.StateMachine.States;
using UnityEngine;
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
    public bool HasWon;
    public Animator knifeAttack;

    private string facing = "oblivion";

    private FindTargetState _findTargetState;


    public bool IsDead;
    public float speed;

    public void OnVictory()
    {
        _battleStateMachine.AddState("move",new CelebrationState(root));
        
        Debug.Log(root.gameObject.name + " has won.");
        HasWon = true;
    }

    void Start()
    {
        _battleStateMachine = new BattleStateMachine();
        _characterBuilder = new BaseCharacterBuilder();
        _facingHelper = new FacingHelper(ref facing);
        _findTargetState = new FindTargetState(root, this, 20, "Battle", OnNewTargetFound);

        Character = _characterBuilder.Build(); 

        Debug.Log(root.gameObject.name + "reach: " + Character.Reach);
     
        _battleStateMachine.AddState("move", new FindTargetState(root, this, 10,  "Battle", OnNewTargetFound));
    }

    public void ExitReached()
    {
        Debug.Log("Exit was reached.");

        HasEscaped = true;
    }

    void Update()
    {
        _battleStateMachine.ExecuteUpdate();
        if (IsDead || HasEscaped || HasWon)
        {
            return;
        }

        if (target == null || target.GetComponentInChildren<Brain>().HasEscaped || target.GetComponentInChildren<Brain>().IsDead)
        {
            _battleStateMachine.AddState("move", new FindTargetState(root, this, 20, "Battle", OnNewTargetFound));
        }
        _facingHelper.SetFacing(root, target, "loop");
    }


    public void Die()
    {
        IsDead = true;
        Debug.Log(root.gameObject.name + " has died.");
        _battleStateMachine.AddState("move", new DyingState(root, OnDead ));
    }

    public void OnDead()
    {
        
    }

    private void OnTargetDisappeared()
    {
        _battleStateMachine.AddState("move", new FindTargetState(root, this, 20, "Battle", OnNewTargetFound));
    }

    private void OnExitFound(GameObject newTarget)
    {
        target = newTarget;
        IsFleeing = true;
        _battleStateMachine.AddState("move", new FleeState(root, target, Character.Speed, ExitReached));
    }
    private void OnTargetReached()
    {
          _battleStateMachine.AddState("move", new AttackState(root, target,Character, OnTargetDisappeared, knifeAttack) );
    }

    public void OnHit(int x)
    {
        if (IsDead|| HasEscaped) { return; }
     
        Character.HitPoints -= x;

        if (Character.HitPoints <= 0)
        {
            Die();
            return;
        }

        float hpPer = (float)   Character.HitPoints / Character.MaxHitPoints *100.0f;

        if (hpPer <= 30 && !IsFleeing)
        {
            _battleStateMachine.AddState("move", new FindTargetState(root, this, 20, "Exit", OnExitFound));
        }

        
    }

    private void OnNewTargetFound(GameObject newTarget)
    {
        target = newTarget;
        Debug.Log(root.name + " has found " + target.name);
        _battleStateMachine.AddState("move", new EngageTarget(root, target, Character.Speed, OnTargetReached, Character.Reach));
        _facingHelper.SetFacing(root, target, "loop");
    }
    

}
