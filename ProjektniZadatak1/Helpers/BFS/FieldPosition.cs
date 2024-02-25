namespace ProjektniZadatak1.Helpers
{
    internal class FieldPosition
    {
        int pozX;
        int pozY;

        public FieldPosition(int pozX, int pozY)
        {
            this.pozX = pozX;
            this.pozY = pozY;
        }

        public int PozX { get => pozX; set => pozX = value; }
        public int PozY { get => pozY; set => pozY = value; }
    }
}
