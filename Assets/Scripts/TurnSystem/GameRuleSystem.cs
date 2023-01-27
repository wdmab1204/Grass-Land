using UnityEngine;
using System.Collections;

using TurnSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameRuleSystem : MonoBehaviour
{
    TurnManager turnManager = new TurnManager();
    private Coroutine currentCoroutine = null;
    [SerializeField] private string CurrentActorName = "";

    private void Start()
    {
        var objArr = SceneManager.GetActiveScene().GetRootGameObjects();
        for (int i = 0; i < objArr.Length; i++)
            if (objArr[i].TryGetComponent<ITurnActor>(out ITurnActor actor))
                turnManager.JoinActor(actor);

        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(StartTurnSystemCoroutine());

    }

    private IEnumerator StartTurnSystemCoroutine()
    {
        while (true)
        {
            var currentActor = turnManager.UpdateTurn();
            CurrentActorName = currentActor.ActorObject.name;

            // Wait until the Actor finished his action
            yield return currentActor?.ActionCoroutine();
        }
    }

    public ITurnActor CurrentActor { get => turnManager.CurrentActor; }

    private void OnDisable()
    {
        StopCoroutine(currentCoroutine);
    }
}

