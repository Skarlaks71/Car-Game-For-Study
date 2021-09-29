using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarTest : MonoBehaviour
{
    private List<NodePath> openPoints;
    private List<NodePath> closePoints;

    public int xAxis = 2;
    public int yAxis = 2;
    public Vector3[] grid;
    public GameObject tileGrid;
    public int gridResolution;

    // Start is called before the first frame update
    void Start()
    {
        gridResolution = xAxis*yAxis;
        grid = new Vector3[gridResolution];
        for (int i = 0, x = 0; x < xAxis; x++)
        {
            for (int y = 0; y < yAxis; y++, i++)
            {
                Vector3 pos = GetCoordinateGrid(x, y);
                Vector3 tilePos = new Vector3(pos.x, 1, pos.y);
                Debug.Log(tilePos);
                grid[i] = Instantiate(tileGrid, tilePos, Quaternion.identity).transform.localPosition;

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 GetCoordinateGrid(float x, float y)
    {

        return new Vector3(
            x - ((gridResolution - 1) * tileGrid.transform.localScale.x),
            y - ((gridResolution - 1) * tileGrid.transform.localScale.y));
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

    struct NodePath
    {
        int fCost;
        int gCost;
        int hCost;
        Vector3 posNode;

        public NodePath(int _fCost, int _gCost, int _hCost, Vector3 _posNode)
        {
            fCost = _fCost;
            gCost = _gCost;
            hCost = _hCost;
            posNode = _posNode;
        }
    }
}
