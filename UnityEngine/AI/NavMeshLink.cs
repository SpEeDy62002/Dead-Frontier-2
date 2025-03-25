using System;
using System.Collections.Generic;

namespace UnityEngine.AI
{
	// Token: 0x02000229 RID: 553
	[ExecuteInEditMode]
	[DefaultExecutionOrder(-101)]
	[AddComponentMenu("Navigation/NavMeshLink", 33)]
	[HelpURL("https://github.com/Unity-Technologies/NavMeshComponents#documentation-draft")]
	public class NavMeshLink : MonoBehaviour
	{
		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000B3D RID: 2877 RVA: 0x0004C8FB File Offset: 0x0004ACFB
		// (set) Token: 0x06000B3E RID: 2878 RVA: 0x0004C903 File Offset: 0x0004AD03
		public int agentTypeID
		{
			get
			{
				return this.m_AgentTypeID;
			}
			set
			{
				this.m_AgentTypeID = value;
				this.UpdateLink();
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000B3F RID: 2879 RVA: 0x0004C912 File Offset: 0x0004AD12
		// (set) Token: 0x06000B40 RID: 2880 RVA: 0x0004C91A File Offset: 0x0004AD1A
		public Vector3 startPoint
		{
			get
			{
				return this.m_StartPoint;
			}
			set
			{
				this.m_StartPoint = value;
				this.UpdateLink();
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000B41 RID: 2881 RVA: 0x0004C929 File Offset: 0x0004AD29
		// (set) Token: 0x06000B42 RID: 2882 RVA: 0x0004C931 File Offset: 0x0004AD31
		public Vector3 endPoint
		{
			get
			{
				return this.m_EndPoint;
			}
			set
			{
				this.m_EndPoint = value;
				this.UpdateLink();
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000B43 RID: 2883 RVA: 0x0004C940 File Offset: 0x0004AD40
		// (set) Token: 0x06000B44 RID: 2884 RVA: 0x0004C948 File Offset: 0x0004AD48
		public float width
		{
			get
			{
				return this.m_Width;
			}
			set
			{
				this.m_Width = value;
				this.UpdateLink();
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000B45 RID: 2885 RVA: 0x0004C957 File Offset: 0x0004AD57
		// (set) Token: 0x06000B46 RID: 2886 RVA: 0x0004C95F File Offset: 0x0004AD5F
		public bool bidirectional
		{
			get
			{
				return this.m_Bidirectional;
			}
			set
			{
				this.m_Bidirectional = value;
				this.UpdateLink();
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000B47 RID: 2887 RVA: 0x0004C96E File Offset: 0x0004AD6E
		// (set) Token: 0x06000B48 RID: 2888 RVA: 0x0004C976 File Offset: 0x0004AD76
		public bool autoUpdate
		{
			get
			{
				return this.m_AutoUpdatePosition;
			}
			set
			{
				this.SetAutoUpdate(value);
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000B49 RID: 2889 RVA: 0x0004C97F File Offset: 0x0004AD7F
		// (set) Token: 0x06000B4A RID: 2890 RVA: 0x0004C987 File Offset: 0x0004AD87
		public int area
		{
			get
			{
				return this.m_Area;
			}
			set
			{
				this.m_Area = value;
				this.UpdateLink();
			}
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x0004C996 File Offset: 0x0004AD96
		private void OnEnable()
		{
			this.AddLink();
			if (this.m_AutoUpdatePosition && this.m_LinkInstance.valid)
			{
				NavMeshLink.AddTracking(this);
			}
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x0004C9BF File Offset: 0x0004ADBF
		private void OnDisable()
		{
			NavMeshLink.RemoveTracking(this);
			this.m_LinkInstance.Remove();
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x0004C9D2 File Offset: 0x0004ADD2
		public void UpdateLink()
		{
			this.m_LinkInstance.Remove();
			this.AddLink();
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x0004C9E8 File Offset: 0x0004ADE8
		private static void AddTracking(NavMeshLink link)
		{
			if (NavMeshLink.s_Tracked.Count == 0)
			{
				NavMesh.onPreUpdate = (NavMesh.OnNavMeshPreUpdate)Delegate.Combine(NavMesh.onPreUpdate, new NavMesh.OnNavMeshPreUpdate(NavMeshLink.UpdateTrackedInstances));
			}
			NavMeshLink.s_Tracked.Add(link);
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x0004CA40 File Offset: 0x0004AE40
		private static void RemoveTracking(NavMeshLink link)
		{
			NavMeshLink.s_Tracked.Remove(link);
			if (NavMeshLink.s_Tracked.Count == 0)
			{
				NavMesh.onPreUpdate = (NavMesh.OnNavMeshPreUpdate)Delegate.Remove(NavMesh.onPreUpdate, new NavMesh.OnNavMeshPreUpdate(NavMeshLink.UpdateTrackedInstances));
			}
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x0004CA99 File Offset: 0x0004AE99
		private void SetAutoUpdate(bool value)
		{
			if (this.m_AutoUpdatePosition == value)
			{
				return;
			}
			this.m_AutoUpdatePosition = value;
			if (value)
			{
				NavMeshLink.AddTracking(this);
			}
			else
			{
				NavMeshLink.RemoveTracking(this);
			}
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x0004CAC8 File Offset: 0x0004AEC8
		private void AddLink()
		{
			if (this.m_LinkInstance.valid)
			{
				return;
			}
			this.m_LinkInstance = NavMesh.AddLink(new NavMeshLinkData
			{
				startPosition = this.m_StartPoint,
				endPosition = this.m_EndPoint,
				width = this.m_Width,
				costModifier = -1f,
				bidirectional = this.m_Bidirectional,
				area = this.m_Area,
				agentTypeID = this.m_AgentTypeID
			}, base.transform.position, base.transform.rotation);
			if (this.m_LinkInstance.valid)
			{
				this.m_LinkInstance.owner = this;
			}
			this.m_LastPosition = base.transform.position;
			this.m_LastRotation = base.transform.rotation;
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x0004CBA8 File Offset: 0x0004AFA8
		private bool HasTransformChanged()
		{
			return this.m_LastPosition != base.transform.position || this.m_LastRotation != base.transform.rotation;
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x0004CBE5 File Offset: 0x0004AFE5
		private void OnDidApplyAnimationProperties()
		{
			this.UpdateLink();
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x0004CBF0 File Offset: 0x0004AFF0
		private static void UpdateTrackedInstances()
		{
			foreach (NavMeshLink navMeshLink in NavMeshLink.s_Tracked)
			{
				if (navMeshLink.HasTransformChanged())
				{
					navMeshLink.UpdateLink();
				}
			}
		}

		// Token: 0x04000DB2 RID: 3506
		[SerializeField]
		private int m_AgentTypeID;

		// Token: 0x04000DB3 RID: 3507
		[SerializeField]
		private Vector3 m_StartPoint = new Vector3(0f, 0f, -2.5f);

		// Token: 0x04000DB4 RID: 3508
		[SerializeField]
		private Vector3 m_EndPoint = new Vector3(0f, 0f, 2.5f);

		// Token: 0x04000DB5 RID: 3509
		[SerializeField]
		private float m_Width;

		// Token: 0x04000DB6 RID: 3510
		[SerializeField]
		private bool m_Bidirectional = true;

		// Token: 0x04000DB7 RID: 3511
		[SerializeField]
		private bool m_AutoUpdatePosition;

		// Token: 0x04000DB8 RID: 3512
		[SerializeField]
		private int m_Area;

		// Token: 0x04000DB9 RID: 3513
		private NavMeshLinkInstance m_LinkInstance = default(NavMeshLinkInstance);

		// Token: 0x04000DBA RID: 3514
		private Vector3 m_LastPosition = Vector3.zero;

		// Token: 0x04000DBB RID: 3515
		private Quaternion m_LastRotation = Quaternion.identity;

		// Token: 0x04000DBC RID: 3516
		private static readonly List<NavMeshLink> s_Tracked = new List<NavMeshLink>();
	}
}
