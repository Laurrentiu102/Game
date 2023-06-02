using System;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class TreeMeshData
{
    public SerializableVector3[] vertices;
    public int[] triangles;
    public SerializableVector3[] normals;
    public SerializableVector2[] uvs;
    public SerializableVector4[] tangents;
    public int[][] submeshes;
    public int[] materialIndices;

    public TreeMeshData(Mesh originalMesh,Renderer renderer)
    {
        vertices = SerializableVector3.ConvertFromUnityVector3(originalMesh.vertices);
        triangles = originalMesh.triangles;
        normals = SerializableVector3.ConvertFromUnityVector3(originalMesh.normals);
        uvs = SerializableVector2.ConvertFromUnityVector2(originalMesh.uv);
        tangents = SerializableVector4.ConvertFromUnityVector4(originalMesh.tangents);
        submeshes = new int[originalMesh.subMeshCount][];
        for (int i = 0; i < originalMesh.subMeshCount; i++)
        {
            submeshes[i] = originalMesh.GetTriangles(i);
        }
        materialIndices = new int[originalMesh.subMeshCount];
        for (int i = 0; i < originalMesh.subMeshCount; i++)
            materialIndices[i] = Array.IndexOf(renderer.sharedMaterials, renderer.materials[i]);
    }
    
    public static Mesh ConvertToUnityMesh(TreeMeshData meshData)
    {
        Mesh mesh = new Mesh();
        
        mesh.vertices = SerializableVector3.ConvertToUnityVector3(meshData.vertices);
        mesh.triangles = meshData.triangles;
        mesh.normals = SerializableVector3.ConvertToUnityVector3(meshData.normals);
        mesh.uv = SerializableVector2.ConvertToUnityVector2(meshData.uvs);
        mesh.tangents = SerializableVector4.ConvertToUnityVector4(meshData.tangents);

        mesh.subMeshCount = meshData.submeshes.Length;
        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            int[] triangles = meshData.submeshes[i];
            mesh.SetTriangles(triangles, i);
        }
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }
}

[Serializable]
public class SerializableVector4 : ISerializable
{
    public float x;
    public float y;
    public float z;
    public float w;

    public static SerializableVector4[] ConvertFromUnityVector4(Vector4[] vector)
    {
        SerializableVector4[] vertices = new SerializableVector4[vector.Length];
        int i = 0;
        foreach (Vector4 v in vector)
        {
            vertices[i] = new SerializableVector4();
            vertices[i].x = v.x;
            vertices[i].y = v.y;
            vertices[i].z = v.z;
            vertices[i++].w = v.w;
        }
        return vertices; 
    }

    public static Vector4[] ConvertToUnityVector4(SerializableVector4[] vector)
    {
        Vector4[] vertices = new Vector4[vector.Length];
        for (int i = 0; i < vector.Length; i++)
        {
            vertices[i]= new Vector4(vector[i].x, vector[i].y, vector[i].z, vector[i].w);
        }
        return vertices;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("x", x);
        info.AddValue("y", y);
        info.AddValue("z", z);
        info.AddValue("w", w);
    }
    
    protected SerializableVector4(SerializationInfo info, StreamingContext context)
    {
        x = info.GetSingle("x");
        y = info.GetSingle("y");
        z = info.GetSingle("z");
        w = info.GetSingle("w");
    }
    
    public SerializableVector4()
    {
    }

    public SerializableVector4(Vector4 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
        w = vector.w;
    }
}


[Serializable]
public class SerializableVector3 : ISerializable
{
    public float x;
    public float y;
    public float z;

    public static SerializableVector3[] ConvertFromUnityVector3(Vector3[] vector)
    {
        SerializableVector3[] vertices = new SerializableVector3[vector.Length];
        int i = 0;
        foreach (Vector3 v in vector)
        {
            vertices[i] = new SerializableVector3();
            vertices[i].x = v.x;
            vertices[i].y = v.y;
            vertices[i++].z = v.z;
        }
        return vertices; 
    }
    
    public static Vector3[] ConvertToUnityVector3(SerializableVector3[] vector)
    {
        Vector3[] vertices = new Vector3[vector.Length];
        for (int i = 0; i < vector.Length; i++)
        {
            vertices[i]= new Vector3(vector[i].x, vector[i].y, vector[i].z);
        }
        return vertices;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("x", x);
        info.AddValue("y", y);
        info.AddValue("z", z);
    }
    
    protected SerializableVector3(SerializationInfo info, StreamingContext context)
    {
        x = info.GetSingle("x");
        y = info.GetSingle("y");
        z = info.GetSingle("z");
    }
    
    public SerializableVector3()
    {
    }

    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }
}

[Serializable]
public class SerializableVector2 : ISerializable
{
    public float x;
    public float y;
    public static SerializableVector2[] ConvertFromUnityVector2(Vector2[] vector)
    {
        SerializableVector2[] vertices = new SerializableVector2[vector.Length];
        int i = 0;
        foreach (Vector2 v in vector)
        {
            vertices[i] = new SerializableVector2();
            vertices[i].x = v.x;
            vertices[i++].y = v.y;
        }
        return vertices; 
    }
    
    public static Vector2[] ConvertToUnityVector2(SerializableVector2[] vector)
    {
        Vector2[] vertices = new Vector2[vector.Length];
        for (int i = 0; i < vector.Length; i++)
        {
            vertices[i]= new Vector2(vector[i].x, vector[i].y);
        }
        return vertices;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("x", x);
        info.AddValue("y", y);
    }
    
    protected SerializableVector2(SerializationInfo info, StreamingContext context)
    {
        x = info.GetSingle("x");
        y = info.GetSingle("y");
    }
    
    public SerializableVector2()
    {
    }

    public SerializableVector2(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
    }
}