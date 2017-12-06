
using UnityEngine;

public class CheckEvent : MonoBehaviour
{

    public GameObject self;
	// Use this for initialization
    public void OnAttackComplete()
    {
        var brain = self.GetComponentInChildren<Brain>();
        var targetBrain =brain.target.GetComponentInChildren<Brain>();
       
        brain.Character.SetStamina(-5);
        targetBrain.OnFinishedBeingAttacked();
        brain.OnFinishedMyAttack();
        targetBrain.OnHit(brain.Character.AttackPower);

        brain.StopAttackAnimation();
    }
    
}
