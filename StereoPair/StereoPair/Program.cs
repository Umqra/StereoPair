using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geometry;
using Point = Geometry.Point;

namespace StereoPair
{
	class Program
	{
		static private int sizeX = 900, sizeY = 500;
		static Camera camera;

		public static void DrawPolyhedronToBitmap(Bitmap image, Geometry.Polyhedron polyhedron)
		{
			var graphics = Graphics.FromImage(image);
			Point2D[][] frames = camera.GetFrames(polyhedron);
			for (int i = 0; i < frames.Length; i++)
			{	
				for (int j = 0; j < frames[i].Length; j++)
				{
					Geometry.Point2D a = frames[i][j];
					Geometry.Point2D b = frames[i][(j + 1) % frames[i].Length];

					graphics.DrawLine(new Pen(Brushes.Blue), new PointF((float)a.x + sizeX / 2f, -(float)a.y + sizeY / 2f), new PointF((float)b.x + sizeX / 2f, -(float)b.y + sizeY / 2f));
				}
			}
		}
		static void Main(string[] args)
		{
			camera = new Camera(new Point(0, 0, 300));
			var image = new Bitmap(sizeX, sizeY);
			//image.
			DrawPolyhedronToBitmap(image, Reader.ReadData("../../data.txt"));
			var form = new Form
			{
				Text = "Stereo Pair",
				ClientSize = new Size(sizeX, sizeY)
			};
			form.Controls.Add(new PictureBox { Image = image, Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.CenterImage});
//			form.Controls.Add(new PictureBox { Image = image, Dock = DockStyle.Right, SizeMode = PictureBoxSizeMode.CenterImage});
			form.ShowDialog();
		}
	}
}
