using System;
using System.Collections.Generic;
using E4.Utilities;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
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

	/* MonoSingleton */
	protected override void InitializeComponent()
	{
		base.InitializeComponent();

		// 초기화
		Init();
	}

	/* 메서드 */
	void Init()
	{
		foreach (var panel in panels)
		{
			panel.panel.onEnableEvent.AddListener(() => panel.onEnableEvent.Invoke());
			panel.panel.onDisableEvent.AddListener(() => panel.onDisableEvent.Invoke());
		}

		foreach (var btn in buttons)
		{
			btn.button.onClick.AddListener(() =>
			{
				btn.onClickEvent.Invoke();
				SoundManager.Instance.PlaySoundEffect("Button_Click");
			});
		}
	}
}
