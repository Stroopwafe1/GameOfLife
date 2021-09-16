using Controller;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameOfLifeGUI {

	public static class Visualiser {
		private static Dictionary<string, Bitmap> _imageCache = new Dictionary<string, Bitmap>();
		private static int sizeMultiplier = 16;

		public static BitmapSource DrawNext() {
			int size = Data.Board.GetSize();
			Bitmap bg = CreateEmptyBitmap(size * sizeMultiplier, size * sizeMultiplier);
			Bitmap golBitmap = DrawAliveCells(bg);

			return CreateBitmapSourceFromGdiBitmap(golBitmap);
		}

		public static Bitmap DrawAliveCells(Bitmap bg) {
			Bitmap golBitmap = new Bitmap(bg);
			Graphics graphics = Graphics.FromImage(golBitmap);
			SolidBrush solidBrush = new SolidBrush(System.Drawing.Color.White);

			var aliveCells = Data.Board.GetAliveCells();
			for (int i = 0; i < aliveCells.Length; i++) {
				var cell = aliveCells[i];
				graphics.FillRectangle(solidBrush, cell.X * sizeMultiplier, cell.Y * sizeMultiplier, 1 * sizeMultiplier, 1 * sizeMultiplier);
			}
			return (Bitmap)golBitmap.Clone();
		}

		public static Bitmap CreateEmptyBitmap(int width, int height) {
			Bitmap returnValue;
			if (_imageCache.ContainsKey("empty")) return (Bitmap)GetImageFromCache("empty").Clone();
			returnValue = new Bitmap(width, height);
			Graphics graphics = Graphics.FromImage(returnValue);
			SolidBrush solidBrush = new SolidBrush(System.Drawing.Color.Black);
			graphics.FillRectangle(solidBrush, 0, 0, width, height);
			_imageCache.Add("empty", returnValue);
			return (Bitmap)returnValue.Clone();
		}

		public static Bitmap GetImageFromCache(string imageURL) {
			if (_imageCache.ContainsKey(imageURL)) {
				return _imageCache[imageURL];
			}
			Bitmap image = new Bitmap(imageURL);
			_imageCache.Add(imageURL, image);
			return image;
		}

		public static void ClearCache() {
			_imageCache?.Clear();
		}

		public static BitmapSource CreateBitmapSourceFromGdiBitmap(Bitmap bitmap) {
			if (bitmap == null)
				throw new ArgumentNullException("bitmap");

			var rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

			var bitmapData = bitmap.LockBits(
				rect,
				ImageLockMode.ReadWrite,
				System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			try {
				var size = (rect.Width * rect.Height) * 4;

				return BitmapSource.Create(
					bitmap.Width,
					bitmap.Height,
					bitmap.HorizontalResolution,
					bitmap.VerticalResolution,
					PixelFormats.Bgra32,
					null,
					bitmapData.Scan0,
					size,
					bitmapData.Stride);
			} finally {
				bitmap.UnlockBits(bitmapData);
			}
		}
	}
}