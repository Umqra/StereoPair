using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
	public class Polyhedron
	{
		public readonly int n;
		public readonly Polygon[] faces;

		public Polyhedron(int n_, Polygon[] faces_)
		{
			n = n_;
			faces = faces_;
		}
	}
}
