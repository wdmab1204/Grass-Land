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
    [SerializeField] private FollowCamera camera;

    [Space(10)]
    [SerializeField] private MapGenerator mapGenerator;

    private void Start()
    {
        var objArr = SceneManager.GetActiveScene().GetRootGameObjects();
        for (int i = 0; i < objArr.Length; i++)
            if (objArr[i].TryGetComponent<ITurnActor>(out ITurnActor actor))
                turnManager.JoinActor(actor);

        mapGenerator.CreateMapAndApply(Room.ThemeType.Grassland, Room.RoomType.Normal, (15, 15), Vector3Int.zero);

        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(StartTurnSystemCoroutine());

    }

    private IEnumerator StartTurnSystemCoroutine()
    {
        while (true)
        {
            var currentActor = turnManager.UpdateTurn();
            CurrentActorName = currentActor.ActorObject.name;
            camera.target = currentActor.ActorObject.transform;

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

