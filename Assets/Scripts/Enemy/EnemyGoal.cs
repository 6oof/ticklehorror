using UnityEngine;

public class EnemyGoal:MonoBehaviour
{
    public string description = "Default task";
    public float waitTime = 3;
    

    void OnDrawGizmos()
    {   
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS( transform.position, transform.rotation, Vector3.one );
        Gizmos.DrawFrustum(Vector3.zero, 30, 1,0,1);
    }
}
