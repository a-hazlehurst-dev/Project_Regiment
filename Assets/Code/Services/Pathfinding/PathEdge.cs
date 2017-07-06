
namespace Assets.Code.Services.Pathfinding
{
    public class PathEdge<T>
    {
        public float Cost { get; set; } //cost to ENTER the tile.

        public PathNode<T> Node { get; set; }
    }
}
