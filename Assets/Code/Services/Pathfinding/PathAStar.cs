using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Services.Pathfinding;
using Priority_Queue;
using System.Linq;

public class PathAStar  {

	Queue<Tile> _path;

	public PathAStar(TileDataGrid tileData,Tile tileStart, Tile tileEnd){

		//check to see if we need to create a tile graph.
		if (GameManager.Instance.TileGraph == null) {
			GameManager.Instance.TileGraph = new PathTileGraph (tileData);
		}
		//walkable list of nodes.
		Dictionary<Tile, PathNode<Tile>> nodes = GameManager.Instance.TileGraph.nodes;

		if(nodes.ContainsKey(tileStart)){
			Debug.LogError("PAthAStar: start tile isnt in the list of tile graphs");
			return;
		}
		if(nodes.ContainsKey(tileEnd)){
			Debug.LogError("PAthAStar: end tile isnt in the list of tile graphs");
			return;
		}

		PathNode<Tile> start = nodes [tileStart];
		PathNode<Tile> goal = nodes [tileEnd];


		List<PathNode<Tile>> ClosedSet = new List<PathNode<Tile>> ();// nodes already to evaluate.
		//List<PathNode<Tile>> OpenSet = new List<PathNode<Tile>> ();
		//OpenSet.Add (start);

		SimplePriorityQueue<PathNode<Tile>> OpenSet = new SimplePriorityQueue<PathNode<Tile>> ();
		OpenSet.Enqueue (start,0);

		Dictionary<PathNode<Tile>, PathNode<Tile>> Came_From = new Dictionary<PathNode<Tile>, PathNode<Tile>> ();
		Dictionary<PathNode<Tile>, float> g_score = new Dictionary<PathNode<Tile>, float> ();

		foreach (var n in nodes.Values) {
			g_score [n] = Mathf.Infinity; // ensure all nodes start with a massive movement value.
		}
		g_score [start] = 0;


		Dictionary<PathNode<Tile>, float> f_score = new Dictionary<PathNode<Tile>, float> ();
		foreach (var n in nodes.Values) {
			f_score [n] = Mathf.Infinity; // ensure all nodes start with a massive movement value.
		}
		f_score [start] = heuristic_cost_estimate (start, goal);

		while (OpenSet.Count > 0) {
			PathNode<Tile> current = OpenSet.Dequeue ();

			if (current == goal) {
				//TODO: found the path;
				reconstruct_path(Came_From, current);
				return;
			}

			ClosedSet.Add (current);

			foreach(PathEdge<Tile> edge_neighbour in current.Edges){

				var neighbour = edge_neighbour.Node;

				if(ClosedSet.Contains(neighbour)) { continue;} //already completed neighbour.

				float movementcostToNeighbour = neighbour.Data.MovementCost * dist_between (current, neighbour);

				float tentative_g_score = g_score [current] + movementcostToNeighbour;

				if(OpenSet.Contains(neighbour) && tentative_g_score >= g_score[neighbour]){
					continue;
				}

				Came_From [neighbour] = current;
				g_score [neighbour] = tentative_g_score;
				f_score [neighbour] = g_score [neighbour] = heuristic_cost_estimate(neighbour, goal);

				if (!OpenSet.Contains (neighbour)) {
					OpenSet.Enqueue (neighbour, f_score[neighbour]);
				}
			}//foearch neighbour
		}//while

		//no path from start to goal.

	}

	void reconstruct_path(Dictionary<PathNode<Tile>, PathNode<Tile>> Came_From, PathNode<Tile> current){
		//so curent is the goal.
		Queue<Tile> total_path = new Queue<Tile>();
		total_path.Enqueue (current.Data);

		while (Came_From.ContainsKey (current)) {
			current = Came_From [current];
			total_path.Enqueue (current.Data);
		}

		//total path is a queue running backwards from the end to the start tile.

		_path =  new Queue<Tile> (total_path.Reverse ());
	}

	float dist_between(PathNode<Tile> a, PathNode<Tile> b)
	{
		//neighbours have dist of 1.
		//diags have distance of 1.414;

		if (Mathf.Abs(a.Data.X - b.Data.X) + 
			Mathf.Abs(a.Data.Y - b.Data.Y)==1){
			return 1f;
		}

		if (Mathf.Abs(a.Data.X - b.Data.X) == 1 && 
			Mathf.Abs(a.Data.Y - b.Data.Y) == 1 ){
			return 1.41421356237f;
		}

		return Mathf.Sqrt (
			Mathf.Pow(a.Data.X - b.Data.X,2) +
			Mathf.Pow(a.Data.Y - b.Data.Y,2)
		);

	}

	float heuristic_cost_estimate(PathNode<Tile> a, PathNode<Tile> b)
	{
		return Mathf.Sqrt (
			Mathf.Pow(a.Data.X - b.Data.X,2) +
			Mathf.Pow(a.Data.Y - b.Data.Y,2)
		);
	}

	public Tile Dequeue(){
		return _path.Dequeue ();
	}

	public int Length(){
		if (_path != null) {
			return _path.Count;
		}
		return 0;
	}
}
