using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200029A RID: 666
	public class PostProcessingContext
	{
		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x00054E86 File Offset: 0x00053286
		// (set) Token: 0x06000CCA RID: 3274 RVA: 0x00054E8E File Offset: 0x0005328E
		public bool interrupted { get; private set; }

		// Token: 0x06000CCB RID: 3275 RVA: 0x00054E97 File Offset: 0x00053297
		public void Interrupt()
		{
			this.interrupted = true;
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x00054EA0 File Offset: 0x000532A0
		public PostProcessingContext Reset()
		{
			this.profile = null;
			this.camera = null;
			this.materialFactory = null;
			this.renderTextureFactory = null;
			this.interrupted = false;
			return this;
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x00054EC6 File Offset: 0x000532C6
		public bool isGBufferAvailable
		{
			get
			{
				return this.camera.actualRenderingPath == RenderingPath.DeferredShading;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x00054ED6 File Offset: 0x000532D6
		public bool isHdr
		{
			get
			{
				return this.camera.allowHDR;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x00054EE3 File Offset: 0x000532E3
		public int width
		{
			get
			{
				return this.camera.pixelWidth;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x00054EF0 File Offset: 0x000532F0
		public int height
		{
			get
			{
				return this.camera.pixelHeight;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x00054EFD File Offset: 0x000532FD
		public Rect viewport
		{
			get
			{
				return this.camera.rect;
			}
		}

		// Token: 0x04000FC7 RID: 4039
		public PostProcessingProfile profile;

		// Token: 0x04000FC8 RID: 4040
		public Camera camera;

		// Token: 0x04000FC9 RID: 4041
		public MaterialFactory materialFactory;

		// Token: 0x04000FCA RID: 4042
		public RenderTextureFactory renderTextureFactory;
	}
}
