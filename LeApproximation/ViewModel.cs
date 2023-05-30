/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeApproximation
{
    internal class ViewModel
    {
    }
}*/

using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media.TextFormatting;

namespace LeApproximation       
{
    internal class ViewModel : INotifyPropertyChanged
    {
        private int integrationMethodIndex = 0;
        private int functionFormulaIndex = 0;
        private uint quadNodesNumber = 3;
        private double minAccuracy = 0.01;
        private double approximationLeftBound = -1;
        private double approximationRightBound = 1;
        private int approximationTerminationConditionIndex = 0;
        private double terminationConstant = 1;
        private string terminationLabel = "m=";

        private uint storedQuadNodesNumber = 3;

        int computationIndex = 0;

        private string terminalText = "";
        private ObservableCollection<string> terminalLines = new ObservableCollection<string>();

        Command integrationMethodIndexChangeCommand = new Command();
        Command functionFormulaIndexChangeCommand = new Command();
        Command replotCommand = new Command();
        Command approximateCommand = new Command();
        Command clearTerminalCommand = new Command();

        private readonly PlotModel plotModel;
        private readonly Model model;

        public ViewModel()
        {
            this.plotModel = new PlotModel();
            this.InitPlot();

            this.model = new Model();

            this.integrationMethodIndexChangeCommand.ExecuteReceived += IntegrationMethodIndexChangeCommand_ExecuteReceived;
            this.functionFormulaIndexChangeCommand.ExecuteReceived += FunctionFormulaIndexChangeCommand_ExecuteReceived;
            this.replotCommand.ExecuteReceived += ReplotCommand_ExecuteReceived;
            this.approximateCommand.ExecuteReceived += ApproximateCommand_ExecuteReceived;
            this.clearTerminalCommand.ExecuteReceived += ClearTerminalCommand_ExecuteReceived;

            this.terminalLines.CollectionChanged += TerminalLines_CollectionChanged;

            this.terminalLines.Add("Welcome to LeApproximation - Legendre polynomials function approximator");

            this.model.ComputationMonitorSignalReceived += Model_ComputationMonitorSignalReceived;

            this.Plot();
        }

        private void Model_ComputationMonitorSignalReceived(object? sender, LeApproximationData.ComputationDumpEventArgs e)
        {
            lock (this.terminalLines)
            {
                this.terminalLines.Add(e.Line);
            }

        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void TerminalLines_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            /*if (terminalLines.Count > 20)
            {
                terminalLines.RemoveAt(0);
            }*/
            this.terminalText = String.Join('\n', this.terminalLines);
            OnPropertyChanged(nameof(TerminalText));
        }

        private void ClearTerminalCommand_ExecuteReceived(object? sender, EventArgs e)
        {
            terminalLines.Clear();
        }

        private async void ApproximateCommand_ExecuteReceived(object? sender, EventArgs e)
        {
            //this.terminalLines.Add("Calculating integral...");
            int computationLocalIndex = computationIndex++;
            this.terminalLines.Add($"Starting to compute approximation [{computationLocalIndex}]...");
            if (this.integrationMethodIndex == 0)
            {
                //await Task.Run(() => this.model.CalculateIntegralSimpson());
                if(this.approximationTerminationConditionIndex == 0)
                {
                    await Task.Run(() => this.model.CalculateApproximation_Order_Simpson());
                }
                else
                {
                    await Task.Run(() => this.model.CalculateApproximation_Accuracy_Simpson());
                }
            }
            else
            {
                //await Task.Run(() => this.model.CalculateIntegralGaussLegendre());
                if (this.approximationTerminationConditionIndex == 0)
                {
                    await Task.Run(() => this.model.CalculateApproximation_Order_GaussLegendre());
                }
                else
                {
                    await Task.Run(() => this.model.CalculateApproximation_Accuracy_GaussLegendre());
                }
            }
            this.terminalLines.Add($"Approximation process [{computationLocalIndex}] completed.");
            this.Plot();
        }

        private void ReplotCommand_ExecuteReceived(object? sender, EventArgs e)
        {
            this.Plot();
        }

        private void FunctionFormulaIndexChangeCommand_ExecuteReceived(object? sender, EventArgs e)
        {
            this.Plot();
        }

        private void IntegrationMethodIndexChangeCommand_ExecuteReceived(object? sender, EventArgs e)
        {
            if (this.integrationMethodIndex == 0)
            {
                this.storedQuadNodesNumber = this.quadNodesNumber;
                this.quadNodesNumber = 3;
            }
            else
            {
                this.quadNodesNumber = this.storedQuadNodesNumber;
            }
            OnPropertyChanged(nameof(this.QuadNodesNumber));
            OnPropertyChanged(nameof(this.QuadNodesNumberEnabled));

            this.Plot();
        }

