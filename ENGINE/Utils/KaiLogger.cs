namespace Kai_Engine.ENGINE.Utils
{
    public static class KaiLogger
    {
        public static void Info(string location, string message, bool spaced)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            if (spaced)
                Console.WriteLine($"\n[KAI ENGINE] |{location}| {message}\n");
            else
                Console.WriteLine($"[KAI ENGINE] |{location}| {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Warn(string location, string message, bool spaced)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (spaced)
                Console.WriteLine($"\n[KAI ENGINE] |{location}| {message}\n");
            else
                Console.WriteLine($"[KAI ENGINE] |{location}| {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Error(string location, string message, bool spaced)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (spaced)
                Console.WriteLine($"\n[KAI ENGINE] |{location}| {message}\n");
            else
                Console.WriteLine($"[KAI ENGINE] |{location}| {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Important(string location, string message, bool spaced)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            if (spaced)
                Console.WriteLine($"\n[KAI ENGINE] |{location}| {message}\n");
            else
                Console.WriteLine($"[KAI ENGINE] |{location}| {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
