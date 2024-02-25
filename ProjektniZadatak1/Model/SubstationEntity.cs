using ProjektniZadatak1.Helpers;

namespace ProjektniZadatak1.Model
{
    public class SubstationEntity : PowerEntity
    {
        public SubstationEntity() { }

        public SubstationEntity(long id, string name, double x, double y)
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
