using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Parabox.CSG
{
    /// <summary>
    /// Represents a polygon face with an arbitrary number of vertices.
    /// </summary>
    internal sealed class Polygon
    {
        public Material material;
        public Plane plane;
        public List<Vertex> vertices;

        public Polygon(List<Vertex> list, Material mat)
        {
            vertices = list;
            plane = new Plane(list[0].position, list[1].position, list[2].position);
            material = mat;
        }

        public void Flip()
        {
            vertices.Reverse();

            for (int i = 0; i < vertices.Count; i++)
                vertices[i].Flip();

            plane = plane.Flipped();
        }

        public override string ToString()
        {
            return $"[{vertices.Count}] {plane.normal}";
        }

        public bool CanMerge(Polygon other)
        {
            if (material != other.material)
                return false;

            if (!IsCoplanar(other))
                return false;

            for (int i = 0; i < other.vertices.Count; i++)
            {
                int j;
                if ((j = vertices.IndexOf(other.vertices[i])) == -1)
                    continue;

                if (vertices[Mod(j + 1, vertices.Count)].Equals(other.vertices[Mod(i + 1, other.vertices.Count)]))
                    return true;

                if (vertices[Mod(j - 1, vertices.Count)].Equals(other.vertices[Mod(i + 1, other.vertices.Count)]))
                    return true;
            }

            return false;
        }

        private static int Mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }

        internal bool IsCoplanar(Polygon other)
        {
            return Mathf.Approximately(Mathf.Abs(Vector3.Dot(plane.normal.normalized, other.plane.normal.normalized)), 1);
        }

        public Polygon Merge(Polygon other)
        {
            List<Vertex> otherVertices = new List<Vertex>(other.vertices);

            for (int i = 0; i < otherVertices.Count; i++)
            {
                int j;
                if ((j = vertices.IndexOf(otherVertices[i])) == -1)
                    continue;

                if (vertices[Mod(j + 1, vertices.Count)].Equals(otherVertices[Mod(i + 1, otherVertices.Count)]))
                    return Merge(otherVertices.AsEnumerable().Reverse().ToList(), Mod(j + 1, vertices.Count), otherVertices.Count - (1 + i));

                if (vertices[Mod(j - 1, vertices.Count)].Equals(otherVertices[Mod(i + 1, otherVertices.Count)]))
                    return Merge(otherVertices, j, Mod(i + 1, otherVertices.Count));
            }

            return null;
        }

        private Polygon Merge(List<Vertex> otherVertices, int thisStart, int otherStart)
        {
            List<Vertex> thisVertices = new List<Vertex>(vertices);
            
            List<Vertex> mergedVertices = new List<Vertex>();
            
            mergedVertices.AddRange(thisVertices.GetRange(thisStart + 1, thisVertices.Count - thisStart - 1));
            if(thisStart != 0)
                mergedVertices.AddRange(thisVertices.GetRange(0, thisStart));
            
            mergedVertices.AddRange(otherVertices.GetRange(otherStart + 1, otherVertices.Count - otherStart - 1));
            if(otherStart != 0)
                mergedVertices.AddRange(otherVertices.GetRange(0, otherStart));


            List<Vertex> mergedWithoutDiplicates = RemoveColinearVertices(mergedVertices);

            return new Polygon(mergedWithoutDiplicates, material);
        }

        private  List<Vertex> RemoveColinearVertices(List<Vertex> mergedVertices)
        {
            List<Vertex> mergedWithoutColinear = new List<Vertex>();

            Vector3 lastDirection = mergedVertices[0].position - mergedVertices[mergedVertices.Count-1].position;
            Vector3 lastNormal = mergedVertices[mergedVertices.Count-1].normal;

            for (int i = 0; i < mergedVertices.Count; i++)
            {
                Vector3 currentDirection = mergedVertices[(i + 1) % mergedVertices.Count].position - mergedVertices[i].position;

                if (Vector3.Cross(lastDirection, currentDirection).sqrMagnitude == 0f && 
                    lastNormal == mergedVertices[i].normal && 
                    mergedVertices[i].normal == mergedVertices[(i + 1) % mergedVertices.Count].normal)
                    continue;

                mergedWithoutColinear.Add(mergedVertices[i]);
                lastDirection = currentDirection;
                lastNormal = mergedVertices[i].normal;
            }

            return mergedWithoutColinear;
        }
    }
}