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
		public static void DrawPolygon(Geometry.Polygon polygon)
		{
			var image = new Bitmap(sizeX, sizeY);
			var graphics = Graphics.FromImage(image);
			Geometry.Point2D[] vertices = polygon.ConvertTo2D(polygon.GetBasis()[0], polygon.GetBasis()[1]);
			for (int i = 0; i < vertices.Length; i++)
			{
				Geometry.Point2D a = vertices[i];
				Geometry.Point2D b = vertices[(i + 1) % vertices.Length];

				graphics.DrawLine(new Pen(Brushes.Blue), new PointF((float)a.x + sizeX / 2f, -(float)a.y + sizeY / 2f), new PointF((float)b.x + sizeX / 2f, -(float)b.y + sizeY / 2f));
			}
			var form = new Form
			{
				Text = "Harter–Heighway dragon",
				ClientSize = new Size(sizeX, sizeY)
			};

			//Добавляем специальный элемент управления PictureBox, который умеет отображать созданное нами изображение.
			form.Controls.Add(new PictureBox { Image = image, Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.CenterImage });
			form.ShowDialog();
		}
		static void Main(string[] args)
		{
			DrawPolygon(new Polygon(3, new []{new Point(0, 0, 0), new Point(100, 0, 0), new Point(100, 200, 0)}));
			Console.WriteLine("Hello, git!");//
		}
	}
}
