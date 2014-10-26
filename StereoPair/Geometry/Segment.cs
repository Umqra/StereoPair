using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
	class Segment
	{
		private Point A, B;
		public Segment(Point A_, Point B_)
		{
			A = A_;
			B = B_;
		}

		private Line getLine()
		{
			return new Line(A, B - A);		
		}

		public bool OnSameLine(Segment a)
		{
			return getLine().Equals(a.getLine());
		}

		public bool Collinear(Segment a)
		{
			return getLine().Parallel(a.getLine());
		}

		public bool CheckBelongingOfPoint(Point O)
		{
			return ((A - O).DotProduct(B - O) < 0);
		}

		/// <summary>
		/// Finds intersection of two segments.
		/// </summary>
		/// <param name="a"></param>
		/// <returns>A segment which is intersection of two given segments. null if intersection is empty.</returns>
		public Segment FindIntersection(Segment a)
		{
			if (this.OnSameLine(a))
			{
				Point L = null, R = null;
				if (this.CheckBelongingOfPoint(a.A))
					L = a.A;
				else if (a.CheckBelongingOfPoint(A))
					L = A;
				if (this.CheckBelongingOfPoint(a.B))
					R= a.B;
				else if (a.CheckBelongingOfPoint(B))
					R = B;
				if (L == null || R == null)
					return null;
				return new Segment(L, R);
			}
			if (this.Collinear(a))
				return null;
			if (getLine().Skew(a.getLine()))
				return null;
			Point O = getLine().Intersect(a.getLine());
			if (this.CheckBelongingOfPoint(O) && a.CheckBelongingOfPoint(O))
			{
				return new Segment(O, O);
			}
			return null;
		}
	}
}