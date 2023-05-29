using System.Collections;
using TurnSystem;
using UnityEngine;
using DG.Tweening;

[DisallowMultipleComponent]
public class MonsterActor : TurnActor
{
    Animator animator;
    public new Collider collider;
    delegate IEnumerator AIAction();
    AIAction[] actionArray;
    SpriteRenderer sprite;
    Transform player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        collider.enabled = false;
        sprite = GetComponent<SpriteRenderer>();
        actionArray = new AIAction[]
        {
            Run,
            Attack,
            RunAndAttack,
        };
    }

    private void Start()
    {
        player = GameObject.Find("Warrior").transform;
    }

    public override void UpdateTurn()
    {
        StartCoroutine(DoAction());
    }

    IEnumerator DoAction()
    {
        yield return actionArray[Random.Range(0, actionArray.Length)]();
        Next();
    }

    IEnumerator RunAndAttack()
    {
        yield return Run();
        yield return Attack();
    }

    IEnumerator Attack()
    {
        animator.Play("Enemy1-Attack1");
        if (player.position.x >= transform.position.x)
        {
            sprite.flipX = false;
            collider.transform.localPosition = new Vector3(1, collider.transform.localPosition.y, collider.transform.localPosition.z);
        }
        else
        {
            sprite.flipX = true;
            collider.transform.localPosition = new Vector3(-1, collider.transform.localPosition.y, collider.transform.localPosition.z);
        }
        yield return null;

        float animationTime = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationTime);
    }

    public float distance = 3;
    public LayerMask layerMask;
    IEnumerator Run()
    {
        var playerPos = player.position;
        var directionNormalVector = (playerPos - transform.position).normalized;
        directionNormalVector = Quaternion.Euler(0, Random.Range(-15, 15), 0) * directionNormalVector;
        var direction = directionNormalVector * (distance - 0.5f);

        
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        Vector3 dest;

        if (Physics.Raycast(ray, out hit, distance, layerMask))
        {
            //앞에 장애물이 있다면 거리 재계산
            direction = directionNormalVector * (hit.distance - 0.5f);
            Debug.Log("장애물 : " + hit.transform.name);
        }

        //목적지 설정
        dest = transform.position + direction;

        //목적지방향으로 바라보기
        if (dest.x >= transform.position.x) sprite.flipX = false;
        else sprite.flipX = true;

        print(dest.x);

        //액션 실행
        Debug.DrawRay(transform.position, direction, Color.blue, 1.0f);
        animator.Play("Run");
        yield return MoveObjectUsingTranslate(dest, 1.5f);
    }

    IEnumerator MoveObjectUsingTranslate(Vector3 targetPosition, float moveDuration)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        animator.Play("Idle");
    }

    float GetRandomRadian()
    {
        float angle = Random.Range(-7.5f, 7.5f);
        return angle * Mathf.Deg2Rad;
    }

    float Radian(float angle) => angle * Mathf.Deg2Rad;

    public void OnCollider() => collider.enabled = true;
    public void OffCollider() => collider.enabled = false;
    public void Next() => GameRuleSystem.Instance.Next();
}

