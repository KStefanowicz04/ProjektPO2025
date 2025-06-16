# Projekt - Wypożyczalnia rowerów miejskich

## Używanie programu

Po uruchomieniu programu wyświetli się menu główne z następującymi opcjami:
* Zaloguj się:<br>
  Aby wykonać tę operację należy kliknąć 1 oraz enter.<br>
  Aby zalogować się na konto, należy w kolejnych liniach podać imię, nazwisko, numer telefonu oraz adres e-mail.<br>
  Aby zalogować się na konto, należy najpierw się zarejestrować.
* Wyloguj się:<br>
  Aby wykonać tę operację należy kliknąć 2 oraz enter.<br>
  Aby wylogować się z konta, należy być na nie zalogowany.<br>
* Zarejestruj się:<br>
  Aby wykonać tę operację należy kliknąć 3 oraz enter.
  Aby zarejestrować konto, należy w kolejnych liniach podać imię, nazwisko, numer telefonu oraz adres e-mail.<br>
  Numer telefonu powinien składać się z 9 znaków.<br>
  Adres e-mail powinien zawierać znak '@'.<br>
  W ostatniej linii można również wpłacić pieniądze na konto już podczas rejestracji - należy podać kwotę w formacie zz.gg gdzie zz oznacza złote a gg grosze, którą chce się zasilić konto. Na koniec należy kliknąć enter.<br>
  Każda osoba może posiadać kilka kont, lecz każde konto powinno zostać zarejestrowane przy użyciu innego adresu e-mail oraz innego numeru telefonu.<br>
* Wypożycz rower:<br>
  Aby wykonać tę operację należy kliknąć 4 oraz enter.<br>
  Aby wypożyczyć rower, zalogowany użytkownik powinien w kolejnych liniach podać numer ID roweru, nazwę stacji początkowej wypożyczenia oraz datę i godzinę wypożyczenia w formacie DateTime, tj. w formacie MM/DD/YYYY/ hh:mm:ss, gdzie MM oznacza miesiąc, DD dzień, YYYY rok, hh godzinę, mm minutę, a  ss sekundę. 
  Nazwy stacji oraz numery ID rowerów można odnaleźć korzystając z opcji Lista wypożyczlani rowerów lub Lista rowerów  w danej wypożyczalni.
* Historia wypożyczeń użytkownika:<br>
  Aby wykonać tę operację należy kliknąć 5 oraz enter.<br>
  Zalogowany użytkownik uzyska informacje o historii zakończonych wypożyczeń - stacji początkowej i stacji końcowej, czasie wypożyczenia oraz ID roweru.<br>
* Lista wypożyczalni rowerów:<br>
  Aby wykonać tę operację należy kliknąć 6 oraz enter.<br>
  Uzyskujemy informacje o nazwach stacji rowerowych, liczbie miejsc na rowery na stacjach, liczbie miejsc wolnych na rowery na stacjach oraz ID rowerów i typy rowerów znajdujących się na stacjach.<br>
* Lista rowerów w danej wypożyczalni:<br>
  Aby wykonać tę operację należy kliknąć 7 oraz enter.<br>
  Uzyskujemy informacje o nazwie stacji rowerowej, liczbie miejsc na rowery na stacji, liczbie miejsc wolnych na rowery na stacji oraz ID rowerów i typy rowerów znajdujących się na stacji.<br>
* Zwróć rower:<br>
  Aby wykonać tę operację należy kliknąć 8 oraz enter.<br>
  Aby zwrócić rower, zalogowany użytkownik powinien w kolejnych liniach podać numer ID roweru, nazwę stacji końcowej wypożyczenia oraz datę i godzinę końca wypożyczenia w formacie DateTime tj. w formacie MM/DD/YYYY/ hh:mm:ss, gdzie MM oznacza miesiąc, DD dzień, YYYY rok, hh godzinę, mm minutę, a ss sekundę.<br>
* Dodaj pieniądze na konto:<br>
  Aby wykonać tę operację należy kliknąć 9 oraz enter.<br>
  Zalogowany użytkownik podaje kwotę w formacie zz.gg gdzie zz oznacza złote a gg grosze, którą chce zasilić konto. Na koniec należy kliknąć enter.<br>
* Aktywne wypożyczenia:<br>
  Aby wykonać tę operację należy kliknąć 10 oraz enter.<br>
  Klikając 10 oraz enter zalogowany użytkownik uzyska informacje o aktwynych wypożyczeń - stacji początkowej, czasie wypożyczenia oraz ID roweru.<br>
