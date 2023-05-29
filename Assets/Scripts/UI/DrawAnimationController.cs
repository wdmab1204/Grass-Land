using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawAnimationController : MonoBehaviour
{
    public bool showCardUI = false;
    [Space]
    public GameObject panel;
    public GameObject cardPrefab;

    private void Start()
    {
        if (!showCardUI)
        {
            panel.transform.position = new Vector3(panel.transform.position.x, -300);
        }
    }

    public void Show()
    {
        for (int i = 0; i < panel.transform.childCount; i++)
        {
            panel.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    //create card ui
    public void Play()
    {
        GameObject obj = Instantiate(cardPrefab);
        obj.transform.SetParent(panel.transform);
    }

}
