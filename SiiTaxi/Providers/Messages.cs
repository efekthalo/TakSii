using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiiTaxi.Providers
{
    public class Messages
    {
        public const string NotValidCompanyEmail = "Podałeś niepoprawny adres E-mail!";

        public const string NotValidPhone = "Podałeś niepoprawny numer telefonu!";

        public const string NotValidAltEmail = "Podałeś niepoprawny alternatywny adres E-mail!";

        public const string NotValidCaptcha = "Potwierdź, że nie jeseś robotem!";

        public const string DatabaseError = "Problem z dodaniem przejazdu do bazy! Skontaktuj się z administratorem!";

        public const string AddNewTaxiSuccess = "Pomyślnie dodano przejazd do bazy. Sprawdź swój E-mail by potwierdzić zamówienie.";

        public const string IncludeTaxiSuccess = "Pomyślnie dołączyłeś do przejazdu!";

        public const string NotValidDate = "Podałeś niepoprawną datę przejazdu!";

        public const string ConfirmFailed = "Nie udało się potwierdzić zamówienia!";

        public const string ConfirmSucceed = "Potwierdziłeś swoje zamówienie!";

        public const string TaxiConfirmed = "Taxi zostało już potwierdzone.";

        public const string SendCodeFailed = "Kod nie został wysłany.";

        public const string SendCodeSucceed = "Kod został wysłany.";

        public const string UpdateApproverSucceed = "Pomyślnie zaaktualizwano dane Approvera.";

        public const string UpdateApproverFailed = "Problem z aktualizacją danych Approvera.";

        public const string AddNewApproverSucceed = "Pomyślnie dodano nowego Approvera.";

        public const string AddNewApproverFailed = "Problem z dodaniem nowego Approvera.";

        public const string ApproverNotFound = "Nie znaleziono żądanego Approvera!";

        public const string TaxiNotFound = "Nie znaleziono takiego przejazdu!";

        public const string TaxiFull = "Taxi jest już pełne!";

        public const string JoinedAlready = "Już jesteś pasażerem tego Taxi!";

    }

}