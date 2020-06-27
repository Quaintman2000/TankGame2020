using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
    public float noiseRadius = 0f;
    public float closeEnough;

    public float NoiseRadius
    {
        get { return noiseRadius; }
        set
        {
            noiseRadius = Mathf.Max(noiseRadius, value);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(noiseRadius > 0)
        {
            noiseRadius = noiseRadius * 0.7f;
            if(noiseRadius <= closeEnough)
            {
                noiseRadius = 0;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, noiseRadius);
    }
    public void AddNoise(float noiseLevel)
    {
        noiseRadius = noiseLevel;
    }
}
