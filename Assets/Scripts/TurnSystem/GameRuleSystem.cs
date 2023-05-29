using UnityEngine;
using System.Collections;

using TurnSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameRuleSystem : MonoBehaviour
{
    private static GameRuleSystem instance;

    // 싱글톤 인스턴스에 접근하는 정적 프로퍼티
    public static GameRuleSystem Instance
    {
        get { return instance; }
    }

    TurnManager turnManager = new TurnManager();
    [SerializeField] private string CurrentActorName = "";
    [SerializeField] private new FollowCamera camera;

    private void Awake()
    {
        // 인스턴스가 이미 존재하는 경우 중복 생성을 방지하기 위해 자기 자신을 파괴
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 인스턴스를 할당
        instance = this;

        // 필요한 경우에 따라 유지할 수 있도록 게임 오브젝트 파괴 방지
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        foreach (var rootObj in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (rootObj.TryGetComponent<TurnActor>(out TurnActor actor))
            {
                turnManager.JoinActor(actor);
            }
        }

        Next();
    }
    public void Next()
    { 
        var currentActor = turnManager.GetNextTurn();
        currentActor.UpdateTurn();

        CurrentActorName = currentActor.name;
        camera.target = currentActor.transform;
    }

    public TurnActor CurrentActor { get => turnManager.CurrentActor; }
}

