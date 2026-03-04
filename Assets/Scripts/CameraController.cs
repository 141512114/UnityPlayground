using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CameraInstance[] cameras;
    public Transform        target;
    public float            followSpeed = 5f;

    private CameraInstance _currentCameraInstance;

    private void Start()
    {
        // Aktiviere die erste Kamera und deaktiviere die anderen
        for ( int i = 0; i < cameras.Length; i++ )
        {
            CameraInstance cInstance = cameras[ i ];
            Camera         cam       = cInstance.GetCamera();

            cam.gameObject.SetActive( i == 0 || cInstance.IsMainCamera() );
            if ( i == 0 || cInstance.IsMainCamera() ) _currentCameraInstance = cInstance;
        }

        // Wenn die aktuelle Kamera die Hauptkamera ist, deaktiviere die erste Kamera aus der Liste, damit sie nicht mit der Hauptkamera kollidiert
        if ( _currentCameraInstance.IsMainCamera() ) cameras[ 0 ].GetCamera().gameObject.SetActive( false );

        transform.position = target.position;
        // Setze die Kamera auf die gleiche Höhe wie das Ziel, aber mit einem festen Abstand
        Camera  currentCamera = _currentCameraInstance.GetCamera();
        Vector3 initPosition  = new( transform.position.x, transform.position.y, currentCamera.transform.position.z );
        currentCamera.transform.position = Vector3.Lerp( currentCamera.transform.position, initPosition, followSpeed * Time.deltaTime );
    }

    private void Update()
    {
        // Wechsle die Kamera mit der Tab-Taste
        if ( !Input.GetKeyDown( KeyCode.Tab ) ) return;
        int currentIndex = Array.IndexOf( cameras, _currentCameraInstance );
        int nextIndex    = ( currentIndex + 1 ) % cameras.Length;

        // Deaktiviere die aktuelle Kamera und aktiviere die nächste
        _currentCameraInstance.GetCamera().gameObject.SetActive( false );
        _currentCameraInstance = cameras[ nextIndex ];
        _currentCameraInstance.GetCamera().gameObject.SetActive( true );
    }

    private void FixedUpdate()
    {
        transform.position = target.position;

        // Wenn die Kamera statisch ist, soll sie sich nicht bewegen
        if ( _currentCameraInstance.IsStatic() ) return;

        Camera  currentCamera   = _currentCameraInstance.GetCamera();
        Vector3 desiredPosition = new( transform.position.x, transform.position.y, currentCamera.transform.position.z );
        // Bewege die Kamera sanft zur gewünschten Position
        currentCamera.transform.position = Vector3.Lerp( currentCamera.transform.position, desiredPosition, followSpeed * Time.deltaTime );
    }
}
