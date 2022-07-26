using TextCopy;

namespace PasswordGenerator {
    public class Program {

        public static void Main(string[] args)
        {
            int width = 10;
            bool maj, min, symbol;
            maj = min = symbol = true;

            (string, Type)[] acceptedArgs = new (string, Type)[] {
                ("width", typeof(int)),
                ("w", typeof(int)),
                ("maj", typeof(bool)),
                ("min", typeof(bool)),
                ("symbol", typeof(bool)),
                ("s", typeof(bool))
            };

            foreach (string arg in args) {
                string param = arg;
                string value = "";

                if (arg.Contains(":"))
                    (param, value) = (arg.Split(":")[0], arg.Split(":")[1]);
                else
                    value = Convert.ToString(true); // value = true if not provided

                Type? type = acceptedArgs.FindType(param);
                var parsedValue = TryParse(type, value);

                // check if parameter is correct
                if (string.IsNullOrEmpty(param) || type == null || value == null) {
                    Console.Error.WriteLine("You provided wrong parameters !");
                    return;
                }

                // set attribute
                switch (param.ToUpper()) {
                    case "MAJ":
                        maj = (bool) parsedValue;
                        break;
                    case "MIN":
                        min = (bool) parsedValue;
                        break;
                    case "WIDTH":
                    case "W":
                        width = (int) parsedValue;
                        break;
                    case "SYMBOL":
                    case "S":
                        symbol = (bool) parsedValue;
                        break;           
                }
            }

            new Generator(width, maj, min, symbol);
        }
    
        // tryParse function for each accepted type
        public static dynamic? TryParse(Type t, string value) {
            int intValue = -1;
            bool boolValue = false;

            var res = t.Name switch {
                "Boolean" => bool.TryParse(value, out boolValue),
                "Int32" => int.TryParse(value, out intValue),
                _ => false
            };

            return res ? intValue != -1 ? intValue : boolValue : null;
        }
    }

    public static class Extensions 
    {
        public static Type? FindType(this (string, Type)[] list, string item)
        {
            foreach ((string e, Type v) in list) {
                if (e == item)
                    return v;
            }

            return null;
        }
    }

    // PasswordGenerator class
    public class Generator {
        private int numberOfSymbol = 2; // default value for width = 10 (20% of width); 
        private char[] SYMBOL_LIST = new char[5]{ '#', '%', '*', '$', '&' };
        private char[] LETTER_UPPER = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        private char[] LETTER_LOWER = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        private int[] NUMBER_LIST = new int[10]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }; 
        private int WIDTH = 10;
        private bool MAJ = true;
        private bool MIN = true;
        private bool SYMBOL = true;

        public Generator(int width, bool maj, bool min, bool symbol) {
            this.WIDTH = width;
            this.MAJ = maj;
            this.MIN = min;
            this.SYMBOL = symbol;
            this.numberOfSymbol = symbol ? 20 * width / 100 : 0; 

            if (CheckingError())
                return;

            string password = Run();
            Console.WriteLine(password);

            TextCopy.ClipboardService.SetText(password);
        }

        private string Run() {
            List<int> positionOfSymbol = new List<int>();
            fillSymbolPosition(ref positionOfSymbol);

            Random rand = new Random();

            List<char> result = new List<char>();
            for (int i = 0 ; i < this.WIDTH; i++) {
                if (positionOfSymbol.Contains(i)) {
                    // insert symbol
                    result.Add(SYMBOL_LIST[rand.Next(5)]);
                }else {
                    // MAJ or MIN
                    char letter;
                    int rank = new Random().Next(26);

                    if (MAJ && MIN)
                        letter = Convert.ToBoolean(new Random().Next(0,2)) ? LETTER_UPPER[rank] : LETTER_LOWER[rank];
                    else 
                        letter = MAJ ? LETTER_UPPER[rank] : LETTER_LOWER[rank];
                    
                    result.Add(letter);
                }
            }

            return string.Join("", result);
        }

        private List<int> fillSymbolPosition(ref List<int> positionOfSymbol) {
            Random rand = new Random();
            for (int i = 0; i < this.numberOfSymbol; i++)
                positionOfSymbol.Add(rand.Next(this.WIDTH));

            return positionOfSymbol;
        }

        private bool CheckingError() {
            if (this.WIDTH == 0)
                Console.Error.WriteLine("Width must be greater than 0");
            else if (this.MAJ == false && this.MIN == false)
                Console.Error.WriteLine("Maj & Min can't be both false");
            else return false;

            return true;
        }
    }
} 

