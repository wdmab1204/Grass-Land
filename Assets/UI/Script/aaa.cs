using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements; 

public class aaa : MonoBehaviour
{
    VisualElement bottomContainer;
    Button openButton, closeButton;
    VisualElement bottomSheet, scrim;
    VisualElement boy, warrior;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        bottomContainer = root.Q<VisualElement>("Container-Bottom");
        openButton = root.Q<Button>("button_open");
        closeButton = root.Q<Button>("button_close");
        bottomSheet = root.Q<VisualElement>("Bottom-Sheet");
        scrim = root.Q<VisualElement>("Scrim");
        boy = root.Q<VisualElement>("Image_Boy");
        warrior = root.Q<VisualElement>("image_warrior");

        bottomContainer.style.display = DisplayStyle.None;
        openButton.RegisterCallback<ClickEvent>(OnOpenButtonClicked);
        closeButton.RegisterCallback<ClickEvent>(OnCloseButtonClicked);

        Invoke("AnimateBoy", 0.1f);

        bottomSheet.RegisterCallback<TransitionEndEvent>(OnBottomSheetDown);
    }

    void AnimateBoy()
    {
        boy.RemoveFromClassList("image--boy--inair");
    }

    void OnOpenButtonClicked(ClickEvent evt)
    {
        bottomContainer.style.display = DisplayStyle.Flex;

        bottomSheet.AddToClassList("bottomsheet--up");
        scrim.AddToClassList("scrim--fadein");

        AnimateWarrior();
    }

    void AnimateWarrior()
    {
        warrior.ToggleInClassList("image--warrior--up");
        warrior.RegisterCallback<TransitionEndEvent>(evt =>
        {
            warrior.ToggleInClassList("image--warrior--up");
        });
    }

    void OnCloseButtonClicked(ClickEvent evt)
    {
        bottomSheet.RemoveFromClassList("bottomsheet--up");
        scrim.RemoveFromClassList("scrim--fadein");
    }

    void OnBottomSheetDown(TransitionEndEvent evt)
    {
        if (!bottomSheet.ClassListContains("bottomsheet--up"))
            bottomContainer.style.display = DisplayStyle.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
