using System;
using UnityEngine;


namespace IslandMonkey
{
	// TODO Dictionary와 Enum으로 매핑 리팩토링 필요할듯
	public class EquipmentComponent : MonoBehaviour
	{
		[Serializable]
		public struct EquipmentSet
		{
			public GameObject RightHandEquipment;
			public GameObject LeftHandEquipment;
			public GameObject NeckEquipment;
			public GameObject LeftFootEquipment;
			public GameObject RightFootEquipment;
		}

		[SerializeField] Transform rightHandSocket;
		[SerializeField] Transform leftHandSocket;
		[SerializeField] Transform neckSocket;
		[SerializeField] Transform rightFootSocket;
		[SerializeField] Transform leftFootSocket;

		[ContextMenu("UnEquip")]
		public void UnEquip()
		{
			DetachEquipmentFromSocket(rightHandSocket);
			DetachEquipmentFromSocket(leftHandSocket);
			DetachEquipmentFromSocket(neckSocket);
			DetachEquipmentFromSocket(rightFootSocket);
			DetachEquipmentFromSocket(leftFootSocket);
		}

		public void Equip(EquipmentSet equipmentSet)
		{
			UnEquip();

			AttachEquipmentToSocket(rightHandSocket, equipmentSet.RightHandEquipment);
			AttachEquipmentToSocket(leftHandSocket, equipmentSet.LeftHandEquipment);
			AttachEquipmentToSocket(neckSocket, equipmentSet.NeckEquipment);
			AttachEquipmentToSocket(rightFootSocket, equipmentSet.RightFootEquipment);
			AttachEquipmentToSocket(leftFootSocket, equipmentSet.LeftFootEquipment);
		}

		void DetachEquipmentFromSocket(Transform socket)
		{
			var equipments = socket.GetComponentsInChildren<Equipment>();

			foreach (var equipment in equipments)
			{
				Destroy(equipment.gameObject);
			}
		}

		void AttachEquipmentToSocket(Transform socket, GameObject prefab)
		{
			if (socket && prefab)
			{
				Transform equipment = Instantiate(prefab.transform, socket, true);
				equipment.position = socket.position;
				equipment.rotation = gameObject.transform.rotation;
			}
		}
	}
}
