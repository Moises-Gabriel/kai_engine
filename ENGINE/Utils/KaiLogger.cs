namespace Kai_Engine.ENGINE.Utils
{
    public static class KaiLogger
    {
        public static void Info(string message, bool spaced)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            if (spaced)
                Console.WriteLine($"\n[KAI ENGINE] {message}\n");
            else
                Console.WriteLine($"[KAI ENGINE] {message}");
            Console.ForegroundColor = ConsoleColor.White;

        }

        public static void Warn(string message, bool spaced)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (spaced)
                Console.WriteLine($"\n[KAI ENGINE] {message}\n");
            else
                Console.WriteLine($"[KAI ENGINE] {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Error(string message, bool spaced)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (spaced)
                Console.WriteLine($"\n[KAI ENGINE] {message}\n");
            else
                Console.WriteLine($"[KAI ENGINE] {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void Important(string message, bool spaced)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            if (spaced)
                Console.WriteLine($"\n[KAI ENGINE] {message}\n");
            else
                Console.WriteLine($"[KAI ENGINE] {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
