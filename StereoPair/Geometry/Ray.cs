using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
	class Ray
	{
		public readonly Point A, v;

		public Ray()
		{
			A = v = new Point();
		}

		public Ray(Point _A, Point _v)
		{
			A = _A;
			v = _v;
		}
		public Line GetLine()
		{
			return new Line(A, v);
		}

		public bool CheckBelongingOfPoint(Point P)
		{
			return GeometryOperations.PointOnRay(P, this);
		}

		public Segment Intersect(Segment s)
		{
			return GeometryOperations.IntersectRaySegment(this, s);
		}

		public bool OnSameLine(Segment s)
		{
			return GeometryOperations.RaySegmentOnSameLine(this, s);
		}

		public bool OnSameLine(Line l)
		{
			return GeometryOperations.LineRayOnSameLine(l, this);
		}
	}
}
