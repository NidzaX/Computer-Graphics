using ProjektniZadatak1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.IO;
using System.Xml.Linq;
using ProjektniZadatak1.Enums;
using ProjektniZadatak1.Windows;
using ProjektniZadatak1.Helpers;
using Path = System.Windows.Shapes.Path;
using Microsoft.Win32;

namespace ProjektniZadatak1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties
        // History
        public bool WasCleared { get; set; }
        public static List<UIElement> UndoHistory { get; set; } = new List<UIElement>();
        public static List<UIElement> CanvasObjects { get; set; } = new List<UIElement>();
        public static List<System.Windows.Point> polygonPoints = new List<System.Windows.Point>();
        public static Dictionary<UIElement, TextBlock> ShapeTextPairs = new Dictionary<UIElement, TextBlock>();

        // Visulisation
        public static List<UIElement> SubstationEntities = new List<UIElement>();
        public static List<UIElement> NodeEntities = new List<UIElement>();
        public static List<UIElement> SwitchEntities = new List<UIElement>();

        public static List<UIElement> EntitiesWith03Connections = new List<UIElement>();
        public static List<UIElement> EntitiesWith35Connections = new List<UIElement>();
        public static List<UIElement> EntitiesWithMoreThan5Connections = new List<UIElement>();

        public static List<UIElement> LinesWithR01 = new List<UIElement>();
        public static List<UIElement> LinesWithR12 = new List<UIElement>();
        public static List<UIElement> LinesWithRMoreThan2 = new List<UIElement>();

        public static List<UIElement> LinesFromSteel = new List<UIElement>();
        public static List<UIElement> LinesFromAcsr = new List<UIElement>();
        public static List<UIElement> LinesFromCopper = new List<UIElement>();
        public static List<UIElement> LinesFromOtherMaterial = new List<UIElement>();

        public static List<UIElement> UndergroundLines = new List<UIElement>();

        public static List<UIElement> OpenLines = new List<UIElement>();
        public static List<UIElement> OpenSwitches = new List<UIElement>();
        public static List<UIElement> ClosedLines = new List<UIElement>();
        public static List<UIElement> ClosedSwitches = new List<UIElement>();
        public static List<UIElement> OtherEntites = new List<UIElement>();
        public static List<UIElement> OtherLines2 = new List<UIElement>();

        // Other
        public XmlEntities xmlEntities { get; set; } = null;
        public string baseFolder = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.Length - 10);
        public string xmlPath;
        public bool loadModel = false;
        public double noviX, noviY;
        public List<double> listaX = new List<double>();
        public List<double> listaY = new List<double>();
        public List<PowerEntity> listaPowerEntities = new List<PowerEntity>();
        public List<PowerEntity> listaScaledPowerEntities = new List<PowerEntity>();
        public List<LineEntity> listaLines = new List<LineEntity>();
        public bool[,] matrica = new bool[300, 300];
        public List<LinijaVoda> listaSvihLinija = new List<LinijaVoda>();
        public List<System.Windows.Point> listaTacki = new List<System.Windows.Point>();
        public Tuple<Path, Path> obojeniElementi = new Tuple<Path, Path>(null, null);
        public double pozicijaX, pozicijaY;
        public List<double> poligonX = new List<double>();
        public List<double> poligonY = new List<double>();
        public List<UIElement> obrisaniElementi = new List<UIElement>();
        public bool clear = false;
        public bool edit = false;
        public int pozicijaEditovanogElementa = -1;
        public bool iscrtaoPresek = false;
        public Dictionary<Polyline, Brush> transparentLines = new Dictionary<Polyline, Brush>();
        public Dictionary<Ellipse, Brush> transparentEntities = new Dictionary<Ellipse, Brush>();
        public Dictionary<Polyline, LineEntity> lineDictionary = new Dictionary<Polyline, LineEntity>();
        public Dictionary<long, Tuple<PowerEntity, Ellipse>> nodeDictionary = new Dictionary<long, Tuple<PowerEntity, Ellipse>>();
        public bool showHideSubs = false;
        public Dictionary<Ellipse, Brush> transparentSubstations = new Dictionary<Ellipse, Brush>();
        public Dictionary<Ellipse, Brush> transparentSwitches = new Dictionary<Ellipse, Brush>();
        public Dictionary<Ellipse, Brush> transparentNodes = new Dictionary<Ellipse, Brush>();
        public List<Path> interrsections = new List<Path>();
        public Ellipse elipsaRepeat = null;
        public Polygon polygonRepeat = null;
        public TextBlock textRepeat = null;
        public bool repeat = false;
        public int brojPoligona = 0;
        public static Ellipse ellipse = null;
        public static Polygon polygon = null;
        public static TextBox text = null;
        public static TextBlock shapeText = null;
        public ShapeOption? SelectedShape { get; set; } = null;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            ColorComboBox.SelectedIndex = 0;
        }

        #region Load Network
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 300; i++)
            {
                for (int j = 0; j < 300; j++)
                {
                    matrica[i, j] = false;
                }
            }
            
            xmlEntities = ParseXml();
            LoadXML();
            FormMatrix();
            FormLines();
        }

        public XmlEntities ParseXml()
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            string xmlFile = "Geographic.xml";

            string filePath = System.IO.Path.Combine(baseFolder, directory, xmlFile); // Combine the base folder, selected directory, and selected XML file

            if (!File.Exists(filePath))
            {
                return null;
            }

            //Load xml
            XDocument xdoc = XDocument.Load(filePath);

            double longit = 0;
            double latid = 0;

            //Run query
            var lines = xdoc.Descendants("LineEntity")
                     .Select(line => new LineEntity
                     {
                         Id = (long)line.Element("Id"),
                         Name = (string)line.Element("Name"),
                         ConductorMaterial = (string)line.Element("ConductorMaterial"),
                         IsUnderground = (bool)line.Element("IsUnderground"),
                         R = (float)line.Element("R"),
                         FirstEnd = (long)line.Element("FirstEnd"),
                         SecondEnd = (long)line.Element("SecondEnd"),
                         LineType = (string)line.Element("LineType"),
                         ThermalConstantHeat = (long)line.Element("ThermalConstantHeat"),
                         ToolTip = $"Line:\nID: {(long)line.Element("Id")}\nName: {(string)line.Element("Name")}\nIs underground: {((bool)line.Element("IsUnderground") ? "Yes" : "No")}\nConductorMaterial: {(string)line.Element("ConductorMaterial")}\nLine type: {(string)line.Element("LineType")}\nThermal constant heat: {(long)line.Element("ThermalConstantHeat")}\nFirst end: {(long)line.Element("FirstEnd")}\nSecond end: {(long)line.Element("SecondEnd")}",
                         Vertices = line.Element("Vertices").Descendants("Point").Select(p => new Model.Point
                         {
                             X = (double)p.Element("X"),
                             Y = (double)p.Element("Y"),
                         }
                         ).ToList()
                     }).ToList();

            foreach (var line in lines)
            {
                foreach (var point in line.Vertices)
                {
                    GeneralHelper.ToLatLon(point.X, point.Y, 34, out latid, out longit);
                    point.Latitude = latid;
                    point.Longitude = longit;
                }
            }

            var substations = xdoc.Descendants("SubstationEntity")
                     .Select(sub => new SubstationEntity
                     {
                         Id = (long)sub.Element("Id"),
                         Name = (string)sub.Element("Name"),
                         X = (double)sub.Element("X"),
                         Y = (double)sub.Element("Y"),
                         ToolTip = "Substation:\nID: " + (long)sub.Element("Id") + "\nName: " + (string)sub.Element("Name")
                     }).ToList();

            foreach (var item in substations)
            {
                GeneralHelper.ToLatLon(item.X, item.Y, 34, out latid, out longit);
                item.Latitude = latid;
                item.Longitude = longit;

                item.NumberOfConnections = lines.Where(l => l.FirstEnd == item.Id || l.SecondEnd == item.Id).Count();
            }

            var nodes = xdoc.Descendants("NodeEntity")
                     .Select(node => new NodeEntity
                     {
                         Id = (long)node.Element("Id"),
                         Name = (string)node.Element("Name"),
                         X = (double)node.Element("X"),
                         Y = (double)node.Element("Y"),
                         ToolTip = "Node:\nID: " + (long)node.Element("Id") + "\nName: " + (string)node.Element("Name")
                     }).ToList();

            foreach (var item in nodes)
            {
                GeneralHelper.ToLatLon(item.X, item.Y, 34, out latid, out longit);
                item.Latitude = latid;
                item.Longitude = longit;

                item.NumberOfConnections = lines.Where(l => l.FirstEnd == item.Id || l.SecondEnd == item.Id).Count();
            }

            var switches = xdoc.Descendants("SwitchEntity")
                     .Select(sw => new SwitchEntity
                     {
                         Id = (long)sw.Element("Id"),
                         Name = (string)sw.Element("Name"),
                         Status = (string)sw.Element("Status"),
                         X = (double)sw.Element("X"),
                         Y = (double)sw.Element("Y"),
                         ToolTip = "Switch:\nID: " + (long)sw.Element("Id") + "\nName: " + (string)sw.Element("Name") + "\nStatus: " + (string)sw.Element("Status")
                     }).ToList();

            foreach (var item in switches)
            {
                GeneralHelper.ToLatLon(item.X, item.Y, 34, out latid, out longit);
                item.Latitude = latid;
                item.Longitude = longit;

                item.NumberOfConnections = lines.Where(l => l.FirstEnd == item.Id || l.SecondEnd == item.Id).Count();
            }

            return new XmlEntities { Substations = substations, Switches = switches, Nodes = nodes, Lines = lines };
        }

        public void LoadXML()
        {
            poligonX.Clear();
            poligonY.Clear();

            foreach (var obj in xmlEntities.Nodes)
            {
                listaX.Add(obj.X);
                listaY.Add(obj.Y);
                listaPowerEntities.Add(obj);
            }
            foreach (var obj in xmlEntities.Substations)
            {
                listaX.Add(obj.X);
                listaY.Add(obj.Y);
                listaPowerEntities.Add(obj);
            }
            foreach (var obj in xmlEntities.Switches)
            {
                listaX.Add(obj.X);
                listaY.Add(obj.Y);
                listaPowerEntities.Add(obj);
            }
            foreach (var obj in xmlEntities.Lines)
            {
                listaX.Add(obj.X);
                listaY.Add(obj.Y);
                listaLines.Add(obj);
            }
        }

        public void FormMatrix()
        {
            int newX;
            int newY;
            List<Tuple<int, int>> reservedPosition = new List<Tuple<int, int>>();

            double firstX = listaPowerEntities.Min(xx => xx.X);
            double firstY = listaPowerEntities.Min(yy => yy.Y);
            double lastX = listaPowerEntities.Max(xx => xx.X);
            double lastY = listaPowerEntities.Max(yy => yy.Y);

            foreach (var element in listaPowerEntities)
            {
                newY = (int)((299 * (element.Y - firstY)) / (lastY - firstY));
                newX = (int)((299 * (element.X - firstX)) / (lastX - firstX));

                int korak = 1;
                bool validPositionFound = false;

                while (!validPositionFound)
                {
                    reservedPosition.Clear();

                    reservedPosition.Add(new Tuple<int, int>(newX + korak, newY + korak));
                    reservedPosition.Add(new Tuple<int, int>(newX + korak, newY - korak));
                    reservedPosition.Add(new Tuple<int, int>(newX - korak, newY + korak));
                    reservedPosition.Add(new Tuple<int, int>(newX - korak, newY - korak));

                    for (int i = korak - 1; i > -korak; i--)
                    {
                        reservedPosition.Add(new Tuple<int, int>(newX + korak, newY + i));
                        reservedPosition.Add(new Tuple<int, int>(newX + i, newY - korak));
                        reservedPosition.Add(new Tuple<int, int>(newX - korak, newY + i));
                        reservedPosition.Add(new Tuple<int, int>(newX + i, newY + korak));
                    }

                    foreach (var position in reservedPosition)
                    {
                        if (position.Item1 >= 0 && position.Item1 < 300 && position.Item2 >= 0 && position.Item2 < 300)
                        {
                            if (matrica[position.Item1, position.Item2] == false)
                            {
                                matrica[position.Item1, position.Item2] = true;

                                Ellipse ellipse = new Ellipse();
                                ellipse.Width = 2;
                                ellipse.Height = 2;
                                ellipse.ToolTip = element.ToolTip;
                                ellipse.MouseLeftButtonDown += Entity_MouseLeftButtonDown;
                                ellipse.MouseRightButtonDown += Entity_MouseRightButtonDown;

                                if (element.GetType() == typeof(SubstationEntity))
                                {
                                    ellipse.Fill = System.Windows.Media.Brushes.Blue;
                                    ellipse.Tag = $"substation";
                                    SubstationEntities.Add(ellipse);
                                    OtherEntites.Add(ellipse);
                                }
                                else if (element.GetType() == typeof(NodeEntity))
                                {
                                    ellipse.Fill = System.Windows.Media.Brushes.Crimson;
                                    ellipse.Tag = $"node";
                                    NodeEntities.Add(ellipse);
                                    OtherEntites.Add(ellipse);
                                }
                                else if (element.GetType() == typeof(SwitchEntity))
                                {
                                    ellipse.Fill = System.Windows.Media.Brushes.Green;
                                    ellipse.Tag = $"switch";
                                    SwitchEntities.Add(ellipse);

                                    if (((SwitchEntity)element).Status == "Open")
                                    {
                                        OpenSwitches.Add(ellipse);
                                    }
                                    else
                                    {
                                        ClosedSwitches.Add(ellipse);
                                    }
                                }

                                switch (element.NumberOfConnections)
                                {
                                    // 0-3
                                    case 0:
                                    case 1:
                                    case 2:
                                        EntitiesWith03Connections.Add(ellipse);
                                        break;
                                    // 3-5
                                    case 3:
                                    case 4:
                                        EntitiesWith35Connections.Add(ellipse);
                                        break;
                                    // 5+
                                    default:
                                        EntitiesWithMoreThan5Connections.Add(ellipse);
                                        break;
                                }

                                validPositionFound = true; // Set the flag to true to exit the loop

                                Canvas.SetLeft(ellipse, newX * 3);
                                Canvas.SetBottom(ellipse, newY * 3);

                                PowerEntity scaledEntity = element;
                                scaledEntity.X = newY;
                                scaledEntity.Y = newX;

                                listaScaledPowerEntities.Add(scaledEntity);
                                canvas.Children.Add(ellipse);
                                nodeDictionary.Add(element.Id, new Tuple<PowerEntity, Ellipse>(element, ellipse));

                                break;
                            }
                        }
                    }

                    korak++;
                }
            }
        }

        private void FormLines()
        {
            foreach (LineEntity line in listaLines)
            {
                double firstX = -1;
                double firstY = -1;
                double secondX = -1;
                double secondY = -1;

                foreach (var ent in listaScaledPowerEntities)
                {
                    if (line.FirstEnd == ent.Id)
                    {
                        firstX = ent.X;
                        firstY = ent.Y;
                    }
                    if (line.SecondEnd == ent.Id)
                    {
                        secondX = ent.X;
                        secondY = ent.Y;
                    }
                }

                if (firstX == -1 || firstY == -1 || secondX == -1 || secondY == -1)
                {
                    continue;
                }

                line.PocetakX = firstX;
                line.PocetakY = firstY;
                line.KrajX = secondX;
                line.KrajY = secondY;

                System.Windows.Point start = new System.Windows.Point(firstX, firstY);
                System.Windows.Point end = new System.Windows.Point(secondX, secondY);

                BFSPassage1(line, start, end);
            }

            foreach (LineEntity line in listaLines)
            {
                double firstX = -1;
                double firstY = -1;
                double secondX = -1;
                double secondY = -1;

                foreach (var ent in listaScaledPowerEntities)
                {
                    if (line.FirstEnd == ent.Id)
                    {
                        firstX = ent.X;
                        firstY = ent.Y;
                    }
                    if (line.SecondEnd == ent.Id)
                    {
                        secondX = ent.X;
                        secondY = ent.Y;
                    }
                }

                if (firstX == -1 || firstY == -1 || secondX == -1 || secondY == -1)
                {
                    continue;
                }

                System.Windows.Point start = new System.Windows.Point(firstX, firstY);
                System.Windows.Point end = new System.Windows.Point(secondX, secondY);

                if (line.Prolaz != "1")
                {
                    BFSPassage2(line, start, end);
                }

                line.Prolaz = "0";
            }
            iscrtaoPresek = true;
        }

        private void BFSPassage1(LineEntity line, System.Windows.Point start, System.Windows.Point end)
        {
            FieldPosition pocetak = new FieldPosition((int)line.PocetakX, (int)line.PocetakY);
            FieldPosition kraj = new FieldPosition((int)line.KrajX, (int)line.KrajY);
            FieldPosition[,] prev = BFSPath.BFSFind(line, BFSVariables.BFSLines);
            List<FieldPosition> putanja = BFSPath.ReconstructingPath(pocetak, kraj, prev);

            if (putanja == null)
            {
                line.Prolaz = "0";
            }
            else
            {
                line.Prolaz = "1";

                System.Windows.Point p1 = new System.Windows.Point(pocetak.PozY * 3 + 1, -pocetak.PozX * 3 + 910 - 1);
                System.Windows.Point p3 = new System.Windows.Point(kraj.PozY * 3 + 1, -kraj.PozX * 3 + 910 - 1);
                putanja.Remove(pocetak);
                putanja.Remove(kraj);

                if (start.X != end.X)
                {
                    Polyline polyline = new Polyline();
                    polyline.Stroke = Brushes.Black;
                    polyline.StrokeThickness = 0.5;
                    polyline.ToolTip = line.ToolTip;

                    polyline.MouseRightButtonDown += CanvasPolyline_MouseRightButtonDown;

                    PointCollection points = new PointCollection();
                    points.Add(p1);
                    foreach (FieldPosition zauzeto in putanja)
                    {
                        BFSVariables.BFSLines[zauzeto.PozX, zauzeto.PozY] = 'X';
                        points.Add(new System.Windows.Point(zauzeto.PozY * 3 + 1, -zauzeto.PozX * 3 + 910 - 1));
                    }
                    points.Add(p3);
                    polyline.Points = points;

                    for (int i = 0; i < points.Count - 1; i++)
                    {
                        LinijaVoda novaLinija = new LinijaVoda();
                        novaLinija.Id = line.Id;
                        novaLinija.FirstEnd = points[i];
                        novaLinija.SecondEnd = points[i + 1];
                        listaSvihLinija.Add(novaLinija);
                    }

                    switch (line.R)
                    {
                        case double r when r >= 0 && r < 1:
                            LinesWithR01.Add(polyline);
                            break;
                        case double r when r >= 1 && r < 2:
                            LinesWithR12.Add(polyline);
                            break;
                        case double r when r >= 2:
                            LinesWithRMoreThan2.Add(polyline);
                            break;
                    }

                    if (line.ConductorMaterial.Equals("Steel"))
                    {
                        polyline.Stroke = new SolidColorBrush(Colors.Gray);
                        LinesFromSteel.Add(polyline);
                    }
                    else if (line.ConductorMaterial.Equals("Acsr"))
                    {
                        polyline.Stroke = new SolidColorBrush(Colors.Salmon);
                        LinesFromAcsr.Add(polyline);
                    }
                    else if (line.ConductorMaterial.Equals("Copper"))
                    {
                        polyline.Stroke = new SolidColorBrush(Colors.Brown);
                        LinesFromCopper.Add(polyline);
                    }
                    else if (line.ConductorMaterial.Equals("Other"))
                    {
                        polyline.Stroke = new SolidColorBrush(Colors.Green);
                        LinesFromOtherMaterial.Add(polyline);
                    }

                    if (line.IsUnderground)
                    {
                        UndergroundLines.Add(polyline);
                    }

                    SwitchEntity entity = xmlEntities.Switches.FirstOrDefault(s => s.Id == line.FirstEnd);

                    if (entity == null)
                    {
                        OtherLines2.Add(polyline);
                    }
                    else if (entity.Status == "Open")
                    {
                        OpenLines.Add(polyline);
                    }
                    else
                    {
                        ClosedLines.Add(polyline);
                    }

                    canvas.Children.Add(polyline);
                    lineDictionary.Add(polyline, line);
                }
            }
        }

        private void BFSPassage2(LineEntity line, System.Windows.Point start, System.Windows.Point end)
        {
            FieldPosition pocetak = new FieldPosition((int)line.PocetakX, (int)line.PocetakY);
            FieldPosition kraj = new FieldPosition((int)line.KrajX, (int)line.KrajY);
            FieldPosition[,] prev = BFSPath.BFSFind(line, BFSVariables.BFSLines2);
            List<FieldPosition> path = BFSPath.ReconstructingPath(pocetak, kraj, prev);

            if (path == null)
            {
                line.Prolaz = "0";
            }
            else
            {
                line.Prolaz = "2";

                System.Windows.Point p1 = new System.Windows.Point(pocetak.PozY * 3 + 1, -pocetak.PozX * 3 + 910 - 1);
                System.Windows.Point p3 = new System.Windows.Point(kraj.PozY * 3 + 1, -kraj.PozX * 3 + 910 - 1);
                path.Remove(pocetak);
                path.Remove(kraj);

                if (start.X != end.X)
                {
                    Polyline polyline = new Polyline();
                    polyline.Stroke = Brushes.Black;
                    polyline.StrokeThickness = 0.5;
                    polyline.ToolTip = line.ToolTip;

                    polyline.MouseRightButtonDown += CanvasPolyline_MouseRightButtonDown;

                    PointCollection points = new PointCollection();
                    points.Add(p1);
                    foreach (FieldPosition zauzeto in path)
                    {
                        points.Add(new System.Windows.Point(zauzeto.PozY * 3 + 1, -zauzeto.PozX * 3 + 910 - 1));
                    }
                    points.Add(p3);
                    polyline.Points = points;

                    List<LinijaVoda> listaNovihLinija = new List<LinijaVoda>();
                    for (int i = 0; i < points.Count - 1; i++)
                    {
                        LinijaVoda novaLinija = new LinijaVoda();
                        novaLinija.Id = line.Id;
                        novaLinija.FirstEnd = points[i];
                        novaLinija.SecondEnd = points[i + 1];
                        listaSvihLinija.Add(novaLinija);
                        listaNovihLinija.Add(novaLinija);
                    }

                    foreach (var novaLinijaLista in listaNovihLinija)
                    {
                        foreach (var linija in listaSvihLinija)
                        {
                            if (linija.Id == novaLinijaLista.Id)
                                continue;
                            else
                            {
                                if (CheckForIntersection(linija.FirstEnd, linija.SecondEnd, novaLinijaLista.FirstEnd, novaLinijaLista.SecondEnd))
                                {
                                    FindIntersection(linija.FirstEnd, linija.SecondEnd, novaLinijaLista.FirstEnd, novaLinijaLista.SecondEnd);
                                }
                            }
                        }
                    }

                    switch (line.R)
                    {
                        case double r when r >= 0 && r < 1:
                            LinesWithR01.Add(polyline);
                            break;
                        case double r when r >= 1 && r < 2:
                            LinesWithR12.Add(polyline);
                            break;
                        case double r when r >= 2:
                            LinesWithRMoreThan2.Add(polyline);
                            break;
                    }

                    if (line.ConductorMaterial.Equals("Steel"))
                    {
                        polyline.Stroke = new SolidColorBrush(Colors.Gray);
                        LinesFromSteel.Add(polyline);
                    }
                    else if (line.ConductorMaterial.Equals("Acsr"))
                    {
                        polyline.Stroke = new SolidColorBrush(Colors.Salmon);
                        LinesFromAcsr.Add(polyline);
                    }
                    else if (line.ConductorMaterial.Equals("Copper"))
                    {
                        polyline.Stroke = new SolidColorBrush(Colors.Brown);
                        LinesFromCopper.Add(polyline);
                    }
                    else if (line.ConductorMaterial.Equals("Other"))
                    {
                        polyline.Stroke = new SolidColorBrush(Colors.Green);
                        LinesFromOtherMaterial.Add(polyline);
                    }

                    if (line.IsUnderground)
                    {
                        UndergroundLines.Add(polyline);
                    }

                    SwitchEntity entity = xmlEntities.Switches.FirstOrDefault(s => s.Id == line.FirstEnd);

                    if (entity == null)
                    {
                        OtherLines2.Add(polyline);
                    }
                    else if (entity.Status == "Open")
                    {
                        OpenLines.Add(polyline);
                    }
                    else
                    {
                        ClosedLines.Add(polyline);
                    }

                    canvas.Children.Add(polyline);
                    lineDictionary.Add(polyline, line);
                }
            }
        }

        public bool CheckForIntersection(System.Windows.Point A, System.Windows.Point B, System.Windows.Point C, System.Windows.Point D)
        {
            return Check(A, C, D) != Check(B, C, D) && Check(A, B, C) != Check(A, B, D);
        }

        public bool Check(System.Windows.Point A, System.Windows.Point B, System.Windows.Point C)
        {
            return (C.Y - A.Y) * (B.X - A.X) > (B.Y - A.Y) * (C.X - A.X);
        }

        public void FindIntersection(System.Windows.Point p1, System.Windows.Point p2, System.Windows.Point p3, System.Windows.Point p4)
        {
            System.Windows.Point IntersectionPoint = new System.Windows.Point();

            double a1 = 1e+10, a2 = 1e+10;
            double b1, b2;
            if ((p2.X - p1.X) != 0)
                a1 = (p2.Y - p1.Y) / (p2.X - p1.X);
            if ((p4.X - p3.X) != 0)
                a2 = (p4.Y - p3.Y) / (p4.X - p3.X);
            b1 = p1.Y - a1 * p1.X;
            b2 = p3.Y - a2 * p3.X;

            IntersectionPoint.X = (b2 - b1) / (a1 - a2);
            IntersectionPoint.Y = a2 * IntersectionPoint.X + b2;

            foreach (var tacka in listaTacki)
            {
                if (tacka.X == IntersectionPoint.X && tacka.Y == IntersectionPoint.Y)
                {
                    return;
                }
            }
            Path cross = new Path() { Fill = Brushes.Black };
            cross.Data = new EllipseGeometry(IntersectionPoint, 1, 1);
            interrsections.Add(cross);
            canvas.Children.Add(cross);
        }

        private void Entity_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse selectedEntity = (Ellipse)sender;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg;*.jpeg;*.gif;*.bmp)|*.png;*.jpg;*.jpeg;*.gif;*.bmp|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));

                switch (selectedEntity.Tag.ToString())
                {
                    case "substation":
                        foreach (Ellipse entity in SubstationEntities)
                        {
                            entity.Stroke = new ImageBrush(bitmapImage);
                        }
                        break;
                    case "node":
                        foreach (Ellipse entity in NodeEntities)
                        {
                            entity.Stroke = new ImageBrush(bitmapImage);
                        }
                        break;
                    case "switch":
                        foreach (Ellipse entity in SwitchEntities)
                        {
                            entity.Stroke = new ImageBrush(bitmapImage);
                        }
                        break;
                }
            }
        }

        private void Entity_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ellipse selectedEntity = (Ellipse)sender;

            System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();

            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.Windows.Media.Color chosenColor = System.Windows.Media.Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);

                SolidColorBrush newBrush = new SolidColorBrush(chosenColor);

                switch (selectedEntity.Tag.ToString())
                {
                    case "substation":
                        foreach (Ellipse entity in SubstationEntities)
                        {
                            entity.Stroke = newBrush;
                        }
                        break;
                    case "node":
                        foreach (Ellipse entity in NodeEntities)
                        {
                            entity.Stroke = newBrush;
                        }
                        break;
                    case "switch":
                        foreach (Ellipse entity in SwitchEntities)
                        {
                            entity.Stroke = newBrush;
                        }
                        break;
                }
            }
        }

        private void CanvasPolyline_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (obojeniElementi.Item1 != null && obojeniElementi.Item2 != null)
            {
                canvas.Children.Remove(obojeniElementi.Item1);
                canvas.Children.Remove(obojeniElementi.Item2);
            }
            int.TryParse(txtBoxAnimationDuration.Text, out int duration);

            Polyline vod = (Polyline)sender;
            System.Windows.Point p1 = vod.Points.First();
            System.Windows.Point p2 = vod.Points.Last();

            // Get the selected color from the ComboBox
            ComboBoxItem selectedColorItem = (ComboBoxItem)ColorComboBox.SelectedItem;
            string selectedColor = selectedColorItem?.Content?.ToString();

            // Set a default color (Yellow) if parsing fails or if no color is selected
            Brush fillColor = Brushes.Yellow;
            if (!string.IsNullOrWhiteSpace(selectedColor))
            {
                try
                {
                    fillColor = (Brush)new BrushConverter().ConvertFromString(selectedColor);
                }
                catch { }
            }

            Path elipsa1 = new Path() { Fill = fillColor };
            elipsa1.Data = new EllipseGeometry(p1, 2, 2);
            canvas.Children.Add(elipsa1);

            Path elipsa2 = new Path() { Fill = fillColor };
            elipsa2.Data = new EllipseGeometry(p2, 2, 2);
            canvas.Children.Add(elipsa2);

            obojeniElementi = new Tuple<Path, Path>(elipsa1, elipsa2);

            DoubleAnimation da = new DoubleAnimation();
            da.From = 1;
            da.To = 2;
            da.Duration = new Duration(TimeSpan.FromSeconds(duration));
            ScaleTransform scale1 = new ScaleTransform(2, 2, p1.X, p1.Y);
            elipsa1.RenderTransform = scale1;

            ScaleTransform scale2 = new ScaleTransform(2, 2, p2.X, p2.Y);
            elipsa2.RenderTransform = scale2;

            scale1.BeginAnimation(ScaleTransform.ScaleXProperty, da);
            scale1.BeginAnimation(ScaleTransform.ScaleYProperty, da);
            scale2.BeginAnimation(ScaleTransform.ScaleXProperty, da);
            scale2.BeginAnimation(ScaleTransform.ScaleYProperty, da);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Allow only numeric input
            e.Handled = !IsNumericInput(e.Text);
        }

        private bool IsNumericInput(string input)
        {
            return int.TryParse(input, out int _);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Validate and adjust the value to the range 1-10
                if (int.TryParse(textBox.Text, out int enteredValue))
                {
                    enteredValue = Math.Max(1, Math.Min(10, enteredValue));
                    textBox.Text = enteredValue.ToString();
                }
                else
                {
                    // Reset to 2 if not a valid integer
                    textBox.Text = "2";
                }
            }
        }
        #endregion

        #region Draw Shapes
        #region Drawing
        private void canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (SelectedShape)
            {
                case ShapeOption.Ellipse:
                    // Create ellipse
                    ellipse = null;
                    var ellipsePosition = Mouse.GetPosition(canvas);
                    var ellipseWindow = new EllipseWindow(ellipsePosition);

                    ellipseWindow.ShowDialog();

                    // Check for ellipse
                    if (ellipse == null)
                    {
                        SelectedShape = null;
                        MakeButtonsNormal();
                        return;
                    }

                    // Check for and draw text in ellipse
                    if (shapeText != null)
                    {
                        canvas.Children.Add(shapeText);
                        ShapeTextPairs[ellipse] = shapeText;
                        Panel.SetZIndex(shapeText, 1);
                        shapeText = null;
                    }

                    // Draw ellipse
                    ellipse.MouseLeftButtonDown += Ellipse_MouseLeftButtonDown;
                    canvas.Children.Add(ellipse);

                    // Undo/Redo/Clear Commands
                    CanvasObjects.Add(ellipse);
                    UndoHistory.Clear();
                    polygonPoints.Clear();
                    ellipse = null;
                    SelectedShape = null;
                    MakeButtonsNormal();
                    break;
                case ShapeOption.Polygon:
                    var position = Mouse.GetPosition(canvas);
                    polygonPoints.Add(position);
                    break;
                case ShapeOption.Text:
                    // Create ellipse
                    var textPosition = Mouse.GetPosition(canvas);
                    var textWindow = new TextWindow(textPosition);

                    textWindow.ShowDialog();

                    // Check for ellipse
                    if (text == null)
                    {
                        SelectedShape = null;
                        MakeButtonsNormal();
                        return;
                    }

                    // Draw text
                    text.PreviewMouseLeftButtonDown += Text_PreviewMouseLeftButtonDown;
                    canvas.Children.Add(text);

                    // Undo/Redo/Clear Commands
                    CanvasObjects.Add(text);
                    UndoHistory.Clear();
                    polygonPoints.Clear();
                    text = null;
                    SelectedShape = null;
                    MakeButtonsNormal();
                    break;
            }
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (SelectedShape != ShapeOption.Polygon)
            {
                return;
            }

            // Exit the method if the list is null or empty
            if (polygonPoints == null || polygonPoints.Count < 3)
            {
                return;
            }

            // Create polygon
            var polygonWindow = new PolygonWindow(polygonPoints);
            polygonWindow.ShowDialog();

            // Check for polygon
            if (polygon == null)
            {
                SelectedShape = null;
                MakeButtonsNormal();
                return;
            }

            // Check for and draw text in polygon
            if (shapeText != null)
            {
                canvas.Children.Add(shapeText);
                ShapeTextPairs[polygon] = shapeText;
                Panel.SetZIndex(shapeText, 1);
                shapeText = null;
            }

            // Draw polygon
            polygon.MouseLeftButtonDown += Polygon_MouseLeftButtonDown;
            canvas.Children.Add(polygon);

            // Undo/Redo/Clear Commands
            CanvasObjects.Add(polygon);
            UndoHistory.Clear();
            polygonPoints.Clear();

            polygon = null;
            SelectedShape = null;
            MakeButtonsNormal();
        }

        public void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var clickedEllipse = (Ellipse)sender;
            TextBlock textBlock;
            ShapeTextPairs.TryGetValue(clickedEllipse, out textBlock);

            var ellipseWindow = new EllipseWindow(clickedEllipse, textBlock);
            ellipseWindow.ShowDialog();

            if (ellipse == null)
            {
                return;
            }

            ellipse.Margin = clickedEllipse.Margin;

            // First draw text if there is any
            if (shapeText != null)
            {
                GeneralHelper.AdjustMarginToCenter(shapeText, ellipse);
                canvas.Children.Remove(textBlock);
                canvas.Children.Add(shapeText);
                ShapeTextPairs.Remove(clickedEllipse);
                ShapeTextPairs[ellipse] = shapeText;
                Panel.SetZIndex(shapeText, 1);
            }

            ellipse.MouseLeftButtonDown += Ellipse_MouseLeftButtonDown;
            canvas.Children.Remove(clickedEllipse);
            canvas.Children.Add(ellipse);

            var index = CanvasObjects.IndexOf(clickedEllipse);
            CanvasObjects.RemoveAt(index);
            CanvasObjects.Insert(index, ellipse);

            ellipse = null;
            shapeText = null;
        }

        public void Polygon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var clickedPolygon = (Polygon)sender;
            TextBlock textBlock;
            ShapeTextPairs.TryGetValue(clickedPolygon, out textBlock);

            var polygonWindow = new PolygonWindow(clickedPolygon, textBlock);
            polygonWindow.ShowDialog();

            if (polygon == null)
            {
                return;
            }

            polygon.Margin = clickedPolygon.Margin;

            // First draw text if there is any
            if (shapeText != null)
            {
                GeneralHelper.AdjustMarginToCenter(shapeText, polygon);
                canvas.Children.Remove(textBlock);
                canvas.Children.Add(shapeText);
                ShapeTextPairs.Remove(clickedPolygon);
                ShapeTextPairs[polygon] = shapeText;
                Panel.SetZIndex(shapeText, 1);
            }

            polygon.MouseLeftButtonDown += Polygon_MouseLeftButtonDown;
            canvas.Children.Remove(clickedPolygon);
            canvas.Children.Add(polygon);

            var index = CanvasObjects.IndexOf(clickedPolygon);
            CanvasObjects.RemoveAt(index);
            CanvasObjects.Insert(index, polygon);

            polygon = null;
            shapeText = null;
        }

        public void Text_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var clickedText = (TextBox)sender;
            var textWindow = new TextWindow(clickedText);
            textWindow.ShowDialog();

            if (text == null)
            {
                return;
            }

            text.Margin = clickedText.Margin;
            text.PreviewMouseLeftButtonDown += Text_PreviewMouseLeftButtonDown;
            canvas.Children.Remove(clickedText);
            canvas.Children.Add(text);

            var index = CanvasObjects.IndexOf(clickedText);
            CanvasObjects.RemoveAt(index);
            CanvasObjects.Insert(index, text);
        }

        private void Shape_Click(object sender, RoutedEventArgs e)
        {
            var clickedButton = sender as Button;

            if (clickedButton == null)
            {
                return;
            }

            MakeButtonGreen(clickedButton.Name);
        }
        #endregion

        #region History
        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            if (WasCleared)
            {
                foreach (UIElement element in UndoHistory)
                {
                    if (ShapeTextPairs.ContainsKey(element))
                    {
                        canvas.Children.Add(ShapeTextPairs[element]);
                    }
                    canvas.Children.Add(element);
                    CanvasObjects.Add(element);
                }

                UndoHistory.Clear();
                WasCleared = false;

                return;
            }

            if (canvas.Children.Count > 0)
            {
                var element = canvas.Children[canvas.Children.Count - 1];
                UndoHistory.Add(element);
                CanvasObjects.Remove(element);
                canvas.Children.Remove(element);
                if (ShapeTextPairs.ContainsKey(element))
                {
                    canvas.Children.Remove(ShapeTextPairs[element]);
                }
            }
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            if (UndoHistory.Count > 0 && WasCleared == false)
            {
                var element = UndoHistory[UndoHistory.Count - 1];
                if (ShapeTextPairs.ContainsKey(element))
                {
                    canvas.Children.Add(ShapeTextPairs[element]);
                }
                canvas.Children.Add(element);
                CanvasObjects.Add(element);
                UndoHistory.RemoveAt(UndoHistory.Count - 1);
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            UndoHistory.Clear();

            foreach (UIElement element in CanvasObjects)
            {
                UndoHistory.Add(element);
                canvas.Children.Remove(element);
                if (ShapeTextPairs.ContainsKey(element))
                {
                    canvas.Children.Remove(ShapeTextPairs[element]);
                }
            }

            CanvasObjects.Clear();
            WasCleared = true;
        }
        #endregion

        #region Other
        private void MakeButtonsNormal()
        {
            foreach (var button in buttonsGrid.Children.OfType<Button>())
            {
                button.Background = new SolidColorBrush(Colors.LightGray);
            }
        }

        private void MakeButtonGreen(string buttonName)
        {
            foreach (var button in buttonsGrid.Children.OfType<Button>())
            {
                // Unselect other buttons
                if (button.Name != buttonName)
                {
                    button.Background = new SolidColorBrush(Colors.LightGray);
                    continue;
                }

                // Toggle selection for pressed button
                if (button.Background is SolidColorBrush solidColorBrush && solidColorBrush.Color == Colors.LightGreen)
                {
                    button.Background = new SolidColorBrush(Colors.LightGray);
                    SelectedShape = null;
                }
                else
                {
                    button.Background = new SolidColorBrush(Colors.LightGreen);
                    if (buttonName == "btnEllipse" || buttonName == "btnPolygon" || buttonName == "btnText")
                    {
                        SelectedShape = (ShapeOption)Enum.Parse(typeof(ShapeOption), buttonName.Substring(3));
                    }
                    polygonPoints.Clear();
                }
            }
        }
        #endregion
        #endregion

        #region Other Functionalities
        // Inactive Network
        private void OnlyOpenNetwork_Check(object sender, RoutedEventArgs e)
        {
            foreach(Ellipse entity in OpenSwitches)
            {
                entity.Stroke = new SolidColorBrush(Colors.Red);
            }
            foreach(Polyline entity in OpenLines)
            {
                entity.Stroke = new SolidColorBrush(Colors.Red);
            }
            foreach(Ellipse entity in ClosedSwitches)
            {
                entity.Stroke = new SolidColorBrush(Colors.Blue);
            }
            foreach(Polyline entity in ClosedLines)
            {
                entity.Stroke = new SolidColorBrush(Colors.Blue);
            }
            foreach(Ellipse entity in OtherEntites)
            {
                entity.Stroke = new SolidColorBrush(Colors.Transparent);
            }
            foreach(Polyline entity in OtherLines2)
            {
                entity.Stroke = new SolidColorBrush(Colors.Transparent);
            }
        }
        
        private void OnlyOpenNetwork_Uncheck(object sender, RoutedEventArgs e)
        {
            RevertEntityFillColors();
            RevertLineFillColors();
        }

        // Entities by Connections
        private void ColorEntitiesByConnections_Check(object sender, RoutedEventArgs e)
        {
            foreach(Ellipse entity in EntitiesWith03Connections)
            {
                entity.Stroke = Brushes.MediumVioletRed;
            }
            foreach(Ellipse entity in EntitiesWith35Connections)
            {
                entity.Stroke = Brushes.Red;
            }
            foreach(Ellipse entity in EntitiesWithMoreThan5Connections)
            {
                entity.Stroke = Brushes.DarkRed;
            }
        }

        private void ColorEntitiesByConnections_Uncheck(object sender, RoutedEventArgs e) => RevertEntityFillColors();

        // Lines by Resistance
        private void ColorLinesByResistance_Check(object sender, RoutedEventArgs e)
        {
            foreach (Polyline line in LinesWithR01)
            {
                line.Stroke = Brushes.Red;
            }
            foreach (Polyline line in LinesWithR12)
            {
                line.Stroke = Brushes.Orange;
            }
            foreach (Polyline line in LinesWithRMoreThan2)
            {
                line.Stroke = Brushes.Yellow;
            }
        }

        private void ColorLinesByResistance_Uncheck(object sender, RoutedEventArgs e) => RevertLineFillColors();

        // Other
        private void RevertEntityFillColors()
        {
            foreach (Ellipse entity in SubstationEntities)
            {
                entity.Stroke = Brushes.Blue;
            }
            foreach (Ellipse entity in NodeEntities)
            {
                entity.Stroke = Brushes.Crimson;
            }
            foreach (Ellipse entity in SwitchEntities)
            {
                entity.Stroke = Brushes.Green;
            }
        }

        private void RevertLineFillColors()
        {
            foreach (Polyline line in LinesFromSteel)
            {
                line.Stroke = Brushes.Gray;
            }
            foreach (Polyline line in LinesFromAcsr)
            {
                line.Stroke = Brushes.Salmon;
            }
            foreach (Polyline line in LinesFromCopper)
            {
                line.Stroke = Brushes.Brown;
            }
            foreach (Polyline line in LinesFromOtherMaterial)
            {
                line.Stroke = Brushes.Green;
            }
            foreach (Polyline line in UndergroundLines)
            {
                line.Stroke = Brushes.Black;
            }
        }

        // Image Save
        private void SavePicture_Click(object sender, RoutedEventArgs e)
        {
            double scaleFactor = 2;

            double width = canvas.ActualWidth * scaleFactor;
            double height = canvas.ActualHeight * scaleFactor;

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)width, (int)height, 96, 96, PixelFormats.Default);
            bmp.Render(canvas);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            string filePath = baseFolder + "\\Photos\\" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".png";
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(stream);
            }
        }
        #endregion
    }
}
