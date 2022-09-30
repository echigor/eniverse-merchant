namespace Eniverse.ClientModel
{
    public class Merchant
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private decimal _credits;
        public decimal Credits
        {
            get { return _credits; }
            set { _credits = value; }
        }

        private string _starshipName;
        public string StarshipName
        {
            get { return _starshipName; }
            set { _starshipName = value; }
        }

        private short _cargoHoldVolume;
        public short CargoHoldVolume
        {
            get { return _cargoHoldVolume; }
            set { _cargoHoldVolume = value; }
        }

        private int _currentStationID;
        public int CurrentStationID
        {
            get { return _currentStationID; }
            set { _currentStationID = value; }
        }

        public Merchant()
        {
        }
    }
}
