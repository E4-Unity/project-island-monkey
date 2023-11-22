using System;
using UnityEngine;

namespace IslandMonkey
{
	// TODO
	/**
	 * switch 대신 Dictionary 활용하면 좋은데 Dictionary는 Serialize가 안 됨.
	 * List 2개와 커스텀 에디터 사용하면 비슷한 효과를 낼 수 있겠지만 추후에 고려.
	 */

	[Serializable]
	public struct MonkeyTextureSet
	{
		public Texture Body;
		public Texture Face;
	}

	[CreateAssetMenu(fileName = "Monkey Skin Data", menuName = "Data/Monkey/Skin")]
	public class MonkeySkinData : ScriptableObject
	{
		[SerializeField] MonkeyTextureSet basic;
		[SerializeField] MonkeyTextureSet boss;
		[SerializeField] MonkeyTextureSet barista;
		[SerializeField] MonkeyTextureSet buildingOwner;

		public MonkeyTextureSet GetMonkeyTextureSet(MonkeyType monkeyType)
		{
			MonkeyTextureSet result;

			switch (monkeyType)
			{
				case MonkeyType.Basic:
					result = basic;
					break;
				case MonkeyType.Boss:
					result = boss;
					break;
				case MonkeyType.Barista:
					result = barista;
					break;
				case MonkeyType.BuildingOwner:
					result = buildingOwner;
					break;
				default:
					result = basic;
					break;
			}

			return result;
		}
	}
}
