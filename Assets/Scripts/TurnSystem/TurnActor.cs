using System.Collections;
using TMPro;
using UnityEngine;

namespace TurnSystem
{
    public abstract class TurnActor : MonoBehaviour
    {
        public abstract System.Collections.IEnumerator ActionCoroutine();
        public virtual IEnumerator GoDestination(Vector3 targetPosition)
        {
            while (true)
            {
                float distance = Vector3.Distance(transform.position, targetPosition);

                // 목표 위치에 도달하지 않은 경우 이동
                if (distance > 0.1f)
                {
                    // 목표 위치까지 이동 벡터 계산
                    Vector3 direction = (targetPosition - transform.position).normalized;
                    Vector3 movement = direction * 1 * Time.deltaTime;

                    // 이동
                    transform.position += movement;
                    yield return null;

                }
                else
                {
                    yield break;
                }
            }
        }
    }
}
