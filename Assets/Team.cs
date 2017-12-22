using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets
{
    public class Team
    {
        public string Name { get; set; }
        public List<GameObject> TeamMembers { get; set; }
        public Color TeamColor;

        public bool IsActive
        {
            get
            {
                foreach (var member in TeamMembers)
                {
                    var brain = member.GetComponentInChildren<Brain>();
                    
                    if (brain.Character.IsActive())
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public Team()
        {
            TeamMembers = new List<GameObject>();
        }

        public void AddCharacter(GameObject go)
        {
            TeamMembers.Add(go);
        }
    }
}
