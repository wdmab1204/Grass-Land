using UnityEngine;
using TurnSystem;
using System.Collections;

[DisallowMultipleComponent]
public class MonsterActor : MonoBehaviour, ITurnActor
{
    public GameObject ActorObject { get; set; }
    public ActorState ActorState { get; set; }

    

    public IEnumerator ActionCoroutine()
    {
        ActorState = ActorState.Start;
        Debug.Log("Im Monster!!");
        yield return new WaitForSeconds(2.0f);
        Debug.Log("I'll kill you!!");
        yield return new WaitForSeconds(2.0f);
        ActorState = ActorState.End;
    }

    private void Awake()
    {
        ActorObject = this.gameObject;
    }
}

