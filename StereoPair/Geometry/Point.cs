using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
    public class Point
    {
	    public readonly double x, y, z;

	    public Point()
	    {
		    x = y = z = 0;
	    }

	    public Point(double _x, double _y, double _z)
	    {
		    x = _x;
		    y = _y;
		    z = _z;
	    }

	    public static Point operator + (Point a, Point b)
	    {
		    return new Point(a.x + b.x, a.y + b.y, a.z + b.z);
	    }

	    public static Point operator - (Point a, Point b)
	    {
		    return new Point(a.x - b.x, a.y - b.y, a.z - b.z);
	    }

	    public static Point operator * (Point a, double k)
	    {
		    return new Point(a.x * k, a.y * k, a.z * k);
	    }

	    public static Point operator * (double k, Point a)
	    {
		    return new Point(a.x * k, a.y * k, a.z * k);
	    }

	    public static Point operator / (Point a, double k)
	    {
		    if (k.IsEqual(0))
				throw new ArgumentException("Can't divide point to 0.0");
		    return a * (1 / k);
	    }

	    public Point2D ConvertTo2D(Plane p, Point xBasis, Point yBasis)
	    {
		    Point v = this - p.P;
		    double xCoord = v.DotProduct(xBasis) / xBasis.Length() / xBasis.Length();
			double yCoord = v.DotProduct(yBasis) / yBasis.Length() / yBasis.Length();
		    return new Point2D(xCoord, yCoord);
	    }

	    public Point Normalize(double len)
	    {
			if (len.IsLess(0))
				throw new Exception("Can't set negative length");
		    if (ComprasionDouble.AreEqual(len, 0))
		    {
			    if (IsNullVector())
				    return this;
				throw new ArgumentException("Can't set non-zero length to null vector.");
		    }
		    return this / Length() * len;
	    }

	    public double Length()
	    {
		    return Math.Sqrt(this.DotProduct(this));
	    }

	    public double DotProduct(Point a)
	    {
		    return x * a.x + y * a.y + z * a.z;
	    }

	    public Point CrossProduct(Point a)
	    {
		    return new Point(
				y * a.z - z * a.y,
				-(x * a.z - z * a.x),
				x * a.y - y * a.x
			    );
	    }
	    public bool IsNullVector()
	    {
		    return x.IsEqual(0) && y.IsEqual(0) && z.IsEqual(0);
	    }

	    public bool Equals(Point a)
	    {
		    return x.IsEqual(a.x) && y.IsEqual(a.y) && z.IsEqual(a.z);
	    }

	    public bool OnPlane(Plane a)
	    {
		    return GeometryOperations.PointOnPlane(this, a);
	    }

	    public bool OnRay(Ray r)
	    {
		    return GeometryOperations.PointOnRay(this, r);
	    }

	    public override bool Equals(object obj)
	    {
		    if (obj == null || GetType() != obj.GetType())
			    return false;
		    Point p = obj as Point;
		    return Equals(p);
	    }

	    public override int GetHashCode()
	    {
		    int hashcode = x.GetHashCode() ^
							y.GetHashCode() ^
							z.GetHashCode();
		    return hashcode;
	    }

	    public override string ToString()
	    {
		    return String.Format("{0} {1} {2} ", x, y, z);
	    }

		public double GetUnsignedAngle(Point v)
		{
			return GeometryOperations.GetUnsignedAngle(this, v);
		}

		public double GetAngle(Point v, Point normal)
	    {
			return GeometryOperations.GetAngle(this, v, normal);
	    }

	    public Point OrthogonalProjectionOnPlane(Plane plane)
	    {
		    return GeometryOperations.OrthogonalProjectionPointOnPlane(this, plane);
	    }
		public Point CentralProjectionOnPlane(Plane plane, Point center)
		{
			return GeometryOperations.CentralProjectionPointOnPlane(this, plane, center);
		}

	    public bool Collinear(Point v)
	    {
		    return this.CrossProduct(v).Length().IsEqual(0);
	    }

	    public Point Rotate(Point v, double angle)
	    {
		    Point a = v * this.DotProduct(v) / v.Length() / v.Length();
		    Point b = this - a;
		    Point u = v.CrossProduct(b) / v.Length();
		    Point result = a + (u * Math.Sin(angle) + b * Math.Cos(angle));
			//Thank you very much, sweet exception :)
		    //if (!Math.Abs(this.GetAngle(result, v)).IsEqual(Math.Abs(angle)))
			//	throw new Exception("Angles are not equal " + Math.Abs((this.GetAngle(result, v))) + " " + Math.Abs(angle));
		    return result;
	    }
    }
}
