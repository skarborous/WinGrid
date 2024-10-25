using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System;

namespace GridOverlay
{
    public partial class MainWindow : Window
    {
        private double gridSpacing = 50;
        private double gridOpacity = 0.5;
        private bool isExactFit = true;  // Default mode is Exact Fit

        public MainWindow()
        {
            InitializeComponent();

            // Set window to fullscreen and fix to top-left corner
            this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;
            this.Topmost = true;
            this.AllowsTransparency = true;

            // Use primary screen dimensions
            this.Left = 0;
            this.Top = 0;
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;

            // Set the default values for the text blocks
            SpacingValueText.Text = gridSpacing.ToString("F0");
            TransparencyValueText.Text = gridOpacity.ToString("F1");

            DrawGridLines(gridSpacing);
        }

        private void DrawGridLines(double spacing)
        {
            // Clear existing grid lines
            OverlayCanvas.Children.Clear();

            // Get window dimensions
            double width = this.Width;
            double height = this.Height;

            // Determine if Exact Fit mode is enabled
            double adjustedSpacingX = spacing;
            double adjustedSpacingY = spacing;

            if (isExactFit)
            {
                // Calculate adjusted spacing to fit the screen exactly
                double columns = Math.Floor(width / spacing);
                double rows = Math.Floor(height / spacing);

                adjustedSpacingX = width / columns;
                adjustedSpacingY = height / rows;
            }

            // Draw the grid lines based on adjusted spacing
            for (double x = 0; x <= width; x += adjustedSpacingX)
            {
                Line verticalLine = new Line
                {
                    X1 = x,
                    Y1 = 0,
                    X2 = x,
                    Y2 = height,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 1,
                    Opacity = gridOpacity
                };
                OverlayCanvas.Children.Add(verticalLine);
            }

            for (double y = 0; y <= height; y += adjustedSpacingY)
            {
                Line horizontalLine = new Line
                {
                    X1 = 0,
                    Y1 = y,
                    X2 = width,
                    Y2 = y,
                    Stroke = Brushes.Gray,
                    StrokeThickness = 1,
                    Opacity = gridOpacity
                };
                OverlayCanvas.Children.Add(horizontalLine);
            }

            // Draw the center crosshair lines at the screen center
            double centerX = width / 2;
            double centerY = height / 2;

            Line verticalCrosshair = new Line
            {
                X1 = centerX,
                Y1 = 0,
                X2 = centerX,
                Y2 = height,
                Stroke = Brushes.Yellow,
                StrokeThickness = 2,
                Opacity = gridOpacity
            };
            OverlayCanvas.Children.Add(verticalCrosshair);

            Line horizontalCrosshair = new Line
            {
                X1 = 0,
                Y1 = centerY,
                X2 = width,
                Y2 = centerY,
                Stroke = Brushes.Yellow,
                StrokeThickness = 2,
                Opacity = gridOpacity
            };
            OverlayCanvas.Children.Add(horizontalCrosshair);
        }

        private void SpacingSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Update grid spacing and redraw grid
            gridSpacing = e.NewValue;
            DrawGridLines(gridSpacing);

            // Update the spacing value display safely
            if (SpacingValueText != null)
            {
                SpacingValueText.Text = gridSpacing.ToString("F0"); // Display as an integer
            }
        }

        private void TransparencySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Update grid opacity and redraw grid
            gridOpacity = e.NewValue;
            DrawGridLines(gridSpacing);

            // Update the transparency value display safely
            if (TransparencyValueText != null)
            {
                TransparencyValueText.Text = gridOpacity.ToString("F1"); // Display with one decimal
            }
        }

        private void ExactFitCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            // Enable Exact Fit mode and redraw grid
            isExactFit = true;
            DrawGridLines(gridSpacing);
        }

        private void ExactFitCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Disable Exact Fit mode and redraw grid
            isExactFit = false;
            DrawGridLines(gridSpacing);
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Close the application
        }
    }
}
