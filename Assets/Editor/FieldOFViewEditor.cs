
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Enemy))]  
public class FieldOFViewEditor : Editor
{
    private void OnSceneGUI()
    {
        Enemy enemy = (Enemy)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(enemy.transform.position, Vector3.up, Vector3.forward, 360, enemy.radius);
        Vector3 viewAngle1 = DirectionFromAngle(enemy.transform.eulerAngles.y, -enemy.angle / 2);
        Vector3 viewAngle2 = DirectionFromAngle(enemy.transform.eulerAngles.y, enemy.angle / 2);
        Handles.color = Color.yellow;
        Handles.DrawLine(enemy.transform.position, enemy.transform.position + viewAngle1 * enemy.radius);
        Handles.DrawLine(enemy.transform.position, enemy.transform.position + viewAngle2 * enemy.radius);

        if (enemy.canSeePlayer)
        {
            Handles.color+= Color.red;
            Handles.DrawLine(enemy.transform.position, enemy.player.transform.position);
        }
    }
    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees*Mathf.Deg2Rad),0, Mathf.Cos(angleInDegrees *Mathf.Deg2Rad));    
    }
}
