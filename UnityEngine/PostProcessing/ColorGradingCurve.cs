using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200029D RID: 669
	[Serializable]
	public sealed class ColorGradingCurve
	{
		// Token: 0x06000CD8 RID: 3288 RVA: 0x00054FC4 File Offset: 0x000533C4
		public ColorGradingCurve(AnimationCurve curve, float zeroValue, bool loop, Vector2 bounds)
		{
			this.curve = curve;
			this.m_ZeroValue = zeroValue;
			this.m_Loop = loop;
			this.m_Range = bounds.magnitude;
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x00054FF0 File Offset: 0x000533F0
		public void Cache()
		{
			if (!this.m_Loop)
			{
				return;
			}
			int length = this.curve.length;
			if (length < 2)
			{
				return;
			}
			if (this.m_InternalLoopingCurve == null)
			{
				this.m_InternalLoopingCurve = new AnimationCurve();
			}
			Keyframe keyframe = this.curve[length - 1];
			keyframe.time -= this.m_Range;
			Keyframe keyframe2 = this.curve[0];
			keyframe2.time += this.m_Range;
			this.m_InternalLoopingCurve.keys = this.curve.keys;
			this.m_InternalLoopingCurve.AddKey(keyframe);
			this.m_InternalLoopingCurve.AddKey(keyframe2);
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x000550A8 File Offset: 0x000534A8
		public float Evaluate(float t)
		{
			if (this.curve.length == 0)
			{
				return this.m_ZeroValue;
			}
			if (!this.m_Loop || this.curve.length == 1)
			{
				return this.curve.Evaluate(t);
			}
			return this.m_InternalLoopingCurve.Evaluate(t);
		}

		// Token: 0x04000FDC RID: 4060
		public AnimationCurve curve;

		// Token: 0x04000FDD RID: 4061
		[SerializeField]
		private bool m_Loop;

		// Token: 0x04000FDE RID: 4062
		[SerializeField]
		private float m_ZeroValue;

		// Token: 0x04000FDF RID: 4063
		[SerializeField]
		private float m_Range;

		// Token: 0x04000FE0 RID: 4064
		private AnimationCurve m_InternalLoopingCurve;
	}
}
