using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace StereoPair
{
	class Camera
	{
		public Point position;
		public Plane plane;
		public const double EyeDistance = 10;
		public const double DistToPlane = 50;

		public Camera(Point _position, Plane _plane)
		{
			position = _position;
			plane = _plane;
		}

		public void UpdatePlane()
		{
			Point P = GetDirectionOfView().Normalize(DistToPlane);//TODO: fix constant
			Point v = GetDirectionOfView().Normalize(1);
			plane = new Plane(P, v);
		}

		public Camera(Point _position)
		{
			position = _position;
			UpdatePlane();
		}

		public Point GetDirectionOfView()
		{
			return new Point(0, 0, 0) - position;
		}

		public void Rotate(Point normal, double angle)
		{
			position = position.Rotate(normal, angle);
			UpdatePlane();
		}

		public void SetLength(double len)
		{
			position = position.Normalize(len);
			UpdatePlane();
		}

		public Point[] GetEyes()
		{
			Point dir = GetDirectionOfView();
			Point vertical = new Point(0, 1, 0);
			Point toEye = dir.CrossProduct(vertical).Normalize(1) * EyeDistance;
			return new[] {position - toEye, position + toEye};
		}

		public Point2D[][] GetFrames(Polyhedron polyhedron, Point eye)
		{
			List<Point2D[]> frames = new List<Point2D[]>();
			Point yBasis = GeometryOperations.CentralProjectionVectorOnPlane(new Point(0, 1, 0), plane, eye);
			Point xBasis = plane.GetSecondBasisVector(yBasis);
			foreach (var polygon in polyhedron.faces)
			{
				if (this.IsVisible(polyhedron, polygon))
					frames.Add(polygon.CentralProjectionToPlane(plane, eye).ConvertTo2D(plane, xBasis, yBasis));
			}
			return frames.ToArray();
		}

		public Point2D[][][] GetFrames(Polyhedron polyhedron)
		{
			Point[] eyes = GetEyes();
			Point2D[][][] frames = new Point2D[2][][];
			frames[0] = GetFrames(polyhedron, eyes[0]);
			frames[1] = GetFrames(polyhedron, eyes[1]);
			return frames;
		}

		private bool IsVisible(Polyhedron polyhedron, Polygon polygon)
		{
			Point centralVertex = polygon.GetRandomPointInside();
					
			foreach (var face in polyhedron.faces)
			{
				Plane facePlane = face.GetPlane();
				Segment viewSegment = new Segment(position, centralVertex);
				if (face == polygon || facePlane.CheckBelongingOfLine(viewSegment.GetLine()))
					continue;
				Point interPoint = facePlane.Intersect(viewSegment.GetLine()); //TODO: something wrong... But now it seems to be ok
				if ((face.IsOnSide(interPoint) || face.IsInside(interPoint)) && viewSegment.CheckBelongingOfPoint(interPoint))
					return false;
			}
			return true;
		}
	}
}
