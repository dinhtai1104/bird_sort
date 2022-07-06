using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
   **********************
	Author : Taii
	Company: SuperGame

   **********************
*/

public class BranchBezierPath : MonoBehaviour
{
	public Transform p0, p1;
    public Transform targetForTest;
    public int step;
	public List<Vector2> CreatePath(Vector3 target)
    {
        List<Vector2> path = new List<Vector2>();
        for (float t = 0; t <= 1; t += 1.0f / step)
        {
            Vector2 bezierPath = Bezier.GetPositionAtTime(p0.position, p1.position, target, t);
            path.Add(bezierPath);
        }
        path.Add(target);
        return path;
    }

    private void OnDrawGizmos()
    {
        if (p0 && p1 && targetForTest) 
        {
            Vector2 lastPos = p0.position;
            for (float t = 0; t <= 1; t += 1.0f / step)
            {
                Vector2 bezierPath = Bezier.GetPositionAtTime(p0.position, p1.position, targetForTest.position, t);
                Gizmos.DrawLine(lastPos, bezierPath);
                lastPos = bezierPath;
            }
            Gizmos.DrawLine(lastPos, targetForTest.position);
        }
    }
}
public class Bezier
{
    public static Vector2 GetPositionAtTime(Vector2 p0, Vector2 p1, Vector2 p2, float time)
    {
        return (1 - time) * (1 - time) * p0 +
            2 * (1 - time) * time * p1 +
            time * time * p2;
    }

    public static Vector3 GetPositionAtTime(Vector3 p0, Vector3 p1, Vector3 p2, float time)
    {
        return (1 - time) * (1 - time) * p0 +
            2 * (1 - time) * time * p1 +
            time * time * p2;
    }
}