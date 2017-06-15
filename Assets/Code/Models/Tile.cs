using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile 
{
	public int X { get; protected set; }
	public int Y { get; protected set; } 
    public int Floor { get; set; }
	public bool Tree { get; set; }
    public int Wall { get; set; }
    public bool Embelishment { get; set; }

	public Tile(int x, int y, int floor, bool tree,  bool embelishment)
	{
		X = x;
		Y = y;
        Floor = floor;
        Tree = tree;
        Wall = Wall;
        Embelishment = embelishment;
	}
		
}
