using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTargetCamera : MonoBehaviour
{
    public List<Transform> targets;
    public Vector2 offset;
    public Vector2 minMaxX;
    public float maxZ;

    public float Zmultiplier;

    private void LateUpdate()
    {
        // VERY MESSY SCRIPT

        if (targets.Count == 0)
            return;
        Vector2 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        // BOUND X
        if (newPosition.x > offset.x + minMaxX.y)
            newPosition.x = offset.x + minMaxX.y;
        if (newPosition.x < offset.x + minMaxX.x)
            newPosition.x = offset.x + minMaxX.x;

        // BOUND Y
        newPosition.y = 1.252385f;
        
        // BOUND Z
        float distanceBetweenTargets = Mathf.Abs(targets[0].position.x - targets[1].position.x);
        float zOffset = distanceBetweenTargets * Zmultiplier;
        zOffset = Mathf.Min(maxZ, zOffset);

        newPosition.z = -10 - zOffset;


        transform.position = newPosition;
    }

    Vector2 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }
}
