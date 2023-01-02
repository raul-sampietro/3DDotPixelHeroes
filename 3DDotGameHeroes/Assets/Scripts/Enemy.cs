using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float movSpeed;
    public string playerTag;

    protected Vector3 movDirection = new(0, 0, 0);
    protected float maxRotationSpeed = 180.0f;
    protected string movementPattern;

    protected GameObject knight = null;
    protected Animator animator;

    public void SetMovementPattern(string movementPatter)
    {
        this.movementPattern = movementPatter;
    }

    public void GetAnimator()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    protected GameObject FindPlayer()
    {
        return GameObject.FindGameObjectWithTag(playerTag);
    }

    protected void RotateYAxes(Vector3 direction)
    {
        Quaternion rotation = Quaternion.FromToRotation(transform.forward, direction);
        rotation.ToAngleAxis(out float angle, out Vector3 axis);
        if (angle > maxRotationSpeed * Time.deltaTime) angle = maxRotationSpeed * Time.deltaTime;
        if (axis.y < 0.0f) angle = -angle;
        transform.Rotate(new Vector3(0, 1, 0), angle, Space.World);
    }
}
