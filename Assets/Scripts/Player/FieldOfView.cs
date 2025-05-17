using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius = 5f; // Радіус видимості
    [Range(0, 360)]
    public float viewAngle = 90f; // Кут огляду (можна змінювати)
    public LayerMask obstacleMask; // Перешкоди (Solid)
    public MeshFilter viewMeshFilter;
    public Material invertMaterial;

    private Mesh viewMesh;
    private Transform player;

    void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "Field of View Mesh";
        viewMeshFilter.mesh = viewMesh;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //viewMeshFilter.GetComponent<MeshRenderer>().material = invertMaterial;
        invertMaterial = GetComponent<MeshRenderer>().material;
    }

    void LateUpdate()
    {
        FollowMouse();
        DrawFieldOfView();
        invertMaterial.SetVector("_Center", new Vector4(transform.position.x, transform.position.y, 0, 0));
        invertMaterial.SetFloat("_Radius", viewRadius);
        invertMaterial.SetFloat("_Angle", viewAngle);

    }

    void FollowMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - player.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - viewAngle / 2); // Щоб центр був на мишці
    }

    void DrawFieldOfView()
    {
        List<Vector3> viewPoints = new List<Vector3>();
        float angleStep = viewAngle / 50; // Кількість променів (50 = достатньо для плавності)

        for (int i = 0; i <= 50; i++)
        {
            float angle = transform.eulerAngles.z + angleStep * i;
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, viewRadius, obstacleMask);

            if (hit)
                viewPoints.Add(hit.point);
            else
                viewPoints.Add(transform.position + direction * viewRadius);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < viewPoints.Count; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < viewPoints.Count - 1)
            {
                int triIndex = i * 3;
                triangles[triIndex] = 0;
                triangles[triIndex + 1] = i + 1;
                triangles[triIndex + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }
}