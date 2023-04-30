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
        StartCoroutine(ReadyGame());

        //if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        //currentCoroutine = StartCoroutine(StartTurnSystemCoroutine());
    }

    IEnumerator ReadyGame()
    {
        // 맵 그리
        DrawMap();

        //몬스터 생성
        InstantiateMobs();

        yield return null;

        //var arr = Physics2D.OverlapCircleAll(Vector2.zero, 100f);
        //foreach (var ar in arr) Debug.Log(ar.name);

        //플레이어 시작위치 정하기
        InitPlayerPosition();

        //턴 순서 정하기
        ClearQueue();
        EnqueueTurnOrder();

        turnManager.LogTurnQueue();

        yield return StartTurnSystemCoroutine();
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
        player.transform.position = tilemapManager.GetCurrentVisitedRoom().WorldPosition;
    }

    void EnqueueTurnOrder()
    {
        var actors = tilemapManager.GetCurrentVisitedRoom().Gets<TurnActor>(1 << LayerMask.NameToLayer("Monster"));
        foreach (var mobActor in actors)
            turnManager.JoinActor(mobActor);

        turnManager.JoinActor(player);
    }

    public void UpdateTurnOrderQueue()
    {
        var currentVisitedRoom = tilemapManager.GetCurrentVisitedRoom();

        var actors = currentVisitedRoom.Gets<TurnActor>();

        foreach (var actor in actors) turnManager.JoinActor(actor);

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
        //StopCoroutine(currentCoroutine);
    }
}

