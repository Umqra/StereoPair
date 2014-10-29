using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geometry;
using Point = Geometry.Point;

namespace StereoPair
{
	class Drawer
	{
		Thread thread;
		public Form form;
		static private int sizeX = 500, sizeY = 500;
		static Camera camera;

		public static Color[] colors = new Color[] { Color.Red, Color.Blue, Color.Green, Color.Gold, Color.Fuchsia, Color.Chartreuse, Color.DodgerBlue, Color.Teal };

		public static void DrawSetOfPolygonsToBitmap(Bitmap image, Geometry.Point2D[][] polygons)
		{
			var graphics = Graphics.FromImage(image);
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			for (int i = 0; i < polygons.Length; i++)
			{
				PointF[] pointsF = new PointF[polygons[i].Length];
				for (int j = 0; j < polygons[i].Length; j++)
				{
					Geometry.Point2D a = polygons[i][j];
					Geometry.Point2D b = polygons[i][(j + 1) % polygons[i].Length];
					PointF af = new PointF((float)a.x + sizeX / 2f, -(float)a.y + sizeY / 2f);
					PointF bf = new PointF((float)b.x + sizeX / 2f, -(float)b.y + sizeY / 2f);
					pointsF[j] = af;
					//graphics.DrawLine(new Pen(Brushes.Blue), new PointF((float)a.x + sizeX / 2f, -(float)a.y + sizeY / 2f), new PointF((float)b.x + sizeX / 2f, -(float)b.y + sizeY / 2f));
				}
				graphics.FillPolygon(new SolidBrush(colors[i % colors.Length]), pointsF);
			}
		}

		public static void DrawPolyhedronToBitmaps(Bitmap[] image, Geometry.Polyhedron polyhedron)
		{
			Point2D[][][] frames = camera.GetFrames(polyhedron);
			DrawSetOfPolygonsToBitmap(image[0], frames[0]);
			DrawSetOfPolygonsToBitmap(image[1], frames[1]);
		}
		
		public Drawer(Form form_)
		{
			thread = new Thread(this.Draw);
			form = form_;
			thread.Start();
		}
		/// <summary>
		/// Strange function, which was here http://www.csharp-examples.net/set-doublebuffered/
		/// And it works
		/// </summary>
		/// <param name="control"></param>
		public static void SetDoubleBuffered(Control control)
		{
			// set instance non-public property with name "DoubleBuffered" to true
			typeof(Control).InvokeMember("DoubleBuffered",
				BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
				null, control, new object[] { true });
		}

		void Draw()
		{
			SetDoubleBuffered(form);
			form.Show();
			while (true)
			{
				camera = new Camera(new Point(0, 0, 300));
				Bitmap[] images = new[] { new Bitmap(sizeX, sizeY), new Bitmap(sizeX, sizeY) };
				DrawPolyhedronToBitmaps(images, Reader.ReadData("../../data.txt"));
				form.Controls.Clear();
				form.Controls.Add(new PictureBox { Image = images[0], Dock = DockStyle.Left, ClientSize = new Size(sizeX, sizeY), SizeMode = PictureBoxSizeMode.StretchImage });
				form.Controls.Add(new PictureBox { Image = images[1], Dock = DockStyle.Right, ClientSize = new Size(sizeX, sizeY), SizeMode = PictureBoxSizeMode.StretchImage });
				//form.Controls.Add(new PictureBox { Image = image, Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.CenterImage});
				//			form.Controls.Add(new PictureBox { Image = image, Dock = DockStyle.Right, SizeMode = PictureBoxSizeMode.CenterImage});
				form.Refresh();
				Thread.Sleep(50);
				break;
			}
		}
	}
}
