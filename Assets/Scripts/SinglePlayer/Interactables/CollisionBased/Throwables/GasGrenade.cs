using UnityEngine;

public class GasGrenade : Throwable
{
    [SerializeField] private GameObject gasParticles;
    public void HandleGas()
    {
        gasParticles.SetActive(true);
    }
}
