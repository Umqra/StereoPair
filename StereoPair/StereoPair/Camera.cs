using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;
using Point = Geometry.Point;

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
			Point P = position + GetDirectionOfView().Normalize(DistToPlane);//TODO: fix constant
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

		public AppPolygon2D[] GetFrames(AppPolyhedron polyhedron, Point eye, int ColorMode)
		{
			List<AppPolygon2D> frames = new List<AppPolygon2D>();
			Point yBasis = GeometryOperations.CentralProjectionVectorOnPlane(new Point(0, 1, 0), plane, eye).Normalize(0.1);
			Point xBasis = plane.GetSecondBasisVector(yBasis).Normalize(0.1);
			Polygon[] faces = polyhedron.faces;
			for (int i = 0; i < faces.Length; i++)
			{
				if (this.IsVisible(polyhedron, faces[i]))
				{
					Point2D[] converted = faces[i].CentralProjectionToPlane(plane, eye).ConvertTo2D(plane, xBasis, yBasis);
					AppPolygon2D frame = new AppPolygon2D(converted.Length, converted, (ColorMode == 0 ? GetFaceColor(faces[i], eye) : polyhedron.ListColors[i]));
					frames.Add(frame);
				}
			}
			return frames.ToArray();
		}

		public void TopSort(int v, List<int>[] edges, int[] topSort, ref int ind)
		{
			if (topSort[v] != -1)
				return;
			topSort[v] = -2;
			foreach (var to in edges[v])
			{
				TopSort(to, edges, topSort, ref ind);
			}
			topSort[v] = ind++;
		}

		public AppPolygon2D[] GetFramesTopSort(AppPolyhedron polyhedron, Point eye, int ColorMode)
		{
			List <int>[] edges = new List<int>[polyhedron.faces.Length];
			int[] topSort = new int[polyhedron.faces.Length];
			for (int i = 0; i < topSort.Length; i++)
				topSort[i] = -1;
			for (int i = 0; i < polyhedron.faces.Length; i++)
			{
				edges[i] = new List<int>();
				for (int j = 0; j < polyhedron.faces.Length; j++)
				{
					if (polyhedron.faces[i].IsOverlapped(polyhedron.faces[j], plane, eye))
					{
						edges[i].Add(j);
					}
				}
			}

			int ind = 0;
			for (int i = 0; i < polyhedron.faces.Length; i++)
			{
				TopSort(i, edges, topSort, ref ind);
			}
			AppPolygon[] faces = Enumerable.Range(0, polyhedron.faces.Length)
				.OrderBy(x => topSort[x])
				.Select(x => new AppPolygon(polyhedron.faces[x].n, polyhedron.faces[x].vertices, (ColorMode == 0 ? GetFaceColor((Polygon)polyhedron.faces[x], eye) : polyhedron.ListColors[x])))
				.ToArray();

			List<AppPolygon2D> frames = new List<AppPolygon2D>();
			Point yBasis = GeometryOperations.CentralProjectionVectorOnPlane(new Point(0, 1, 0), plane, eye).Normalize(0.1);
			Point xBasis = plane.GetSecondBasisVector(yBasis).Normalize(0.1);
			for (int i = 0; i < faces.Length; i++)
			{
				Point2D[] converted = faces[i].CentralProjectionToPlane(plane, eye).ConvertTo2D(plane, xBasis, yBasis);
				AppPolygon2D frame = new AppPolygon2D(converted.Length, converted, (ColorMode == 0 ? GetFaceColor(faces[i], eye) : faces[i].color/*polyhedron.ListColors[i]*/));
				frames.Add(frame);
			}
			frames.Reverse();
			return frames.ToArray();
		}

		public Color GetFaceColor(Polygon face, Point eye)
		{
			Point pointInside = face.GetRandomPointInside();
			double normalizeCoeff = Math.Max(1.1, (pointInside - eye).Length() / 100);
			double grayValue = 240 / normalizeCoeff;
			int gray = (int) grayValue;
			return Color.FromArgb(gray, gray, gray);
		}

		public AppPolygon2D[][] GetFrames(AppPolyhedron polyhedron, int ColorMode)
		{
			Point[] eyes = GetEyes();
			AppPolygon2D[][] frames = new AppPolygon2D[2][];
			//frames[0] = GetFrames(polyhedron, eyes[0], ColorMode);
			//frames[1] = GetFrames(polyhedron, eyes[1], ColorMode);
			frames[0] = GetFramesTopSort(polyhedron, eyes[0], ColorMode);
			frames[1] = GetFramesTopSort(polyhedron, eyes[1], ColorMode);
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
				if (interPoint == null)
					continue;
				if ((face.IsOnSide(interPoint) || face.IsInside(interPoint)) && viewSegment.CheckBelongingOfPoint(interPoint))
					return false;
			}
			return true;
		}
	}
}
