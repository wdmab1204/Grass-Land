using UnityEngine;
using TurnSystem;
using System.Collections;

[DisallowMultipleComponent]
public class PlayerActor : MonoBehaviour, ITurnActor
{
    public GameObject Actor { get; set; }
    public ActorState ActorState { get; set; }

    public IEnumerator ActionCoroutine()
    {
        ActorState = ActorState.Start;
        Debug.Log("Im player!!");
        yield return new WaitForSeconds(2.0f);
        Debug.Log("Thank you!!");
        yield return new WaitForSeconds(2.0f);
        ActorState = ActorState.End;
    }

    private void Awake()
    {
        Actor = this.gameObject;
    }
}

