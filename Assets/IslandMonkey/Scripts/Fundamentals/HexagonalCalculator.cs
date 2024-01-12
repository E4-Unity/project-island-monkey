using System;
using UnityEngine;

/// <summary>
/// 육각 좌표계 좌표 변환 계산기
/// </summary>
[Serializable]
public class HexagonalCalculator
{
	/* 필드 */
	float m_BaseDistance;
	Vector2 m_Offset_P;
	Vector2 m_Offset_Q;
	Vector2 m_Offset_R;

	/* 프로퍼티 */
	public float BaseDistance
	{
		get => m_BaseDistance;
		set => Init(value);
	}

	/* 생성자 */
	public HexagonalCalculator(float distance) => Init(distance);

	/* API */
	/// <summary>
	/// 초기화
	/// </summary>
	/// <param name="distance">배치 간격</param>
	void Init(float distance)
	{
		// 필드 초기화
		m_BaseDistance = distance;

		// 축 별 단위 길이 재설정
		float offsetX = 0.5f * distance; // x
		float offsetY = Mathf.Sqrt(3) * 0.5f * distance; // y

		m_Offset_P = new Vector2(offsetX, offsetY / 3); // x + 1/3 y
		m_Offset_Q = new Vector2(-offsetX, offsetY / 3); // -x + 1/3 y
		m_Offset_R = new Vector2(0, - offsetY * 2 / 3); // - 2/3 y
	}

	/// <summary>
	/// HexIndex 총 개수를 구하는 메서드
	/// </summary>
	/// <param name="row">가장 끝 테두리 번호</param>
	/// <returns>HexIndex 총 개수</returns>
	public int GetMaxHexIndex(int row) => 3 * row * (row - 1);

	/// <summary>
	/// HexIndex를 유니티 좌표(x, 0, z)로 변환해준다.
	/// </summary>
	/// <param name="hexIndex">육각 좌표계의 좌표 대신 사용하는 인덱스</param>
	/// <returns>유니티 좌표(x, 0, z)</returns>
	public Vector2 GetPosition(int hexIndex)
	{
		GetCoordinates(hexIndex, out var p, out var q, out var r);
		return GetPosition(p, q, r);
	}

	/// <summary>
	/// 육각 좌표(p, q, r)를 실제 좌표(x, 0, z)로 변환해준다.
	/// </summary>
	public Vector2 GetPosition(int p, int q, int r)
	{
		return p * m_Offset_P + q * m_Offset_Q + r * m_Offset_R;
	}

	/// <summary>
	/// HexIndex 를 육각 좌표계의 좌표로 변환
	/// </summary>
	/// <param name="hexIndex">육각 좌표계의 좌표 대신 사용하는 인덱스</param>
	/// <param name="p">p축 좌표</param>
	/// <param name="q">q축 좌표</param>
	/// <param name="r">r축 좌표</param>
	public void GetCoordinates(int hexIndex, out int p, out int q, out int r)
	{
		// 테두리 번호 및 오프셋 구하기
		ConvertIndex(hexIndex, out int row, out int offset);

		// (1, -1, 0)이 index 0번
		p = row;
		q = -row;
		r = 0;

		// TODO 하드 코딩이라 리팩토링 필요
		// 회전
		for (int i = 0; i < offset; i++)
		{
			switch (i / row)
			{
				case 0:
					p--;
					r++;
					break;
				case 1:
					p--;
					q++;
					break;
				case 2:
					r--;
					q++;
					break;
				case 3:
					r--;
					p++;
					break;
				case 4:
					q--;
					p++;
					break;
				case 5:
					q--;
					r++;
					break;
			}
		}
	}

	/// <summary>
	/// HexIndex 를 행과 열로 변환
	/// </summary>
	/// <param name="hexIndex">육각 좌표계의 좌표 대신 사용하는 인덱스</param>
	/// <param name="row">테두리 위치로 바깥쪽으로 갈수록 1 씩 증가</param>
	/// <param name="column">해당 테두리 내의 3시 방향을 시작으로 시계 방향으로 진행한 횟수</param>
	public void ConvertIndex(int hexIndex, out int row, out int column)
	{
		// 1번부터 시작
		int n = 2;
		while (GetMaxHexIndex(n) <= hexIndex)
		{
			n++;
		}

		row = n - 1;
		column = hexIndex - GetMaxHexIndex(row);
	}
}
