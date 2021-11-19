using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HBCodeChallange
{
    interface IRectangular
    {
        int maxXPosition { get; }
        int maxYPositoin { get; }
    }

    class Rectangular : IRectangular
    {
        public int maxXPosition { get; }

        public int maxYPositoin { get; }

        public Rectangular(int maxXPosition, int maxYPosition)
        {
            this.maxXPosition = maxXPosition;
            this.maxYPositoin = maxYPositoin;
        }
    }

    interface IRover
    {
        int XPosition { get; set; }
        int YPosition { get; set; }
        string CompassDirection { get; set; }
        char[] RoverTripInputs { get; }
    }

    class Rover : IRover
    {
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public string CompassDirection { get; set; }
        public char[] RoverTripInputs { get; }

        public Rover(char[] roverTripInputs)
        {
            this.RoverTripInputs = roverTripInputs;
        }
    }

    class Program
    {
        static Rectangular rectangular;

        static List<IRover> rovers = new List<IRover>();


        static void doWork(string[] inputs)
        {

            try
            {
                //İlk girdi platonun boyutunu temsil eder, daha sonraki her 2 satır bir rover'a ait bilgiyi temsil eder
                //Bu sebeple ilk girdiyi hesaba katmazsak girdi sayısının 2'şer satırlık bilgi kümesinden oluşması gerekir
                //Bu koşul sağlanmazsa giriş dizesi hatalıdır
                //Giriş dizesi doğru ise x,y ikilisine göre platform nesnesi oluşturulur
                if (inputs.Length % 2 != 1) throw new Exception("Giriş dizesi hatalı");
                else
                {
                    string[] rectSize = inputs[0].Split(' ');
                    int rectMaxXPositon = Convert.ToInt32(rectSize[0]);
                    int rectMaxYPositon = Convert.ToInt32(rectSize[1]);
                    rectangular = new Rectangular(rectMaxXPositon, rectMaxYPositon);

                    //İlk satır atlanarak her bir ikişer satırlık veri Rover nesnesi için bilgieri taşır
                    //Bu bilgiler ile Rover nesnesi oluşturulur.
                    //Oluşan her bir Rover nesnesi platform üzerinde kaç adet araç bulunduğunu tutan listeye aktarılır.
                    for (int index = 1; index < inputs.Length; index = index + 2)
                    {
                        string[] roverPositons = inputs[index].Split(' ');
                        char[] tripInputs = inputs[index + 1].ToCharArray();

                        IRover rover = new Rover(tripInputs)
                        {
                            XPosition = Convert.ToInt32(roverPositons[0]),
                            YPosition = Convert.ToInt32(roverPositons[1]),
                            CompassDirection = roverPositons[2]
                        };
                        rovers.Add(rover);
                    }

                    //Her bir Rover nesnesi için konum ve gezi hesaplamaları yapılarak çıktı ekrana yazdırılır.
                    rovers.ForEach(rover => calculateRoverLastTripPosition(rover));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void calculateRoverLastTripPosition(IRover rover)
        {
            int xPosition = rover.XPosition;
            int yPosition = rover.YPosition;
            string compassDirection = rover.CompassDirection;

            for (int index = 0; index < rover.RoverTripInputs.Length; index++)
            {
                switch (rover.RoverTripInputs[index])
                {
                    case 'L':
                        if (compassDirection == "N") compassDirection = "W";
                        else if (compassDirection == "W") compassDirection = "S";
                        else if (compassDirection == "S") compassDirection = "E";
                        else if (compassDirection == "E") compassDirection = "N";
                        break;
                    case 'R':
                        if (compassDirection == "N") compassDirection = "E";
                        else if (compassDirection == "E") compassDirection = "S";
                        else if (compassDirection == "S") compassDirection = "W";
                        else if (compassDirection == "W") compassDirection = "N";
                        break;
                    case 'M':
                        if (compassDirection == "N") yPosition++;
                        else if (compassDirection == "W") xPosition--;
                        else if (compassDirection == "S") yPosition--;
                        else if (compassDirection == "E") xPosition++;
                        break;
                }
            }

            Console.WriteLine($"{xPosition} {yPosition} {compassDirection}");
        }

        static void Main(string[] args)
        {
            string[] inputs = new string[] { "5 5", "1 2 N", "LMLMLMLMM", "3 3 E", "MMRMMRMRRM" };

            doWork(inputs);
        }
    }
}
