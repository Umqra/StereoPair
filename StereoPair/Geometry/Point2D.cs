using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
	public class Point2D
	{
		public readonly double x, y;

		public Point2D()
		{
			x = y = 0;
		}
		public Point2D(double x_, double y_)
		{
			x = x_;
			y = y_;
		}

		public static Point2D operator +(Point2D a, Point2D b)
		{
			return new Point2D(a.x + b.x, a.y + b.y);
		}

		public static Point2D operator -(Point2D a, Point2D b)
		{
			return new Point2D(a.x - b.x, a.y - b.y);
		}

		public static Point2D operator * (Point2D a, double k)
		{
			return new Point2D(a.x * k, a.y * k);
		}

		public static Point2D operator *(double k, Point2D a)
		{
			return new Point2D(a.x * k, a.y * k);
		}

	}
}
