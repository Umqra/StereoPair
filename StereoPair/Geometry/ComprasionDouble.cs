using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
	public static class ComprasionDouble
	{
		public const double Epsilon = 1e-8;

		public static bool AreEqual(double a, double b, double eps)
		{
			return Math.Abs(a - b) < eps;
		}
		public static bool AreEqual(double a, double b)
		{
			return AreEqual(a, b, Epsilon);
		}

		public static bool AreLess(double a, double b, double eps)
		{
			return a < b && !AreEqual(a, b);
		}

		public static bool AreLess(double a, double b)
		{
			return AreLess(a, b, Epsilon);
		}

		public static bool AreGreater(double a, double b, double eps)
		{
			return a > b && !AreEqual(a, b);
		}

		public static bool AreGreater(double a, double b)
		{
			return AreGreater(a, b, Epsilon);
		}
	}
}
