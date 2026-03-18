using UnityEngine;

public class BugController : MonoBehaviour
{
    public float animationSpeed = 10f;

    private Animator _anim;

    private int _isMoving;

    private void Start()
    {
        _anim       = GetComponent< Animator >();
        _anim.speed = animationSpeed;

        _isMoving = Animator.StringToHash( "IsMoving" );
    }

    private void Update()
    {
        if ( Input.GetKeyDown( KeyCode.T ) ) _anim.SetTrigger( "Die" );
        if ( Input.GetKeyDown( KeyCode.I ) ) _anim.SetBool( _isMoving, false );
        if ( Input.GetKeyDown( KeyCode.M ) ) _anim.SetBool( _isMoving, true );
    }
}
