using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;


namespace ASCII_Art_Terminal
{
    class Program
    {

        static Image<Rgba32> image; //todo Find out why these need to be static
        static bool imagePathIsValid = false;
        static string filePath;
        static readonly string folderPath = @"C:\Users\sebco\source\repos\AdvancedBeginner\ASCIIArtTerminal\images\";
        static Rgba32[,] pixelBuffer;
        static int[,] pixelLightnessBuffer;


        static void Main()
        {
            filePath = GetFilePath(folderPath);
            while (!imagePathIsValid)
            {
                TryLoadImage();
            }
            PixelsToArray(image);
            AccessImagePixels();
            ConvertRGBAToLightnessValues(pixelBuffer);
            ConvertLightnessValueToASCIIChar(pixelLightnessBuffer);
        }

        private static void ConvertLightnessValueToASCIIChar(int[,] pixelLightnessBuffer)
        {
            string charRange = "`^\",:;Il!i~+_-?][}{1)(|\\/tfjrxnuvczXYUJCLQ0OZmwqpdbkhao*#MW&8%B@$";
        }



        /// <summary>
        /// Asks the user for the file name, including extension
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        static string GetFilePath(string folderPath)
        {
            Console.WriteLine($"Enter full file name of image to be edited in {folderPath}");
            string file = Console.ReadLine();
            string filePath = folderPath + file;
            return filePath;
        }

        /// <summary>
        /// Try load image from user entered path
        /// </summary>
        static void TryLoadImage() //Todo Make this method private and setup proper parameter inputs
        {
            try
            {
                image = Image.Load<Rgba32>(filePath);
                Console.WriteLine($"Correct image loaded: {filePath}");
                Console.WriteLine($"Width:   {image.Width}, Height: {image.Height}");
                image.Save(RemoveFileExtension(filePath) + ".jpg"); //todo This doesn't save a new file
                imagePathIsValid = true;
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine("Image not found");
                GetFilePath(folderPath);
            }

        }

        /// <summary>
        /// Removes a path's files extension
        /// </summary>
        /// <param name="path"></param>
        /// <returns>string minues any file extension</returns>
        private static string RemoveFileExtension(string path)
        {
            int extensionStart = path.LastIndexOf('.');
            return path.Substring(0, extensionStart);
        }

        /// <summary>
        /// Create a 2D array of pixel RGBA32 values for the given image input
        /// </summary>
        private static void PixelsToArray(Image<Rgba32> imageInput)
        {

            pixelBuffer = new Rgba32[image.Width, image.Height];


            for (int yHeight = 0; yHeight < image.Height; yHeight++)
            {
                Span<Rgba32> pixelRowSpan = image.GetPixelRowSpan(yHeight);
                for (int xWidth = 0; xWidth < image.Width; xWidth++)
                {
                    // get pixel data as a tuple (no need for a 3D array as tuple is self contained)
                    pixelBuffer[xWidth, yHeight] = pixelRowSpan[xWidth];
                    //  Console.WriteLine($"Value of pixel Y {yHeight}, X {xWidth} is {pixelBuffer[xWidth, yHeight]}");
                }
            }
        }

        /// <summary>
        /// Print a pixel's RGBA values from co-ordinates entered by the user.
        /// </summary>
        private static void AccessImagePixels()
        {
            Console.WriteLine("Access pixel values here.\n");

            Console.WriteLine($"Choose the pixels X co-ordinate from range {image.Width}");
            int xCoord = int.Parse(Console.ReadLine()); //todo add exception handling for invalid entries

            Console.WriteLine($"Choose the pixels Y co-ordinate from range {image.Height}");
            int yCoord = int.Parse(Console.ReadLine()); //todo add exception handling for invalid entries

            Console.WriteLine($"RGBA values for pixel {xCoord}, {yCoord}: {pixelBuffer[xCoord, yCoord]}");
        }

        /// <summary>
        /// Converts an array's RGB values into int lightness values
        /// </summary>
        /// <param name="pixelBuffer"></param>
        private static void ConvertRGBAToLightnessValues(Rgba32[,] pixelBuffer)
        {
            pixelLightnessBuffer = new int[image.Width, image.Height];

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    pixelLightnessBuffer[x, y] = (pixelBuffer[x, y].R + pixelBuffer[x, y].G + pixelBuffer[x, y].B) / 3;
                }
            }

        }
    }
}
