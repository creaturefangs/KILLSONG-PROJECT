using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Throwable", order = 3)]
public class SO_Throwable : ScriptableObject
{
    [Tooltip("Name of throwable, could be used for death messages, etc.")]
    public string throwableName;

    [Tooltip("Particle system prefab that plays when throwable is detonated")]
    public GameObject prefabDetonationVfx;

    [Tooltip("Minimum time it takes for this throwable to detonate after release")]
    public float minDetonationTime;

    [Tooltip("Maximum time it takes for this throwable to detonate after release")]
    public float maxDetonationTime;

    [Tooltip("How far can objects be affected by this throwable before not being affected?")]
    [Range(2.0f, 7.5f)]
    public float effectRadius;

    [Tooltip("Maximum range this projectile can be thrown")]
    [Range(2.0f, 20.0f)]
    public float projectileThrowRange;

    [Tooltip("How long does the inflicted effect last after detonation?")]
    [Range(.5f, 6f)]
    public float detonationEffectTime;

    [Tooltip("Time it takes for this object to be destroyed after detonation")]
    [Range(.01f, 1.0f)]
    public float postDetonationDestructionTime;
}