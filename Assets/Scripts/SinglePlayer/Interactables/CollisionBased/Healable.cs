using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healable : CollisionBasedInteractableObject, IHealthBoost
{
    private CPlayerHealth _playerHealth;

    private void Awake()
    {
        _playerHealth = player.GetComponent<CPlayerHealth>();
    }
    
    public float totalAmountToHeal;


    public void ImmediateHealthIncrease(float amount)
    {
        if(player == null) return;
        player.GetComponent<CPlayerHealth>().Heal(amount);
    }

    public IEnumerator DelayedHealthIncrease(float amountToIncrease, float totalTimeToIncrease)
    {
        float currentTime = 0;
        
        while(currentTime < totalAmountToHeal){
            _playerHealth.Heal(Mathf.Lerp(
                _playerHealth.currentHealth,
                 _playerHealth.currentHealth + amountToIncrease,
                  Time.deltaTime * totalTimeToIncrease));

            currentTime = 0;
        }
        yield return null;
    }
}
