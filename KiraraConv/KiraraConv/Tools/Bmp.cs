using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Charlotte.Tools
{
	public class Bmp
	{
		public RectTable<Dot> table;

		public Bmp(int w, int h, Dot dot)
		{
			table = new RectTable<Dot>(w, h, dot);
		}

		public class Dot
		{
			public int a;
			public int r;
			public int g;
			public int b;

			public Dot(int a, int r, int g, int b)
			{
				this.a = a;
				this.r = r;
				this.g = g;
				this.b = b;
			}

			public static Dot getDot(Color color)
			{
				return new Dot(color.A, color.R, color.G, color.B);
			}

			public Color getColor()
			{
				return Color.FromArgb(a, r, g, b);
			}
		}

		public static Bmp create(Bitmap src)
		{
			Bmp dest = new Bmp(src.Width, src.Height, Dot.getDot(Color.White));

			for (int x = 0; x < src.Width; x++)
			{
				for (int y = 0; y < src.Height; y++)
				{
					dest.table.set(x, y, Dot.getDot(src.GetPixel(x, y)));
				}
			}
			return dest;
		}

		public Bitmap getBitmap()
		{
			Bitmap dest = new Bitmap(table.w, table.h);

			for (int x = 0; x < table.w; x++)
			{
				for (int y = 0; y < table.h; y++)
				{
					dest.SetPixel(x, y, table.get(x, y).getColor());
				}
			}
			return dest;
		}

		public Bmp expand(int w, int h)
		{
			throw null; // unimpl
		}

		public void drawLine(int x1, int y1, int x2, int y2, Dot dot)
		{
			int denom = Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2));

			for (int numer = 0; numer <= denom; numer++)
			{
				double rate = (double)numer / denom;

				table.set(
					IntTools.toInt(x1 + rate * (x2 - x1)),
					IntTools.toInt(y1 + rate * (y2 - y1)),
					dot
					);
			}
		}

		public void drawRect(int x1, int y1, int x2, int y2, Dot dot)
		{
			drawLine(x1, y1, x1, y2, dot);
			drawLine(x1, y2, x2, y2, dot);
			drawLine(x2, y2, x2, y1, dot);
			drawLine(x2, y1, x1, y1, dot);
		}

		public void fillRect(int l, int t, int r, int b, Dot dot)
		{
			for (int x = l; x <= r; x++)
			{
				for (int y = t; y <= b; y++)
				{
					table.set(x, y, dot);
				}
			}
		}

		public Bmp getRect(int l, int t, int w, int h)
		{
			Bmp dest = new Bmp(w, h, Dot.getDot(Color.White));

			for (int x = 0; x < w; x++)
			{
				for (int y = 0; y < h; y++)
				{
					dest.table.set(x, y, table.get(l + x, t + y));
				}
			}
			return dest;
		}
	}
}
