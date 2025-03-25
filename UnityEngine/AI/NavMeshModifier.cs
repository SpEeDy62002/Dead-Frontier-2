using System;
using System.Collections.Generic;

namespace UnityEngine.AI
{
	// Token: 0x0200022A RID: 554
	[ExecuteInEditMode]
	[AddComponentMenu("Navigation/NavMeshModifier", 32)]
	[HelpURL("https://github.com/Unity-Technologies/NavMeshComponents#documentation-draft")]
	public class NavMeshModifier : MonoBehaviour
	{
		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000B57 RID: 2903 RVA: 0x0004CC81 File Offset: 0x0004B081
		// (set) Token: 0x06000B58 RID: 2904 RVA: 0x0004CC89 File Offset: 0x0004B089
		public bool overrideArea
		{
			get
			{
				return this.m_OverrideArea;
			}
			set
			{
				this.m_OverrideArea = value;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000B59 RID: 2905 RVA: 0x0004CC92 File Offset: 0x0004B092
		// (set) Token: 0x06000B5A RID: 2906 RVA: 0x0004CC9A File Offset: 0x0004B09A
		public int area
		{
			get
			{
				return this.m_Area;
			}
			set
			{
				this.m_Area = value;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000B5B RID: 2907 RVA: 0x0004CCA3 File Offset: 0x0004B0A3
		// (set) Token: 0x06000B5C RID: 2908 RVA: 0x0004CCAB File Offset: 0x0004B0AB
		public bool ignoreFromBuild
		{
			get
			{
				return this.m_IgnoreFromBuild;
			}
			set
			{
				this.m_IgnoreFromBuild = value;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000B5D RID: 2909 RVA: 0x0004CCB4 File Offset: 0x0004B0B4
		public static List<NavMeshModifier> activeModifiers
		{
			get
			{
				return NavMeshModifier.s_NavMeshModifiers;
			}
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x0004CCBB File Offset: 0x0004B0BB
		private void OnEnable()
		{
			if (!NavMeshModifier.s_NavMeshModifiers.Contains(this))
			{
				NavMeshModifier.s_NavMeshModifiers.Add(this);
			}
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x0004CCD8 File Offset: 0x0004B0D8
		private void OnDisable()
		{
			NavMeshModifier.s_NavMeshModifiers.Remove(this);
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x0004CCE6 File Offset: 0x0004B0E6
		public bool AffectsAgentType(int agentTypeID)
		{
			return this.m_AffectedAgents.Count != 0 && (this.m_AffectedAgents[0] == -1 || this.m_AffectedAgents.IndexOf(agentTypeID) != -1);
		}

		// Token: 0x04000DBF RID: 3519
		[SerializeField]
		private bool m_OverrideArea;

		// Token: 0x04000DC0 RID: 3520
		[SerializeField]
		private int m_Area;

		// Token: 0x04000DC1 RID: 3521
		[SerializeField]
		private bool m_IgnoreFromBuild;

		// Token: 0x04000DC2 RID: 3522
		[SerializeField]
		private List<int> m_AffectedAgents = new List<int>(new int[] { -1 });

		// Token: 0x04000DC3 RID: 3523
		private static readonly List<NavMeshModifier> s_NavMeshModifiers = new List<NavMeshModifier>();
	}
}
