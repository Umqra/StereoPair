using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
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
		public static bool PointOnLine(Point A, Line l)
		{
			return (A - l.A).CrossProduct(l.v).Length().IsEqual(0);
		}
		public static bool LineOnPlane(Line l, Plane p)
		{
			return PointOnPlane(l.A, p) && l.v.DotProduct(p.n).IsEqual(0);
		}
		public static bool PointOnRay(Point A, Ray r)
		{
			return PointOnLine(A, r.GetLine()) && (A - r.A).DotProduct(r.v).IsGreaterOrEqual(0);
		}

		public static bool PointOnSegment(Point A, Segment s)
		{
			return PointOnRay(A, new Ray(s.A, s.B - s.A)) &&
					PointOnRay(A, new Ray(s.B, s.A - s.B));
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
		public static Segment IntersectRaySegment(Ray r, Segment s)
		{
			if (r.GetLine().Equals(s.GetLine()))
			{
				if (r.CheckBelongingOfPoint(s.A))
				{
					if (r.CheckBelongingOfPoint(s.B))
						return s;
					return new Segment(r.A, s.A);
				}
				if (r.CheckBelongingOfPoint(s.B))
					return new Segment(r.A, s.B);
				return null;
			}
			if (r.GetLine().Parallel(s.GetLine()))
				return null;
			Point P = r.GetLine().Intersect(s.GetLine());
			if (PointOnRay(P, r) && PointOnSegment(P, s))
				return new Segment(P, P);
			return null;
		}

		public static bool LineSegmentOnSameLine(Line l, Segment s)
		{
			return l.Equals(s.GetLine());
		}
		public static bool LineRayOnSameLine(Line l, Ray r)
		{
			return l.Equals(r.GetLine());
		}
		public static bool RaySegmentOnSameLine(Ray r, Segment s)
		{
			return r.GetLine().Equals(s.GetLine());
		}

		public static Point ProjectPointOnPlane(Point P, Plane plane)
		{
			return IntersectLinePlane(new Line(P, plane.n), plane);
		}
		public static Polygon ProjectPolygonOnPlane(Polygon polygon, Plane plane)
		{
			List <Point> vertices = new List<Point>();
			foreach (var point in polygon.vertices)
				vertices.Add(ProjectPointOnPlane(point, plane));
			return new Polygon(vertices.Count, vertices.ToArray());
		}
	}
}
