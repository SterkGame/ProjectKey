using UnityEngine;
using UnityEngine.AI;
//using Unity.AI.Navigation;
using NavMeshPlus.Extensions;
using Unity.VisualScripting;
using NavMeshPlus.Components;

public class NavMeshUpdater : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;
    public CollectSources2d collectSources2D;

    void Awake()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
        collectSources2D = GetComponent<CollectSources2d>();

        if (navMeshSurface == null)
        {
            Debug.LogError("Не знайдено 'Navigation Surface'");
        }

        if (collectSources2D == null)
        {
            Debug.LogError("Не знайдено 'Navigation CollectSources2D'");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) 
        {
            UpdateNavMesh(); 
        }
    }

    public void UpdateNavMesh()
    {
        if (collectSources2D != null)
        {
            //collectSources2D.CollectSources();
        }

        if (navMeshSurface != null)
        {
            //Debug.Log("Оновлення 2D NavMesh");
            navMeshSurface.BuildNavMesh();
        }
    }
}
