
using System.Linq;
using UnityEngine;

namespace Assets.Code.StateMachine
{
    public class FindTargetState : IState
    {
        private readonly GameObject _self;
        private readonly Brain _brain;
        private readonly float _searchRadius;
        private readonly LayerMask _layerMask;

        public FindTargetState(GameObject self, Brain brain, float searchRadius, LayerMask layerMask)
        {
            this._self = self;
            this._brain = brain;
            this._searchRadius = searchRadius;
            this._layerMask = layerMask;
        }

        public void Enter()
        {
            Debug.Log("Entering Search Mode");
        }

        public void Execute()
        {
            Debug.Log("Firing OverlapSphere");
            var collisions = Physics.OverlapSphere(_self.transform.position + Vector3.up, _searchRadius).ToList() ;
            bool shouldremoveSelf = false;
            Collider myCollider = new Collider();
            foreach(var collision in collisions)
            {
                if(collision.gameObject.name == _self.name)
                {
                    myCollider = collision;
                    shouldremoveSelf = true;
                }
            }
            if (shouldremoveSelf )
            {
                collisions.Remove(myCollider);
            }

            if (collisions.Any())
            {
                this._brain.target = collisions.First().gameObject;
                Debug.Log(_self.name +" has found "+ _brain.target.name);
            }
        }

        public void Exit()
        {
          
        }
    }
}
