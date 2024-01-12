using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using E4.Utilities;
using UnityEngine;

namespace IslandMonkey
{
	/// <summary>
	/// E4 Utilities 패키지의 DataManager 를 간편하게 사용하기 위한 템플릿 MonoBehaviour 클래스로
	/// 저장할 데이터 클래스가 한 종류일 경우 적합합니다.
	/// </summary>
	/// <typeparam name="T">ISavable 를 상속받은 저장할 데이터 클래스</typeparam>
	public abstract class DataManagerClient<T> : MonoBehaviour where T : class, ISavable, new()
	{
		/* 필드 */
		T m_Data;

		/* 프로퍼티 */
		protected T Data => m_Data ??= DataManager.LoadData<T>();

		/* MonoBehaviour */
		protected virtual void Awake()
		{
			// 데이터 비동기 로딩
			DataManager.LoadDataAsync<T>();
		}

		/* 메서드 */
		protected T LoadData() => DataManager.LoadData<T>();
		protected Task<T> LoadDataAsync() => DataManager.LoadDataAsync<T>();
		protected void SaveData() => DataManager.SaveData<T>();
		protected void SaveDataImmediately() => DataManager.SaveDataImmediately<T>();
		protected void UnloadData() => DataManager.UnloadData<T>();
		protected void DeleteData() => DataManager.DeleteData<T>();
	}

	// TODO Model 클래스를 기본 구현 인터페이스로 변경?
	/// <summary>
	/// DataManagerClient 와 Model 클래스를 합친 클래스
	/// </summary>
	/// <typeparam name="T">ISavable 를 상속받은 저장할 데이터 클래스</typeparam>
	public abstract class DataManagerClientModel<T> : DataManagerClient<T>, INotifyPropertyChanged where T : class, ISavable, new()
	{
		/* Model */
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected bool SetField<TObject>(ref TObject field, TObject value, [CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<TObject>.Default.Equals(field, value)) return false;
			field = value;
			OnPropertyChanged(propertyName);
			return true;
		}
	}
}
