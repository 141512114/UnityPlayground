using UnityEngine;

public abstract class AbstractMovement : MonoBehaviour
{
    public abstract void Move();

    protected abstract void ClampVelocity();
}
