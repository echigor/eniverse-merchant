namespace Eniverse.Model
{
    public class Product
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

        private short _availableVolume;
        public short AvailableVolume
        {
            get { return _availableVolume; }
            set { _availableVolume = value; }
        }

        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }

        public Product()
        {
        }

    }
}
