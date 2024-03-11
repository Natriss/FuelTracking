using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml;
using System.Collections.Generic;

namespace FuelTracking.Core.Models
{
	public class Chart
	{
		private readonly Canvas _canvas;
		private readonly int _canvasHeight;
		private int _canvasContentWidth;
		private readonly int _barWidth;
		private readonly int _barSpacing;
		private const int _labelSpacing = 40;

		public Chart(Canvas canvas, int canvasHeight = 300, int barWidth = 30, int barSpacing = 20)
		{
			_canvas = canvas;
			_canvasHeight = canvasHeight;
			_barWidth = barWidth;
			_barSpacing = barSpacing;
		}

		/// <summary>
		/// Draws the chart
		/// </summary>
		/// <param name="data"></param>
		public void DrawChart(List<FuelTrack> data)
		{
			if (_canvas != null)
			{
				_canvas.Children.Clear();
			}

			//Checks if it cqn render a chart without issues.
			if (data == null || data.Count == 0)
			{
				return;
			}
						
			_canvasContentWidth = _labelSpacing + ((_barWidth + _barSpacing) * data.Count);
			_canvas.Height = _canvasHeight;
			_canvas.Width = _canvasContentWidth;
			double maxValue = 0;

			foreach (FuelTrack dataItem in data)
			{
				if (dataItem.Price > maxValue)
				{
					maxValue = dataItem.Price;
				}
			}

			double maxLabelValue = (((maxValue / 25) + 1) * 25);
			//Unit height is the height created for all UiElements based on the height of the canvas devided by the the maximum possible value.
			//This changes depending the values of the canvas and the data.
			double unitHeight = _canvasHeight / maxLabelValue;

			for (int i = 0; i < maxLabelValue; i += 25)
			{
				DrawLineAndLabel(i, unitHeight);
			}

			for (int i = 0; i < data.Count; i++)
			{
				DrawBar(data[i], i, unitHeight);
			}
		}

		/// <summary>
		/// Drawing all the bars for the chart
		/// </summary>
		/// <param name="data"></param>
		/// <param name="position"></param>
		/// <param name="unitHeight"></param>
		private void DrawBar(FuelTrack data, int position, double unitHeight)
		{
			Border border = new()
			{
				Width = _barWidth,
				Height = data.Price * unitHeight,
				Background = Application.Current.Resources["AccentFillColorSelectedTextBackgroundBrush"] as Brush,
				BorderBrush = Application.Current.Resources["ControlStrokeColorDefaultBrush"] as Brush,
				BorderThickness = new Thickness(1),
				CornerRadius = new(4, 4, 0, 0),
				DataContext = data,
			};

			Canvas.SetTop(border, _canvasHeight - border.Height);
			Canvas.SetLeft(border, position * (_barWidth + _barSpacing) + _labelSpacing);
			Canvas.SetZIndex(border, 1);

			_canvas.Children.Add(border);
		}

		/// <summary>
		/// Drawing the labels and the lines for the chart
		/// </summary>
		/// <param name="value"></param>
		/// <param name="unitHeight"></param>
		private void DrawLineAndLabel(int value, double unitHeight)
		{
			TextBlock textBlock = new()
			{
				Text = string.Format("€{0}", value.ToString()),
				Style = Application.Current.Resources["BaseTextBlockStyle"] as Style,
				Width = _labelSpacing,
				Height = 18
			};

			Canvas.SetTop(textBlock, _canvasHeight - ((value * unitHeight) + textBlock.Height));
			Canvas.SetLeft(textBlock, 0);
			Canvas.SetZIndex(textBlock, 0);

			_canvas.Children.Add(textBlock);

			Line line = new()
			{
				X1 = 0,
				Y1 = _canvasHeight - value * unitHeight,
				X2 = _canvasContentWidth,
				Y2 = _canvasHeight - value * unitHeight,
				Stroke = Application.Current.Resources["ControlStrokeColorDefaultBrush"] as Brush,
				StrokeThickness = 1,
			};

			Canvas.SetZIndex(line, 0);

			_canvas.Children.Add(line);
		}
	}
}
