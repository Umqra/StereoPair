using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
	class Polygon
	{
		public int n;
		public Point[] vertexes;

		public Polygon (int n_, Point[] vertexes_)
		{
			n = n_;
			vertexes = vertexes_;
		}

		public int CheckBelongingOfPoint(Point O)
		{
			Segment checkSegment = new Segment(O, Point.RandINFPoint);//TODO: Change second point
			int cnt = 0;
			for (int i = 0; i < vertexes.Length; i++)
			{
				Segment currSegment = new Segment(vertexes[i], vertexes[(i + 1) % vertexes.Length]);
				if (currSegment.CheckBelongingOfPoint(O))
					return 0;
			}
			for (int i = 0; i < vertexes.Length; i++)
			{
				Segment currSegment = new Segment(vertexes[i], vertexes[(i + 1) % vertexes.Length]);
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
