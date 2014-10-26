using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
	static class GeometryOperations
	{
		public static bool PointOnPlane(Point A, Plane p)
		{
			return (A - p.P).DotProduct(p.n).IsEqual(0);
		}

		public static Point IntersectLinePlane(Line l, Plane p)
		{
			if (p.CheckBelongingOfLine(l))
				throw new ArgumentException("Bad argument - line belong to plane");
			if (p.n.DotProduct(l.v).IsEqual(0))
				return null;
			double k = (p.P.DotProduct(p.n) - l.A.DotProduct(p.n)) / (p.n.DotProduct(l.v));
			return l.A + k * l.v;
		}

		public static bool LineOnPlane(Line l, Plane p)
		{
			return PointOnPlane(l.A, p) && l.v.DotProduct(p.n).IsEqual(0);
		}
	}
}
