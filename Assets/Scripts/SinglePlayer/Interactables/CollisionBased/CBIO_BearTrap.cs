using UnityEngine;
public class CBIO_BearTrap : DamageInstigator
{
    void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        Debug.Log("Triggered bear trap");
    }

    public void ActivateBearTrap()
    {
        
    }
}
