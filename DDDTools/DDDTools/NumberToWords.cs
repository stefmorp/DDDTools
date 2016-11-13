using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDTools
{

    class NumberToWords
    {
        private Dictionary<int, string> Words = new Dictionary<int, string>();
        private int value;
        private int tens;
        private int hundreds;
        private int thousands;
        private int millions;

        public NumberToWords(int val)
            {

            Words.Add(0, "zero");
            Words.Add(1, "uno");
            Words.Add(2, "due");
            Words.Add(3, "tre");
            Words.Add(4, "quattro");
            Words.Add(5, "cinque");
            Words.Add(6, "sei");
            Words.Add(7, "sette");
            Words.Add(8, "otto");
            Words.Add(9, "nove");
            Words.Add(10, "dieci");
            Words.Add(11, "undici");
            Words.Add(12, "dodici");
            Words.Add(13, "tredici");
            Words.Add(14, "quattordici");
            Words.Add(15, "quindici");
            Words.Add(16, "sedici");
            Words.Add(17, "diciassette");
            Words.Add(18, "diciotto");
            Words.Add(19, "diciannove");
            Words.Add(20, "venti");
            Words.Add(21, "ventuno");
            Words.Add(22, "ventidue");
            Words.Add(23, "ventitré");
            Words.Add(24, "ventiquattro");
            Words.Add(25, "venticinque");
            Words.Add(26, "ventisei");
            Words.Add(27, "ventisette");
            Words.Add(28, "ventotto");
            Words.Add(29, "ventinove");
            Words.Add(30, "trenta");
            Words.Add(31, "trentuno");
            Words.Add(32, "trentadue");
            Words.Add(33, "trentatré");
            Words.Add(34, "trentaquattro");
            Words.Add(35, "trentacinque");
            Words.Add(36, "trentasei");
            Words.Add(37, "trentasette");
            Words.Add(38, "trentotto");
            Words.Add(39, "trentanove");
            Words.Add(40, "quaranta");
            Words.Add(41, "quarantuno");
            Words.Add(42, "quarantadue");
            Words.Add(43, "quarantatré");
            Words.Add(44, "quarantaquattro");
            Words.Add(45, "quarantacinque");
            Words.Add(46, "quarantasei");
            Words.Add(47, "quarantasette");
            Words.Add(48, "quarantotto");
            Words.Add(49, "quarantanove");
            Words.Add(50, "cinquanta");
            Words.Add(51, "cinquantuno");
            Words.Add(52, "cinquantadue");
            Words.Add(53, "cinquantatré");
            Words.Add(54, "cinquantaquattro");
            Words.Add(55, "cinquantacinque");
            Words.Add(56, "cinquantasei");
            Words.Add(57, "cinquantasette");
            Words.Add(58, "cinquantotto");
            Words.Add(59, "cinquantanove");
            Words.Add(60, "sessanta");
            Words.Add(61, "sessantuno");
            Words.Add(62, "sessantadue");
            Words.Add(63, "sessantatré");
            Words.Add(64, "sessantaquattro");
            Words.Add(65, "sessantacinque");
            Words.Add(66, "sessantasei");
            Words.Add(67, "sessantasette");
            Words.Add(68, "sessantotto");
            Words.Add(69, "sessantanove");
            Words.Add(70, "settanta");
            Words.Add(71, "settantuno");
            Words.Add(72, "settantadue");
            Words.Add(73, "settantatré");
            Words.Add(74, "settantaquattro");
            Words.Add(75, "settantacinque");
            Words.Add(76, "settantasei");
            Words.Add(77, "settantasette");
            Words.Add(78, "settantotto");
            Words.Add(79, "settantanove");
            Words.Add(80, "ottanta");
            Words.Add(81, "ottantuno");
            Words.Add(82, "ottantadue");
            Words.Add(83, "ottantatré");
            Words.Add(84, "ottantaquattro");
            Words.Add(85, "ottantacinque");
            Words.Add(86, "ottantasei");
            Words.Add(87, "ottantasette");
            Words.Add(88, "ottantotto");
            Words.Add(89, "ottantanove");
            Words.Add(90, "novanta");
            Words.Add(91, "novantuno");
            Words.Add(92, "novantadue");
            Words.Add(93, "novantatré");
            Words.Add(94, "novantaquattro");
            Words.Add(95, "novantacinque");
            Words.Add(96, "novantasei");
            Words.Add(97, "novantasette");
            Words.Add(98, "novantotto");
            Words.Add(99, "novantanove");
            Words.Add(100, "cento");




            value = val;
            tens = val % 100;
            hundreds = (int)(Math.Floor((decimal)val / 100) % 10);
            thousands = (int)(Math.Floor((decimal)val / 1000) % 1000);
            millions = (int)(Math.Floor((decimal)val / 1000000) % 1000000);
            Console.Write("Value: ");
            Console.WriteLine(value);
            Console.Write("Tens: ");
            Console.Write(Words[tens] + " " );
            Console.WriteLine(tens);
            Console.Write("Hundreds: ");
            Console.Write(Words[hundreds] + " ");
            Console.WriteLine(hundreds);
            Console.Write("Thousands: ");
            Console.Write(Words[thousands] + " ");
            Console.WriteLine(thousands);
            Console.Write("Millions: ");
            Console.Write(Words[millions] + " ");
            Console.WriteLine(millions);


        }

        public string GetString(string decimal_part)
        {
            string result = "";

            switch (Words[millions])
            {
                case "zero":
                    break;
                case "uno":
                    result = result + "Un milione e ";
                    break;
                default:
                    result = result + Words[millions] + " milioni e ";
                    break;
            }
            switch (Words[thousands])
            {
                case "zero":
                    break;
                case "uno":
                    result = result + "mille";
                    break;
                default:
                    result = result + Words[thousands] + "mila";
                    break;
            }
            switch (Words[hundreds])
            {
                case "zero":
                    break;
                case "uno":
                    result = result + "cento";
                    break;
                default:
                    result = result + Words[hundreds] + "cento";
                    break;
            }
            switch (Words[tens])
            {
                case "zero":
                    break;
                default:
                    result = result + Words[tens];
                    break;
            }

            return result + " / " + decimal_part;
        }


    }
}
