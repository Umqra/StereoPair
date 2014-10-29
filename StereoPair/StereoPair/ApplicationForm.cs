﻿using System;
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
using RandomGenerator;
using Point = Geometry.Point;
using Timer = System.Windows.Forms.Timer;

namespace StereoPair
{
	public sealed class ApplicationForm : Form
	{
		static private int sizeX = 1200, sizeY = 600;
		static Camera camera;
		private double distFromCenter = 300;
		private Point2D DistBetweenPictures = new Point2D(sizeX / 4, 0);
		private double autoRotatingAngle = 0;

		private static Color[] colors = new Color[] { Color.Red, Color.Blue, Color.Green, Color.Gold, Color.Fuchsia, Color.Chartreuse, Color.DodgerBlue, Color.Teal };

		public ApplicationForm()
		{
			camera = new Camera(new Point(0, 0, distFromCenter));
			Text = "Stereo Pair";
			ClientSize = new Size(sizeX, sizeY);
			var timer = new Timer {Interval = 30, Enabled = true};
			timer.Tick += timer_Tick;
			DoubleBuffered = true;
		}

		void timer_Tick(object sender, EventArgs e)
		{
			camera.Rotate(new Point(0, 1, 0), autoRotatingAngle);
			Invalidate();
		}

		private static void DrawSetOfPolygons(Geometry.Point2D[][] polygons, PaintEventArgs e, Point2D shift)
		{
			//var graphics = Graphics.FromImage(image);
			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			for (int i = 0; i < polygons.Length; i++)
			{
				PointF[] pointsF = new PointF[polygons[i].Length];
				for (int j = 0; j < polygons[i].Length; j++)
				{
					Geometry.Point2D a = polygons[i][j];
					pointsF[j] = new PointF((float)a.x + (float)shift.x, -(float)a.y + (float)shift.y);
					//graphics.DrawLine(new Pen(Brushes.Blue), new PointF((float)a.x + sizeX / 2f, -(float)a.y + sizeY / 2f), new PointF((float)b.x + sizeX / 2f, -(float)b.y + sizeY / 2f));
				}
				e.Graphics.FillPolygon(new SolidBrush(colors[i % colors.Length]), pointsF);
			}
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			double angle = Math.PI / 30;
			double holeSize = 2 * angle;
			double dLen = 20;
			if (e.KeyChar == 'a')
				camera.Rotate(new Point(0, 1, 0), -angle);
			else if (e.KeyChar == 'd')
				camera.Rotate(new Point(0, 1, 0), angle);
			else if (e.KeyChar == 'w')
			{
				if (camera.position.GetUnsignedAngle(new Point(0, 1, 0)).IsGreater(holeSize))
					camera.Rotate(new Point(0, 1, 0).CrossProduct(camera.position), -angle);
			}
			else if (e.KeyChar == 's')
			{
				if ((Math.PI - camera.position.GetUnsignedAngle(new Point(0, 1, 0))).IsGreater(holeSize))
					camera.Rotate(new Point(0, 1, 0).CrossProduct(camera.position), angle);
			}
			else if (e.KeyChar == 'e')
			{
				if (camera.position.Length().IsGreater(Camera.DistToPlane))
					camera.SetLength(camera.position.Length() - dLen);
			}
			else if (e.KeyChar == 'q')
			{
				camera.SetLength(camera.position.Length() + dLen);
			}
			else if (e.KeyChar == '1')
			{
				DistBetweenPictures -= new Point2D(sizeX / 300, 0);
			}
			else if (e.KeyChar == '2')
			{
				DistBetweenPictures += new Point2D(sizeX / 300, 0);
			}
			else if (e.KeyChar == 'r')
			{
				if (autoRotatingAngle.IsEqual(0))
					autoRotatingAngle = angle;
				else
					autoRotatingAngle = 0;
			}
			else if (e.KeyChar == 'n')
			{
				Random newRandom = new Random();
				Generate.WritePolyhedronToData(newRandom.Next(20) + 10);
			}
			Invalidate();
		}

		private static void DrawPolyhedron(Geometry.Polyhedron polyhedron, PaintEventArgs e, Point2D shift1, Point2D shift2)
		{
			Point2D[][][] frames = camera.GetFrames(polyhedron);
			DrawSetOfPolygons(frames[0], e, shift1);
			DrawSetOfPolygons(frames[1], e, shift2);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Bitmap[] images = new[] { new Bitmap(sizeX, sizeY), new Bitmap(sizeX, sizeY) };
			Point2D shift = new Point2D(sizeX / 2, sizeY / 2);
			Point2D shift1 = shift - DistBetweenPictures;
			Point2D shift2 = shift + DistBetweenPictures;
			DrawPolyhedron(Reader.ReadData("../../data.txt"), e, shift1, shift2);
		}
	}
}
