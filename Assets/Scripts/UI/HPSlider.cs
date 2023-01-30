using UnityEngine;
using System.Collections;
using GameEntity;
using UnityEngine.UI;

public class HPSlider : MonoBehaviour
{
	[SerializeField] private Entity entity;
	private Slider slider;

	void Start()
	{
		slider = GetComponent<Slider>();
		slider.maxValue = entity.MaxHp;
		slider.value = entity.Hp;

		entity.OnHealthChanged += (hp) =>
		{
			this.slider.value = hp;
			Debug.Log("Aaaa");
		};
	}

	// Update is called once per frame
	void Update()
	{
			
	}
}

