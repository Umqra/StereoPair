using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace RandomGenerator
{
    public class Generate
    {
	    public const int MaxCoordinat = 50;
	    public static Random Rand = new Random();
	    public static double SphereRadius = 50;
	    public static int GetRandomInt()
	    {
		    return Rand.Next(-MaxCoordinat, MaxCoordinat);
	    }
	    public static Point GetRandomPoint()
	    {
		    Random r = new Random();
			return new Point(
				GetRandomInt(), 
				GetRandomInt(), 
				GetRandomInt()
				);
	    }

	    public static Plane GetPlane(List<Point> points)
	    {
		    Point A = points[0], B = points[1], C = points[2];
		    return new Plane(A, B - A, C - A);
	    }

	    public static Tuple<double, double> GetTuple2D(Point P, Plane plane, Point e1, Point e2)
	    {
		    return Tuple.Create(
				P.ConvertTo2D(plane, e1, e2).x,
				P.ConvertTo2D(plane, e1, e2).y
			    );
	    }
	    public static Polygon GetConvexHull(List<Point> points)
	    {
		    Plane plane = GetPlane(points);
		    Point xBasis = GeometryOperations.OrthogonalProjectionVectorOnPlane(GetRandomPoint(), plane);
		    Point yBasis = plane.GetSecondBasisVector(xBasis);
		    Point leftBottomPoint = points.OrderBy(P => GetTuple2D(P, plane, xBasis, yBasis)).ElementAt(0);
		    Point2D leftBottomPoint2D = leftBottomPoint.ConvertTo2D(plane, xBasis, yBasis);
		    points = points
			    .OrderBy(P => (P.ConvertTo2D(plane, xBasis, yBasis) - leftBottomPoint2D).GetAngle())
			    .ThenBy(P => (P.ConvertTo2D(plane, xBasis, yBasis) - leftBottomPoint2D).Length())
				.ToList();
		    return new Polygon(points.Count, points.ToArray());
	    }

	    public static Polygon GetFace(List<Point> points, Plane plane)
	    {
		    List<Point> onPlane = new List<Point>();
		    foreach (var point in points)
		    {
			    if (plane.CheckBelongingOfPoint(point))
				    onPlane.Add(point);
		    }
		    return GetConvexHull(onPlane);
	    }
		public static Polyhedron GetRandomPolyhedron()
		{
			List<Polygon> faces = new List<Polygon>();
		    List<Point> points = new List<Point>();
		    int n = 30;
		    for (int i = 0; i < n; i++)
			    points.Add(GetRandomPoint());
			for (int a = 0; a < n; a++)
				for (int b = a + 1; b < n; b++)
					for (int c = b + 1; c < n; c++)
					{
						Point A = points[a], B = points[b], C = points[c];
						if ((A - B).Collinear(C - A))
							continue;
						Polygon polygon = GetFace(points, new Plane(A, B - A, C - A));
						if (IsItFace(points, polygon))
							faces.Add(polygon);
					}
			return new Polyhedron(faces.Count, faces.ToArray());
		}

	    private static bool IsItFace(List<Point> points, Polygon polygon)
	    {
		    Plane plane = polygon.GetPlane();
		    bool onTheSide = false, onTheOtherSide = false;
		    foreach (var point in points)
		    {
			    if (plane.PointOnTheSide(point))
				    onTheSide = true;
				else if (!plane.PointOnTheSide(point) && !plane.CheckBelongingOfPoint(point))
					onTheOtherSide = true;
		    }
		    return onTheOtherSide ^ onTheSide;
	    }

	    public static void WritePolyhedronToData()
	    {
		    Polyhedron polyhedron = GetRandomPolyhedron();
		    string pathToData = "poly.txt";
		    Console.WriteLine(polyhedron.ToString());
		    File.WriteAllLines(pathToData, polyhedron.ToString().Split('\n'));
	    }
    }
}
