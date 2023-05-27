using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트에 Rigidbody가 있는지 확인
        Rigidbody otherRigidbody = collision.gameObject.GetComponent<Rigidbody>();
        if (otherRigidbody != null)
        {
            // 충돌한 오브젝트에 힘을 가해서 움직이도록 함
            Vector3 force = collision.relativeVelocity; // 상대 속도에 비례하는 힘을 가함 (원하는 값으로 조정 가능)
            otherRigidbody.AddForce(force, ForceMode.Impulse);
        }
    }
}