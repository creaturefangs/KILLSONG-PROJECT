using UnityEngine;

public class ElectricFenceLaser : ElectricFence
{
    private new void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(triggerTagCheck)) return;
        
        EnvironmentalSoundController.Instance.PlaySoundAtLocation(base.zapSound, 1.0f, other.transform.position);
    }
}
