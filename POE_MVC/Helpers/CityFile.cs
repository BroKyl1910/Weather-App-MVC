namespace POE_MVC.Helpers
{
    //Class to deserialize cities.json

    //File, which is a list of cities, is represented by CityFile
    public class CityFile
    {
        public City[] cities { get; set; }
    }

    //Each city represented by City object
    public class City
    {
        public int id { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public Coord coord { get; set; }

        public override string ToString()
        {
            return name + ", " + country;
        }
    }

    public class Coord
    {
        public float lon { get; set; }
        public float lat { get; set; }
    }
}



