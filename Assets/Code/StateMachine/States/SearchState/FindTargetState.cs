
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
        private readonly string _layerMask;
        private Action<GameObject> OnNewTargetFound;
        public string Name { get { return "Searching"; } }
        public string StateType {  get { return "search"; } }
        public string Who { get { return _self.gameObject.name; } }
        public FindTargetState(GameObject self, Brain brain, float searchRadius, string layerMask, Action<GameObject> cbOnTargetFound)
        {
            this._self = self;
           
            this._brain = brain;
            this._searchRadius = searchRadius;
            _layerMask = layerMask;
            OnNewTargetFound += cbOnTargetFound;
        }

        public void Enter()
        {
        }

        public void Execute()
        {
            var collisions = Physics.OverlapSphere(_self.transform.position + Vector3.up, _searchRadius).ToList() ;
            
            bool shouldremoveSelf = false;
            Collider myCollider = new Collider();
            List<Collider> deadColliders = new List<Collider>();

            var search = collisions.Where(x => x.gameObject.layer == LayerMask.NameToLayer(_layerMask)).ToList();

            if (_layerMask == "Battle")
            {
                foreach (var item in search)
                {
                    if (item.gameObject.name == _self.name)
                    {
                        myCollider = item;
                        shouldremoveSelf = true;
                    }
                    var itemBrain = item.gameObject.GetComponentInChildren<Brain>();
                    if (itemBrain.Character.IsDead()|| itemBrain.TeamName == _brain.TeamName)
                    {
                        deadColliders.Add(item);
                    }

                }

                if (shouldremoveSelf)
                {
                    search.Remove(myCollider);
                }
                search = search.Except(deadColliders).ToList();
            }

            if (search.Any())
            {
             
                var picked = search[UnityEngine.Random.Range(0, search.Count())];

                OnNewTargetFound(picked.gameObject);
            }
        }

        public void Exit()
        {
          
        }
    }
}
