using System;

class Program
{
    // Klasa abstrakcyjna Bike
    public abstract class Bike
    {
        // Pola
        int id;
        public int ID  // getter
        {
            get { return id; }
        }
        double price;
        public double Price  // getter
        {
            get { return price; }
        }

        // Konstruktor
        public Bike(double price)
        {
            // Losowanie 4-numerowego ID roweru
            Random rand = new Random();
            id = rand.Next() % 9999;

            this.price = price;
        }
    }

    // Rower zwykły
    public class RegularBike : Bike
    {
        // Konstruktor
        public RegularBike(double price) : base(price) { }
    }

    // Rower dziecięcy
    public class ChildBike : Bike
    {
        // Konstruktor
        public ChildBike(double price) : base(price) { }
    }

    // Rower tandem
    public class TandemBike : Bike
    {
        // Konstruktor
        public TandemBike(double price) : base(price) { }
    }


    // Agregator stacji rowerowych
    public class StationsAggregation
    {
        // Lista wszystkich stacji
        List<BikeStation> stations = new List<BikeStation>();

        // Konstruktor
        public StationsAggregation() { }

        // Metody
        //
        // Dodaje podaną stację do listy stacji
        public void AddStation(BikeStation newStation)
        {
            stations.Add(newStation);
        }

        // Usuwa podaną stację z listy stacji
        public void RemoveStation(BikeStation removedStation)
        {
            stations.Remove(removedStation);
        }

        // Wypisuje informacje o stacjach w liście stacji
        public void GetStationsInfo()
        {
            // Jeśli w agregatorze nie ma żadnych stacji...
            if (stations.Count <= 0)
            {
                Console.WriteLine("W agregatorze nie ma żadnych stacji!");
                return;
            }

            // W przeciwnym wypadku...
            Console.WriteLine("Informacje o stacjach w tym agregatorze stacji:");
            foreach (BikeStation station in stations)
            {
                Console.WriteLine(station.ToString());
            }
            
        }
    }


    // Stacja rowerowa
    public class BikeStation
    {
        // Pola
        string? name;
        List<Bike> allBikes = new List<Bike>();
        int totalSlots;
        int freeSlots;
        public int FreeSlots  // getter
        {
            get { return freeSlots; }
        }

        // Konstruktor
        public BikeStation(string name, int totalSlots)
        {
            this.name = name;
            this.totalSlots = totalSlots;
            freeSlots = totalSlots - allBikes.Count();
        }

        // Metody
        //
        // Dodaje podany rower do stacji, jeśli jest miejsce
        public void AddBike(Bike newBike)
        {
            if (freeSlots > 0)
            {
                allBikes.Add(newBike);
                freeSlots = totalSlots - allBikes.Count();
            }
        }

        // Usuwa podany rower ze stacji
        public void RemoveBike(Bike removedBike)
        {
            allBikes.Remove(removedBike);
            freeSlots = totalSlots - allBikes.Count();
        }


        // Zwraca listę zwykłych rowerów
        public List<Bike> GetRegular()
        {
            List<Bike> regularBikes = new List<Bike>();  // Lista tymczasowa

            foreach (Bike bikeElement in allBikes)
            {
                // Jeśli dany rower jest zwykłym rowerem, zostaje dodany do zwracanej listy
                if (bikeElement is RegularBike)
                {
                    regularBikes.Add(bikeElement);
                    Console.WriteLine(bikeElement);
                }
            }

            return regularBikes;
        }

        // Zwraca listę rowerów dziecięcych
        public List<Bike> GetChild()
        {
            List<Bike> childBikes = new List<Bike>();

            foreach (Bike bikeElement in allBikes)
            {
                if (bikeElement is ChildBike)
                {
                    childBikes.Add(bikeElement);
                    Console.WriteLine(bikeElement);
                }
            }

            return childBikes;
        }

        // Zwraca listę rowerów tandem
        public List<Bike> GetTandem()
        {
            List<Bike> tandemBikes = new List<Bike>();

            foreach (Bike bikeElement in allBikes)
            {
                if (bikeElement is TandemBike)
                {
                    tandemBikes.Add(bikeElement);
                    Console.WriteLine(bikeElement);
                }
            }

            return tandemBikes;
        }

        // Zwraca informacje o stacji rowerowej
        public override string ToString()
        {
            return "Nazwa stacji: " + name + "\nIlosc miejsc: " + totalSlots + "\nIlosc wolnych miejsc: " + freeSlots;
        }
    }



    // Funkcja main
    static void Main(string[] args)
    {
        // Przykładowe rowery
        RegularBike rower1 = new RegularBike(20);
        RegularBike rower2 = new RegularBike(20);
        ChildBike crower1 = new ChildBike(20);
        ChildBike crower2 = new ChildBike(20);
        ChildBike crower3 = new ChildBike(20);

        // Stacja rowerowa
        BikeStation stacja = new BikeStation("Stacja 1", 6);
        stacja.AddBike(rower1);
        stacja.AddBike(rower2);
        stacja.AddBike(crower1);
        stacja.AddBike(crower2);
        stacja.AddBike(crower3);

        stacja.GetRegular();
        stacja.GetChild();
        stacja.GetTandem();
        Console.WriteLine("Free: " + stacja.FreeSlots);

        //Console.WriteLine(stacja.ToString());

        StationsAggregation agregatorStacji = new StationsAggregation();
        agregatorStacji.AddStation(stacja);
        agregatorStacji.GetStationsInfo();
        agregatorStacji.RemoveStation(stacja);
        agregatorStacji.GetStationsInfo();
    }
}