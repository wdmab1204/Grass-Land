using UnityEngine;

public class SingletonBehaviour : MonoBehaviour
{
    private static SingletonBehaviour instance;

    // 싱글톤 인스턴스에 접근하는 정적 프로퍼티
    public static SingletonBehaviour Instance
    {
        get { return instance; }
    }

    protected virtual void Awake()
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
}
