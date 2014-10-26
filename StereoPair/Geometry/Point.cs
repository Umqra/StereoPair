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

	    public Point Normalize(int len)
	    {
		    if (ComprasionDouble.AreEqual(len, 0))
		    {
			    if (IsNullVector())
				    return this;
				throw new ArgumentException("Can't set non-zero length to null vector.");
		    }
		    return this / Length();
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
		    return String.Format("x = {0}, y = {1}, z = {2}\n", x, y, z);
	    }
    }
}
