using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Code.World;

namespace Assets
{
    public class Team
    {
        public string Name { get; set; }
        public List<BaseCharacter> TeamMembers { get; set; }

        public bool IsActive
        {
            get
            {
                foreach (var member in TeamMembers)
                {
                    if (member.IsActive())
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public Team()
        {
            TeamMembers = new List<BaseCharacter>();
        }

        public Team(List<BaseCharacter> teamMembers)
        {
            TeamMembers = teamMembers;
        }

        


    }
}
