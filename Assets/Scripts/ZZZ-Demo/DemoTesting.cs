using UnityEngine;
using UnityEngine.UI;

public class DemoTesting : MonoBehaviour
{
    public GameObject barPrefab;  // Drag a UI Image prefab here
    public Vector2 bottomLeftCorner;
    public float baseLength = 2;
    public float height = 1;
    public float shearAngle = 30;  // in degrees

    void Start()
    {
        DrawParallelogram();
    }


    void DrawParallelogram()
    {
        if (barPrefab == null)
        {
            Debug.LogError("Bar prefab not assigned!");
            return;
        }


        // Create and configure the four bars
        GameObject bar1 = Instantiate(barPrefab, transform);
        GameObject bar2 = Instantiate(barPrefab, transform);
        GameObject bar3 = Instantiate(barPrefab, transform);
        GameObject bar4 = Instantiate(barPrefab, transform);


        // Helper function to adjust position and rotation
        void ConfigureBar(GameObject bar, Vector2 start, Vector2 end, float thickness)
        {
            RectTransform rect = bar.GetComponent<RectTransform>();
            rect.position = (start + end) / 2;
            float distance = Vector2.Distance(start, end);
            rect.sizeDelta = new Vector2(distance, thickness);
            float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;
            rect.rotation = Quaternion.Euler(0, 0, angle);
        }


        // Calculate corners
        Vector2 bottomRight = bottomLeftCorner + new Vector2(baseLength, 0);
        Vector2 topLeft = bottomLeftCorner + new Vector2(height * Mathf.Cos(shearAngle * Mathf.Deg2Rad), height * Mathf.Sin(shearAngle * Mathf.Deg2Rad));
        Vector2 topRight = topLeft + new Vector2(baseLength, 0);


        float thickness = 5f; // Thickness of the bar


        ConfigureBar(bar1, bottomLeftCorner, bottomRight, thickness); //bottom
        ConfigureBar(bar2, topLeft, topRight, thickness); //top
        ConfigureBar(bar3, bottomLeftCorner, topLeft, thickness); // left
        ConfigureBar(bar4, bottomRight, topRight, thickness); //right

    }

    void OnDrawGizmos()
    {
        // Calculate corners
        Vector2 bottomRight = bottomLeftCorner + new Vector2(baseLength, 0);
        Vector2 topLeft = bottomLeftCorner + new Vector2(height * Mathf.Cos(shearAngle * Mathf.Deg2Rad), height * Mathf.Sin(shearAngle * Mathf.Deg2Rad));
        Vector2 topRight = topLeft + new Vector2(baseLength, 0);


        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(bottomLeftCorner, bottomRight);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(bottomLeftCorner, topLeft);
        Gizmos.DrawLine(bottomRight, topRight);
    }
}