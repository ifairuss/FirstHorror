using UnityEngine;

public class DeadZoneTest : MonoBehaviour
{
    [SerializeField] private int damageZone;

    public HealthManager healthManager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            healthManager.OnTakeDamage(damageZone);
        }
    }
}
