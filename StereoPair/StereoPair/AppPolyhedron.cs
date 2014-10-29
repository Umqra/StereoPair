using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace StereoPair
{
	public class AppPolyhedron : Polyhedron
	{
		public Color[] ListColors;
		public AppPolyhedron(int n_, Polygon[] faces_) : base(n_, faces_)
		{
			Random random = new Random();
			ListColors = new Color[faces_.Length];
			for (var i = 0; i < faces_.Length; i++)
				ListColors[i] = Color.FromArgb(150, 150, 150);
		}
	}
}
