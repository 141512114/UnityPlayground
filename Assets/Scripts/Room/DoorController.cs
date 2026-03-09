using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private new BoxCollider collider;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
