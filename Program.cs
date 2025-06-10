using System;

class Program
{
    // Wyjątki
    //
    // Rejestracja: nazwa użytkownika jest zajęta
    public class DuplicateUserException : Exception
    {
        public DuplicateUserException(string message) : base("Użytkownika już istnieje!") { }
    }
    // Rejestracja: numer telefonu musi mieć 9 liczb
    public class WrongPhoneNumberException : Exception
    {
        public WrongPhoneNumberException(string message) : base("Numer telefonu jest niepoprawny!") { }
    }
    // Rejestracja: mail musi zawierać "@"
    public class WrongMailException : Exception
    {
        public WrongMailException(string message) : base("Mail jest niepoprawny!") { }
    }
    // Rejestracja: pieniądze muszą >=0
    public class WrongMoneyException : Exception
    {
        public WrongMoneyException(string message) : base("Niepoprawna ilość pieniędzy!") { }
    }

    // Logowanie: Użytkownik próbuje się zalogować gdy inny użytkownik już jest zalogowany
    public class SessionConflictException : Exception
    {
        public SessionConflictException(string message) : base("Użytkownik już jest zalogowany!") { }
    }
    // Logowanie: Nie znaleziono użytkownika
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base("Nie znaleziono użytkownika!") { }
    }
    // Logowanie: Użytkownik nie jest zalogowany
    public class AuthenticationRequiredException : Exception
    {
        public AuthenticationRequiredException(string message) : base("Użytkownik musi być zalogowany żeby wykonać operację!") { }
    }

    // Wypożyczanie: Dana stacja nie istnieje w agregatorze
    public class WrongStationException : Exception
    {
        public WrongStationException(string message) : base("Dana stacja nie istnieje!") { }
    }
    // Wypożyczanie: Brak środków na koncie
    public class NoFundsException : Exception
    {
        public NoFundsException(string message) : base("Brak środków na koncie!") { }
    }

    // DateTime: Niepoprawny format daty
    public class WrongDateFormatException : Exception
    {
        public WrongDateFormatException(string message) : base("Niepoprawny format daty!") { }
    }

    // Bike: Dany rower nie istnieje
    public class BikeNotFoundException : Exception
    {
        public BikeNotFoundException(string message) : base("Dany rower nie istnieje!") { }
    }

    // Rent: Nie znaleziono wypożyczenia
    public class RentNotFoundException : Exception
    {
        public RentNotFoundException(string message) : base("Nie znaleziono wypożyczenia!") { }
    }

    // Stacja: Brak miejsc
    public class NotEnoughFreeSpaceException : Exception
    {
        public NotEnoughFreeSpaceException(string message) : base("Brak miejsc na stacji!") { }
    }
    





    class User
    {
        private string Imie;
        private string Nazwisko;
        private string Numertel;
        private string Mail;
        private double Pieniadze;
        private List<BikeRent> Rent_Active;
        private List<BikeRent> Rent_History;
        public User(string Imie, string Nazwisko, string Numertel, string Mail, double Pieniadze = 0)
        {
            this.Imie = Imie;
            this.Nazwisko = Nazwisko;
            this.Numertel = Numertel;
            this.Mail = Mail;
            this.Pieniadze = Pieniadze;
            //tworzenie listy Rent_Active
            this.Rent_Active = new List<BikeRent>();
            //tworzenie listy Rent_History
            this.Rent_History = new List<BikeRent>();
        }
        public string getImie()
        {
            return Imie;
        }
        public string getNazwisko()
        {
            return Nazwisko;
        }
        public string getNumertel()
        {
            return Numertel;
        }
        public string getMail()
        {
            return Mail;
        }

        //poniższe tylko wtedy gdy zalogowany:
        public void Rent(int Bike_id, string Station_name, string Start_time)
        {
            BikeStation a = agregatorStacji.getStation(Station_name);

            // Podana stacja nie istnieje w agregatorze stacji
            if (a == null) { throw new WrongStationException(""); }
            if (Pieniadze < 0) { throw new NoFundsException("");  }

            DateTime dataa;
            // Zły format daty
            if (DateTime.TryParse(Start_time, out dataa)) { throw new WrongDateFormatException("");  }

            Bike rentowany = a.RemoveBike(Bike_id);
            if (rentowany != null)
            {
                Rent_Active.Add(new BikeRent(rentowany, dataa, a));
                Console.WriteLine("Wypożyczenie roweru przebiegło pomyślnie!");
                return;
            }
            else  // Dany rower nie istnieje
            {
                throw new BikeNotFoundException("");
            }
        }

        public void Return_Bike(int Bike_id, string Station_name, string End_time)
        {
            BikeRent aa = null;

            for (int i = 0; i < Rent_Active.Count; i++)
            {
                //if (Rent_Active[i].rentBike(Bike_id) != null)
                if (Rent_Active[i].Bicycle.ID == Bike_id)
                {
                    aa = Rent_Active[i];
                    break;
                }
            }

            if (aa == null) { throw new RentNotFoundException(""); }

            BikeStation ostatnia = agregatorStacji.getStation(Station_name);

            if (ostatnia == null) { throw new WrongStationException(""); }

            if (ostatnia.FreeSlots > 0)
            {

                DateTime data;
                if (DateTime.TryParse(End_time, out data))
                {
                    aa.SetEndTime(data);
                }
                else
                {
                    throw new WrongDateFormatException("");
                }
                ostatnia.AddBike(aa.Bicycle);
                aa.SetEndStation(Station_name);
                this.Pieniadze = this.Pieniadze - aa.SetCost();
                Rent_Active.Remove(aa);

                Rent_History.Add(aa);
            }
            else
            {
                throw new NotEnoughFreeSpaceException("");
            }

        }

        public void Deposit(double Value)
        {
            if (Value > 0)
            {
                this.Pieniadze += Value;
            }
            else
            {
                Console.WriteLine("Nieodpowiednia kwota");
            }
        }
        public void Display_Rent_Active()
        {
            Console.WriteLine("Aktywne wypożyczenia:");
            for (int i = 0; i < Rent_Active.Count; i++)
            {
                Console.WriteLine(Rent_Active[i].ToString());
            }
            if (Rent_Active.Count == 0) Console.WriteLine("brak");
        }
        public void Display_Rent_History()
        {

            Console.WriteLine("Nieaktywne wypożyczenia:");
            for (int i = 0; i < Rent_History.Count; i++)
            {
                Console.WriteLine(Rent_History[i].ToString());
            }
            if (Rent_History.Count == 0) Console.WriteLine("brak");
        }
    }

    class Users_Manager
    {
        private List<User> Users;
        private User CurrentUser;
        public Users_Manager()
        {
            this.Users = new List<User>();
            this.CurrentUser = null;
        }
        public void Register(string Imie, string Nazwisko, string Numertel, string Mail, double Pieniadze = 0)
        {
            // Wyjątek - użytkownik już istnieje
            foreach (User userInst in Users)
            {
                if (userInst.getImie() == Imie && userInst.getNazwisko() == Nazwisko && (userInst.getNumertel() == Numertel || userInst.getMail() == Mail))
                {
                    throw new DuplicateUserException("");
                }
            }
            // Wyjątek - numer tel musi mieć 9 liczb
            if (Numertel.Length != 9) { throw new WrongPhoneNumberException(""); }
            // Wyjątek - mail musi zawierać "@"
            if (Mail.Contains("@") != true) { throw new WrongMailException(""); }
            // Wyjątek - liczba pieniędzy musi >= 0
            if (Pieniadze < 0) { throw new WrongMoneyException(""); }

            // Brak wyjątków - dodanie nowego użytkownika
            Users.Add(new User(Imie, Nazwisko, Numertel, Mail, Pieniadze));
            Console.WriteLine("Rejestracja przebiegła pomyślnie!");
        }

        public void Login(string Imie, string Nazwisko, string Numertel, string Mail)
        {
            // Logowanie bez wcześniejszego wylogowania
            if (CurrentUser != null) { throw new SessionConflictException(""); }

            foreach (User userInst in Users)
            {
                if (userInst.getImie() == Imie && userInst.getNazwisko() == Nazwisko && userInst.getNumertel() == Numertel && userInst.getMail() == Mail)
                {
                    CurrentUser = userInst;
                    Console.WriteLine("Zalogowano jako " + CurrentUser.getImie());
                    return;
                }
            }
            // Logowanie nie powiodło się
            throw new UserNotFoundException("");
        }

        public void Logout()
        {
            // Trzeba być zalogowanym żeby się wylogować
            if (CurrentUser == null) { throw new AuthenticationRequiredException(""); }

            CurrentUser = null;
        }

        public User getCurrentUser()
        {
            if (CurrentUser == null) { throw new AuthenticationRequiredException(""); }
            return CurrentUser;
        }
    }


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


        // Pola do generowania ID w konstruktorze
        static HashSet<int> takenIDs = new HashSet<int>();
        static Random rand = new Random();

        // Konstruktor
        public Bike(double price)
        {
            // Generowanie losowego ID w pętli while aż ID będzie unikalne
            do
            {
                id = rand.Next(1, int.MaxValue);
            }
            while (!takenIDs.Add(id));

            this.price = price;
        }


        public override string ToString()
        {
            return "ID roweru: " + id + "\nRodzaj roweru: " + this.GetType().Name;
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
        public BikeStation getStation(string Name)
        {
            for (int i = 0; i < stations.Count; i++)
            {
                if (stations[i].getName() == Name)
                {
                    return stations[i];
                }
            }

            Console.WriteLine("Nie ma takiej stacji");
            return null;
        }

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
                station.getStationInfo();
            }
            
        }
    }


    // Stacja rowerowa
    public class BikeStation
    {
        // Pola
        private string name;
        private List<Bike> allBikes = new List<Bike>();
        private int totalSlots;
        private int freeSlots;
        public int FreeSlots  // getter
        {
            get { return freeSlots; }
        }

        // Konstruktor
        public BikeStation(string name, int totalSlots)
        {
            this.name = name;
            this.totalSlots = totalSlots;
            freeSlots = totalSlots - allBikes.Count;
        }

        // Metody
        //
        // Dodaje podany rower do stacji, jeśli jest miejsce
        public void AddBike(Bike newBike)
        {
            if (freeSlots > 0)
            {
                allBikes.Add(newBike);
                freeSlots = totalSlots - allBikes.Count;
            }
        }

        // Usuwa podany rower ze stacji
        public Bike RemoveBike(int removedBike_id)
        {
            int czy_znaleziono = 0;
            for (int i = 0; i < allBikes.Count; i++)
            {
                if (removedBike_id == allBikes[i].ID)
                {
                    Bike nasz = allBikes[i];
                    allBikes.RemoveAt(i);
                    freeSlots = totalSlots - allBikes.Count;
                    czy_znaleziono = 1;
                    return nasz;
                    break;
                }
            }
            //if (czy_znaleziono == 0) Console.WriteLine("nie znaleziono takiego roweru");
            return null;
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

        public string getName()
        {
            return name;
        }

        public void getStationInfo()
        {
            Console.WriteLine("Rowery zwykłe:");
            List<Bike> listka = this.GetRegular();
            for (int i = 0; i < listka.Count; i++)
            {
                Console.Write(listka[i].ID + " ");
            }
            Console.WriteLine();
            Console.WriteLine("Rowery dziecięce:");
            listka = this.GetChild();
            for (int i = 0; i < listka.Count; i++)
            {
                Console.Write(listka[i].ID + " ");
            }
            Console.WriteLine();
            Console.WriteLine("Rowery typu tandem:");
            listka = this.GetTandem();
            for (int i = 0; i < listka.Count; i++)
            {
                Console.Write(listka[i].ID + " ");
            }
            Console.WriteLine();
        }
        // Zwraca informacje o stacji rowerowej
        public override string ToString()
        {
            return "Nazwa stacji: " + name + "\nIlosc miejsc: " + totalSlots + "\nIlosc wolnych miejsc: " + freeSlots;
        }
    }

    // Wypożyczenie roweru
    public class BikeRent
    {
        // Pola
        //
        // Rower dowolnego rodzaju
        private Bike bicycle;
        public Bike Bicycle   // getter
        {
            get { return bicycle; }
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
        string endStation;
        public string EndStation  // getter, setter
        {
            get { return endStation; }
            set { endStation = value; }
        }

        // Kosz wypożyczenia roweru
        // Pole ma setter jako metodę
        double cost;
        public double Cost  // getter
        {
            get { return cost; }
        }


        // Konstruktor
        public BikeRent(Bike bicycl, DateTime startTime, BikeStation startStation)
        {
            this.bicycle = bicycl;
            this.startTime = startTime;
            this.startStation = startStation;
        }


        // Metody
        //
        // Ustawienie ceny wypożyczenia roweru
        public double SetCost()
        {
            TimeSpan span = endTime - startTime;  // Czas od początku do końca wypożyczenia roweru.
            double totalTime = span.TotalMinutes;
            // Cena = cena roweru + czas wypożyczenia, gdzie każda minuta kosztuje 2 grosze
            cost = (totalTime * 0.02); /*bikePrice +*/
            return cost;
        }

        public void SetEndTime(DateTime EndTime2)
        {
            this.endTime = EndTime2;
        }

        public void SetEndStation(string EndStation2)
        {
            this.endStation = EndStation2;
        }

        // Zwraca string z informacjami o wypożyczeniu - informacje o rowerze, długość czasu wypożyczenia, stacja początkowa, stacja końcowa (jeśli jest)
        public override string ToString()
        {
            string returnString = ("ID Roweru: " + bicycle.ID + "\nCzas wypożyczenia: " + (endTime - startTime) + "\nStacja początkowa: " + startStation.ToString());

            // Stacja końcowa może nie być zapisana, np. gdy rower jest jeszcze używany.
            if (endStation != null)
            {
                returnString += ("\nStacja końcowa: " + endStation.ToString());
            }

            return returnString;
        }
    }



    public static StationsAggregation agregatorStacji;
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
        BikeStation stacja = new BikeStation("Stacja1", 7);
        stacja.AddBike(rower1);
        stacja.AddBike(rower2);
        stacja.AddBike(crower1);
        stacja.AddBike(crower2);
        stacja.AddBike(crower3);

        agregatorStacji = new StationsAggregation();
        agregatorStacji.AddStation(stacja);
        agregatorStacji.GetStationsInfo();

        Users_Manager manago = new Users_Manager();
        // Przykładowy użytkownika
        manago.Register("a", "a", "123123123", "a@a", 100000);

        string im, na, nr, ma, input, nazwa_stacji, czass;
        double cash;
        int idd;
        User currentt = null;

        // Menu główne
        while (true)
        {
            Console.WriteLine("\nWybierz operację:");
            Console.WriteLine("1: Zaloguj się\n2: Wyloguj się\n3: Zarejestruj się\n4: Wypożycz rower\n5: Historia wypożyczeń użytkownika\n" +
                "6: Lista wypożyczalni rowerów\n7: Lista rowerów w danej wypożyczalni\n8: Zwróć rower \n9: Dodaj pieniądze na konto\n10: Aktywne wypożyczenia\n11: Zakończ program");

            string command = Console.ReadLine();


            switch (command)
            {
                case "1":  // Logowanie
                    Console.WriteLine("Podaj imię");
                    im = Console.ReadLine();
                    Console.WriteLine("Podaj nazwisko");
                    na = Console.ReadLine();
                    Console.WriteLine("Podaj numer telefonu");
                    nr = Console.ReadLine();
                    Console.WriteLine("Podaj mail");
                    ma = Console.ReadLine();

                    try
                    {
                        manago.Login(im, na, nr, ma);
                    }
                    catch (UserNotFoundException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (SessionConflictException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    break;

                case "2":  // Wylogowanie

                    try
                    {
                        manago.Logout();
                    }
                    catch (AuthenticationRequiredException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    
                    break;

                case "3":  // Rejestracja
                    Console.WriteLine("Podaj imię");
                    im = Console.ReadLine();
                    Console.WriteLine("Podaj nazwisko");
                    na = Console.ReadLine();
                    Console.WriteLine("Podaj numer telefonu");
                    nr = Console.ReadLine();
                    Console.WriteLine("Podaj mail");
                    ma = Console.ReadLine();
                    Console.WriteLine("Jeśli chcesz, podaj sumę, którą chcesz wpłacić");
                    input = Console.ReadLine();
                    if (double.TryParse(input, out cash)) { }
                    else
                    {
                        Console.WriteLine("Błędny format liczby!");
                        break;
                    }

                    try
                    {
                        manago.Register(im, na, nr, ma, cash);
                    }
                    catch (WrongMoneyException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (WrongMailException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (WrongPhoneNumberException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (DuplicateUserException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    break;

                case "4":  // Wypożycz rower
                    Console.WriteLine("Podaj nr ID roweru");
                    input = Console.ReadLine();
                    if (int.TryParse(input, out idd)) { }
                    else
                    {
                        Console.WriteLine("Błędnie wpisany numer ID");
                        break;
                    }

                    Console.WriteLine("Podaj nazwę stacji");
                    nazwa_stacji = Console.ReadLine();

                    Console.WriteLine("Podaj czas w formacie DateTime");
                    czass = Console.ReadLine();

                    try
                    {
                        currentt = manago.getCurrentUser();
                    }
                    catch (AuthenticationRequiredException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    

                    

                    try
                    {
                        currentt.Rent(idd, nazwa_stacji, czass);
                    }
                    catch (BikeNotFoundException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (WrongDateFormatException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (NoFundsException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (WrongStationException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    break;

                case "5":  // Historia wypożyczeń użytkownika

                    try
                    {
                        currentt = manago.getCurrentUser();
                        currentt.Display_Rent_History();
                    }
                    catch (AuthenticationRequiredException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    
                    

                    break;

                case "6":  // Lista wypożyczalni rowerów
                    agregatorStacji.GetStationsInfo();
                    break;

                case "7":  // Lista rowerów dostępnych w wypożyczalni
                    Console.WriteLine("Podaj nazwę wypożyczalni rowerów");
                    input = Console.ReadLine();
                    BikeStation bikestacja = agregatorStacji.getStation(input);
                    if (bikestacja != null)
                    {
                        Console.WriteLine(bikestacja.ToString());
                        bikestacja.getStationInfo();
                    }
                    else
                    {
                        Console.WriteLine("Błędna nazwa stacji");
                    }
                    break;

                case "8": // Zwróć rower
                    Console.WriteLine("Podaj nr ID roweru");
                    input = Console.ReadLine();
                    if (int.TryParse(input, out idd))
                    {

                    }
                    else
                    {
                        Console.WriteLine("Błędnie wpisany numer ID");
                        break;
                    }
                    Console.WriteLine("Podaj nazwę stacji");
                    nazwa_stacji = Console.ReadLine();
                    Console.WriteLine("Podaj czas w formacie DateTime");
                    
                    czass = Console.ReadLine();

                    try
                    {
                        currentt = manago.getCurrentUser();
                    }
                    catch (AuthenticationRequiredException ex)
                    {
                        Console.WriteLine(ex.Message);
                        break;
                    }

                    try
                    {
                        currentt.Return_Bike(idd, nazwa_stacji, czass);
                    }
                    catch (NotEnoughFreeSpaceException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (WrongDateFormatException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (WrongStationException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (RentNotFoundException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    break;

                case "9": // Dodaj pieniadze na konto
                    try
                    {
                        currentt = manago.getCurrentUser();
                    }
                    catch (AuthenticationRequiredException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    Console.WriteLine("Podaj kwotę");
                    input = Console.ReadLine();
                    if (double.TryParse(input, out cash))
                    {

                    }
                    else
                    {
                        Console.WriteLine("Błędnie wpisana kwota");
                        break;
                    }
                    currentt.Deposit(cash);
                    break;

                case "10": // Aktywne wypożyczenia użytkownika
                    try
                    {
                        currentt = manago.getCurrentUser();
                    }
                    catch (AuthenticationRequiredException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    currentt.Display_Rent_Active();
                    break;

                case "11":  // Koniec programu
                    return;

                default:
                    Console.WriteLine("BŁĄD! Wybierz poprawną operację!");
                    break;
            }

        }
    }
}