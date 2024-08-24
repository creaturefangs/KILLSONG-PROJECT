using System.Collections;

public interface IHealthBoost
{
    /// <summary>
    /// Immediately increase health by specified amount.
    /// </summary>
    /// <param name="amount"></param>
    void ImmediateHealthIncrease(float amount);

    /// <summary>
    /// Increases health by specified amount over specificed amount of time.
    /// </summary>
    /// <param name="amountToIncrease"></param>
    /// <param name="totalTimeToIncrease"></param>
    /// <returns></returns>
    IEnumerator DelayedHealthIncrease(
        float amountToIncrease, float totalTimeToIncrease);    
}
