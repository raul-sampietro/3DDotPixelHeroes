using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightSwordSwipe : MonoBehaviour
{
    public void SwipeTo(GameObject player, Vector3 to) {
        if (Vector3.Angle(transform.forward, to) == 0) return;

        Quaternion.FromToRotation(transform.forward, to)
            .ToAngleAxis(out float angle, out Vector3 axis);
        if (axis.y < 0.0f)
            angle = -angle;
        if (Mathf.Approximately(Mathf.Abs(angle), 45f))
            transform.RotateAround(player.transform.position, player.transform.up, angle*2);
    }
}
