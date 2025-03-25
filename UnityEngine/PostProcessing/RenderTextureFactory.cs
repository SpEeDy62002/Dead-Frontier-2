using System;
using System.Collections.Generic;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020002A0 RID: 672
	public sealed class RenderTextureFactory : IDisposable
	{
		// Token: 0x06000CE6 RID: 3302 RVA: 0x000555A3 File Offset: 0x000539A3
		public RenderTextureFactory()
		{
			this.m_TemporaryRTs = new HashSet<RenderTexture>();
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x000555B8 File Offset: 0x000539B8
		public RenderTexture Get(RenderTexture baseRenderTexture)
		{
			return this.Get(baseRenderTexture.width, baseRenderTexture.height, baseRenderTexture.depth, baseRenderTexture.format, (!baseRenderTexture.sRGB) ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.sRGB, baseRenderTexture.filterMode, baseRenderTexture.wrapMode, "FactoryTempTexture");
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x00055608 File Offset: 0x00053A08
		public RenderTexture Get(int width, int height, int depthBuffer = 0, RenderTextureFormat format = RenderTextureFormat.ARGBHalf, RenderTextureReadWrite rw = RenderTextureReadWrite.Default, FilterMode filterMode = FilterMode.Bilinear, TextureWrapMode wrapMode = TextureWrapMode.Clamp, string name = "FactoryTempTexture")
		{
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, depthBuffer, format);
			temporary.filterMode = filterMode;
			temporary.wrapMode = wrapMode;
			temporary.name = name;
			this.m_TemporaryRTs.Add(temporary);
			return temporary;
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x00055648 File Offset: 0x00053A48
		public void Release(RenderTexture rt)
		{
			if (rt == null)
			{
				return;
			}
			if (!this.m_TemporaryRTs.Contains(rt))
			{
				throw new ArgumentException(string.Format("Attempting to remove a RenderTexture that was not allocated: {0}", rt));
			}
			this.m_TemporaryRTs.Remove(rt);
			RenderTexture.ReleaseTemporary(rt);
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x00055698 File Offset: 0x00053A98
		public void ReleaseAll()
		{
			foreach (RenderTexture renderTexture in this.m_TemporaryRTs)
			{
				RenderTexture.ReleaseTemporary(renderTexture);
			}
			this.m_TemporaryRTs.Clear();
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x000556D9 File Offset: 0x00053AD9
		public void Dispose()
		{
			this.ReleaseAll();
		}

		// Token: 0x04000FE4 RID: 4068
		private HashSet<RenderTexture> m_TemporaryRTs;
	}
}
