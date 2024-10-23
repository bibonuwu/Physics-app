using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Windows;
using System.Windows.Media.Animation;

using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Physics
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializePlot(); // Графикті бастапқы орнату
        }

        // Графикті орнату
        // Графикті орнату
        // Графикті орнату
        private void InitializePlot()
        {
            // Кинетикалық энергия үшін графикті орнату
            var kineticPlotModel = new PlotModel { Title = "Кинетикалық энергия" };
            kineticPlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Жылдамдық (м/с)",
                Minimum = 0,
                Maximum = 100
            });
            kineticPlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Энергия (Дж)",
                Minimum = 0
            });
            kineticEnergyPlot.Model = kineticPlotModel;

            // Потенциалды энергия үшін графикті орнату
            var potentialPlotModel = new PlotModel { Title = "Потенциалды энергия" };
            potentialPlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Биіктік (м)",  // Ось X для высоты (метры)
                Minimum = 0,
                Maximum = 100 // Максимальная высота
            });
            potentialPlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Энергия (Дж)",
                Minimum = 0
            });
            potentialEnergyPlot.Model = potentialPlotModel;
        }


        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Қолданушы енгізген мәндерді алу
                double mass = double.Parse(massTextBox.Text);
                double height = double.Parse(heightTextBox.Text);
                double speed = double.Parse(speedTextBox.Text);

                // Еркін түсу үдеуі (g)
                double g = 9.81;

                // Потенциалды энергияны есептеу
                double potentialEnergy = mass * g * height;

                // Кинетикалық энергияны есептеу
                double kineticEnergy = 0.5 * mass * speed * speed;

                // Толық механикалық энергия
                double totalEnergy = potentialEnergy + kineticEnergy;

                // Нәтижені көрсету
                resultTextBlock.Text = $"Потенциалды энергия (Eₚ): {potentialEnergy:F2} Дж\n" +
                                       $"Кинетикалық энергия (Eₖ): {kineticEnergy:F2} Дж\n" +
                                       $"Толық энергия (E): {totalEnergy:F2} Дж";

                // ProgressBar-ларды анимация арқылы жаңарту
                AnimateProgressBar(potentialEnergyBar, potentialEnergy);
                AnimateProgressBar(kineticEnergyBar, kineticEnergy);
                AnimateProgressBar(totalEnergyBar, totalEnergy);

                // Графикті жаңарту
                UpdatePlot(mass, height);
            }
            catch (FormatException)
            {
                resultTextBlock.Text = "Сандық мәндерді дұрыс енгізіңіз.";
            }
        }

        private void AnimateProgressBar(System.Windows.Controls.ProgressBar progressBar, double value)
        {
            double maxValue = 1000; // Мысал үшін максимал мән
            DoubleAnimation animation = new DoubleAnimation
            {
                To = Math.Min(value, maxValue),
                Duration = TimeSpan.FromSeconds(1)
            };
            progressBar.BeginAnimation(System.Windows.Controls.ProgressBar.ValueProperty, animation);
        }

        // Графикті жаңарту функциясы
        private void UpdatePlot(double mass, double height)
        {
            // Кинетикалық энергия үшін серия жасау
            var kineticSeries = new LineSeries
            {
                Title = "Кинетикалық энергия",
                Color = OxyColors.Red,
                StrokeThickness = 2,
                MarkerType = MarkerType.Circle
            };

            // Потенциалды энергия үшін серия жасау
            var potentialSeries = new LineSeries
            {
                Title = "Потенциалды энергия",
                Color = OxyColors.Blue,
                StrokeThickness = 2,
                MarkerType = MarkerType.Square
            };

            // Әртүрлі жылдамдық мәндері үшін есептеу (кинетикалық энергия)
            for (double v = 0; v <= 100; v += 5)
            {
                double g = 9.81;
                double kineticEnergy = 0.5 * mass * v * v;
                kineticSeries.Points.Add(new DataPoint(v, kineticEnergy));
            }

            // Әртүрлі биіктік мәндері үшін есептеу (потенциалды энергия)
            for (double h = 0; h <= 100; h += 5)
            {
                double g = 9.81;
                double potentialEnergy = mass * g * h;
                potentialSeries.Points.Add(new DataPoint(h, potentialEnergy));
            }

            // Кинетикалық энергия графигін жаңарту
            kineticEnergyPlot.Model.Series.Clear();
            kineticEnergyPlot.Model.Series.Add(kineticSeries);
            kineticEnergyPlot.Model.InvalidatePlot(true);

            // Потенциалды энергия графигін жаңарту
            potentialEnergyPlot.Model.Series.Clear();
            potentialEnergyPlot.Model.Series.Add(potentialSeries);
            potentialEnergyPlot.Model.InvalidatePlot(true);
        }



        // Графикті PNG ретінде сақтау
        private void SaveGraph_Click(object sender, RoutedEventArgs e)
        {
            var pngExporter = new OxyPlot.Wpf.PngExporter { Width = 600, Height = 400 };

            // Сохранение кинетического графика
            var kineticDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "PNG Сурет|*.png",
                Title = "Кинетикалық графикті PNG ретінде сақтау",
                FileName = "kinetic_graph.png"
            };
            bool? kineticResult = kineticDialog.ShowDialog();
            if (kineticResult == true)
            {
                using (var stream = System.IO.File.Create(kineticDialog.FileName))
                {
                    pngExporter.Export(kineticEnergyPlot.Model, stream);
                }
            }

            // Сохранение потенциального графика
            var potentialDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "PNG Сурет|*.png",
                Title = "Потенциалды графикті PNG ретінде сақтау",
                FileName = "potential_graph.png"
            };
            bool? potentialResult = potentialDialog.ShowDialog();
            if (potentialResult == true)
            {
                using (var stream = System.IO.File.Create(potentialDialog.FileName))
                {
                    pngExporter.Export(potentialEnergyPlot.Model, stream);
                }
            }
        }


    }
}