        private void InitPlot()
        {
            this.plotModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            });
            this.plotModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            });
        }

        private void Plot()
        {
            this.plotModel.Series.Clear();

            Func<double, double> inputFunction = this.model.FetchFunction();

            double left = this.approximationLeftBound - 0.25 * (this.approximationRightBound - this.approximationLeftBound);
            double right = this.approximationRightBound + 0.25 * (this.approximationRightBound - this.approximationLeftBound);

            FunctionSeries OX = new FunctionSeries((x) => 0, left, right, 0.01);
            OX.Color = OxyColors.Blue;
            this.plotModel.Series.Add(OX);
            FunctionSeries inputFunctionSeries = new FunctionSeries(inputFunction, left, right, 0.01);
            inputFunctionSeries.Color = OxyColors.Green;
            inputFunctionSeries.Title = "Original function";
            this.plotModel.Series.Add(inputFunctionSeries);
            FunctionSeries outputFunctionSeries = new FunctionSeries(this.model.Approximation, left, right, 0.01);
            outputFunctionSeries.Color = OxyColors.Red;
            outputFunctionSeries.Title = "Approximation";
            this.plotModel.Series.Add(outputFunctionSeries);

            plotModel.Legends.Add(new Legend()
            {
                LegendTitle = "Legend",
                LegendPosition = LegendPosition.TopRight,
            });

            /*ScatterSeries series = new ScatterSeries
            {
                MarkerFill = OxyColors.Red,
                MarkerType = MarkerType.Triangle,
                MarkerSize = 10
            };
            if(this.IntegrationMethodIndex == 0)
            {
                series.Points.Add(new ScatterPoint(this.integrationLeftBound, 0));
                series.Points.Add(new ScatterPoint(0.5*(this.integrationLeftBound+this.integrationRightBound), 0));
                series.Points.Add(new ScatterPoint(this.integrationRightBound, 0));
            }
            else
            {
                (double, double)[] nodes = this.model.FetchNodesGaussLegendre();
                for (int i = 0; i < nodes.Length; i++)
                {
                    series.Points.Add(new ScatterPoint(nodes[i].Item2, 0));
                }
            }
            
            this.plotModel.Series.Add(series);*/

            this.plotModel.InvalidatePlot(true);
            OnPropertyChanged(nameof(this.PlotModel));

            /*
                double y = model.functions[formulaIndex].Invoke(x);
                ScatterSeries series = new ScatterSeries
                {
                    MarkerFill = MethodIndex == 0 ? OxyColors.Blue : OxyColors.Red,
                    MarkerType = MarkerType.Triangle,
                    MarkerSize = 10
                };
                series.Points.Add(new ScatterPoint(x, y));
                Debug.WriteLine($"x: {x}; y: {y}");

                this.PlotModel.Series.Add(series);
                terminalLines.Add($"Solution marker at ({x}, {y})");
                this.PlotModel.InvalidatePlot(true);
             */
            this.model.ClearApproximation();
        }

        public int IntegrationMethodIndex
        {
            get => this.integrationMethodIndex;
            set
            {
                this.integrationMethodIndex = value;
            }
        }
        public int FunctionFormulaIndex
        {
            get => this.functionFormulaIndex;
            set
            {
                this.functionFormulaIndex = value;
                this.model.FunctionIndex = value;
            }
        }
        public uint QuadNodesNumber
        {
            get => this.quadNodesNumber;
            set
            {
                this.quadNodesNumber = value;
                this.model.QuadNodesNumber = (int)value;
            }
        }
        public double MinAccuracy
        {
            get => this.minAccuracy;
            set
            {
                this.minAccuracy = value;
                this.model.IntegrationAccuracy = value;
            }
        }
        public double ApproximationLeftBound
        {
            get => this.approximationLeftBound;
            set
            {
                this.approximationLeftBound = value;
                this.model.LeftBound = value;
            }
        }
        public double ApproximationRightBound
        {
            get => this.approximationRightBound;
            set
            {
                this.approximationRightBound = value;
                this.model.RightBound = value;
            }
        }

        public int ApproximationTerminationConditionIndex
        {
            get { return this.approximationTerminationConditionIndex; }
            set
            {
                this.approximationTerminationConditionIndex = value;
                if(0 == value)
                {
                    this.terminationLabel = "m=";
                }
                else
                {
                    this.terminationLabel = "E=";
                }
                OnPropertyChanged(nameof(TerminationLabel));
            }
        }
        public double TerminationConstant
        {
            get { return this.terminationConstant; }
            set
            {
                this.terminationConstant = value;
                this.model.TerminationConstant = value;
            }
        }
        public string TerminationLabel => this.terminationLabel;

        public bool QuadNodesNumberEnabled => this.integrationMethodIndex != 0;
        public string TerminalText { get => terminalText; }

        public Command IntegrationMethodIndexChangeCommand => this.integrationMethodIndexChangeCommand;
        public Command FunctionFormulaIndexChangeCommand => this.functionFormulaIndexChangeCommand;
        public Command ReplotCommand => this.replotCommand;
        public Command ApproximateCommand => this.approximateCommand;
        public Command ClearTerminalCommand => this.clearTerminalCommand;

        public PlotModel PlotModel { get => this.plotModel; }
    }
}

