using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200029E RID: 670
	public static class GraphicsUtils
	{
		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000CDB RID: 3291 RVA: 0x00055101 File Offset: 0x00053501
		public static bool isLinearColorSpace
		{
			get
			{
				return QualitySettings.activeColorSpace == ColorSpace.Linear;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x0005510B File Offset: 0x0005350B
		public static bool supportsDX11
		{
			get
			{
				return SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.supportsComputeShaders;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000CDD RID: 3293 RVA: 0x00055124 File Offset: 0x00053524
		public static Texture2D whiteTexture
		{
			get
			{
				if (GraphicsUtils.s_WhiteTexture != null)
				{
					return GraphicsUtils.s_WhiteTexture;
				}
				GraphicsUtils.s_WhiteTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
				GraphicsUtils.s_WhiteTexture.SetPixel(0, 0, new Color(1f, 1f, 1f, 1f));
				GraphicsUtils.s_WhiteTexture.Apply();
				return GraphicsUtils.s_WhiteTexture;
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000CDE RID: 3294 RVA: 0x0005518C File Offset: 0x0005358C
		public static Mesh quad
		{
			get
			{
				if (GraphicsUtils.s_Quad != null)
				{
					return GraphicsUtils.s_Quad;
				}
				Vector3[] array = new Vector3[]
				{
					new Vector3(-1f, -1f, 0f),
					new Vector3(1f, 1f, 0f),
					new Vector3(1f, -1f, 0f),
					new Vector3(-1f, 1f, 0f)
				};
				Vector2[] array2 = new Vector2[]
				{
					new Vector2(0f, 0f),
					new Vector2(1f, 1f),
					new Vector2(1f, 0f),
					new Vector2(0f, 1f)
				};
				int[] array3 = new int[] { 0, 1, 2, 1, 0, 3 };
				GraphicsUtils.s_Quad = new Mesh
				{
					vertices = array,
					uv = array2,
					triangles = array3
				};
				GraphicsUtils.s_Quad.RecalculateNormals();
				GraphicsUtils.s_Quad.RecalculateBounds();
				return GraphicsUtils.s_Quad;
			}
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x000552F8 File Offset: 0x000536F8
		public static void Blit(Material material, int pass)
		{
			GL.PushMatrix();
			GL.LoadOrtho();
			material.SetPass(pass);
			GL.Begin(5);
			GL.TexCoord2(0f, 0f);
			GL.Vertex3(0f, 0f, 0.1f);
			GL.TexCoord2(1f, 0f);
			GL.Vertex3(1f, 0f, 0.1f);
			GL.TexCoord2(0f, 1f);
			GL.Vertex3(0f, 1f, 0.1f);
			GL.TexCoord2(1f, 1f);
			GL.Vertex3(1f, 1f, 0.1f);
			GL.End();
			GL.PopMatrix();
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x000553B4 File Offset: 0x000537B4
		public static void ClearAndBlit(Texture source, RenderTexture destination, Material material, int pass, bool clearColor = true, bool clearDepth = false)
		{
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = destination;
			GL.Clear(false, clearColor, Color.clear);
			GL.PushMatrix();
			GL.LoadOrtho();
			material.SetTexture("_MainTex", source);
			material.SetPass(pass);
			GL.Begin(5);
			GL.TexCoord2(0f, 0f);
			GL.Vertex3(0f, 0f, 0.1f);
			GL.TexCoord2(1f, 0f);
			GL.Vertex3(1f, 0f, 0.1f);
			GL.TexCoord2(0f, 1f);
			GL.Vertex3(0f, 1f, 0.1f);
			GL.TexCoord2(1f, 1f);
			GL.Vertex3(1f, 1f, 0.1f);
			GL.End();
			GL.PopMatrix();
			RenderTexture.active = active;
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x0005549A File Offset: 0x0005389A
		public static void Destroy(Object obj)
		{
			if (obj != null)
			{
				Object.Destroy(obj);
			}
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x000554AE File Offset: 0x000538AE
		public static void Dispose()
		{
			GraphicsUtils.Destroy(GraphicsUtils.s_Quad);
		}

		// Token: 0x04000FE1 RID: 4065
		private static Texture2D s_WhiteTexture;

		// Token: 0x04000FE2 RID: 4066
		private static Mesh s_Quad;
	}
}
