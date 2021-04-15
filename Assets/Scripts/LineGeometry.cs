/*
Process for obj (blender):
    Duplicate object
    Delete all faces (only faces)
    Cleanup any ugly lines
    Ship it

Process for fbx (Maya): *might need ascii fbx*
    ??
    ??
    TBA

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class LineGeometry : MonoBehaviour
{
    public enum GeomType
    {
        file,
        generate,
        crystal
    }
    public GeomType geomType = GeomType.generate;

    public string path;
    public Material linemat;

    private int[] edges;
    private int numVerts;

    void Start()
    {

        //TODO convert to sub-mesh
        GameObject linemesh = new GameObject();
        linemesh.transform.parent = transform;
        linemesh.transform.position = this.transform.position;
        linemesh.transform.rotation = this.transform.rotation;
        linemesh.transform.localScale = Vector3.one;
        MeshFilter lineMeshFilter = linemesh.AddComponent<MeshFilter>();
        
        Mesh lineMesh = new Mesh();
        lineMeshFilter.mesh = lineMesh;
        lineMesh.vertices = GetComponent<MeshFilter>().mesh.vertices;

        //For some reason vertices.Length gives x3 actual length?
        //maybe counting length of each vector3
        numVerts = lineMesh.vertices.Length /3;

        int[] line = new int[0];

        switch(geomType)
        {
            case GeomType.file:
                if ((path.EndsWith(".obj")||path.EndsWith(".txt")))
                {
                    line = LoadLineData(path);   
                }
                else
                {
                    line = GetLineData(GetComponent<MeshFilter>().mesh.triangles);
                }
            break;

            case GeomType.generate:
                line = GetLineData(GetComponent<MeshFilter>().mesh.triangles);
            break;

            case GeomType.crystal:
                line = new int[]
                {
                    1,2,
                    4,3,
                    5,6,
                    8,7,
                    3,1,
                    2,4,
                    3,5,
                    6,4,
                    7,5,
                    6,8,
                    1,7,
                    8,2,
                    9,3,
                    4,10,
                    10,9,
                    6,11,
                    11,10,
                    5,12,
                    12,11,
                    9,12,
                    13,9,
                    10,14,
                    13,14,
                    11,15,
                    14,15,
                    12,16,
                    15,16,
                    16,13
                };
                for (int i = 0; i < line.Length; i++)
                {
                    line[i] -= 1;
                }
            break;
        }

        Debug.Log(lineMesh.vertices.Length);
        
        lineMesh.SetIndices(line, MeshTopology.Lines, 0);
        MeshRenderer linerenderer = linemesh.AddComponent<MeshRenderer>();
        linerenderer.material = linemat;
    }

    //Line Data Generation
    public struct Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X {get;}
        public int Y {get;}

        public override bool Equals(object other)
        {
            if (other == null || !this.GetType().Equals(other.GetType()))
            {
                return false;
            }
            else
            {
                Point p = (Point) other;
                return (X == p.X) && (Y == p.Y);
            }
        }

        public override int GetHashCode()
        {
            return (X << 2) ^ Y;
        }
    }

    int[] GetLineData(int[] triangles)
    {
        HashSet<Point> lines = new HashSet<Point>();

        for(int i = 0; i < triangles.Length/3; i++)
        {
            int j = i * 3;

            lines.Add(new Point(triangles[j], triangles[j+1]));
            lines.Add(new Point(triangles[j+1], triangles[j+2]));
            lines.Add(new Point(triangles[j+2], triangles[j]));
        }

        int[] intArray = new int[lines.Count * 2];
        int count = 0;
        foreach (Point line in lines)
        {
            intArray[count] = line.X;
            intArray[count+1] = line.Y;
            count+=2;
        }
        return intArray;
    }

    int[] LoadLineData(string path)
    {
        string finalPath = Application.dataPath + '\\' + path;
        List<int> lines = new List<int>();
        using (StreamReader sr = new StreamReader(finalPath))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith("l"))
                {
                    string[] splitLine = line.Split(' ');
                    lines.Add(System.Int32.Parse(splitLine[1]));
                    lines.Add(System.Int32.Parse(splitLine[2]));
                }
            }
        }

        if (finalPath.EndsWith(".obj"))
        {
            for (int i = 0; i < lines.Count; i++)
            {
                Debug.Log(lines[i]);
                lines[i] -= 1;
            }
        }

        return lines.ToArray(); 
    }
}