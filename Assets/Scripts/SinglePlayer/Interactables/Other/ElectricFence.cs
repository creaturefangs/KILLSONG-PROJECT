using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ElectricFence : DamageInstigator
{
    public float timeSinceLastZap;
    public GameObject zapParticles;
    public float timeToDestroyAfterZap;

    //TODO: 
    //Setup a timer so that zaps dont spam
    public List<ElectricFenceLaser> lasers;
                                                                  //hehe
    public void SpawnAndSetZapLocation(Transform location, GameObject fenctigator, ref bool triggered) 
    {
        GameObject zapParticleSystem = Instantiate(zapParticles, location.position, Quaternion.identity);

        zapParticleSystem.transform.parent = fenctigator.transform;

        StartCoroutine(DestroyAfterTime(zapParticleSystem));
    }

    private IEnumerator DestroyAfterTime(GameObject particleToDestroy)
    {
        yield return new WaitForSeconds(timeToDestroyAfterZap);
        Destroy(particleToDestroy);
    }
}
