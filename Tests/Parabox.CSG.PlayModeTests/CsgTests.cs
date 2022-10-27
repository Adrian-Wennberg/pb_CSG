using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Parabox.CSG.PlayModeTests
{
    public class CsgTests
    {
        [UnityTest]
        public IEnumerator CsgSubtract_SubrtactingCubeFromCube_ReturnEmptlyModel()
        {
            // Arrange
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            yield return null;

            // Act
            Model result = CSG.Subtract(cube1, cube2);

            // Assert
            Assert.IsFalse(result.vertices.Any());
        }
        
        [UnityTest]
        public IEnumerator CsgSubtract_SubrtactingCubeFromBiggerCube_ReturnMinimalRepresentation()
        {
            // Arrange
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = Vector3.one * 0.5f;
            cube2.transform.position = Vector3.one;
            cube2.transform.localScale *= 2;
            yield return null;
            int newPolygonCount = 12;
            int newVertexCount = 2 * 4 * 3 + 3 * 7;

            // Act
            Model result = CSG.Subtract(cube2, cube1);

            // Assert
            Assert.AreEqual(newPolygonCount, result.ToPolygons().Count, $"Polygon count is not as expected");
            
            Assert.AreEqual(newVertexCount, result.vertices.Count, $"Vertex count is not as expected");
        }
        
        [UnityTest]
        public IEnumerator CsgSubtract_SubrtactingHalfCubeFromCube_ReturnMinimalRepresentation()
        {
            // Arrange
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3(1, 0.75f, 1);
            cube1.transform.localScale = new Vector3(1, 0.5f, 1);
            cube2.transform.position = Vector3.one;
            yield return null;
            int newPolygonCount = 6;
            int newVertexCount = 4 * 6;

            // Act
            Model result = CSG.Subtract(cube2, cube1);

            // Assert
            Assert.AreEqual(newPolygonCount, result.ToPolygons().Count, $"Polygon count is not as expected");
            Assert.AreEqual(newVertexCount, result.vertices.Count, $"Vertex count is not as expected");
        }

        [UnityTest]
        public IEnumerator CsgUnion_UnionCubeWithItself_SameCubeModel()
        {
            // Arrange
            GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            yield return null;
            Mesh cubeMesh = cube1.GetComponent<MeshFilter>().mesh;

            // Act
            Model result = CSG.Union(cube1, cube2);

            // Assert
            Assert.That(result.mesh.vertices, Is.SubsetOf(cubeMesh.vertices));
            Assert.That(cubeMesh.vertices, Is.SubsetOf(result.mesh.vertices));

            Assert.AreEqual(cubeMesh.vertices.Length, result.mesh.vertices.Length);
        }
    }
}