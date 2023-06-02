using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public class TerrainChunk
{
    const float colliderGenerationDistanceThreshold = 5;
    public event System.Action<TerrainChunk, bool> onVisibilityChanged;
    public Vector2 coord;

    public GameObject meshObject;
    Vector2 sampleCentre;
    Bounds bounds;

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    MeshCollider meshCollider;

    LODInfo[] detailLevels;
    LODMesh[] lodMeshes;
    int colliderLODIndex;

    HeightMap heightMap;
    bool heightMapReceived;
    int previousLODIndex = -1;
    bool hasSetCollider;
    float maxViewDst;

    HeightMapSettings heightMapSettings;
    MeshSettings meshSettings;
    Transform viewer;

    public Material trunkMaterial;
    public Material leavesMaterial;
    public static List<GameObject> treeMeshes = new List<GameObject>();

    public TerrainChunk(Vector2 coord, HeightMapSettings heightMapSettings, MeshSettings meshSettings,
        LODInfo[] detailLevels, int colliderLODIndex, Transform parent, Transform viewer, Material material)
    {
        CorutineForChunks.instance.StartCoroutine(FunctionAux(coord,heightMapSettings, meshSettings, detailLevels,
            colliderLODIndex, parent, viewer, material));
    }

    public IEnumerator FunctionAux(Vector2 coord, HeightMapSettings heightMapSettings, MeshSettings meshSettings,
        LODInfo[] detailLevels, int colliderLODIndex, Transform parent, Transform viewer, Material material)
    {
        trunkMaterial = Resources.Load<Material>("Materials/Trunk");
        leavesMaterial = Resources.Load<Material>("Materials/Leaves");
        this.coord = coord;
        this.detailLevels = detailLevels;
        this.colliderLODIndex = colliderLODIndex;
        this.heightMapSettings = heightMapSettings;
        this.meshSettings = meshSettings;
        this.viewer = viewer;
        
        GenerateTreeMeshes();

        sampleCentre = coord * meshSettings.meshWorldSize / meshSettings.meshScale;
        Vector2 position = coord * meshSettings.meshWorldSize;
        bounds = new Bounds(position, Vector2.one * meshSettings.meshWorldSize);

        meshObject = new GameObject("Terrain Chunk");
        meshRenderer = meshObject.AddComponent<MeshRenderer>();
        meshFilter = meshObject.AddComponent<MeshFilter>();
        meshCollider = meshObject.AddComponent<MeshCollider>();
        meshRenderer.material = material;
        meshObject.transform.position = new Vector3(position.x, 0, position.y);
        meshObject.transform.parent = parent;
        SetVisible(false);

        lodMeshes = new LODMesh[detailLevels.Length];
        for (int i = 0; i < detailLevels.Length; i++)
        {
            lodMeshes[i] = new LODMesh(detailLevels[i].lod);
            lodMeshes[i].updateCallback += UpdateTerrainChunk;
            if (i == colliderLODIndex)
            {
                lodMeshes[i].updateCallback += UpdateCollisionMesh;
            }

            yield return null;
        }
        
        maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
    }

    public void Load()
    {
        ThreadedDataRequester.RequestData(
            () => HeightMapGenerator.GenerateHeightMap(meshSettings.numVertsPerLine, meshSettings.numVertsPerLine,
                heightMapSettings, sampleCentre, coord), OnHeightMapReceived);
    }

    bool IsTreeHeightGood(float offsetX,float offsetY)
    {
        if (heightMap.values[Mathf.Abs(Mathf.RoundToInt(offsetX + meshSettings.numVertsPerLine / 2f)),
                Mathf.Abs(Mathf.RoundToInt(offsetY - meshSettings.numVertsPerLine / 2f))] < 30*0.2f)
            return false;
        if (heightMap.values[Mathf.Abs(Mathf.RoundToInt(offsetX + meshSettings.numVertsPerLine / 2f)),
                Mathf.Abs(Mathf.RoundToInt(offsetY - meshSettings.numVertsPerLine / 2f))] >30*0.51f)
            return false;
        return true;
    }

    public void GenerateTreeMeshes()
    { 
        if (treeMeshes.Count == 0)
        {
            for (int i = 0; i < heightMapSettings.numberOfTreePrefabs; i++)
            {
                using (FileStream fileStream = File.Open(Application.persistentDataPath+"/TreeMeshData/tree_"+i+".mesh", FileMode.Open))
                {

                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    TreeMeshData treeMeshData = (TreeMeshData)binaryFormatter.Deserialize(fileStream);
                    Mesh mesh = TreeMeshData.ConvertToUnityMesh(treeMeshData);
                    GameObject tree = new GameObject("Tree");
                    tree.AddComponent<MeshFilter>().mesh = mesh;
                    tree.AddComponent<MeshRenderer>().materials = new Material[2] { trunkMaterial, leavesMaterial };
                    tree.AddComponent<MeshCollider>().sharedMesh = mesh;
                    tree.hideFlags = HideFlags.HideInHierarchy;
                    tree.SetActive(false);
                    treeMeshes.Add(tree);
                }
            }
        }
    }

    IEnumerator GenTrees()
    {
        for (int i = 0; i < heightMapSettings.numberOfTreesPerChunk; i++)
        {
            Random.InitState(heightMapSettings.noiseSettings.seed+(int)sampleCentre.x+i+(int)sampleCentre.y);
            float offsetX = - meshSettings.numVertsPerLine/2f +Random.Range(3, meshSettings.numVertsPerLine-3);
            float offsetY = meshSettings.numVertsPerLine/2f -Random.Range(3, meshSettings.numVertsPerLine-3);
            if (IsTreeHeightGood(offsetX,offsetY))
            {
                GameObject tree = GameObject.Instantiate(treeMeshes[Random.Range(0, heightMapSettings.numberOfTreePrefabs)]);
                tree.transform.parent = meshObject.transform;
                tree.hideFlags &= ~HideFlags.HideInHierarchy;
                tree.transform.localPosition =  new Vector3( offsetX,heightMap.values[Mathf.Abs(Mathf.RoundToInt(offsetX+meshSettings.numVertsPerLine/2f)),Mathf.Abs(Mathf.RoundToInt(offsetY-meshSettings.numVertsPerLine/2f))]-2f,offsetY);
                tree.SetActive(true);
            }
            yield return null;
        }
    }

    void OnHeightMapReceived(object heightMapObject)
    {
        heightMap = (HeightMap)heightMapObject;
        heightMapReceived = true;
        TreeGeneratorMono.instance.StartCoroutine(GenTrees());
        UpdateTerrainChunk();
    }


    Vector2 viewerPosition
    {
        get { return new Vector2(viewer.position.x, viewer.position.z); }
    }


    public void UpdateTerrainChunk()
    {
        if (heightMapReceived)
        {
            float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));

            bool wasVisible = IsVisible();
            bool visible = viewerDstFromNearestEdge <= maxViewDst;

            if (visible)
            {
                int lodIndex = 0;

                for (int i = 0; i < detailLevels.Length - 1; i++)
                {
                    if (viewerDstFromNearestEdge > detailLevels[i].visibleDstThreshold)
                    {
                        lodIndex = i + 1;
                    }
                    else
                    {
                        break;
                    }
                }

                if (lodIndex != previousLODIndex)
                {
                    LODMesh lodMesh = lodMeshes[lodIndex];
                    if (lodMesh!=null && lodMesh.hasMesh)
                    {
                        previousLODIndex = lodIndex;
                        meshFilter.mesh = lodMesh.mesh;
                    }
                    else if (lodMesh!=null && !lodMesh.hasRequestedMesh)
                    {
                        lodMesh.RequestMesh(heightMap, meshSettings);
                    }
                }
            }

            if (wasVisible != visible)
            {
                SetVisible(visible);
                if (onVisibilityChanged != null)
                {
                    onVisibilityChanged(this, visible);
                }
            }
        }
    }

    public void UpdateCollisionMesh()
    {
        if (!hasSetCollider)
        {
            float sqrDstFromViewerToEdge = bounds.SqrDistance(viewerPosition);

            if (sqrDstFromViewerToEdge < detailLevels[colliderLODIndex].sqrVisibleDstThreshold)
            {
                if (!lodMeshes[colliderLODIndex].hasRequestedMesh)
                {
                    lodMeshes[colliderLODIndex].RequestMesh(heightMap, meshSettings);
                }
            }

            if (sqrDstFromViewerToEdge < colliderGenerationDistanceThreshold * colliderGenerationDistanceThreshold)
            {
                if (lodMeshes[colliderLODIndex].hasMesh)
                {
                    meshCollider.sharedMesh = lodMeshes[colliderLODIndex].mesh;
                    hasSetCollider = true;
                }
            }
        }
    }

    public void SetVisible(bool visible)
    {
        meshObject.SetActive(visible);
    }

    public bool IsVisible()
    {
        return meshObject.activeSelf;
    }
}

class LODMesh
{
    public Mesh mesh;
    public bool hasRequestedMesh;
    public bool hasMesh;
    int lod;
    public event System.Action updateCallback;

    public LODMesh(int lod)
    {
        this.lod = lod;
    }

    void OnMeshDataReceived(object meshDataObject)
    {
        mesh = ((MeshData)meshDataObject).CreateMesh();
        hasMesh = true;

        updateCallback();
    }

    public void RequestMesh(HeightMap heightMap, MeshSettings meshSettings)
    {
        hasRequestedMesh = true;
        ThreadedDataRequester.RequestData(() => MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, lod),
            OnMeshDataReceived);
    }
}