* Zakończ program:<br>
  Aby wykonać tę operację należy kliknąć 11 oraz enter.<br>
  Operacja ta zatrzymuje działanie programu.<br>


## Informacje o klasach dla programistów

* Klasa abstrakcyjna Bike:<br>
Jest to klasa na której podstawie stworzone zostały klasy pochodne podane niżej. Rower może zostać wypożyczony przez każdego zalogowanego użytkownika.<br>
Składa się z pól:
  - int id - (ma setter) unikalne ID roweru, używane przy dodawaniu i zabieraniu rowera ze stacji oraz przy tworzeniu obiektu wypożyczenia
  - double price - (ma setter) - bazowa cena wypożyczenia roweru
  - static HashSet\<int> takenIDs - statyczny HashSet wszystkich ID zajętych przez rowery
  - static Random rand - pole używane przy losowaniu ID

  Ma metody:
  - konstruktor Bike - generuje ID, a jeśli jest unikalne zapisuje je do takenIDs
  - ToString - zwraca ID roweru oraz jego klasę<br>

  Uwaga: podczas generowania ID w konstruktorze nowa wartość jest porównywana z każdym zapisanym ID za pomocą pętli while. To rozwiązanie jest nieoptymalne i staje się bardzo wolne przy bardzo dużej liczbie rowerów.

* Klasy RegularBike, ChildBike, TandemBike:<br>
są pochodnymi klasy abstrakcyjnej Bike. Te klasy nie różnią się zasadniczo od klasy Bike ani siebie nawzajem.

* Klasa BikeStation:<br>
Stacja rowerowa; zawiera listę wszystkich dostępnych rowerów. Rowery mogą zostać oddane i zabrane ze stacji przez użytkownika. Oddać rower można tylko gdy jest dostępne miejsce na stacji, wypożyczyć rower można tylko gdy na stacji jest rower do wypożyczenia.<br>
BikeStation jest w relacji kompozycji z klasą Bike.<br>
Składa się z pól:
  - string name - nazwa danej stacji
  - List\<Bike> allBikes - lista rowerów
  - int totalSlots - ilość wszystkich dostępnych miejsc na rowery
  - int freeSlots - ma getter; ilość wolnych miejsc na rowery

  Ma metody:
  - konstruktor BikeStation - przyjmuje nazwę i łączną ilość miejsc stacji
  - AddBike - dodaje dany rower do list dostępnych rowerów
  - RemoveBike - usuwa dany rower z listy rowerów
  - GetRegular, GetChild, GetTandem - zwracją ilość rowerów danego typu
  - GetName - zwraca nazwę stacji
  - GetStationBikes - zwraca informacje o rowerach dostępnych na danej stacji korzystając z powyższych trzech metod
  - ToString - zwraca informacje o stacji - nazwę, ilość miejsc, ilość wolnych miejsc

* Klasa StationsAggregation:<br>
Zawiera wszystkie stacje rowerowe. Używana do łatwego wypisywania informacji o stacjach.<br>
StationsAggregation jest w relacji agregacji z klasą BikeStation.<br>
Składa się z pola:
  - List\<BikeStation> stations - lista stacji

  Ma metody:
  - konstruktor StationsAggregation - jest pusty
  - BikeStation getStation - zwraca daną stację, jeśli dana stacja znajduje się na liście
  - AddStation - dodaje daną stację do listy
  - RemoveStation - usuwa daną stację z listy, jeśli występuje w liście
  - GetStationsInfo - wypisuje nazwę każdej stacji w liście oraz korzysta z metody GetStationBikes() danej stacji

* Klasa BikeRent:<br>
Klasa reprezentująca wypożyczenie roweru przez użytkownika. Zapisuje dane o godzinie i dacie oraz stacjach oddania i wypożyczenia roweru, koszcie wypożyczenia roweru.<br>
Składa się z pól (wszystkie mają gettery):
  - Bike bicycle - wypożyczany rower
  - DateTime startTime - czas wypożyczenia
  - DateTime endTime - (ma setter) czas zwórcenia roweru
  - BikeStation startStation - stacja z której wypożyczony został rower
  - BikeStation endStation - (ma setter) stacja do której rower oddany został
  - double cost - koszt wypożyczenia roweru

  Ma metody:
  - konstruktor BikeRent - przyjmuje rower, czas wypożyczenia, stację początkową
  - SetCost - ustawia koszt wypożyczenia roweru zależnie od czasu wypożyczenia
  - SetEndTime - setter endTime
  - SetEndStation - setter endStation

