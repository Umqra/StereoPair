using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Configuration;
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

		public static bool IsLess(double a, double b, double eps)
		{
			return a < b && !AreEqual(a, b);
		}

		public static bool IsLess(double a, double b)
		{
			return IsLess(a, b, Epsilon);
		}

		public static bool IsGreater(double a, double b, double eps)
		{
			return a > b && !AreEqual(a, b);
		}

		public static bool IsGreater(double a, double b)
		{
			return IsGreater(a, b, Epsilon);
		}

		public static bool AreNotEqual(double a, double b, double eps)
		{
			return !AreEqual(a, b, eps);
		}

		public static bool AreNotEqual(double a, double b)
		{
			return AreNotEqual(a, b, Epsilon);
		}
	}
}
