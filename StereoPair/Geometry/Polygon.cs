using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
		/// <summary>
		/// Returns ABSOLUTELY random basis, which can be not orthonormal
		/// </summary>
		/// <returns></returns>
		public Point[] GetRandomBasis()
		{
			return new Point[] {(vertices[1] - vertices[0]).Normalize(1), (vertices[2] - vertices[1]).Normalize(1)};
		}

		public Plane GetPlane()
		{
			Point[] e = GetRandomBasis();
			return new Plane(vertices[0], e[0], e[1]);	
		}

		public Polygon OrthogonalProjectionToPlane(Plane plane)
		{
			return GeometryOperations.OrthogonalProjectionPolygonOnPlane(this, plane);
		}
		public Polygon CentralProjectionToPlane(Plane plane, Point center)
		{
			return GeometryOperations.CentralProjectionPolygonOnPlane(this, plane, center);
		}

		public Point GetRandomPointInside()
		{
			for (int i = 0; i < n; i++)
			{
				Point A = vertices[i];
				Point B = vertices[(i + 1) % n];
				Point C = vertices[(i + 2) % n];
				Point O = (A + B + C) / 3;
				if (IsInside(O))
					return O;
			}
			throw new Exception("Point is not found");
		}

		public IEnumerable<Point> GetRelevantPoints(Polygon a, Plane plane, Point O)
		{
			Polygon pol1 = a.CentralProjectionToPlane(plane, O);
			Polygon pol2 = this.CentralProjectionToPlane(plane, O);
			foreach (var point in pol1.vertices)
				yield return point;
			foreach (var point in pol2.vertices)
				yield return point;
			for (int i = 0; i < pol1.vertices.Length; i++)
			{
				for (int j = 0; j < pol2.vertices.Length; j++)
				{
					Segment segm1 = new Segment(pol1.vertices[i], pol1.vertices[(i + 1) % pol1.vertices.Length]);
					Segment segm2 = new Segment(pol2.vertices[j], pol2.vertices[(j + 1) % pol2.vertices.Length]);
					if (segm1.FindIntersection(segm2) != null)
					{
						Segment intersection = segm1.FindIntersection(segm2);
						Point A = intersection.A, B = intersection.B;
							
						yield return segm1.FindIntersection(segm2).A;
						if (A != B)
							yield return segm1.FindIntersection(segm2).B;
					}
				}
			}
		}

		public int GetTypeOverlapping(Polygon a, Plane plane, Point O)
		{
			foreach (Point point in GetRelevantPoints(a, plane, O))
			{
				Line line = new Line(O, (point - O));
				if (this.GetPlane().CheckBelongingOfLine(line) || a.GetPlane().CheckBelongingOfLine(line))
					continue;
				Point A = this.GetPlane().Intersect(line);
				if (A == null || !this.IsInside(A) && !this.IsOnSide(A))
					continue;
				Point B = a.GetPlane().Intersect(line);
				if (B == null || !a.IsInside(B) && !a.IsOnSide(B))
					continue;
				if ((A - O).Length().IsGreater((B - O).Length()))
					return 1;
				else if ((A - O).Length().IsLess((B - O).Length()))
					return -1;
			}
			return 0;
		}

		public Point GetBounds()
		{
			double minX = vertices[0].x, maxX = vertices[0].x, 
					minY = vertices[0].y, maxY = vertices[0].y, 
					minZ = vertices[0].z, maxZ = vertices[0].z;
			for (int i = 0; i < vertices.Length; i++)
			{
				minX = Math.Min(minX, vertices[i].x);
				minY = Math.Min(minY, vertices[i].y);
				minZ = Math.Min(minZ, vertices[i].z);
				maxX = Math.Max(maxX, vertices[i].x);
				maxY = Math.Max(maxY, vertices[i].y);
				maxZ = Math.Max(maxZ, vertices[i].z);
			}
			return new Point(maxX - minX, maxY - minY, maxZ - minZ);
		}

		public Point2D[] ConvertTo2D(Plane currPlane, Point e1, Point e2)
		{
			//if (vertices[0].Equals(vertices[1]) || vertices[1].Equals(vertices[2]) || (vertices[1] - vertices[0]).CrossProduct(vertices[2] - vertices[1]).Length().IsEqual(0))
				//throw new NotImplementedException("Polygon is bad");
			if (e1.CrossProduct(e2).Length().IsEqual(0) || !e1.CrossProduct(e2).CrossProduct(currPlane.n).Length().IsEqual(0))
				throw new Exception("Bad basis");
			Point2D[] result = new Point2D[vertices.Length];
			for (int i = 0; i < vertices.Length; i++)
			{
				result[i] = vertices[i].ConvertTo2D(currPlane, e1, e2);
			}
			return result;
		}

		public bool IsOnSide(Point O)
		{
			for (int i = 0; i < vertices.Length; i++)
			{
				Segment currSegment = new Segment(vertices[i], vertices[(i + 1) % vertices.Length]);
				if (currSegment.CheckBelongingOfPoint(O))
					return true;
			}
			return false;
		}

		public bool IsInside(Point O)
		{
			if (!this.GetPlane().CheckBelongingOfPoint(O))
				throw new Exception("Point is not on the plane");
			if (IsOnSide(O))
				return false;
			double sumAngle = 0;
			Point normal = this.GetPlane().n;
			for (int i = 0; i < n; i++)
			{
				double currAngle = (vertices[(i + 1) % n] - O).GetAngle(vertices[i] - O, normal);
				sumAngle += currAngle;
			}
			if (sumAngle.IsEqual(0))
				return false;
			else if (Math.Abs(sumAngle).IsEqual(2 * Math.PI))
				return true;
			else
				throw new Exception("Strange angle " + sumAngle);
		}

		public override string ToString()
		{
			string result = "";
			result += String.Format("{0} ", n);
			foreach (var vertex in vertices)
				result += vertex.ToString();
			result += "\n";
			return result;
		}
	}
}
