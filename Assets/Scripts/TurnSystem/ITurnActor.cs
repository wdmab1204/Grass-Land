using UnityEngine;

namespace TurnSystem
{
    public abstract class TurnActor : MonoBehaviour
    {
        public abstract System.Collections.IEnumerator ActionCoroutine();
    }
}
