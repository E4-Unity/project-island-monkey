using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Assets._0_IslandMonkey.Scripts.Extension;

namespace Assets._0_IslandMonkey.Scripts.Managers
{
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
        private TextMeshProUGUI gold;
        [SerializeField]
        private TextMeshProUGUI banana;
        [SerializeField]
        private TextMeshProUGUI clam;

        public void Awake()
        {
            foreach (var panel in panels)
            {
                panel.panel.onEnableEvent.AddListener(() => panel.onEnableEvent.Invoke());
                panel.panel.onDisableEvent.AddListener(() => panel.onDisableEvent.Invoke());
            }
            foreach (var btn in buttons)
            {
                btn.button.onClick.AddListener(()=>btn.onClickEvent.Invoke());
            }
        }

        public void SetGold(int gold)
        {
            this.gold.SetText(gold.FormatLargeNumber());
        }

        public void SetClam(int clam)
        {
            this.clam.SetText(clam.FormatLargeNumber());
        }

        public void SetBanana(int banana)
        {
            this.banana.SetText(banana.FormatLargeNumber());
        }
    }
}