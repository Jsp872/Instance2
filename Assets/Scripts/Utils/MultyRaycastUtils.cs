// RaycastUtils.cs
using UnityEngine;

public static class MultyRaycastUtils
{
    public static RaycastHit2D MultiRaycast(Transform origin, Vector2 direction, float distance,
        int count, Vector2 spreadAxis, float spread, int layerMask = Physics2D.AllLayers)
    {
        RaycastHit2D anyHit = new RaycastHit2D();

        if (count <= 1)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin.position, direction, distance, layerMask);
#if UNITY_EDITOR
            Debug.DrawLine(origin.position, (Vector2)origin.position + direction * distance, hit ? Color.green : Color.red);
#endif
            return hit;
        }

        Vector2 worldSpread = origin.TransformDirection(spreadAxis).normalized * spread;

        for (int i = 0; i < count; i++)
        {
            float t = (float)i / (count - 1);
            Vector2 startPos = (Vector2)origin.position + worldSpread * (t - 0.5f);

            RaycastHit2D hit = Physics2D.Raycast(startPos, direction, distance, layerMask);
            anyHit = hit;

#if UNITY_EDITOR
            Debug.DrawLine(startPos, startPos + direction * distance, hit ? Color.green : Color.red);
#endif
        }

        return anyHit;
    }
}