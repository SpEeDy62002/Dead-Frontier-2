using System;
using System.Collections.Generic;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200029F RID: 671
	public sealed class MaterialFactory : IDisposable
	{
		// Token: 0x06000CE3 RID: 3299 RVA: 0x000554BA File Offset: 0x000538BA
		public MaterialFactory()
		{
			this.m_Materials = new Dictionary<string, Material>();
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x000554D0 File Offset: 0x000538D0
		public Material Get(string shaderName)
		{
			Material material;
			if (!this.m_Materials.TryGetValue(shaderName, out material))
			{
				Shader shader = Shader.Find(shaderName);
				if (shader == null)
				{
					throw new ArgumentException(string.Format("Shader not found ({0})", shaderName));
				}
				material = new Material(shader)
				{
					name = string.Format("PostFX - {0}", shaderName.Substring(shaderName.LastIndexOf("/") + 1)),
					hideFlags = HideFlags.DontSave
				};
				this.m_Materials.Add(shaderName, material);
			}
			return material;
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x00055558 File Offset: 0x00053958
		public void Dispose()
		{
			foreach (KeyValuePair<string, Material> keyValuePair in this.m_Materials)
			{
				Material value = keyValuePair.Value;
				GraphicsUtils.Destroy(value);
			}
			this.m_Materials.Clear();
		}

		// Token: 0x04000FE3 RID: 4067
		private Dictionary<string, Material> m_Materials;
	}
}
