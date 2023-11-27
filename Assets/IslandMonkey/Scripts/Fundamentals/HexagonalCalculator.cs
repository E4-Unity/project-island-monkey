using UnityEngine;

public class HexagonalCalculator
{
	readonly Vector2 _pOffset;
	readonly Vector2 _qOffset;
	readonly Vector2 _rOffset;

	public HexagonalCalculator(float distance)
	{
		float xOffset = 0.5f * distance; // x
		float yOffset = Mathf.Sqrt(3) * 0.5f * distance; // y

		_pOffset = new Vector2(xOffset, yOffset / 3); // x + 1/3 y
		_qOffset = new Vector2(-xOffset, yOffset / 3); // -x + 1/3 y
		_rOffset = new Vector2(0, - yOffset * 2 / 3); // - 2/3 y
	}

	// 육각 좌표(p, q, r)를 실제 좌표(x, 0, z)로 변환해준다.
	public Vector2 GetPosition(int p, int q, int r)
	{
		return p * _pOffset + q * _qOffset + r * _rOffset;
	}

	// 육각 좌표 인덱스를 실제 좌표(x, 0, z)로 변환해준다.
	public Vector2 GetPosition(int hexIndex)
	{
		GetCoordinates(hexIndex, out var p, out var q, out var r);
		return GetPosition(p, q, r);
	}

	/**
	 * 육각 좌표계 기준으로 (1, -1, 0)이 index 0번이고,
	 * 시계 방향으로 한 칸씩 이동할 때마다 index가 1씩 증가하며
	 * 원래 위치로 돌아오면 바깥 테두리로 한 칸 밀려난다고 정의하자.
	 * ex) index 3번은 (-1, 0, 1), 7번은 (2, -2, 0)이다.
	 *
	 * 이 index를 육각 좌표(p, q, r)로 변환하는 메서드이다.
	 */
	public void GetCoordinates(int index, out int p, out int q, out int r)
	{
		// 테두리 번호 및 오프셋 구하기
		ConvertIndex(index, out int row, out int offset);

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

	public int SumHexagonalCentredNumbers(int n) => 3 * n * (n - 1);

	// 테두리 번호(row)와 오프셋(column) 구하기
	void ConvertIndex(int index, out int row, out int column)
	{
		// 1번부터 시작
		int n = 2;
		while (SumHexagonalCentredNumbers(n) <= index)
		{
			n++;
		}

		row = n - 1;
		column = index - SumHexagonalCentredNumbers(row);
	}
}
