using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexExplosionEffectController : MonoBehaviour
{
    private ParticleSystem.MainModule _particleSystemMainModule;

    private void Awake(){
        _particleSystemMainModule = GetComponent<ParticleSystem>().main;
    }

    public void Init(Color p_color, Vector3 p_position){
        _particleSystemMainModule.startColor = p_color;
        transform.position = p_position;

        Destroy(gameObject, 0.4f);
    }
}
