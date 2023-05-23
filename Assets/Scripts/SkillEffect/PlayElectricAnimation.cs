using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayElectricAnimation : MonoBehaviour
{
    public GameObject effectPrefab;
    GameObject effectObject;
    SoundPlayer player;

    private void Awake()
    {
        player = GetComponent<SoundPlayer>();
    }

    //animation event trigger
    public void PlayElectric()
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
        player.Play("Electric");
        effectObject.SetActive(true);
        yield return null;

        Animator animator = effectObject.GetComponent<Animator>();
        float animationTime = animator.runtimeAnimatorController.animationClips[0].length;


        yield return new WaitForSeconds(animationTime);
        effectObject.SetActive(false);
    }
}
