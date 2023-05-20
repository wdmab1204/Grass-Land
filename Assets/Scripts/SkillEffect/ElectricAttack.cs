using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricAttack : MonoBehaviour
{
    public GameObject effectPrefab;
    GameObject effectObject;

    public void Play()
    {
        if (effectObject == null)
        {
            effectObject = Instantiate(effectPrefab, transform);
            effectObject.transform.localPosition = new Vector3(1.87f, 0);
        }
        StartCoroutine(PlayEffect());
    }

    public IEnumerator PlayEffect()
    {
        effectObject.SetActive(true);
        yield return null;
        Animator animator = effectObject.GetComponent<Animator>();
        float animationTime = animator.runtimeAnimatorController.animationClips[0].length;
        yield return new WaitForSeconds(animationTime);
        effectObject.SetActive(false);
    }
}
