using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Parabox.CSG.EditModeTests
{
    public class PolygonTests
    {
        private Material DiffusMaterial { get; } = new Material(Shader.Find("Diffuse"));
        
        [Test]
        public void PolygonTests_CombineOnePolygons_GetSamePolygon()
        {
            // Arrange
            List<Vertex> vertices = new List<Vertex>
            {
                new Vertex{position = new Vector3(0, 0, 0)},
                new Vertex{position = new Vector3(0, 0, 1)},
                new Vertex{position = new Vector3(1, 0, 1)},
            };


            List<Polygon> polygons = new List<Polygon>
            {
                new Polygon(new List<Vertex>
                {
                    vertices[0],
                    vertices[1],
                    vertices[2],
                }, DiffusMaterial),
            };
            
            // Act
            List<Polygon> combined = polygons.Combine();
            
            // Assert
            Assert.That(combined, Is.EquivalentTo(polygons));
        }
        
        [Test]
        public void PolygonTests_CombineTwoPolygons_GetOnePolygon()
        {
            // Arrange
            List<Vertex> vertices = new List<Vertex>
            {
                new Vertex{position = new Vector3(0, 0, 0)},
                new Vertex{position = new Vector3(0, 0, 1)},
                new Vertex{position = new Vector3(1, 0, 1)},
                new Vertex{position = new Vector3(1, 0, 0)},
            };


            List<Polygon> polygons = new List<Polygon>
            {
                new Polygon(new List<Vertex>
                {
                    vertices[0],
                    vertices[1],
                    vertices[2],
                }, DiffusMaterial),
                new Polygon(new List<Vertex>
                {
                    vertices[2],
                    vertices[0],
                    vertices[3],
                }, DiffusMaterial),
            };

            Polygon expectedPolygon = new Polygon(new List<Vertex>
            {
                vertices[0],
                vertices[1],
                vertices[2],
                vertices[3],
            }, DiffusMaterial);
            int expectedVertexCount = expectedPolygon.vertices.Count;
            
            // Act
            List<Polygon> combined = polygons.Combine();
            
            // Assert
            Assert.AreEqual(1, combined.Count);

            List<Vertex> combinedPolyVertices = combined[0].vertices;
            Assert.AreEqual(expectedVertexCount, combinedPolyVertices.Count);

            int firstMatch = 0;

            while (firstMatch < expectedVertexCount && !expectedPolygon.vertices[firstMatch].Equals(combinedPolyVertices[0]))
            {
                firstMatch++;
            }
            
            Assert.AreNotEqual(firstMatch, expectedVertexCount);
            
            
            if (!expectedPolygon.vertices[(firstMatch+1)%expectedVertexCount].Equals(combinedPolyVertices[1]))
            {
                combinedPolyVertices.Reverse();
                firstMatch = expectedVertexCount - firstMatch - 1;
            }

            for (int j = 0; j < expectedVertexCount; j++)
            {
                Assert.AreEqual(expectedPolygon.vertices[(firstMatch+j)%expectedVertexCount], combinedPolyVertices[j], $"Expected {expectedPolygon.vertices[(firstMatch+j)%expectedVertexCount].position}, but got {combinedPolyVertices[j].position}");
            }
            
        }
        
        [Test]
        public void PolygonTests_CombineTwoSquares_GetOneRectangle()
        {
            // Arrange
            List<Vertex> vertices = new List<Vertex>
            {
                new Vertex{position = new Vector3(0, 0, 0)},
                new Vertex{position = new Vector3(0, 0, 1)},
                new Vertex{position = new Vector3(1, 0, 1)},
                new Vertex{position = new Vector3(1, 0, 0)},
                new Vertex{position = new Vector3(1, 0, -1)},
                new Vertex{position = new Vector3(0, 0, -1)},
            };


            List<Polygon> polygons = new List<Polygon>
            {
                new Polygon(new List<Vertex>
                {
                    vertices[0],
                    vertices[1],
                    vertices[2],
                    vertices[3],
                }, DiffusMaterial),
                new Polygon(new List<Vertex>
                {
                    vertices[0],
                    vertices[3],
                    vertices[4],
                    vertices[5],
                }, DiffusMaterial)
            };
            
            
            Polygon expectedPolygon = new Polygon(new List<Vertex>
            {
                vertices[1],
                vertices[2],
                vertices[4],
                vertices[5],
            }, DiffusMaterial);
            int expectedVertexCount = expectedPolygon.vertices.Count;
            
            // Act
            List<Polygon> combined = polygons.Combine();
            
            // Assert
            Assert.AreEqual(1, combined.Count);
            
            
            List<Vertex> combinedPolyVertices = combined[0].vertices;
            Assert.AreEqual(expectedVertexCount, combinedPolyVertices.Count, " count does not match");

            int firstMatch = 0;

            while (firstMatch < expectedVertexCount && !expectedPolygon.vertices[firstMatch].Equals(combinedPolyVertices[0]))
            {
                firstMatch++;
            }
            
            Assert.AreNotEqual(firstMatch, expectedVertexCount);
            
            
            if (!expectedPolygon.vertices[(firstMatch+1)%expectedVertexCount].Equals(combinedPolyVertices[1]))
            {
                combinedPolyVertices.Reverse();
                firstMatch = expectedVertexCount - firstMatch - 1;
            }

            for (int j = 0; j < expectedVertexCount; j++)
            {
                Assert.AreEqual(expectedPolygon.vertices[(firstMatch+j)%expectedVertexCount], combinedPolyVertices[j], $"Expected {expectedPolygon.vertices[(firstMatch+j)%expectedVertexCount].position}, but got {combinedPolyVertices[j].position}");
            }

        }
        
        
        [Test]
        public void PolygonTests_CombineFourPolygons_GetOnePolygon()
        {
            // Arrange
            List<Vertex> vertices = new List<Vertex>
            {
                new Vertex{position = new Vector3(0, 0, 0)},
                new Vertex{position = new Vector3(0, 0, 1)},
                new Vertex{position = new Vector3(1, 0, 1)},
                new Vertex{position = new Vector3(1, 0, 0)},
                new Vertex{position = new Vector3(1, 0, -1)},
                new Vertex{position = new Vector3(0, 0, -1)},
                new Vertex{position = new Vector3(-1, 0, -1)},
                new Vertex{position = new Vector3(-1, 0, 0)},
                new Vertex{position = new Vector3(-1, 0, 1)},
            };


            List<Polygon> polygons = new List<Polygon>
            {
                new Polygon(new List<Vertex>
                {
                    vertices[0],
                    vertices[1],
                    vertices[2],
                    vertices[3],
                }, DiffusMaterial),
                new Polygon(new List<Vertex>
                {
                    vertices[0],
                    vertices[3],
                    vertices[4],
                    vertices[5],
                }, DiffusMaterial),               
                new Polygon(new List<Vertex>
                {
                    vertices[0],
                    vertices[5],
                    vertices[6],
                    vertices[7],
                }, DiffusMaterial),                
                new Polygon(new List<Vertex>
                {
                    vertices[0],
                    vertices[7],
                    vertices[8],
                    vertices[1],
                }, DiffusMaterial),
            };
            
            
            Polygon expectedPolygon = new Polygon(new List<Vertex>
            {
                vertices[2],
                vertices[4],
                vertices[6],
                vertices[8],
            }, DiffusMaterial);
            int expectedVertexCount = expectedPolygon.vertices.Count;
            
            // Act
            List<Polygon> combined = polygons.Combine();
            
            // Assert
            Assert.AreEqual(1, combined.Count);
            
            
            List<Vertex> combinedPolyVertices = combined[0].vertices;
            Assert.AreEqual(expectedVertexCount, combinedPolyVertices.Count, "Count does not match");

            int firstMatch = 0;

            while (firstMatch < expectedVertexCount && !expectedPolygon.vertices[firstMatch].Equals(combinedPolyVertices[0]))
            {
                firstMatch++;
            }
            
            Assert.AreNotEqual(firstMatch, expectedVertexCount);
            
            
            if (!expectedPolygon.vertices[(firstMatch+1)%expectedVertexCount].Equals(combinedPolyVertices[1]))
            {
                combinedPolyVertices.Reverse();
                firstMatch = expectedVertexCount - firstMatch - 1;
            }

            for (int j = 0; j < expectedVertexCount; j++)
            {
                Assert.AreEqual(expectedPolygon.vertices[(firstMatch+j)%expectedVertexCount], combinedPolyVertices[j], $"Expected {expectedPolygon.vertices[(firstMatch+j)%expectedVertexCount].position}, but got {combinedPolyVertices[j].position}");
            }

        }
    }
}