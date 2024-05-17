using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodIntensy : MonoBehaviour
{
    public float intensy = 1;
    public ParticleSystem BloodFX_impact_col;
    public ParticleSystem Blood_col;
    public ParticleSystem Smoke;
    public ParticleSystem Splash_round;
    public ParticleSystem blood_Ground;
    void Start()
    {
        BloodFX_impact_col.Emit((int)(intensy * 100));
        Blood_col.Emit((int)(intensy*100));
        var main = Smoke.main;
        var col = Smoke.colorOverLifetime;
        col.color = new ParticleSystem.MinMaxGradient(new Color(1, 1, 1, intensy), new Color(1, 1, 1, 0));
        main.startSize = new ParticleSystem.MinMaxCurve( intensy *3);
        var main1 = Splash_round.main;
        var col2 = Splash_round.colorOverLifetime;
        col2.color = new ParticleSystem.MinMaxGradient(new Color(1, 1, 1, intensy), new Color(1, 1, 1, 0));
        main1.startSize = new ParticleSystem.MinMaxCurve(intensy);
        blood_Ground.Emit((int)(intensy * 200));
        Destroy(gameObject,2);
    }
}
