
public class Tile 
{
	public int X { get; protected set; }
	public int Y { get; protected set; } 
    public int Floor { get; set; }

	public Tile(int x, int y, int floor)
	{
		X = x;
		Y = y;
        Floor = floor;
	}
		
}
