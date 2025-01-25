using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public int damageAmount = 10;


    private void OnTriggerEnter(Collider other)
    {

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();


        if (playerHealth != null)
        {
            playerHealth.UpdateHealth(-damageAmount);
        }
    }
}
