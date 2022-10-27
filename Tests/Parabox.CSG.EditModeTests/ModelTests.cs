using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Parabox.CSG.EditModeTests
{
    public class ModelTests
    {
        [Test]
        public void ModelTests_WhenCreateModelFromPolygons_NoError()
        {
            // Arrange
            List<Polygon> expectedPolygons = new List<Polygon>();
            int vertexCount = 12;

            List<Vertex> vertices = new List<Vertex>();
            for (int i = 0; i < vertexCount; i++)
            {
                vertices.Add(new Vertex
                {
                    position = Vector3.right * i + Vector3.forward * (i % 2),
                    normal = Vector3.up + 0.01f * i * Vector3.right,
                });
            }

            Material mat = new Material(Shader.Find("Diffuse"));
            for (int i = 0; i < vertexCount - 2; i++)
            {
                expectedPolygons.Add(new Polygon(new List<Vertex>
                {
                    vertices[i],
                    vertices[i + 1],
                    vertices[i + 2],
                }, mat));
            }


            // Act
            Model model = new Model(expectedPolygons);


            // Assert
            List<Polygon> modelPolygons = model.ToPolygons();
            
            Assert.IsNotEmpty(expectedPolygons);

        }

        [Test]
        public void ModelTests_WhenCreateModelFromPolygons_MergesVertices()
        {
            // Arrange
            List<Polygon> polygons = new List<Polygon>();
            int vertexCount = 12;

            List<Vertex> vertices = new List<Vertex>();
            for (int i = 0; i < vertexCount; i++)
            {
                vertices.Add(new Vertex
                {
                    position = Vector3.right * i + Vector3.forward * (i % 2),
                    normal = Vector3.up + 0.01f * i * Vector3.right,
                });
            }

            Material mat = new Material(Shader.Find("Diffuse"));
            for (int i = 0; i < vertexCount - 2; i++)
            {
                polygons.Add(new Polygon(new List<Vertex>
                {
                    vertices[i],
                    vertices[i + 1],
                    vertices[i + 2],
                }, mat));
            }


            // Act
            Model model = new Model(polygons);


            // Assert
            Assert.AreEqual(vertexCount, model.vertices.Count);
        }

        [Test]
        public void ModelTests_PloygonIsCoplanar_Works()
        {
            // Arrange
            Material mat = new Material(Shader.Find("Diffuse"));
            Polygon horizPoly1 = new Polygon(new List<Vertex>
            {
                new Vertex {position = Vector3.zero},
                new Vertex {position = Vector3.right},
                new Vertex {position = Vector3.forward + Vector3.right},
                new Vertex {position = Vector3.forward}
            }, mat);
            Polygon horizPoly2 = new Polygon(new List<Vertex>
            {
                new Vertex {position = Vector3.zero},
                new Vertex {position = Vector3.left},
                new Vertex {position = Vector3.forward + Vector3.left},
                new Vertex {position = Vector3.forward}
            }, mat);
            Polygon verticalPoly = new Polygon(new List<Vertex>
            {
                new Vertex {position = Vector3.zero},
                new Vertex {position = Vector3.up},
                new Vertex {position = Vector3.forward + Vector3.up},
                new Vertex {position = Vector3.forward}
            }, mat);

            // Assert
            Assert.True(horizPoly1.IsCoplanar(horizPoly2));
            Assert.False(horizPoly1.IsCoplanar(verticalPoly));
            Assert.False(verticalPoly.IsCoplanar(horizPoly2));
        }

        [Test]
        public void ModelTests_PloygonCanMerge_Works()
        {
            // Arrange
            Material mat = new Material(Shader.Find("Diffuse"));
            Polygon horizPoly1 = new Polygon(new List<Vertex>
            {
                new Vertex {position = Vector3.zero},
                new Vertex {position = Vector3.right},
                new Vertex {position = Vector3.forward + Vector3.right},
                new Vertex {position = Vector3.forward}
            }, mat);
            Polygon horizPoly2 = new Polygon(new List<Vertex>
            {
                new Vertex {position = Vector3.zero},
                new Vertex {position = Vector3.left},
                new Vertex {position = Vector3.forward + Vector3.left},
                new Vertex {position = Vector3.forward}
            }, mat);
            Polygon horizPoly3 = new Polygon(new List<Vertex>
            {
                new Vertex {position = Vector3.left},
                new Vertex {position = Vector3.left + Vector3.left},
                new Vertex {position = Vector3.forward + Vector3.left},
                new Vertex {position = Vector3.forward}
            }, mat);
            Polygon verticalPoly = new Polygon(new List<Vertex>
            {
                new Vertex {position = Vector3.zero},
                new Vertex {position = Vector3.up},
                new Vertex {position = Vector3.forward + Vector3.up},
                new Vertex {position = Vector3.forward}
            }, mat);

            // Assert
            Assert.True(horizPoly1.CanMerge(horizPoly2));
            Assert.False(horizPoly1.CanMerge(horizPoly3));
            Assert.False(horizPoly1.CanMerge(verticalPoly));
            Assert.False(verticalPoly.CanMerge(horizPoly2));
        }
    }
}