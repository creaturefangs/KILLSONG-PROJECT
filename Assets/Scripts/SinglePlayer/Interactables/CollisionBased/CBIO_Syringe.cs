using UnityEngine;
public class CBIO_Syringe : Healable
{   
    [SerializeField] private AudioClip healthPickupSFX;

    private void Start()
    {
        onInteractionEvent.AddListener(() => EnvironmentalSoundController.Instance.PlaySound2D(healthPickupSFX, 1.0f));
    }
            
    private new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
