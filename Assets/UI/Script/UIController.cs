using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
	VisualElement cardPanel;
	StyleSheet uss;
	// Use this for initialization
	void Start()
	{
        var root = GetComponent<UIDocument>().rootVisualElement;
		cardPanel = root.Q<VisualElement>("Card_Panel");

		foreach(var child in cardPanel.Children())
		{
			child.RegisterCallback<MouseEnterEvent>(BiggingCard);
			child.RegisterCallback<MouseLeaveEvent>(SmallingCard);
		}
    }

	void BiggingCard(MouseEnterEvent evt)
	{
		var target = evt.target as VisualElement;
		target.AddToClassList("card--bigger");
	}

	void SmallingCard(MouseLeaveEvent evt)
	{
		var target = evt.target as VisualElement;
		if (target.ClassListContains("card--bigger"))
			target.RemoveFromClassList("card--bigger");
	}

	// Update is called once per frame
	void Update()
	{
			
	}
}

