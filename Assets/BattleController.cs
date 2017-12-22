using System;
using System.Collections.Generic;
using System.Linq;
using Assets;
using NUnit.Framework.Constraints;
using UnityEngine;
public class BattleController : MonoBehaviour
{
    public List<Team> Teams;
    public GameObject go_character;
    private TeamCreator _teamCreator;
    private Action OnBattleComplete;

    void Start()
    {
        _teamCreator = new TeamCreator();
    }

	void Update ()
	{
	    SetupTeams();

	    CheckBattleCompleted();
	}

    public void CheckBattleCompleted()
    {
        if (Teams.Count(x => x.IsActive) == 1)
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

    private void SetupTeams()
    {
        if (Teams != null) return;

        Teams = new List<Team>();

        CreateTeams(2);
    }

    private void CreateTeams(int count)
    {
        if (!_teamCreator.IsRequestedTeamCountValid(count))
        {
            return;
        }

        Teams = _teamCreator.CreateTeams(count);

        _teamCreator.AssignPlayersTeams(Teams, go_character);
    }
}

public class TeamCreator: MonoBehaviour
{
    private Dictionary<string,Color> colours;
    private int teamMemberCount = 10;

    public TeamCreator()
    {
        colours = new Dictionary<string, Color>
        {
            {"yellow", Color.yellow},
            {"green", Color.green},
            {"cyan", Color.cyan},
            {"gray", Color.gray},
            {"blue", Color.blue}
        };
    }
    public bool IsRequestedTeamCountValid(int count)
    {
        if (count > 1 && count <= 4) return true;

        Debug.Log("Wrong number of teams");

        return false;
    }

    public List<Team> CreateTeams(int count)
    {
        var teams = new List<Team>();
        do
        {
            var color = colours.ElementAt(UnityEngine.Random.Range(0, colours.Count));

            var team = new Team { Name = "Team: " +color.Key, TeamColor = color.Value};

            teams.Add(team);

            colours.Remove(color.Key);

        } while (teams.Count != count);

        return teams;
    }

    public void AssignPlayersTeams(List<Team> teams, GameObject go)
    {
        var xPos = 5;
        var zPos = 5;

        foreach (var team in teams)
        {
            for (var x = 0; x < teamMemberCount; x++)
            {
                var go_instance = Instantiate(go);

                go_instance.name = team.Name + "-" + x;

                var brain = go_instance.GetComponentInChildren<Brain>();

                brain.Create(team.Name);
                go_instance.transform.localPosition = new Vector3(xPos, zPos, 0);

                var spr = go_instance.GetComponentsInChildren<SpriteRenderer>();

                foreach (var sr in spr)
                {
                    if (sr.name == "Body")
                    {
                        sr.color = team.TeamColor;
                    }
                }

                team.AddCharacter(go_instance);
                zPos -= 1;
            }
             xPos = -5;
            zPos = 5;
        }
    }
}
