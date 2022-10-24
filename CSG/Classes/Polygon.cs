using System.Collections.Generic;
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

            plane.Flip();
        }

        public override string ToString()
        {
            return $"[{vertices.Count}] {plane.normal}";
        }

        public bool CanMerge(Polygon other)
        {
            if (!IsCoplanar(other))
                return false;

            for (int i = 0; i < other.vertices.Count; i++)
            {
                int j;
                if ((j = vertices.IndexOf(other.vertices[i])) != -1)
                {
                    if (vertices[Mod(j + 1, vertices.Count)].Equals(other.vertices[Mod(i + 1, other.vertices.Count)]))
                        return true;
                    
                    if (vertices[Mod(j - 1, vertices.Count)].Equals(other.vertices[Mod(i + 1, other.vertices.Count)]))
                        return true;
                    
                    if (vertices[Mod(j + 1, vertices.Count)].Equals(other.vertices[Mod(i - 1, other.vertices.Count)]))
                        return true;
                    
                    if (vertices[Mod(j - 1, vertices.Count)].Equals(other.vertices[Mod(i - 1, other.vertices.Count)]))
                        return true;
                }
            }

            return false;
        }
        
        private int Mod(int x, int m) {
            int r = x%m;
            return r<0 ? r+m : r;
        }

        internal bool IsCoplanar(Polygon other)
        {
            return Mathf.Approximately(Mathf.Abs(Vector3.Dot(plane.normal.normalized, other.plane.normal.normalized)), 1);
        }
    }
}