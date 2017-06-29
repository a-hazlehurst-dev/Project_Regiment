using UnityEngine;
using System.Collections.Generic;

namespace Assets.Code.Services.Pathfinding
{
    public class PathTileGraph
    {
        //pathfinding compatible graph of the world and determines the WALKABLE map of the world.

        public Dictionary<Tile, PathNode<Tile>> nodes;

        public PathTileGraph(TileDataGrid tileDataGrid)
        {
            nodes = new Dictionary<Tile, PathNode<Tile>>();

            for (int x = 0; x < tileDataGrid.GridWidth; x++)
            {
                for (int y = 0; y < tileDataGrid.GridHeight; y++)
                {
                    Tile t = tileDataGrid.GetTileAt(x, y);

                    //if (t.MovementCost > 0) //tiles with 0 are not walkable.
                    //{
                        //can be walked on.
                        var node = new PathNode<Tile>();
                        node.Data = t;
                        nodes.Add(t, node);
                    //}
                }

            }
            Debug.Log("nodes created: " + nodes.Count);
            int edgeCount = 0;
            foreach(var t in nodes.Keys)
            {
                //get tiles, tile neighbours and add them as edges to this node.
                PathNode<Tile> node = nodes[t];
                List<PathEdge<Tile>> edges = new List<PathEdge<Tile>>();
                Tile[] neighbours = t.GetNeighbours(true);

                for (int i = 0; i < neighbours.Length; i++)
                {
                    if(neighbours[i]!=null && neighbours[i].MovementCost > 0)
                    {
						//neibours exist and is walkable.
						//check for diagonal clipping.
						if(IsDiaganolClipping(t, neighbours[i])){
							continue;  // dont create an edge (diag is blocked.)
						}

                        PathEdge<Tile> pathEdge = new PathEdge<Tile>();
                        pathEdge.Cost = neighbours[i].MovementCost;
                        pathEdge.Node = nodes[neighbours[i]];
                        edges.Add(pathEdge);
                        edgeCount++;
                    }
                }
                node.Edges = edges.ToArray();
            }
            Debug.Log("edges created: " + edgeCount);
        }

		bool IsDiaganolClipping(Tile curr, Tile neighbour){
			if(Mathf.Abs(curr.X - neighbour.X) + Mathf.Abs(curr.Y - neighbour.Y)==2){
				
				int dX = curr.X - neighbour.X;
				int dY = curr.Y - neighbour.Y;

				if (GameManager.Instance.TileDataGrid.GetTileAt (curr.X -dX, curr.Y).MovementCost == 0) {
					//is clipped.
					return true;
				}

				if (GameManager.Instance.TileDataGrid.GetTileAt (curr.X , curr.Y - dY).MovementCost == 0) {
					//is clipped.
					return true;
				}

			}

			return false;
		}
    }


}
