using UnityEngine;

public class Bullet : DamageInstigator
{
    private new void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
