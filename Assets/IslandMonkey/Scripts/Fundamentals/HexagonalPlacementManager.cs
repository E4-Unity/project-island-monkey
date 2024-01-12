using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IslandMonkey
{
	// TODO MonoBehaviour 제거 후 BuildingManager 에 통합
	/// <summary>
	/// 건물 배치 기능 담당
	/// </summary>
	[Serializable]
	public class HexagonalPlacementManager
	{
		/* 컴포넌트 */
		readonly HexagonalCalculator m_Calculator = new HexagonalCalculator(1);

		/* 필드 */
		// 변수
		int row = 1;
		int maxRow = 3;
		List<int> usedHexIndices = new List<int>();
		List<int> availableHexIndices = new List<int> { 0, 1, 2, 3, 4, 5 };

		/* 프로퍼티 */
		public bool IsFull => row > maxRow;

		public float BaseDistance
		{
			get => m_Calculator.BaseDistance;
			set => m_Calculator.BaseDistance = value;
		}

		/* API */
		/// <summary>
		/// 비어있는 곳 중 랜덤 인덱스 반환
		/// </summary>
		/// <returns>랜덤 인덱스</returns>
		public int GetRandomHexIndex()
		{
			// 유효성 검가
			if (IsFull) return -1;

			// 랜덤 인덱스 뽑기
			var randomIndex = Random.Range(0, availableHexIndices.Count);
			var randomHexIndex = availableHexIndices[randomIndex];

			return randomHexIndex;
		}

		/// <summary>
		/// 건물을 육각 좌표계 기준으로 특정 위치에 배치
		/// </summary>
		/// <param name="target">배치할 건물 오브젝트</param>
		/// <param name="hexIndex">배치할 위치 인덱스</param>
		/// <param name="spawnPosition">배치된 위치 좌표</param>
		public bool TryPlaceBuilding(Building target, int hexIndex, out Vector3 spawnPosition)
		{
			// 초기화
			spawnPosition = Vector3.zero;

			// 유효성 검사
			if (target is null) return false;

			// Hex Index 업데이트
			DisableHexIndex(hexIndex);

			// Hex Index에 대응하는 좌표 구하기
			var pos = m_Calculator.GetPosition(hexIndex);
			spawnPosition = new Vector3(pos.y, 0, pos.x);

			// 건물 배치
			target.transform.position = spawnPosition;

			return true;
		}

		/* 메서드 */
		void DisableHexIndex(int hexIndex)
		{
			// 이미 제거된 Hex Index 는 무시
			if (!availableHexIndices.Contains(hexIndex)) return;

			availableHexIndices.Remove(hexIndex);
			usedHexIndices.Add(hexIndex);

			if (availableHexIndices.Count == 0) // row 꽉 참
			{
				row++;
				if (!IsFull)
				{
					int start = m_Calculator.GetMaxHexIndex(row);
					int end = m_Calculator.GetMaxHexIndex(row + 1);
					availableHexIndices = new List<int>(end - start);
					for (int i = start; i < end; i++)
					{
						availableHexIndices.Add(i);
					}
				}
			}
		}
	}
}
