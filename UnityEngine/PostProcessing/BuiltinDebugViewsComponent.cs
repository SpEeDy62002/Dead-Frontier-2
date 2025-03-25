using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000237 RID: 567
	public sealed class BuiltinDebugViewsComponent : PostProcessingComponentCommandBuffer<BuiltinDebugViewsModel>
	{
		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000BB5 RID: 2997 RVA: 0x0004E43F File Offset: 0x0004C83F
		public override bool active
		{
			get
			{
				return base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Depth) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Normals) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.MotionVectors);
			}
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x0004E474 File Offset: 0x0004C874
		public override DepthTextureMode GetCameraFlags()
		{
			BuiltinDebugViewsModel.Mode mode = base.model.settings.mode;
			DepthTextureMode depthTextureMode = DepthTextureMode.None;
			if (mode != BuiltinDebugViewsModel.Mode.Normals)
			{
				if (mode != BuiltinDebugViewsModel.Mode.MotionVectors)
				{
					if (mode == BuiltinDebugViewsModel.Mode.Depth)
					{
						depthTextureMode |= DepthTextureMode.Depth;
					}
				}
				else
				{
					depthTextureMode |= DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
				}
			}
			else
			{
				depthTextureMode |= DepthTextureMode.DepthNormals;
			}
			return depthTextureMode;
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x0004E4D0 File Offset: 0x0004C8D0
		public override CameraEvent GetCameraEvent()
		{
			return (base.model.settings.mode != BuiltinDebugViewsModel.Mode.MotionVectors) ? CameraEvent.BeforeImageEffectsOpaque : CameraEvent.BeforeImageEffects;
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x0004E4FF File Offset: 0x0004C8FF
		public override string GetName()
		{
			return "Builtin Debug Views";
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x0004E508 File Offset: 0x0004C908
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			BuiltinDebugViewsModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			material.shaderKeywords = null;
			if (this.context.isGBufferAvailable)
			{
				material.EnableKeyword("SOURCE_GBUFFER");
			}
			BuiltinDebugViewsModel.Mode mode = settings.mode;
			if (mode != BuiltinDebugViewsModel.Mode.Depth)
			{
				if (mode != BuiltinDebugViewsModel.Mode.Normals)
				{
					if (mode == BuiltinDebugViewsModel.Mode.MotionVectors)
					{
						this.MotionVectorsPass(cb);
					}
				}
				else
				{
					this.DepthNormalsPass(cb);
				}
			}
			else
			{
				this.DepthPass(cb);
			}
			this.context.Interrupt();
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0004E5AC File Offset: 0x0004C9AC
		private void DepthPass(CommandBuffer cb)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			BuiltinDebugViewsModel.DepthSettings depth = base.model.settings.depth;
			cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._DepthScale, 1f / depth.scale);
			cb.Blit(null, BuiltinRenderTextureType.CameraTarget, material, 0);
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x0004E60C File Offset: 0x0004CA0C
		private void DepthNormalsPass(CommandBuffer cb)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			cb.Blit(null, BuiltinRenderTextureType.CameraTarget, material, 1);
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x0004E640 File Offset: 0x0004CA40
		private void MotionVectorsPass(CommandBuffer cb)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			BuiltinDebugViewsModel.MotionVectorsSettings motionVectors = base.model.settings.motionVectors;
			int num = BuiltinDebugViewsComponent.Uniforms._TempRT;
			cb.GetTemporaryRT(num, this.context.width, this.context.height, 0, FilterMode.Bilinear);
			cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.sourceOpacity);
			cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, BuiltinRenderTextureType.CameraTarget);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, num, material, 2);
			if (motionVectors.motionImageOpacity > 0f && motionVectors.motionImageAmplitude > 0f)
			{
				int tempRT = BuiltinDebugViewsComponent.Uniforms._TempRT2;
				cb.GetTemporaryRT(tempRT, this.context.width, this.context.height, 0, FilterMode.Bilinear);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.motionImageOpacity);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Amplitude, motionVectors.motionImageAmplitude);
				cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, num);
				cb.Blit(num, tempRT, material, 3);
				cb.ReleaseTemporaryRT(num);
				num = tempRT;
			}
			if (motionVectors.motionVectorsOpacity > 0f && motionVectors.motionVectorsAmplitude > 0f)
			{
				this.PrepareArrows();
				float num2 = 1f / (float)motionVectors.motionVectorsResolution;
				float num3 = num2 * (float)this.context.height / (float)this.context.width;
				cb.SetGlobalVector(BuiltinDebugViewsComponent.Uniforms._Scale, new Vector2(num3, num2));
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.motionVectorsOpacity);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Amplitude, motionVectors.motionVectorsAmplitude);
				cb.DrawMesh(this.m_Arrows.mesh, Matrix4x4.identity, material, 0, 4);
			}
			cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, num);
			cb.Blit(num, BuiltinRenderTextureType.CameraTarget);
			cb.ReleaseTemporaryRT(num);
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x0004E848 File Offset: 0x0004CC48
		private void PrepareArrows()
		{
			int motionVectorsResolution = base.model.settings.motionVectors.motionVectorsResolution;
			int num = motionVectorsResolution * Screen.width / Screen.height;
			if (this.m_Arrows == null)
			{
				this.m_Arrows = new BuiltinDebugViewsComponent.ArrowArray();
			}
			if (this.m_Arrows.columnCount != num || this.m_Arrows.rowCount != motionVectorsResolution)
			{
				this.m_Arrows.Release();
				this.m_Arrows.BuildMesh(num, motionVectorsResolution);
			}
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x0004E8CC File Offset: 0x0004CCCC
		public override void OnDisable()
		{
			if (this.m_Arrows != null)
			{
				this.m_Arrows.Release();
			}
			this.m_Arrows = null;
		}

		// Token: 0x04000E07 RID: 3591
		private const string k_ShaderString = "Hidden/Post FX/Builtin Debug Views";

		// Token: 0x04000E08 RID: 3592
		private BuiltinDebugViewsComponent.ArrowArray m_Arrows;

		// Token: 0x02000238 RID: 568
		private static class Uniforms
		{
			// Token: 0x04000E09 RID: 3593
			internal static readonly int _DepthScale = Shader.PropertyToID("_DepthScale");

			// Token: 0x04000E0A RID: 3594
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");

			// Token: 0x04000E0B RID: 3595
			internal static readonly int _Opacity = Shader.PropertyToID("_Opacity");

			// Token: 0x04000E0C RID: 3596
			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x04000E0D RID: 3597
			internal static readonly int _TempRT2 = Shader.PropertyToID("_TempRT2");

			// Token: 0x04000E0E RID: 3598
			internal static readonly int _Amplitude = Shader.PropertyToID("_Amplitude");

			// Token: 0x04000E0F RID: 3599
			internal static readonly int _Scale = Shader.PropertyToID("_Scale");
		}

		// Token: 0x02000239 RID: 569
		private enum Pass
		{
			// Token: 0x04000E11 RID: 3601
			Depth,
			// Token: 0x04000E12 RID: 3602
			Normals,
			// Token: 0x04000E13 RID: 3603
			MovecOpacity,
			// Token: 0x04000E14 RID: 3604
			MovecImaging,
			// Token: 0x04000E15 RID: 3605
			MovecArrows
		}

		// Token: 0x0200023A RID: 570
		private class ArrowArray
		{
			// Token: 0x17000151 RID: 337
			// (get) Token: 0x06000BC1 RID: 3009 RVA: 0x0004E96A File Offset: 0x0004CD6A
			// (set) Token: 0x06000BC2 RID: 3010 RVA: 0x0004E972 File Offset: 0x0004CD72
			public Mesh mesh { get; private set; }

			// Token: 0x17000152 RID: 338
			// (get) Token: 0x06000BC3 RID: 3011 RVA: 0x0004E97B File Offset: 0x0004CD7B
			// (set) Token: 0x06000BC4 RID: 3012 RVA: 0x0004E983 File Offset: 0x0004CD83
			public int columnCount { get; private set; }

			// Token: 0x17000153 RID: 339
			// (get) Token: 0x06000BC5 RID: 3013 RVA: 0x0004E98C File Offset: 0x0004CD8C
			// (set) Token: 0x06000BC6 RID: 3014 RVA: 0x0004E994 File Offset: 0x0004CD94
			public int rowCount { get; private set; }

			// Token: 0x06000BC7 RID: 3015 RVA: 0x0004E9A0 File Offset: 0x0004CDA0
			public void BuildMesh(int columns, int rows)
			{
				Vector3[] array = new Vector3[]
				{
					new Vector3(0f, 0f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(-1f, 1f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(1f, 1f, 0f)
				};
				int num = 6 * columns * rows;
				List<Vector3> list = new List<Vector3>(num);
				List<Vector2> list2 = new List<Vector2>(num);
				for (int i = 0; i < rows; i++)
				{
					for (int j = 0; j < columns; j++)
					{
						Vector2 vector = new Vector2((0.5f + (float)j) / (float)columns, (0.5f + (float)i) / (float)rows);
						for (int k = 0; k < 6; k++)
						{
							list.Add(array[k]);
							list2.Add(vector);
						}
					}
				}
				int[] array2 = new int[num];
				for (int l = 0; l < num; l++)
				{
					array2[l] = l;
				}
				this.mesh = new Mesh
				{
					hideFlags = HideFlags.DontSave
				};
				this.mesh.SetVertices(list);
				this.mesh.SetUVs(0, list2);
				this.mesh.SetIndices(array2, MeshTopology.Lines, 0);
				this.mesh.UploadMeshData(true);
				this.columnCount = columns;
				this.rowCount = rows;
			}

			// Token: 0x06000BC8 RID: 3016 RVA: 0x0004EB83 File Offset: 0x0004CF83
			public void Release()
			{
				GraphicsUtils.Destroy(this.mesh);
				this.mesh = null;
			}
		}
	}
}
