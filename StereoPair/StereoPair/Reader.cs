using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace StereoPair
{
	class Reader
	{
		public static Polyhedron ReadData(String fileName)
		{
			String[] lines = File.ReadAllLines(fileName);
			int amountOfFaces = int.Parse(lines[0]);
			List<Polygon> polygons = new List<Polygon>();
			for (int i = 1; i < lines.Length; i++)
			{
				int[] values = lines[i].Split(' ').Select(int.Parse).ToArray();
				int amountOfVertices = values[0];
				Point[] vertices = new Point[amountOfVertices];
				for (int j = 1; j < amountOfVertices; j += 3)
				{
					vertices[j] = new Point(values[j], values[j + 1], values[j + 2]);
				}
				polygons.Add(new Polygon(amountOfFaces, vertices));
			}
			return new Polyhedron(amountOfFaces, polygons.ToArray());
		}
	}
}
