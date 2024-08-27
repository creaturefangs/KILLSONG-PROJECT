using System;
using UnityEngine;
public class CBIO_BearTrap : DamageInstigator
{
    private Animator _anim;
    [SerializeField] private AudioClip[] bearTrapSFX;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void ActivateBearTrap()
    {
        //Plan animation for closing bear trap
        //_anim.SetTrigger("Close");
        //disable damage infliction here just in case
        canDamage = false;
        //play beartrap sound
        EnvironmentalSoundController.Instance.PlayRandomSoundAtLocation(bearTrapSFX, 1.0f, transform.position);
    }
}
