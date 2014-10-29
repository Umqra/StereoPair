using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
	public class Polyhedron
	{
		public readonly int n;
		public readonly Polygon[] faces;

		public Polyhedron(int n_, Polygon[] faces_)
		{
			n = n_;
			faces = faces_;
		}

		public Point GetBounds()
		{
			double minX = faces[0].vertices[0].x, maxX = faces[0].vertices[0].x,
					minY = faces[0].vertices[0].y, maxY = faces[0].vertices[0].y,
					minZ = faces[0].vertices[0].z, maxZ = faces[0].vertices[0].z;
			foreach (Polygon face in faces)
			{
				for (int i = 0; i < face.vertices.Length; i++)
				{
					minX = Math.Min(minX, face.vertices[i].x);
					minY = Math.Min(minY, face.vertices[i].y);
					minZ = Math.Min(minZ, face.vertices[i].z);
					maxX = Math.Max(maxX, face.vertices[i].x);
					maxY = Math.Max(maxY, face.vertices[i].y);
					maxZ = Math.Max(maxZ, face.vertices[i].z);
				}
			}
			return new Point(maxX - minX, maxY - minY, maxZ - minZ);
		}

		public override string ToString()
		{
			string result = "";
			result += String.Format("{0}\n", n);
			foreach (var polygon in faces)
				result += polygon.ToString();
			return result;
		}
	}
}
