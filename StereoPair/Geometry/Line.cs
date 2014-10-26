using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
	class Line
	{
		private Point A, v;
		public Line(Point A_, Point v_)
		{
			A = A_;
			v = v_;
		}
		
		public bool Parallel(Line a)
		{
			return (v.CrossProduct(a.v).GetLength() == 0); //TODO:Equals
		}
		
		public bool Equals(Line a)
		{
			return (this.Parallel(a) && this.Parallel(new Line(A, a.A - A)));
		}

		/// <summary>
		/// Checks whether lines are skew or not.
		/// </summary>
		/// <param name="a"></param>
		/// <returns>True if lines are skew and false otherwise</returns>
		public bool Skew(Line a)
		{
			if (a.v.CrossProduct(v).GetLength() == 0)
				return false;
			Point normal1 = a.v.CrossProduct(v);
			Point normal2 = a.v.CrossProduct(A - a.A);
			return (normal1.CrossProduct(normal2).GetLength() != 0);
		}

		/// <summary>
		/// Calculates distance between two skew lines. Throws exceptions is lines aren't skew.
		/// </summary>
		/// <param name="a"></param>
		/// <returns>Distance</returns>
		public double GetDistToLine(Line a)
		{
			if (this.Skew(a))
				throw new Exception("Lines aren't skew");
			return (a.A - A).DotProduct(a.v.CrossProduct(v)) / a.v.CrossProduct(v).GetLength();
		}

		/// <summary>
		/// Finds the intersection of two lines. If Lines are equal, parallel or skew throws exception.
		/// </summary>
		/// <param name="a">Line</param>
		/// <returns>Returns a Point which is intersect of two given Lines</returns>
		public Point Intersect(Line a)
		{
			if (this.Parallel(a) || this.Equals(a) || this.Skew(a))
				throw new Exception("Invalid lines");
			double k = (a.A.CrossProduct(a.v) - A.CrossProduct(a.v)).GetLength() / v.CrossProduct(a.v).GetLength();
			return A + v * k;
		}
	}
}
