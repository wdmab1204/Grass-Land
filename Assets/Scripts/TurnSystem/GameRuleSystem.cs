using UnityEngine;
using System.Collections;

using TurnSystem;

public class GameRuleSystem : MonoBehaviour
{
    TurnManager turnManager = new TurnManager();
    public GameObject[] Actors;
    private Coroutine currentCoroutine = null;

    private void Start()
    {
        foreach(var obj in Actors)
        {
            var actor = obj.GetComponent<ITurnActor>();
            turnManager.JoinActor(actor);
        }

        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(StartTurnSystemCoroutine());
    }

    private IEnumerator StartTurnSystemCoroutine()
    {
        while (true)
        {
            var currentActor = turnManager.UpdateTurn();

            // Wait until the Actor finished his action
            yield return currentActor.ActionCoroutine();
        }
    }
}

