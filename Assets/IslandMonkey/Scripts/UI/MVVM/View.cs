using UnityEngine;

namespace IslandMonkey.MVVM
{
	public abstract class View : PublicPropertyBinder
	{
		[SerializeField] ViewModel viewModel;

		protected override void Awake()
		{
			base.Awake();

			if (!viewModel)
			{
				viewModel = GetComponent<ViewModel>();
				if (viewModel is null) return;
			}

			if (viewModel.PropertyNotifier is not null)
				Init(viewModel.PropertyNotifier);
			else
				viewModel.OnInitialized += OnInitialized_Event;
		}

		void OnInitialized_Event()
		{
			Init(viewModel.PropertyNotifier);
			viewModel.OnInitialized -= OnInitialized_Event;
		}
	}
}
