
using System;
using System.Collections.Generic;
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
        private Action<GameObject> OnNewTargetFound;
        public string Name { get { return "Searching"; } }
        public FindTargetState(GameObject self, Brain brain, float searchRadius, LayerMask layerMask, Action<GameObject> cbOnTargetFound)
        {
            this._self = self;
            this._brain = brain;
            this._searchRadius = searchRadius;
            this._layerMask = layerMask;
            OnNewTargetFound += cbOnTargetFound;
        }

        public void Enter()
        {
            Debug.Log(_self.gameObject.name + " has entered Search Mode");
        }

        public void Execute()
        {
            var collisions = Physics.OverlapSphere(_self.transform.position + Vector3.up, _searchRadius).ToList() ;
            bool shouldremoveSelf = false;
            Collider myCollider = new Collider();
            List<Collider> deadColliders = new List<Collider>();
            foreach(var collision in collisions)
            {
                if(collision.gameObject.name == _self.name)
                {
                    myCollider = collision;
                    shouldremoveSelf = true;
                }
                if (collision.gameObject.GetComponentInChildren<Brain>().IsDead)
                {
                    deadColliders.Add(collision);
                }
            }


            if (shouldremoveSelf )
            {
                collisions.Remove(myCollider);
            }
            collisions = collisions.Except(deadColliders).ToList();

            if (collisions.Any())
            {
                OnNewTargetFound(collisions.First().gameObject);
            }
        }

        public void Exit()
        {
          
        }
    }
}
