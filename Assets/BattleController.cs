using System;
using System.Collections.Generic;
using System.Linq;
using Assets;
using UnityEngine;
public class BattleController : MonoBehaviour
{
    public List<Team> Teams;
	// Use this for initialization
	
	// Update is called once per frame
    private Action OnBattleComplete;

	void Update ()
	{

	    SetupTeams();

	    if (Teams.Count(x => x.IsActive) ==1)
	    {
	        //game over.
	        var winner = Teams.SingleOrDefault(x => x.IsActive);
            Debug.Log("Winning Team is: " + winner.Name);
	        OnBattleComplete();
	    }
	}

    public void Register_OnBattleComplete(Action battleComplete)
    {
        OnBattleComplete += battleComplete;
    }
    void SetupTeams()
    {
        if (Teams == null)
        {
            Teams = new List<Team>();

            var brains = FindObjectsOfType<Brain>();
            bool swap = false;
            var teama = new Team {Name = "Team A"};
            var teamb = new Team {Name = "Team B"};
            float t = -3;
            foreach (var brain in brains)
            {
                
                var character = brain.Character;

                if (swap)
                {
                    brain.root.transform.position= new Vector3(-5,t);
                    teama.TeamMembers.Add(character);
                    brain.TeamName = teama.Name;
                    swap = false;
                    continue;
                    
                }
                else
                {
                    
                    brain.root.transform.position = new Vector3(5, t);
                    teamb.TeamMembers.Add(character);
                    brain.TeamName = teamb.Name;
                    swap = true;
                }

                t += 1.5f;
            }
            Teams.Add(teama);
            Teams.Add(teamb);

            foreach (var team in Teams)
            {
                Debug.Log(team.Name);
                foreach (var member in team.TeamMembers)
                {
                    Debug.Log(member.Name);
                }
            }
        }
    }
}
