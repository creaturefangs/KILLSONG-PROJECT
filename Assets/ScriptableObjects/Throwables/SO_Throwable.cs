using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Throwable", order = 3)]
public class SO_Throwable : ScriptableObject
{
    //Name of throwable - could be used for death messages etc...
    public string throwableName;
    //Particle system that plays when throwable is detonated
    public GameObject prefabDetonationVfx;
    //How long does it take for this throwable to detonate after release?
    public float minDetonationTime;
    public float maxDetonationTime;
    //How far can the object that is effected by this throwable be before not being effected?
    [Range(2.0f, 7.5f)] public float effectRadius;
    //How far can this projectile be thrown?
    [Range(2.0f, 20.0f)] public float projectileThrowRange;
    //How long does the inflicted effect take to dissipate after detonation?
    [Range(.5f, 6f)] public float detonationEffectTime;
    //How long should it take for this object to be destroyed after detonation?
    [Range(.01f, 1.0f)] public float postDetonationDestructionTime;
}
              