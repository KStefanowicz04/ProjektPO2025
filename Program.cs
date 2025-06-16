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
    public class WrongmailException : Exception
    {
        public WrongmailException(string message) : base("mail jest niepoprawny!") { }
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
        private string imie;
        private string nazwisko;
        private string numerTel;
        private string mail;
        private double pieniadze;
        private List<BikeRent> rentActive;
        private List<BikeRent> rentHistory;
        public User(string imie, string nazwisko, string numerTel, string mail, double pieniadze = 0)
        {
            this.imie = imie;
            this.nazwisko = nazwisko;
            this.numerTel = numerTel;
            this.mail = mail;
            this.pieniadze = pieniadze;
            //tworzenie listy rentActive
            this.rentActive = new List<BikeRent>();
            //tworzenie listy rentHistory
            this.rentHistory = new List<BikeRent>();
        }
        public string GetImie()
        {
            return imie;
        }
        public string GetNazwisko()
        {
            return nazwisko;
        }
        public string GetNumerTel()
        {
            return numerTel;
        }
        public string GetMail()
        {
            return mail;
        }
        public string GetPieniadze()
        {
            return pieniadze.ToString();
        }

        //poniższe tylko wtedy gdy zalogowany:
        public void Rent(int bikeID, string stationName, string startTime)
        {
            BikeStation a = agregatorStacji.GetStation(stationName);

            // Podana stacja nie istnieje w agregatorze stacji
            if (a == null) { throw new WrongStationException(""); }
            if (pieniadze < 0) { throw new NoFundsException(""); }

            DateTime dataa;
            // Zły format daty
            if (DateTime.TryParse(startTime, out dataa))
            {
                startTime = startTime;
            }
            else
            {
                throw new WrongDateFormatException("");
            }

            Bike rentowany = a.RemoveBike(bikeID);
            if (rentowany != null)
            {
                rentActive.Add(new BikeRent(rentowany, dataa, a));
                Console.WriteLine("Wypożyczenie roweru przebiegło pomyślnie!");
            }
            else  // Dany rower nie istnieje
            {
                throw new BikeNotFoundException("");
            }
        }

        public void ReturnBike(int bikeID, string stationName, string End_time)
        {
            BikeRent aa = null;

            for (int i = 0; i < rentActive.Count; i++)
            {
                //if (rentActive[i].rentBike(bikeID) != null)
                if (rentActive[i].Bicycle.ID == bikeID)
                {
                    aa = rentActive[i];
                    break;
                }
            }

            if (aa == null) { throw new RentNotFoundException(""); }

            BikeStation ostatnia = agregatorStacji.GetStation(stationName);

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
                aa.SetEndStation(stationName);
                this.pieniadze = this.pieniadze - aa.SetCost(aa.Bicycle);
                rentActive.Remove(aa);

                rentHistory.Add(aa);
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
                this.pieniadze += Value;
            }
            else
            {
                Console.WriteLine("Nieodpowiednia kwota");
            }
        }
        public void DisplayRentActive()
        {
            Console.WriteLine("Aktywne wypożyczenia:");
            for (int i = 0; i < rentActive.Count; i++)
            {
                Console.WriteLine(rentActive[i].ToString());
            }
            if (rentActive.Count == 0) Console.WriteLine("brak");
        }
        public void DisplayRentHistory()
        {

            Console.WriteLine("Nieaktywne wypożyczenia:");
            for (int i = 0; i < rentHistory.Count; i++)
            {
                Console.WriteLine(rentHistory[i].ToString());
                Console.WriteLine("Koszt wypożyczenia: "+ (rentHistory[i].Cost).ToString());
            }
            if (rentHistory.Count == 0) Console.WriteLine("brak");
        }
    }

    class UsersManager
    {
        private List<User> Users;
        private User CurrentUser;
        public UsersManager()
        {
            this.Users = new List<User>();
            this.CurrentUser = null;
        }
        public void Register(string imie, string nazwisko, string numerTel, string mail, double pieniadze = 0)
        {
            // Wyjątek - użytkownik już istnieje
            foreach (User userInst in Users)
            {
                if (userInst.GetNumerTel() == numerTel || userInst.GetMail() == mail)
                {
                    throw new DuplicateUserException("");
                }
            }
            // Wyjątek - numer tel musi mieć 9 liczb
            if (numerTel.Length != 9) { throw new WrongPhoneNumberException(""); }
            // Wyjątek - mail musi zawierać "@"
            if (mail.Contains("@") != true) { throw new WrongmailException(""); }
            // Wyjątek - liczba pieniędzy musi >= 0
            if (pieniadze < 0) { throw new WrongMoneyException(""); }

            // Brak wyjątków - dodanie nowego użytkownika
            Users.Add(new User(imie, nazwisko, numerTel, mail, pieniadze));
            Console.WriteLine("Rejestracja przebiegła pomyślnie!");
        }

        public void Login(string imie, string nazwisko, string numerTel, string mail)
        {
            // Logowanie bez wcześniejszego wylogowania
            if (CurrentUser != null) { throw new SessionConflictException(""); }

            foreach (User userInst in Users)
            {
                if (userInst.GetImie() == imie && userInst.GetNazwisko() == nazwisko && userInst.GetNumerTel() == numerTel && userInst.GetMail() == mail)
                {
                    CurrentUser = userInst;
                    Console.WriteLine("Zalogowano jako " + CurrentUser.GetImie());
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

        public User GetCurrentUser()
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
        const double price = 1;
        // Konstruktor
        public RegularBike() : base(price) { }
    }

    // Rower dziecięcy
    public class ChildBike : Bike
    {
        const double price = 2;
        // Konstruktor
        public ChildBike() : base(price) { }
    }

    // Rower tandem
    public class TandemBike : Bike
    {
        const double price = 3;
        // Konstruktor
        public TandemBike() : base(price) { }
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
        public BikeStation GetStation(string Name)
        {
            for (int i = 0; i < stations.Count; i++)
            {
                if (stations[i].GetName() == Name)
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
                station.GetStationInfo();
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
        public Bike RemoveBike(int removedbikeID)
        {
            int czy_znaleziono = 0;
            for (int i = 0; i < allBikes.Count; i++)
            {
                if (removedbikeID == allBikes[i].ID)
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

        public string GetName()
        {
            return name;
        }

        public void GetStationInfo()
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
        public double SetCost(Bike rowerrr)
        {
            TimeSpan span = endTime - startTime;  // Czas od początku do końca wypożyczenia roweru.
            double totalTime = span.TotalMinutes;
            // Cena = cena roweru + czas wypożyczenia, gdzie każda minuta kosztuje 2 grosze
            cost = (totalTime * 0.02)*rowerrr.Price; /*bikePrice +*/
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
        RegularBike rower1 = new RegularBike();
        RegularBike rower2 = new RegularBike();
        ChildBike crower1 = new ChildBike();
        ChildBike crower2 = new ChildBike();
        ChildBike crower3 = new ChildBike();
        TandemBike trower1 = new TandemBike();

        // Stacja rowerowa
        BikeStation stacja = new BikeStation("Stacja1", 7);
        BikeStation stacja2 = new BikeStation("Stacja2", 14);
        stacja.AddBike(rower1);
        stacja.AddBike(rower2);
        stacja.AddBike(crower1);
        stacja.AddBike(crower2);
        stacja.AddBike(crower3);
        stacja.AddBike(trower1);

        agregatorStacji = new StationsAggregation();
        agregatorStacji.AddStation(stacja);
        agregatorStacji.AddStation(stacja2);
        agregatorStacji.GetStationsInfo();

        UsersManager manago = new UsersManager();
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
                "6: Lista wypożyczalni rowerów\n7: Lista rowerów w danej wypożyczalni\n8: Zwróć rower \n9: Dodaj pieniądze na konto\n10: Aktywne wypożyczenia\n11: Liczba środków na koncie\n12: Zakończ program");

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
                        Console.WriteLine("Wylogowano");
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
                    if (double.TryParse(input, out cash)) { input = input; }
                    else
                    {
                        Console.WriteLine("Nie wpłacono środków");
                    }

                    try
                    {
                        manago.Register(im, na, nr, ma, cash);
                    }
                    catch (WrongMoneyException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (WrongmailException ex)
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
                    if (int.TryParse(input, out idd)) { input = input; }
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
                        currentt = manago.GetCurrentUser();
                    }
                    catch (AuthenticationRequiredException ex)
                    {
                        Console.WriteLine(ex.Message);
                        break;
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
                        currentt = manago.GetCurrentUser();
                    }
                    catch (AuthenticationRequiredException ex)
                    {
                        Console.WriteLine(ex.Message);
                        break;
                    }
                    currentt.DisplayRentHistory();
                    
                    

                    break;

                case "6":  // Lista wypożyczalni rowerów
                    agregatorStacji.GetStationsInfo();
                    break;

                case "7":  // Lista rowerów dostępnych w wypożyczalni
                    Console.WriteLine("Podaj nazwę wypożyczalni rowerów");
                    input = Console.ReadLine();
                    BikeStation bikestacja = agregatorStacji.GetStation(input);
                    if (bikestacja != null)
                    {
                        Console.WriteLine(bikestacja.ToString());
                        bikestacja.GetStationInfo();
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
                        input = input;
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
                        currentt = manago.GetCurrentUser();
                    }
                    catch (AuthenticationRequiredException ex)
                    {
                        Console.WriteLine(ex.Message);
                        break;
                    }

                    try
                    {
                        currentt.ReturnBike(idd, nazwa_stacji, czass);
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
                        currentt = manago.GetCurrentUser();
                    }
                    catch (AuthenticationRequiredException ex)
                    {
                        Console.WriteLine(ex.Message);
                        break;
                    }

                    Console.WriteLine("Podaj kwotę");
                    input = Console.ReadLine();
                    if (double.TryParse(input, out cash))
                    {
                        input = input;
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
                        currentt = manago.GetCurrentUser();
                    }
                    catch (AuthenticationRequiredException ex)
                    {
                        Console.WriteLine(ex.Message);
                        break;
                    }
                    currentt.DisplayRentActive();
                    break;
                
                case "11": //ile pieniedzy
                    try
                    {
                        currentt = manago.GetCurrentUser();
                    }
                    catch (AuthenticationRequiredException ex)
                    {
                        Console.WriteLine(ex.Message);
                        break;
                    }
                    Console.WriteLine("Masz na koncie:");
                    Console.WriteLine(currentt.GetPieniadze());
                    break;

                case "12":  // Koniec programu
                    return;

                default:
                    Console.WriteLine("BŁĄD! Wybierz poprawną operację!");
                    break;
            }

        }
    }
}
