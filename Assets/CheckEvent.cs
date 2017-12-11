
using UnityEngine;

public class CheckEvent : MonoBehaviour
{
    public GameObject self;
	// Use this for initialization
    public void OnAttackComplete()
    {
        var brain = self.GetComponentInChildren<Brain>();
        if(brain != null)
        {
            var targetBrain = brain.target.GetComponentInChildren<Brain>();

            brain.Character.SetStamina(-5);
            //targetBrain.OnFinishedBeingAttacked();
            targetBrain.OnHit(brain.Character.AttackPower);

            if (targetBrain.Character.IsDead())
            {
                brain.target = null;
            }
            brain.OnFinishedMyAttack();
        }
    }
    
}
