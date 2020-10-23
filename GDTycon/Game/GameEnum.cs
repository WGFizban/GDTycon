namespace GDTycon.Game
{
    /// <summary>
    /// Klasa statyczna zawierająca enumy i ważne stałe
    /// </summary>
    public static class GameEnum
    {
        /// <summary>
        /// stałe 6 rodzajów technologii w projektach
        /// </summary>
        public static string[] technology = { "front-end", "backend", "baza danych", "mobile", "wordpress", "prestashop" };

        /// <summary>
        /// minimalna ilośc dni by znaleźć nowy projekt
        /// </summary>
        public static readonly int MIN_SEARCHING_DAYS = 5;

        /// <summary>
        /// enum z poziomem trudności projektu
        /// </summary>
        public enum ProjectComplexity
        {
            Łatwy, Średni, Trudny
        }

        /// <summary>
        /// enum z rodzajami zatrudnia pracowników
        /// </summary>
        public enum Occupation
        {
            Tester, Sprzedawca, Programista
        }

        /// <summary>
        /// enum z charakterem klientów
        /// </summary>
        public enum ClientCharacter
        {
            Luzak, Wymagający, Skrwl
        }
    }
}