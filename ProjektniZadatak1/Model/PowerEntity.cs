namespace ProjektniZadatak1.Model
{
    public class PowerEntity
    {
        private long id;

        private string name;

        private double x;

        private double y;

        private double latitude;

        private double longitude;

        private string toolTip;

        private int numberOfConnections;

        public long Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public double X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        public string ToolTip
        {
            get
            {
                return toolTip;
            }

            set
            {
                toolTip = value;
            }
        }

        public int NumberOfConnections
        {
            get
            {
                return numberOfConnections;
            }

            set
            {
                numberOfConnections = value;
            }
        }

        public double Latitude { get => latitude; set => latitude = value; }

        public double Longitude { get => longitude; set => longitude = value; }

        public PowerEntity() { }
    }
}
