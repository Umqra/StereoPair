using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;
using Point = Geometry.Point;

namespace StereoPair
{
	class AppPolygon2D
	{
		private int n;
		public Color PolygonColor;
		public Point2D[] vertices;
		public AppPolygon2D(int n_, Point2D[] vertices_, Color color)
		{
			vertices = vertices_;
			n = n_;
			PolygonColor = color;
		}

	}
}
