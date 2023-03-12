using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float detectionRadius;

    public ContactFilter2D contactFilter = new ContactFilter2D();

    public LayerMask LayerMask;

    void Update()
    {
        //Collider2D[] results = new Collider2D[5];
        //var objectsDetected = Physics2D.OverlapCircle(transform.position, detectionRadius, contactFilter, results);

        //print(results[0].name);

        var detect = Physics2D.OverlapCircleAll(transform.position, detectionRadius, LayerMask);
        if (detect.Length > 0)
        {
            print(detect[0].name);
        }

        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,detectionRadius);
    }
}