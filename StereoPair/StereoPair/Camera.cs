using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometry;

namespace StereoPair
{
	class Camera
	{
		public readonly Point position;
		public readonly Plane plane;

		public Camera(Point _position, Plane _plane)
		{
			position = _position;
			plane = _plane;
		}

		public Polygon[] GetFrame(Polyhedron polyhedron)
		{
			List <Polygon> frames = new List<Polygon>();
			Point xBasis, yBasis;
			foreach (var polygon in polyhedron.faces)
			{
				//if (this.IsVisible(polyhedron, polygon))
					//frames.Add(polygon.CentralProjectionToPlane(plane, position).ConvertTo2D());
			}
			throw new NotImplementedException();
		}

		private bool IsVisible(Polyhedron polyhedron, Polygon polygon)
		{
			throw new NotImplementedException();
		}
	}
}
