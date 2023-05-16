using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAnimationController : MonoBehaviour
{
    public GameObject panel;
    public GameObject cardPrefab;

    public void Play()
    {
        GameObject obj = Instantiate(cardPrefab);
        obj.transform.SetParent(panel.transform);
    }

}
