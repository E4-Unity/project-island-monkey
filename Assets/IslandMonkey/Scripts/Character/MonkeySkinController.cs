using UnityEngine;

namespace IslandMonkey
{
	public class MonkeySkinController : MonoBehaviour
	{
		static readonly int TextureID = Shader.PropertyToID("_MainTex");

		[Header("Renderer")]
		[SerializeField] SkinnedMeshRenderer[] bodies;
		[SerializeField] SkinnedMeshRenderer[] faces;

		[Header("Data")]
		[SerializeField] MonkeySkinData data;

		MonkeyType currentType;

		public void ChangeSkin(MonkeyType monkeyType)
		{
			if (!data || currentType == monkeyType) return;

			ChangeSkin(data.GetMonkeyTextureSet(monkeyType));
			currentType = monkeyType;
		}

		void ChangeSkin(MonkeyTextureSet textureSet)
		{
			if (!textureSet.Body || !textureSet.Face) return;

			foreach (var body in bodies)
			{
				body.material.SetTexture(TextureID, textureSet.Body);
			}

			foreach (var face in faces)
			{
				face.material.SetTexture(TextureID, textureSet.Face);
			}
		}
	}
}
