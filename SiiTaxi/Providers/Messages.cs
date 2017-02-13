using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiiTaxi.Providers
{
    public class Messages
    {
        public const string NotValidCompanyEmail = "Podałeś niepoprawny adres E-mail!";

        public const string NotValidAltEmail = "Podałeś niepoprawny alternatywny adres E-mail!";

        public const string NotValidCaptcha = "Potwierdź, że nie jeseś robotem!";

        public const string DatabaseError = "Problem z dodaniem przejazdu do bazy! Skontaktuj się z administratorem!";

        public const string AddNewTaxiSuccess = "Pomyślnie dodano przejazd do bazy. Sprawdź swój E-mail by potwierdzić zamówienie.";

        public const string IncludeTaxiSuccess = "Pomyślnie dołączyłeś do przeajzdu. Sprawdź swój E-mail by to potwierdzić.";

        public const string NotValidDate = "Podałeś niepoprawną datę przejazdu!";

        public const string ConfirmFailed = "Nie udało się potwierdzić zamówienia!";

        public const string ConfirmSucceed = "Potwierdziłeś swoje zamówienie!";

        public const string TaxiNotExist = "Taxi nie istnieje!";

        public const string TaxiConfirmed = "Taxi zostało już potwierdzone";

        public const string SendCodeFailed = "Kod nie został wysłany";

        public const string SendCodeSucceed = "Kod został wysłany";
    }

}