* Klasa User:<br>
Klasa repezentująca użytkownika. Użytkownik może wypożyczyć i oddać rower do stacji, wyświetlić obecne wypożyczenia i historię wypożyczeń.<br>
Składa się z pól:
  - string imie - imię użytkownika
  - string nazwisko - nazwisko użytkownika
  - string numerTel - numer telefonu użytkownika, musi być 9-cyfrowy
  - string mail - adres e-mail użytkownika, musi zawierać znak "@"
  - double pieniadze - pieniądze na koncie użytkownika
  - List\<BikeRent> rentActive - lista obecnie aktywnych wypożyczeń rowerów
  - List\<BikeRent> rentHistory - lista wypożyczeń rowerów z przeszłości

  Ma metody:
  - konstruktor User - przyjmuje imię, nazwisko, numer telefonu, e-mail, ilość pieniędzy
  - GetImie - zwraca imię użytkownika
  - GetNazwisko - zwraca nazwisko użytkownika
  - GetNumerTel - zwraca numer telefonu użytkownika
  - GetMail - zwraca e-mail użytkownika
  - Rent - przyjmuje ID wypożyczanego roweru, stację na której jest rower, czas wypożyczenia; usuwa dany rower ze stacji, dodaje BikeRent do listy obecnie aktywnych rentActive
  - ReturnBike - przyjmuje ID zwracanego roweru, nazwę stacji, czas oddania roweru; oddaje rower do danej stacji, dodaje BikeRent do listy historii wypożyczeń rentHistory
  - Deposit - dodaje pieniądze do konta jeśli kwota jest poprawna (>0)
  - DisplayRentActive - wyświetla listę aktywnych wypożyczeń w konsoli
  - DisplayRentHistory - wyświetla listę wypożyczeń z przeszłości w konsoli

* Klasa UserManager:<br>
Zawiera listę użytkowników, stosowana do rejestracji, logowania, wylogowywania użytkowników.
  Składa się z pól:
  - List\<User> users - list użytkowników
  - User currentUser - obecnie zalogowany użytkownik
  
  Ma metody:
  - konstruktor UserManager - jest pusty
  - Register - przyjmuje imię, nazwisko, numer telefonu, e-mail, ilość pieniędzy; należy zarejestrować się żeby móc zalogować się i korzystać z niżej opisanych opcji
  - Login - przyjmuje imię, nazwisko, numer telefonu, e-mail; daje dostęp do wypożyczania i zwracania rowerów, wylogowywania, list obecnych i przeszłych wypożyczeń, dodania pieniędzy na konto
  - Logout - wylogowuje obecnego użytkownika
  - GetCurrentUser - zwraca obecnego użytkownika

  
## Informacje o wyjątkach

* Wyjątek DuplicateUserException:<br>
Przy rejestracji - "Użytkownik już istnieje!"
* Wyjątek WrongPhoneNumberException<br>:
Przy rejestracji - "Numer telefonu jest niepoprawny!"
* Wyjątek WrongmailException<br>:
Przy rejestracji - "E-mail jest niepoprawny!"
* Wyjątek WrongMoneyException<br>:
Przy rejestracji - "Niepoprawna ilość pieniędzy!"
* Wyjątek SessionConflictException<br>:
Przy logowaniu - "Użytkownik już jest zalogowany!"
* Wyjątek UserNotFoundException<br>:
Przy logowaniu - "Nie znaleziono użytkownika!"
* Wyjątek AuthenticationRequiredException<br>:
Przy logowaniu - "Użytkownik musi być zalogowany żeby wykonać operację!"
* Wyjątek WrongStationException<br>:
Przy wypożyczaniu - "Dana stacja nie istnieje!"
* Wyjątek NoFundsException<br>:
Przy wypożyczaniu - "Brak środków na koncie!"
* Wyjątek WrongDateFormatException<br>:
Błąd DateTime - "Niepoprawny format daty!"
* Wyjątek WrongDatBikeNotFoundExceptioneFormatException<br>:
Błąd Bike - "Dany rower nie istnieje!"
* Wyjątek RentNotFoundException<br>:
Błąd Rent - "Nie znaleziono wypożyczenia!"
* Wyjątek NotEnoughFreeSpaceException<br>:
Błąd BikeStation - "Brak miejsc na stacji!"