using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearing : MonoBehaviour
{
    public float hearingDistance;
    private Color gizmoColor;

    public bool CanHear(GameObject target)
    {
        //grab the target's noisemaker
        NoiseMaker targetNoiseMaker = target.GetComponent<NoiseMaker>();

        if (targetNoiseMaker != null)
        {
            Vector3 vectorToTarget = target.transform.position - transform.position;
            if (hearingDistance + targetNoiseMaker.noiseRadius > vectorToTarget.magnitude)
            {
                gizmoColor = Color.green;
                return true;
            }
        }
        gizmoColor = Color.red;
        return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, hearingDistance);
    }
}
