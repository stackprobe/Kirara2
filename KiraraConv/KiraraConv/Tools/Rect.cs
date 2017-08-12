using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Charlotte.Tools
{
	public class Rect
	{
		public double l;
		public double t;
		public double w; // 0.0 <=
		public double h; // 0.0 <=

		public Rect()
			: this(0.0, 0.0, 0.0, 0.0)
		{ }

		public Rect(double w, double h)
			: this(0.0, 0.0, w, h)
		{ }

		public Rect(Rect rect)
			: this(rect.l, rect.t, rect.w, rect.h)
		{ }

		public Rect(double l, double t, double w, double h)
		{
			this.l = l;
			this.t = t;
			this.w = w;
			this.h = h;
		}

		public Rect getClone()
		{
			return new Rect(this);
		}

		public static Rect ltrb(double l, double t, double r, double b)
		{
			return new Rect(l, t, r - l, b - t);
		}

		public double getR()
		{
			return l + w;
		}

		public double getB()
		{
			return t + h;
		}

		public void setR(double r)
		{
			w = r - l;
		}

		public void setB(double b)
		{
			h = b - t;
		}

		public void adjustInside(Rect screen)
		{
			double new_w = screen.w;
			double new_h = (h * screen.w) / w;

			if (screen.h < new_h)
			{
				new_w = (w * screen.h) / h;
				new_h = screen.h;

				if (screen.w < new_w)
					throw null;
			}
			l = screen.l + (screen.w - new_w) / 2.0;
			t = screen.t + (screen.h - new_h) / 2.0;
			w = new_w;
			h = new_h;
		}

		public void adjustOutside(Rect screen)
		{
			double new_w = screen.w;
			double new_h = (h * screen.w) / w;

			if (new_h < screen.h)
			{
				new_w = (w * screen.h) / h;
				new_h = screen.h;

				if (new_w < screen.w)
					throw null;
			}
			l = screen.l + (screen.w - new_w) / 2.0;
			t = screen.t + (screen.h - new_h) / 2.0;
			w = new_w;
			h = new_h;
		}
	}
}
