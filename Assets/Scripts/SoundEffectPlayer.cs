using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    public AudioSource src;
    public AudioClip sfxArrow, sfxHurt, sfxDying, sfxEnemyDying, sfxFenceBreak, sfxFireGeneration, sfxFootstepsGrass, sfxLampPost, sfxSwordAttack;

    public void Arrow(){
        src.clip = sfxArrow;
        src.Play();
    }
    public void Hurt(){
        src.clip = sfxHurt;
        src.Play();
    }
    public void Dying(){
        src.clip = sfxDying;
        src.Play();
    }
    public void EnemyDying(){
        src.clip = sfxEnemyDying;
        src.Play();
    }
    public void FenceBreak(){
        src.clip = sfxFenceBreak;
        src.Play();
    }
    public void FireGeneration(){
        src.clip = sfxFireGeneration;
        src.Play();
    }
    public void FootstepsGrass(){
        src.clip = sfxFootstepsGrass;
        src.Play();
    }
    public void LampPost(){
        src.clip = sfxLampPost;
        src.Play();
    }
    public void SwordAttack(){
        src.clip = sfxArrow;
        src.Play();
    }
}
