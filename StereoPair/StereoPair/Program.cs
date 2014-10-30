using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geometry;
using RandomGenerator;
using Point = Geometry.Point;

namespace StereoPair
{
	class Program
	{
		static void Main(string[] args)
		{
//			Generate.WritePolyhedronToData(10);
			Reader.ReadData("../../data.txt");
			int sizeX = 500, sizeY = 500;
			var form = new ApplicationForm();
			form.ShowDialog();
		}
	}
}
