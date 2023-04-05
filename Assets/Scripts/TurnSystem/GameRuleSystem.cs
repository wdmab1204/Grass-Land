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
    [SerializeField] private TilemapManager tilemapManager;

    private void Awake()
    {
        var objArr = SceneManager.GetActiveScene().GetRootGameObjects();
        for (int i = 0; i < objArr.Length; i++)
            if (objArr[i].TryGetComponent<TurnActor>(out TurnActor actor) && actor.isActiveAndEnabled)
                turnManager.JoinActor(actor);

        tilemapManager.InitializeMapAndApply((Room.ThemeType.Grassland, Room.RoomType.Normal), (15, 15));
        //tilemapManager.InitializeNavigation();

    }

    private void Start()
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(StartTurnSystemCoroutine());
    }

    private IEnumerator StartTurnSystemCoroutine()
    {
        while (true)
        {
            var currentActor = turnManager.UpdateTurn();

            CurrentActorName = currentActor.name;
            camera.target = currentActor.transform;

            // Wait until the Actor finished his action
            yield return currentActor?.ActionCoroutine();
        }
    }

    public TurnActor CurrentActor { get => turnManager.CurrentActor; }

    private void OnDisable()
    {
        StopCoroutine(currentCoroutine);
    }
}

