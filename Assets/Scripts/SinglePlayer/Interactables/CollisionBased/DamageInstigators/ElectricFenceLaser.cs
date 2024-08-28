using UnityEngine;

public class ElectricFenceLaser : ElectricFence
{
    private new void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(triggerTagCheck)) return;

        //Allows for damage infliction (calls DamageInstigator and Electric Fence's OnTriggerEnter event).
        base.OnTriggerEnter(other);
        EnvironmentalSoundController.Instance.PlaySoundAtLocation(base.zapSound, 1.0f, other.transform.position);
    }
}
