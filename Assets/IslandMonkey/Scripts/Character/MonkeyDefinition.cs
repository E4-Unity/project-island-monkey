using UnityEngine;

namespace IslandMonkey
{
	public enum MonkeyType
	{
		Basic,
		Boss,
		Barista,
		BuildingOwner
	}

	[CreateAssetMenu(fileName = "Monkey Definition", menuName = "Data/Monkey/Definition")]
	public class MonkeyDefinition : ScriptableObject
	{
		[SerializeField] MonkeyType type;

		public MonkeyType Type => type;
	}
}
