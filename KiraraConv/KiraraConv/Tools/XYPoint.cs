using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Charlotte.Tools
{
	public class XYPoint
	{
		public double x;
		public double y;

		public XYPoint()
			: this(0.0, 0.0)
		{ }

		public XYPoint(XYPoint p)
			: this(p.x, p.y)
		{ }

		public XYPoint(Point p)
			: this((double)p.X, (double)p.Y)
		{ }

		public XYPoint(double x, double y)
		{
			this.x = x;
			this.y = y;
		}

		public XYPoint getClone()
		{
			return new XYPoint(this);
		}

		public void add(XYPoint p)
		{
			x += p.x;
			y += p.y;
		}

		public void reduce(XYPoint p)
		{
			x -= p.x;
			y -= p.y;
		}

		public static XYPoint origin = new XYPoint();

		public void mul(double r, XYPoint origin)
		{
			reduce(origin);
			mul(r);
			add(origin);
		}

		public void mul(double r)
		{
			x *= r;
			y *= r;
		}

		public void div(double r, XYPoint origin)
		{
			reduce(origin);
			div(r);
			add(origin);
		}

		public void div(double r)
		{
			x /= r;
			y /= r;
		}

		public void opposite()
		{
			mul(-1.0);
		}

		public void reciprocal()
		{
			x = 1.0 / x;
			y = 1.0 / y;
		}

		public static XYPoint getPoint(double angle)
		{
			return new XYPoint(
					Math.Cos(angle),
					Math.Sin(angle)
					);
		}

		public static XYPoint getPoint(double angle, double r)
		{
			XYPoint ret = getPoint(angle);
			ret.mul(r);
			return ret;
		}

		public static XYPoint getPoint(double angle, double r, XYPoint origin)
		{
			XYPoint ret = getPoint(angle, r);
			ret.add(origin);
			return ret;
		}

		public double getAngle()
		{
			if (y < 0.0)
				return Math.PI * 2.0 - new XYPoint(x, -y).getAngle();

			if (x < 0.0)
				return Math.PI - new XYPoint(-x, y).getAngle();

			if (y == 0.0)
				return 0.0;

			if (x == 0.0)
				return Math.PI / 2.0;

			double h = Math.PI / 2.0;
			double l = 0.0;
			double t = y / x;

			for (int c = 0; c < 50; c++)
			{
				double m = (h + l) / 2.0;
				double tt = Math.Tan(m);

				if (tt < t)
				{
					l = m;
				}
				else
				{
					h = m;
				}
			}
			return (h + l) / 2.0;
		}

		public double getAngle(XYPoint origin)
		{
			XYPoint tmp = getClone();
			tmp.reduce(origin);
			return tmp.getAngle();
		}

		public double getDistance()
		{
			return Math.Sqrt(x * x + y * y);
		}

		public double getDistance(XYPoint origin)
		{
			XYPoint tmp = getClone();
			tmp.reduce(origin);
			return tmp.getDistance();
		}

		public XYPoint rotate(double angle, double r)
		{
			return getPoint(getAngle() + angle, r);
		}

		public XYPoint rotate(double angle, double r, XYPoint origin)
		{
			XYPoint tmp = getClone();
			tmp.reduce(origin);
			tmp = tmp.rotate(angle, r);
			tmp.add(origin);
			return tmp;
		}

		public XYPoint rotate(double angle)
		{
			return rotate(angle, getDistance());
		}

		public XYPoint rotate(double angle, XYPoint origin)
		{
			XYPoint tmp = getClone();
			tmp.reduce(origin);
			tmp = tmp.rotate(angle);
			tmp.add(origin);
			return tmp;
		}
	}
}
