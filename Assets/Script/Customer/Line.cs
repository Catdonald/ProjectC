using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    private List<Vector3> linePositions = new List<Vector3>();
    public List<Vector3> LinePositions => linePositions;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
        {
            linePositions.Add(child.position);
        }
    }

    public Vector3 GetLinePosition(int index)
    {
        return linePositions[index];
    }

#if UNITY_EDITOR
    [SerializeField] private Color pointColor = Color.blue;
    [SerializeField] private Color lineColor = Color.green;
    [SerializeField] private float pointSize = 0.3f;
    private void OnDrawGizmos()
    {
        Gizmos.color = pointColor;
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Gizmos.DrawSphere(child.position, pointSize);
        }

        Gizmos.color = lineColor;
        for(int i = 0; i < transform.childCount - 1; i++)
        {
            Transform startPosition = transform.GetChild(i);
            Transform endPosition = transform.GetChild(i+1);
            Gizmos.DrawLine(startPosition.position, endPosition.position);
        }
    }
#endif
}
