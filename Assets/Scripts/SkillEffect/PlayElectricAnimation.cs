using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayElectricAnimation : MonoBehaviour
{
    //public GameObject effectPrefab;
    public GameObject effectObject;
    public AudioSource skillSound;
    public SpriteRenderer sprite;

    Vector3 originalPosition;
    Vector3 flipedPosition;

    private void Start()
    {
        originalPosition = effectObject.transform.localPosition;
        flipedPosition = new Vector3(-originalPosition.x, originalPosition.y, originalPosition.z);

        print(originalPosition);
    }

    //animation event trigger
    public void PlayElectric()
    {
        effectObject.SetActive(true);
        //오브젝트 활성화, 비활성화할때 초기프레임에 enter,exit는 호출되지 않는다. 호출하기위해 눈에 보이지 않는 물리연산 추가
        effectObject.transform.parent.GetComponent<Rigidbody>().AddForce(new Vector2(0.00001f, 0));
        if (sprite.flipX == false)
        {
            effectObject.transform.localPosition = originalPosition;
        }
        else
        {
            effectObject.transform.localPosition = flipedPosition;
        }

        skillSound.Play();
        StartCoroutine(PlayEffect());
    }

    public IEnumerator PlayEffect()
    {
        yield return null;
        Animator animator = effectObject.GetComponent<Animator>();
        float animationTime = animator.runtimeAnimatorController.animationClips[0].length;

        yield return new WaitForSeconds(animationTime);
        effectObject.SetActive(false);
        GameRuleSystem.Instance.Next();
    }
}
