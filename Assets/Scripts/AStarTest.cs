using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarTest : MonoBehaviour
{
    private List<NodePath> openPoints;
    private List<NodePath> closePoints;

    public float xAxisBorder = 3;
    public float yAxisBorder = 3;
    public float xAxis = 3;
    public float yAxis = 3;
    public Vector2 cubeSize;
    public float contactRadius = .5f;
    public NodePath[,] grid;
    public Transform player;
    public Transform target;
    public int gridResolution;
    public LayerMask unwalkableMask;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(yAxisBorder / yAxis);
        cubeSize = new Vector2( (float)(xAxisBorder / xAxis), (float)(yAxisBorder / yAxis) );
        
        //gridResolution = xAxis*yAxis;
        grid = new NodePath[(int)xAxis, (int)yAxis];
        Vector3 worldBottomLeft = transform.position - Vector3.right * xAxisBorder / 2 - Vector3.forward * yAxisBorder / 2;
        for (int x = 0; x < xAxis; x++)
        {
            for (int y = 0; y < yAxis; y++)
            {
                Vector3 worldPoint = worldBottomLeft + (x* cubeSize.x + contactRadius)*Vector3.right + (y* cubeSize.y+ contactRadius) * Vector3.forward;
                bool walkable = !Physics.CheckSphere(worldPoint, contactRadius, unwalkableMask);
                grid[x, y] = new NodePath(worldPoint,walkable); 
                
                //Debug.Log(tilePos);


            }
        }
        //SetNodePathPosition();
    }

    /*void SetNodePathPosition()
    {
        foreach(Vector3 posGrid in grid)
        {
            NodePath newNode = new NodePath();
            newNode.posNode = posGrid;
            openPoints.Add(newNode);
        }
        openPoints.First();

    }*/

    // Update is called once per frame
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, new Vector3(xAxisBorder, 1, yAxisBorder));

        if(grid != null)
        {
            foreach (NodePath np in grid)
            {
                Gizmos.color = (np.obstacles)? Color.green: Color.red;
                Gizmos.DrawCube(np.posNode, Vector3.one * (cubeSize.x - .1f));
                Gizmos.DrawWireSphere(np.posNode, contactRadius*cubeSize.x);

            }
        }

    }

    Vector3 GetCoordinateGrid(float x, float y)
    {

        return new Vector3(
            x - ((gridResolution - 1) ),
            y - ((gridResolution - 1) ));
    }

    float CalculateManhattanDistance(Vector3 start, Vector3 end, float numSeg)
    {
        float d = 0;
        for (int i=0; i<numSeg; i++)
        {
            d += (end - start).magnitude;
        }
        return d;
    }

    public struct NodePath
    {
        public int fCost;
        public int gCost;
        public int hCost;
        public Vector3 posNode;
        public bool obstacles;

        public NodePath(Vector3 _posNode, bool _obstacles)
        {
            fCost = 0;
            gCost = 0;
            hCost = 0;
            posNode = _posNode;
            obstacles = _obstacles;
        }

        public NodePath(int _fCost, int _gCost, int _hCost, Vector3 _posNode, bool _obstacles)
        {
            fCost = _fCost;
            gCost = _gCost;
            hCost = _hCost;
            posNode = _posNode;
            obstacles = _obstacles;
        }

        
    }
}
