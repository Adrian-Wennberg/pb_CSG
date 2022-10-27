using System.Collections.Generic;
using System.Linq;

namespace Parabox.CSG
{
    public static class PolygonUtility
    {
        internal static List<Polygon> Combine(this List<Polygon> inPolygons)
        {
            List<Polygon> newPolygons = new List<Polygon>();

            foreach (Polygon polygon in inPolygons)
            {
                List<int> canMergeWith = new List<int>();

                for (int i = 0; i < newPolygons.Count; i++)
                {
                    if (polygon.CanMerge(newPolygons[i]))
                        canMergeWith.Add(i);
                }

                if (!canMergeWith.Any())
                {
                    newPolygons.Add(polygon);
                    continue;
                }

                Polygon currentPolygon = polygon;

                while (canMergeWith.Count != 0)
                {
                    Polygon mergedPolygon = currentPolygon.Merge(newPolygons[canMergeWith[0]]);
                    newPolygons[canMergeWith[0]] = mergedPolygon;
                    newPolygons.Remove(currentPolygon);
                    currentPolygon = mergedPolygon;
                    canMergeWith.Clear();
                    for (int i = 0; i < newPolygons.Count; i++)
                    {
                        if (newPolygons[i] != currentPolygon && currentPolygon.CanMerge(newPolygons[i]))
                            canMergeWith.Add(i);
                    }
                }
            }
            
            return newPolygons;
        }
    }
}