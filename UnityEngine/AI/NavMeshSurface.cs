using System;
using System.Collections.Generic;

namespace UnityEngine.AI
{
	// Token: 0x0200022D RID: 557
	[ExecuteInEditMode]
	[DefaultExecutionOrder(-102)]
	[AddComponentMenu("Navigation/NavMeshSurface", 30)]
	[HelpURL("https://github.com/Unity-Technologies/NavMeshComponents#documentation-draft")]
	public class NavMeshSurface : MonoBehaviour
	{
		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000B6F RID: 2927 RVA: 0x0004CEB6 File Offset: 0x0004B2B6
		// (set) Token: 0x06000B70 RID: 2928 RVA: 0x0004CEBE File Offset: 0x0004B2BE
		public int agentTypeID
		{
			get
			{
				return this.m_AgentTypeID;
			}
			set
			{
				this.m_AgentTypeID = value;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000B71 RID: 2929 RVA: 0x0004CEC7 File Offset: 0x0004B2C7
		// (set) Token: 0x06000B72 RID: 2930 RVA: 0x0004CECF File Offset: 0x0004B2CF
		public CollectObjects collectObjects
		{
			get
			{
				return this.m_CollectObjects;
			}
			set
			{
				this.m_CollectObjects = value;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000B73 RID: 2931 RVA: 0x0004CED8 File Offset: 0x0004B2D8
		// (set) Token: 0x06000B74 RID: 2932 RVA: 0x0004CEE0 File Offset: 0x0004B2E0
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

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000B75 RID: 2933 RVA: 0x0004CEE9 File Offset: 0x0004B2E9
		// (set) Token: 0x06000B76 RID: 2934 RVA: 0x0004CEF1 File Offset: 0x0004B2F1
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

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000B77 RID: 2935 RVA: 0x0004CEFA File Offset: 0x0004B2FA
		// (set) Token: 0x06000B78 RID: 2936 RVA: 0x0004CF02 File Offset: 0x0004B302
		public LayerMask layerMask
		{
			get
			{
				return this.m_LayerMask;
			}
			set
			{
				this.m_LayerMask = value;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000B79 RID: 2937 RVA: 0x0004CF0B File Offset: 0x0004B30B
		// (set) Token: 0x06000B7A RID: 2938 RVA: 0x0004CF13 File Offset: 0x0004B313
		public NavMeshCollectGeometry useGeometry
		{
			get
			{
				return this.m_UseGeometry;
			}
			set
			{
				this.m_UseGeometry = value;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000B7B RID: 2939 RVA: 0x0004CF1C File Offset: 0x0004B31C
		// (set) Token: 0x06000B7C RID: 2940 RVA: 0x0004CF24 File Offset: 0x0004B324
		public int defaultArea
		{
			get
			{
				return this.m_DefaultArea;
			}
			set
			{
				this.m_DefaultArea = value;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000B7D RID: 2941 RVA: 0x0004CF2D File Offset: 0x0004B32D
		// (set) Token: 0x06000B7E RID: 2942 RVA: 0x0004CF35 File Offset: 0x0004B335
		public bool ignoreNavMeshAgent
		{
			get
			{
				return this.m_IgnoreNavMeshAgent;
			}
			set
			{
				this.m_IgnoreNavMeshAgent = value;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000B7F RID: 2943 RVA: 0x0004CF3E File Offset: 0x0004B33E
		// (set) Token: 0x06000B80 RID: 2944 RVA: 0x0004CF46 File Offset: 0x0004B346
		public bool ignoreNavMeshObstacle
		{
			get
			{
				return this.m_IgnoreNavMeshObstacle;
			}
			set
			{
				this.m_IgnoreNavMeshObstacle = value;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000B81 RID: 2945 RVA: 0x0004CF4F File Offset: 0x0004B34F
		// (set) Token: 0x06000B82 RID: 2946 RVA: 0x0004CF57 File Offset: 0x0004B357
		public bool overrideTileSize
		{
			get
			{
				return this.m_OverrideTileSize;
			}
			set
			{
				this.m_OverrideTileSize = value;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x06000B83 RID: 2947 RVA: 0x0004CF60 File Offset: 0x0004B360
		// (set) Token: 0x06000B84 RID: 2948 RVA: 0x0004CF68 File Offset: 0x0004B368
		public int tileSize
		{
			get
			{
				return this.m_TileSize;
			}
			set
			{
				this.m_TileSize = value;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x0004CF71 File Offset: 0x0004B371
		// (set) Token: 0x06000B86 RID: 2950 RVA: 0x0004CF79 File Offset: 0x0004B379
		public bool overrideVoxelSize
		{
			get
			{
				return this.m_OverrideVoxelSize;
			}
			set
			{
				this.m_OverrideVoxelSize = value;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000B87 RID: 2951 RVA: 0x0004CF82 File Offset: 0x0004B382
		// (set) Token: 0x06000B88 RID: 2952 RVA: 0x0004CF8A File Offset: 0x0004B38A
		public float voxelSize
		{
			get
			{
				return this.m_VoxelSize;
			}
			set
			{
				this.m_VoxelSize = value;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000B89 RID: 2953 RVA: 0x0004CF93 File Offset: 0x0004B393
		// (set) Token: 0x06000B8A RID: 2954 RVA: 0x0004CF9B File Offset: 0x0004B39B
		public bool buildHeightMesh
		{
			get
			{
				return this.m_BuildHeightMesh;
			}
			set
			{
				this.m_OverrideTileSize = value;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000B8B RID: 2955 RVA: 0x0004CFA4 File Offset: 0x0004B3A4
		// (set) Token: 0x06000B8C RID: 2956 RVA: 0x0004CFAC File Offset: 0x0004B3AC
		public NavMeshData bakedNavMeshData
		{
			get
			{
				return this.m_BakedNavMeshData;
			}
			set
			{
				this.m_BakedNavMeshData = value;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000B8D RID: 2957 RVA: 0x0004CFB5 File Offset: 0x0004B3B5
		public static List<NavMeshSurface> activeSurfaces
		{
			get
			{
				return NavMeshSurface.s_NavMeshSurfaces;
			}
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x0004CFBC File Offset: 0x0004B3BC
		private void OnEnable()
		{
			NavMeshSurface.Register(this);
			this.AddData();
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x0004CFCA File Offset: 0x0004B3CA
		private void OnDisable()
		{
			this.RemoveData();
			NavMeshSurface.Unregister(this);
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x0004CFD8 File Offset: 0x0004B3D8
		public void AddData()
		{
			if (this.m_NavMeshDataInstance.valid)
			{
				return;
			}
			if (this.m_BakedNavMeshData != null)
			{
				this.m_NavMeshDataInstance = NavMesh.AddNavMeshData(this.m_BakedNavMeshData, base.transform.position, base.transform.rotation);
				this.m_NavMeshDataInstance.owner = this;
			}
			this.m_LastPosition = base.transform.position;
			this.m_LastRotation = base.transform.rotation;
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x0004D05C File Offset: 0x0004B45C
		public void RemoveData()
		{
			this.m_NavMeshDataInstance.Remove();
			this.m_NavMeshDataInstance = default(NavMeshDataInstance);
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x0004D084 File Offset: 0x0004B484
		public NavMeshBuildSettings GetBuildSettings()
		{
			NavMeshBuildSettings settingsByID = NavMesh.GetSettingsByID(this.m_AgentTypeID);
			if (this.overrideTileSize)
			{
				settingsByID.overrideTileSize = true;
				settingsByID.tileSize = this.tileSize;
			}
			if (this.overrideVoxelSize)
			{
				settingsByID.overrideVoxelSize = true;
				settingsByID.voxelSize = this.voxelSize;
			}
			return settingsByID;
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x0004D0E0 File Offset: 0x0004B4E0
		public void Bake()
		{
			List<NavMeshBuildSource> list = this.CollectSources();
			Bounds bounds = new Bounds(this.m_Center, NavMeshSurface.Abs(this.m_Size));
			if (this.m_CollectObjects == CollectObjects.All || this.m_CollectObjects == CollectObjects.Children)
			{
				bounds = this.CalculateWorldBounds(list);
			}
			NavMeshData navMeshData = NavMeshBuilder.BuildNavMeshData(this.GetBuildSettings(), list, bounds, base.transform.position, base.transform.rotation);
			if (navMeshData != null)
			{
				navMeshData.name = base.gameObject.name;
				this.RemoveData();
				this.m_BakedNavMeshData = navMeshData;
				if (base.isActiveAndEnabled)
				{
					this.AddData();
				}
			}
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x0004D18C File Offset: 0x0004B58C
		private static void Register(NavMeshSurface surface)
		{
			if (NavMeshSurface.s_NavMeshSurfaces.Count == 0)
			{
				NavMesh.onPreUpdate = (NavMesh.OnNavMeshPreUpdate)Delegate.Combine(NavMesh.onPreUpdate, new NavMesh.OnNavMeshPreUpdate(NavMeshSurface.UpdateActive));
			}
			if (!NavMeshSurface.s_NavMeshSurfaces.Contains(surface))
			{
				NavMeshSurface.s_NavMeshSurfaces.Add(surface);
			}
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x0004D1F4 File Offset: 0x0004B5F4
		private static void Unregister(NavMeshSurface surface)
		{
			NavMeshSurface.s_NavMeshSurfaces.Remove(surface);
			if (NavMeshSurface.s_NavMeshSurfaces.Count == 0)
			{
				NavMesh.onPreUpdate = (NavMesh.OnNavMeshPreUpdate)Delegate.Remove(NavMesh.onPreUpdate, new NavMesh.OnNavMeshPreUpdate(NavMeshSurface.UpdateActive));
			}
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x0004D250 File Offset: 0x0004B650
		private static void UpdateActive()
		{
			for (int i = 0; i < NavMeshSurface.s_NavMeshSurfaces.Count; i++)
			{
				NavMeshSurface.s_NavMeshSurfaces[i].UpdateDataIfTransformChanged();
			}
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x0004D288 File Offset: 0x0004B688
		private void AppendModifierVolumes(ref List<NavMeshBuildSource> sources)
		{
			List<NavMeshModifierVolume> list;
			if (this.m_CollectObjects == CollectObjects.Children)
			{
				list = new List<NavMeshModifierVolume>(base.GetComponentsInChildren<NavMeshModifierVolume>());
				list.RemoveAll((NavMeshModifierVolume x) => !x.isActiveAndEnabled);
			}
			else
			{
				list = NavMeshModifierVolume.activeModifiers;
			}
			foreach (NavMeshModifierVolume navMeshModifierVolume in list)
			{
				if ((this.m_LayerMask & (1 << navMeshModifierVolume.gameObject.layer)) != 0)
				{
					if (navMeshModifierVolume.AffectsAgentType(this.m_AgentTypeID))
					{
						Vector3 vector = navMeshModifierVolume.transform.TransformPoint(navMeshModifierVolume.center);
						Vector3 lossyScale = navMeshModifierVolume.transform.lossyScale;
						Vector3 vector2 = new Vector3(navMeshModifierVolume.size.x * Mathf.Abs(lossyScale.x), navMeshModifierVolume.size.y * Mathf.Abs(lossyScale.y), navMeshModifierVolume.size.z * Mathf.Abs(lossyScale.z));
						NavMeshBuildSource navMeshBuildSource = default(NavMeshBuildSource);
						navMeshBuildSource.shape = NavMeshBuildSourceShape.ModifierBox;
						navMeshBuildSource.transform = Matrix4x4.TRS(vector, navMeshModifierVolume.transform.rotation, Vector3.one);
						navMeshBuildSource.size = vector2;
						navMeshBuildSource.area = navMeshModifierVolume.area;
						sources.Add(navMeshBuildSource);
					}
				}
			}
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x0004D430 File Offset: 0x0004B830
		private List<NavMeshBuildSource> CollectSources()
		{
			List<NavMeshBuildSource> list = new List<NavMeshBuildSource>();
			List<NavMeshBuildMarkup> list2 = new List<NavMeshBuildMarkup>();
			List<NavMeshModifier> list3;
			if (this.m_CollectObjects == CollectObjects.Children)
			{
				list3 = new List<NavMeshModifier>(base.GetComponentsInChildren<NavMeshModifier>());
				list3.RemoveAll((NavMeshModifier x) => !x.isActiveAndEnabled);
			}
			else
			{
				list3 = NavMeshModifier.activeModifiers;
			}
			foreach (NavMeshModifier navMeshModifier in list3)
			{
				if ((this.m_LayerMask & (1 << navMeshModifier.gameObject.layer)) != 0)
				{
					if (navMeshModifier.AffectsAgentType(this.m_AgentTypeID))
					{
						list2.Add(new NavMeshBuildMarkup
						{
							root = navMeshModifier.transform,
							overrideArea = navMeshModifier.overrideArea,
							area = navMeshModifier.area,
							ignoreFromBuild = navMeshModifier.ignoreFromBuild
						});
					}
				}
			}
			if (this.m_CollectObjects == CollectObjects.All)
			{
				NavMeshBuilder.CollectSources(null, this.m_LayerMask, this.m_UseGeometry, this.m_DefaultArea, list2, list);
			}
			else if (this.m_CollectObjects == CollectObjects.Children)
			{
				NavMeshBuilder.CollectSources(base.transform, this.m_LayerMask, this.m_UseGeometry, this.m_DefaultArea, list2, list);
			}
			else if (this.m_CollectObjects == CollectObjects.Volume)
			{
				Matrix4x4 matrix4x = Matrix4x4.TRS(base.transform.position, base.transform.rotation, Vector3.one);
				Bounds worldBounds = NavMeshSurface.GetWorldBounds(matrix4x, new Bounds(this.m_Center, this.m_Size));
				NavMeshBuilder.CollectSources(worldBounds, this.m_LayerMask, this.m_UseGeometry, this.m_DefaultArea, list2, list);
			}
			if (this.m_IgnoreNavMeshAgent)
			{
				list.RemoveAll((NavMeshBuildSource x) => x.component != null && x.component.gameObject.GetComponent<NavMeshAgent>() != null);
			}
			if (this.m_IgnoreNavMeshObstacle)
			{
				list.RemoveAll((NavMeshBuildSource x) => x.component != null && x.component.gameObject.GetComponent<NavMeshObstacle>() != null);
			}
			this.AppendModifierVolumes(ref list);
			return list;
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x0004D688 File Offset: 0x0004BA88
		private static Vector3 Abs(Vector3 v)
		{
			return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x0004D6B4 File Offset: 0x0004BAB4
		private static Bounds GetWorldBounds(Matrix4x4 mat, Bounds bounds)
		{
			Vector3 vector = NavMeshSurface.Abs(mat.MultiplyVector(Vector3.right));
			Vector3 vector2 = NavMeshSurface.Abs(mat.MultiplyVector(Vector3.up));
			Vector3 vector3 = NavMeshSurface.Abs(mat.MultiplyVector(Vector3.forward));
			Vector3 vector4 = mat.MultiplyPoint(bounds.center);
			Vector3 vector5 = vector * bounds.size.x + vector2 * bounds.size.y + vector3 * bounds.size.z;
			return new Bounds(vector4, vector5);
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x0004D75C File Offset: 0x0004BB5C
		private Bounds CalculateWorldBounds(List<NavMeshBuildSource> sources)
		{
			Matrix4x4 matrix4x = Matrix4x4.TRS(base.transform.position, base.transform.rotation, Vector3.one);
			matrix4x = matrix4x.inverse;
			Bounds bounds = default(Bounds);
			foreach (NavMeshBuildSource navMeshBuildSource in sources)
			{
				switch (navMeshBuildSource.shape)
				{
				case NavMeshBuildSourceShape.Mesh:
				{
					Mesh mesh = navMeshBuildSource.sourceObject as Mesh;
					bounds.Encapsulate(NavMeshSurface.GetWorldBounds(matrix4x * navMeshBuildSource.transform, mesh.bounds));
					break;
				}
				case NavMeshBuildSourceShape.Terrain:
				{
					TerrainData terrainData = navMeshBuildSource.sourceObject as TerrainData;
					bounds.Encapsulate(NavMeshSurface.GetWorldBounds(matrix4x * navMeshBuildSource.transform, new Bounds(0.5f * terrainData.size, terrainData.size)));
					break;
				}
				case NavMeshBuildSourceShape.Box:
				case NavMeshBuildSourceShape.Sphere:
				case NavMeshBuildSourceShape.Capsule:
				case NavMeshBuildSourceShape.ModifierBox:
					bounds.Encapsulate(NavMeshSurface.GetWorldBounds(matrix4x * navMeshBuildSource.transform, new Bounds(Vector3.zero, navMeshBuildSource.size)));
					break;
				}
			}
			bounds.Expand(0.1f);
			return bounds;
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0004D8C4 File Offset: 0x0004BCC4
		private bool HasTransformChanged()
		{
			return this.m_LastPosition != base.transform.position || this.m_LastRotation != base.transform.rotation;
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x0004D901 File Offset: 0x0004BD01
		private void UpdateDataIfTransformChanged()
		{
			if (this.HasTransformChanged())
			{
				this.RemoveData();
				this.AddData();
			}
		}

		// Token: 0x04000DCD RID: 3533
		[SerializeField]
		private int m_AgentTypeID;

		// Token: 0x04000DCE RID: 3534
		[SerializeField]
		private CollectObjects m_CollectObjects;

		// Token: 0x04000DCF RID: 3535
		[SerializeField]
		private Vector3 m_Size = new Vector3(10f, 10f, 10f);

		// Token: 0x04000DD0 RID: 3536
		[SerializeField]
		private Vector3 m_Center = new Vector3(0f, 2f, 0f);

		// Token: 0x04000DD1 RID: 3537
		[SerializeField]
		private LayerMask m_LayerMask = -1;

		// Token: 0x04000DD2 RID: 3538
		[SerializeField]
		private NavMeshCollectGeometry m_UseGeometry;

		// Token: 0x04000DD3 RID: 3539
		[SerializeField]
		private int m_DefaultArea;

		// Token: 0x04000DD4 RID: 3540
		[SerializeField]
		private bool m_IgnoreNavMeshAgent = true;

		// Token: 0x04000DD5 RID: 3541
		[SerializeField]
		private bool m_IgnoreNavMeshObstacle = true;

		// Token: 0x04000DD6 RID: 3542
		[SerializeField]
		private bool m_OverrideTileSize;

		// Token: 0x04000DD7 RID: 3543
		[SerializeField]
		private int m_TileSize = 256;

		// Token: 0x04000DD8 RID: 3544
		[SerializeField]
		private bool m_OverrideVoxelSize;

		// Token: 0x04000DD9 RID: 3545
		[SerializeField]
		private float m_VoxelSize;

		// Token: 0x04000DDA RID: 3546
		[SerializeField]
		private bool m_BuildHeightMesh;

		// Token: 0x04000DDB RID: 3547
		[SerializeField]
		private NavMeshData m_BakedNavMeshData;

		// Token: 0x04000DDC RID: 3548
		private NavMeshDataInstance m_NavMeshDataInstance;

		// Token: 0x04000DDD RID: 3549
		private Vector3 m_LastPosition = Vector3.zero;

		// Token: 0x04000DDE RID: 3550
		private Quaternion m_LastRotation = Quaternion.identity;

		// Token: 0x04000DDF RID: 3551
		private static readonly List<NavMeshSurface> s_NavMeshSurfaces = new List<NavMeshSurface>();
	}
}
