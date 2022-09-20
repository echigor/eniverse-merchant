namespace Eniverse.Model
{
    public class Station
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _planetName;
        public string PlanetName
        {
            get { return _planetName; }
            set { _planetName = value; }
        }

        private string _starSystemName;
        public string StarSystemName
        {
            get { return _starSystemName; }
            set { _starSystemName = value; }
        }

        private double _xCoordinate;
        public double XCoordinate
        {
            get { return _xCoordinate; }
            set { _xCoordinate = value; }
        }

        private double _yCoordinate;
        public double YCoordinate
        {
            get { return _yCoordinate; }
            set { _yCoordinate = value; }
        }

        private double _zCoordinate;
        public double ZCoordinate
        {
            get { return _zCoordinate; }
            set { _zCoordinate = value; }
        }

        public Station()
        {
        }
    }
}
