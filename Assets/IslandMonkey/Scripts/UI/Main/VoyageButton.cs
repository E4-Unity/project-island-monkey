using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace IslandMonkey
{
	public class VoyageButton : MonoBehaviour
	{
		/* 필드 */
		[Header("Component")]
		[FormerlySerializedAs("voyageButton")] [SerializeField] Button m_VoyageButton;
		VoyageDataManager m_VoyageDataManager;
		bool m_HasEventBound;

		/* MonoBehaviour */
		void Awake()
		{
			// 컴포넌트 할당
			m_VoyageButton = GetComponent<Button>();
		}

		void Start()
		{
			if (m_VoyageButton)
			{
				m_VoyageDataManager = GlobalGameManager.Instance.GetVoyageDataManager();
				m_VoyageDataManager.OnCurrentBuildingDataUpdated += OnUpdate;
				m_VoyageButton.onClick.AddListener(ChangeToVoyageScene); // TODO OnUpdate 로 이동?
				OnUpdate();
				m_HasEventBound = true;
			}
		}

		void OnDestroy()
		{
			if (m_HasEventBound)
			{
				m_VoyageDataManager.OnCurrentBuildingDataUpdated -= OnUpdate;
			}
		}

		/* 메서드 */
		void OnUpdate()
		{
			m_VoyageButton.interactable = m_VoyageDataManager.CanEnterVoyageScene;
		}

		void ChangeToVoyageScene() => SceneLoadingManager.Instance.ChangeScene(BuildScene.Voyage, SceneLoadingManager.ChangeSceneType.Animation);
	}
}
