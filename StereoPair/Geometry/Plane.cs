using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
	public class Plane
	{
		public readonly Point P, n;

		public Plane()
		{
			P = new Point();
			n = new Point();
		}

		public Plane(Point _P, Point _n)
		{
			P = _P;
			n = _n;
		}

		public bool CheckBelongingOfPoint(Point P)
		{
			return GeometryOperations.PointOnPlane(P, this);
		}

		public bool CheckBelongingOfLine(Line l)
		{
			return GeometryOperations.LineOnPlane(l, this);
		}
		public Point Intersect(Line l)
		{
			return GeometryOperations.IntersectLinePlane(l, this);
		}
	}
}
