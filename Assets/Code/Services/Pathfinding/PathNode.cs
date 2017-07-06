namespace Assets.Code.Services.Pathfinding
{
    public class PathNode<T>
    {
        public T Data { get; set; }

        public PathEdge<T>[] Edges { get; set; }  //nodes leading away from this node.
        
        
         
    }
}
