using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
	public class Polygon
	{
		public readonly int n;
		public readonly Point[] vertices;

		public Polygon (int n_, Point[] vertices_)
		{
			n = n_;
			vertices = vertices_;
		}

		public int CheckBelongingOfPoint(Point O)
		{
			Point RandINFPoint = new Point(1e8, 1e8, 1e8);//TODO: Create normal random point on INF
			Segment checkSegment = new Segment(O, RandINFPoint);
			int cnt = 0;
			for (int i = 0; i < vertices.Length; i++)
			{
				Segment currSegment = new Segment(vertices[i], vertices[(i + 1) % vertices.Length]);
				if (currSegment.CheckBelongingOfPoint(O))
					return 0;
			}
			for (int i = 0; i < vertices.Length; i++)
			{
				Segment currSegment = new Segment(vertices[i], vertices[(i + 1) % vertices.Length]);
				if (currSegment.OnSameLine(checkSegment))
					throw new Exception("Segments belongs to the same line");//TODO: fix
				if (currSegment.FindIntersection(checkSegment) != null)
					cnt++;
			}
			if (cnt % 2 == 0)
				return -1;
			return 1;
		}
	}
}
