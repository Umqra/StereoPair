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

		public Plane(Point _P, Point v1, Point v2)
		{
			P = _P;
			n = v1.CrossProduct(v2);
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

		public Point ProjectPoint(Point P)
		{
			return GeometryOperations.OrthogonalProjectionPointOnPlane(P, this);
		}

		public Polygon ProjectPolygon(Polygon polygon)
		{
			return GeometryOperations.OrthogonalProjectionPolygonOnPlane(polygon, this);
		}

		public Point GetSecondBasisVector(Point firstBasis)
		{
			return firstBasis.CrossProduct(n);
		}

		public bool PointOnTheSide(Point A)
		{
			return (A - P).DotProduct(n).IsGreater(0);
		}
	}
}
