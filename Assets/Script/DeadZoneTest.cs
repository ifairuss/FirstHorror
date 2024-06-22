using UnityEngine;

public class DeadZoneTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            HealthManager.OnTakeDamage(30);
        }
    }
}
