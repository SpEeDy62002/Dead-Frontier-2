using System;
using System.Collections.Generic;

namespace UnityEngine.AI
{
	// Token: 0x0200022B RID: 555
	[ExecuteInEditMode]
	[AddComponentMenu("Navigation/NavMeshModifierVolume", 31)]
	[HelpURL("https://github.com/Unity-Technologies/NavMeshComponents#documentation-draft")]
	public class NavMeshModifierVolume : MonoBehaviour
	{
		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000B63 RID: 2915 RVA: 0x0004CD88 File Offset: 0x0004B188
		// (set) Token: 0x06000B64 RID: 2916 RVA: 0x0004CD90 File Offset: 0x0004B190
		public Vector3 size
		{
			get
			{
				return this.m_Size;
			}
			set
			{
				this.m_Size = value;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000B65 RID: 2917 RVA: 0x0004CD99 File Offset: 0x0004B199
		// (set) Token: 0x06000B66 RID: 2918 RVA: 0x0004CDA1 File Offset: 0x0004B1A1
		public Vector3 center
		{
			get
			{
				return this.m_Center;
			}
			set
			{
				this.m_Center = value;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000B67 RID: 2919 RVA: 0x0004CDAA File Offset: 0x0004B1AA
		// (set) Token: 0x06000B68 RID: 2920 RVA: 0x0004CDB2 File Offset: 0x0004B1B2
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

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000B69 RID: 2921 RVA: 0x0004CDBB File Offset: 0x0004B1BB
		public static List<NavMeshModifierVolume> activeModifiers
		{
			get
			{
				return NavMeshModifierVolume.s_NavMeshModifiers;
			}
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x0004CDC2 File Offset: 0x0004B1C2
		private void OnEnable()
		{
			if (!NavMeshModifierVolume.s_NavMeshModifiers.Contains(this))
			{
				NavMeshModifierVolume.s_NavMeshModifiers.Add(this);
			}
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x0004CDDF File Offset: 0x0004B1DF
		private void OnDisable()
		{
			NavMeshModifierVolume.s_NavMeshModifiers.Remove(this);
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0004CDED File Offset: 0x0004B1ED
		public bool AffectsAgentType(int agentTypeID)
		{
			return this.m_AffectedAgents.Count != 0 && (this.m_AffectedAgents[0] == -1 || this.m_AffectedAgents.IndexOf(agentTypeID) != -1);
		}

		// Token: 0x04000DC4 RID: 3524
		[SerializeField]
		private Vector3 m_Size = new Vector3(4f, 3f, 4f);

		// Token: 0x04000DC5 RID: 3525
		[SerializeField]
		private Vector3 m_Center = new Vector3(0f, 1f, 0f);

		// Token: 0x04000DC6 RID: 3526
		[SerializeField]
		private int m_Area;

		// Token: 0x04000DC7 RID: 3527
		[SerializeField]
		private List<int> m_AffectedAgents = new List<int>(new int[] { -1 });

		// Token: 0x04000DC8 RID: 3528
		private static readonly List<NavMeshModifierVolume> s_NavMeshModifiers = new List<NavMeshModifierVolume>();
	}
}
