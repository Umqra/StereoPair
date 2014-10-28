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
			for (int i = 0; i < n; i++)
			{
				double currAngle = vertices[(i + 1) % n].GetAngle(vertices[i]);
				sumAngle += currAngle;
			}
			if (sumAngle.IsEqual(0))
				return false;
			else if (Math.Abs(sumAngle).IsEqual(2 * Math.PI))
				return true;
			else
				throw new Exception("Strange angle " + sumAngle);
		}
	}
}
