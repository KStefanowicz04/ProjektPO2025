using System;

// BikeRent
public class BikeRent
{
    // Pola
    //
    // Rower - dowolny rodzaj roweru
    Bike rower = new Bike;
    public Bike Rower   // getter
    {
        get { return rower; }
    }

    // Czas w którym rower został wypożyczony
    DateTime startTime;
    public DateTime StartTime  // getter
    {
        get { return startTime; }
    }

    // Czas w którym rower został oddany do stacji
    DateTime endTime;
    public DateTime EndTime  // getter, setter
    {
        get { return endTime; }
        set { endTime = value; }
    }

    // Stacja początkowa wypożyczenia, ustawiana przy wypożyczeniu
    BikeStation startStation;
    public BikeStation StartStation  // getter
    {
        get { return startStation; }
    }

    // Stacja końcowa wypożyczenia, ustawiana przy oddaniu rowera
    BikeStation endStation;
    public BikeStation EndStation  // getter, setter
    {
        get { return endStation; }
        set {  endStation = value; }
    }

    // Kosz wypożyczenia roweru
    // Pole ma setter jako metodę
    double cost;
    public double Cost  // getter
    {
        get { return cost; }
    }

    // Konstruktor
    public BikeRent() { }

    // Metody
    //
    // Setter pola cost
    public void SetCost(string startTime, string endTime, double bikePrice)
    {

    }

    //
    public override string ToString()
    {

    }
}
