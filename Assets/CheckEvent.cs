
using UnityEngine;

public class CheckEvent : MonoBehaviour
{
    public GameObject self;
	// Use this for initialization
    public void OnAttackComplete()
    {
        var brain = self.GetComponentInChildren<Brain>();
        if(brain != null && brain.target!=null)
        {
            var targetBrain = brain.target.GetComponentInChildren<Brain>();

            brain.Character.SetStamina(-5);
            //targetBrain.OnFinishedBeingAttacked();
            targetBrain.OnHit(brain.Character.AttackPower);

            if (!targetBrain.Character.IsActive())
            {
                brain.target = null;
            }
            brain.OnFinishedMyAttack();
        }
        else
        {
            brain.knifeAttack.SetBool("OnAttack", false);
        }
    }
    
}
