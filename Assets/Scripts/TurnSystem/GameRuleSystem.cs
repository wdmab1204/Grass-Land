using UnityEngine;
using System.Collections;

using TurnSystem;
using UnityEngine.SceneManagement;

public class GameRuleSystem : MonoBehaviour
{
    TurnManager turnManager = new TurnManager();
    private Coroutine currentCoroutine = null;

    private void Start()
    {
        var scene = SceneManager.GetActiveScene();
        var objArr = scene.GetRootGameObjects();

        for (int i = 0; i < objArr.Length; i++)
        {
            if (objArr[i].TryGetComponent<ITurnActor>(out ITurnActor actor))
            {
                turnManager.JoinActor(actor);
            }
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

