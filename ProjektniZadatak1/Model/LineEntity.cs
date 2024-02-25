using System.Collections.Generic;

namespace ProjektniZadatak1.Model
{
    public class LineEntity : PowerEntity
    {
        private bool isUnderground;
        
        private double r;
        
        private string conductorMaterial;
        
        private string lineType;

        private long thermalConstantHeat;
        
        private long firstEnd;

        private long secondEnd;
        
        private List<Point> vertices;

        private double pocetakX;
        private double pocetakY;
        private double krajX;
        private double krajY;
        private string prolaz;

        public bool IsUnderground
        {
            get { return isUnderground; }
            set
            {
                if (isUnderground != value)
                {
                    isUnderground = value;
                }
            }
        }

        public double R
        {
            get { return r; }
            set
            {
                if (r != value)
                {
                    r = value;
                }
            }
        }

        public string ConductorMaterial
        {
            get { return conductorMaterial; }
            set
            {
                if (conductorMaterial != value)
                {
                    conductorMaterial = value;
                }
            }
        }

        public string LineType
        {
            get { return lineType; }
            set
            {
                if (lineType != value)
                {
                    lineType = value;
                }
            }
        }

        public long ThermalConstantHeat
        {
            get { return thermalConstantHeat; }
            set
            {
                if (thermalConstantHeat != value)
                {
                    thermalConstantHeat = value;
                }
            }
        }

        public long FirstEnd
        {
            get { return firstEnd; }
            set
            {
                if (firstEnd != value)
                {
                    firstEnd = value;
                }
            }
        }

        public long SecondEnd
        {
            get { return secondEnd; }
            set
            {
                if (secondEnd != value)
                {
                    secondEnd = value;
                }
            }
        }

        public List<Point> Vertices
        {
            get { return vertices; }
            set
            {
                if (vertices != value)
                {
                    vertices = value;
                }
            }
        }

        public double PocetakX { get => pocetakX; set => pocetakX = value; }
        public double PocetakY { get => pocetakY; set => pocetakY = value; }
        public double KrajX { get => krajX; set => krajX = value; }
        public double KrajY { get => krajY; set => krajY = value; }
        public string Prolaz { get => prolaz; set => prolaz = value; }
    }
}
