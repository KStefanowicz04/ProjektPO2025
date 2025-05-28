using System;

class Stations_Aggregation
{
    private List<Bike_Station> Stations;

    public Stations_Aggregation()
    {
        this.Stations= new List<Bike_Station>();
    }

    public Bike_Station getStation(string Name)
    {
        for (int i = 0; i < Stations.Count;i++)
        {
            if (Stations[i].getName() == Name)
            {
                return Stations[i];
            }
        }
        Console.WriteLine("Nie ma takiej stacji");
        return null;   
    }
}
class Bike_Station
{
    private string Name;

    public Bike_Station(string Name)
    {
        this.Name = Name;
    }


    public string getName()
    {
        //potrzebujemy getName
        return Name;
    }
    public Bike getBike(int Bike_id)
    {
        //
    }
}

class Bike
{

}
class Program
{
    static void Main(string[] args)
    {
        Stations_Aggregation Agregat = new Stations_Aggregation();

    }
}