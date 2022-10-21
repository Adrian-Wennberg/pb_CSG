using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Parabox.CSG.Tests
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