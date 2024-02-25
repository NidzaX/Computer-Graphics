using ProjektniZadatak1.Helpers;

namespace ProjektniZadatak1.Model
{
    public class NodeEntity : PowerEntity
    {
        public NodeEntity() { }

        public NodeEntity(long id, string name, double x, double y)
        {
            Id = id;
            Name = name;


            double convertedX;
            double convertedY;
            GeneralHelper.ToLatLon(x, y, 34, out convertedX, out convertedY);

            X = convertedX;
            Y = convertedY;
        }
    }
}
