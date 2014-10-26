using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geometry
{
	class Polyhedron
	{
		public int n;
		public Polygon[] faces;

		public Polyhedron(int n_, Polygon[] faces_)
		{
			n = n_;
			faces = faces_;
		}
	}
}
