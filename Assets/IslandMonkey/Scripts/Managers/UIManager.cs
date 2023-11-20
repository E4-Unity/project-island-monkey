using Assets._0_IslandMonkey.Scripts.Extension;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
	[Serializable]
	public class MPanel
	{
		public string name;
		public Panel panel;
		public UnityEvent onEnableEvent;
		public UnityEvent onDisableEvent;
	}

	[Serializable]
	public class MButton
	{
		public string name;
		public Button button;
		public UnityEvent onClickEvent;
	}

	public List<MPanel> panels = new List<MPanel>();
	public List<MButton> buttons = new List<MButton>();

	[SerializeField]
	private TextMeshProUGUI goldText;
	[SerializeField]
	private TextMeshProUGUI bananaText;
	[SerializeField]
	private TextMeshProUGUI clamText;

	public void Awake()
	{

		GameManager.OnGoldChanged += SetGold;
		GameManager.OnBananaChanged += SetBanana;
		GameManager.OnClamChanged += SetClam;

		foreach (var panel in panels)
		{
			panel.panel.onEnableEvent.AddListener(() => panel.onEnableEvent.Invoke());
			panel.panel.onDisableEvent.AddListener(() => panel.onDisableEvent.Invoke());
		}

		foreach (var btn in buttons)
		{
			btn.button.onClick.AddListener(() => btn.onClickEvent.Invoke());
		}
	}

	void OnDestroy()
	{
		GameManager.OnGoldChanged -= SetGold;
		GameManager.OnBananaChanged -= SetBanana;
		GameManager.OnClamChanged -= SetClam;
	}

	public void SetGold(int value)
	{
		goldText.SetText(value.FormatLargeNumber());
	}

	public void SetBanana(int value)
	{
		bananaText.SetText(value.FormatLargeNumber());
	}

	public void SetClam(int value)
	{
		clamText.SetText(value.FormatLargeNumber());
	}
}
