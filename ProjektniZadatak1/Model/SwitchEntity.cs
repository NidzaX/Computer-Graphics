using ProjektniZadatak1.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektniZadatak1.Model
{
    public class SwitchEntity : PowerEntity
    {
        private string status;

        public string Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                }
            }
        }

        public SwitchEntity() { }

        public SwitchEntity(long id, string name, string status, double x, double y)
        {
            Id = id;
            Name = name;
            Status = status;

            double convertedX;
            double convertedY;
            GeneralHelper.ToLatLon(x, y, 34, out convertedX, out convertedY);

            X = convertedX;
            Y = convertedY;
        }
    }
}
