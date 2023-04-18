using UnityEngine;
using System.Collections;

using TurnSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameRuleSystem : MonoBehaviour
{
    TurnActor player;

    TurnManager turnManager = new TurnManager();
    Coroutine currentCoroutine = null;
    [SerializeField] private string CurrentActorName = "";
    [SerializeField] private FollowCamera camera;

    [Space(10)]
    [SerializeField] private TilemapManager tilemapManager;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<TurnActor>();
    }

    private void Start()
    {
        //var objArr = SceneManager.GetActiveScene().GetRootGameObjects();
        //for (int i = 0; i < objArr.Length; i++)
        //    if (objArr[i].TryGetComponent<TurnActor>(out TurnActor actor) && actor.isActiveAndEnabled)
        //        turnManager.JoinActor(actor);

        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(StartTurnSystemCoroutine());
    }

    void ReadyGame()
    {
        // 맵 그리
        DrawMap();

        //몬스터 생성
        InstantiateMobs();

        //플레이어 시작위치 정하기
        InitPlayerPosition();

        //턴 순서 정하기
        ClearQueue();
        EnqueueTurnOrder();
    }

    void DrawMap()
    {
        tilemapManager.InitializeMapAndApply((Room.ThemeType.Grassland, Room.RoomType.Normal), (15, 15));
    }

    void InstantiateMobs()
    {
        tilemapManager.InstantiateMobsInMap();
    }

    void InitPlayerPosition()
    {
        player.transform.position = tilemapManager.currentRoom.WorldPosition;
    }

    void EnqueueTurnOrder()
    {
        foreach (var mobActor in tilemapManager.currentRoom.Gets<TurnActor>())
            turnManager.JoinActor(mobActor);

        turnManager.JoinActor(player);
    }

    void ClearQueue()
    {
        turnManager.AllRemoveActor();
    }

    public void SetActorQueueInRoom(Room room)
    {
        var actors = room.GetActors();

        foreach (var actor in actors)
        {
            turnManager.JoinActor(actor);
        }
        turnManager.JoinActor(player);

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

