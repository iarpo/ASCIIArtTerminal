using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Net;

namespace ASCII_Art_Terminal
{
    class Program
    {
        static Rgba32[,] pixelBuffer;
        static int[,] pixelLightnessBuffer;
        static string[] ASCIIArray;

        static void Main()
        {
            string twitterURL;
            string imageURL;

            do
            {
                // Ask user for twitter image URL
                Console.WriteLine("Give me a Twitter image URL: ");
                twitterURL = Console.ReadLine();

                imageURL = GetImageDataFromTwitter(twitterURL);

            } while (twitterURL == imageURL);


            // check if URL is a valid image
            DownloadImageFromTwitterURLAsync(imageURL);

            // set reference to local files
            string path = "C:\\Users\\sebco\\source\\GitHub\\ASCIIArtTerminal\\bin\\Debug\\netcoreapp3.1\\testImage.jpg";
            string pathResized = "C:\\Users\\sebco\\source\\GitHub\\ASCIIArtTerminal\\bin\\Debug\\netcoreapp3.1\\resizedImage.jpg";

            Image<Rgba32> image = Image.Load<Rgba32>(path, new SixLabors.ImageSharp.Formats.Jpeg.JpegDecoder());
            Image<Rgba32> imageResized = Image.Load<Rgba32>(pathResized, new SixLabors.ImageSharp.Formats.Jpeg.JpegDecoder());

            ConvertImageSizeToFitconsole(image, pathResized, 250);

            MapPixelsToArray(imageResized);

            ConvertRGBAToLightnessValues(pixelBuffer, imageResized);

            ConvertLightnessValueToASCIIChar(pixelLightnessBuffer, imageResized);

            PrintASCIIImage(ASCIIArray, imageResized);


        }


        /// <summary>
        /// Resizes original image to given width
        /// </summary>
        /// <param name="image">Image object</param>
        /// <param name="path">Path of local copy of file</param>
        private static void ConvertImageSizeToFitconsole(Image<Rgba32> image, string path, int filePixelWidth)
        {
            using (image)
            {
                image.Mutate(x => x.Resize(filePixelWidth, 0));

                image.Save(path);
            }
        }

        /// <summary>
        /// Converts Twitter's twimg URL to usable image URL
        /// </summary>
        /// <param name="inputURL"></param>
        /// <returns></returns>
        public static string GetImageDataFromTwitter(string inputURL)
        {

            // is png
            if (inputURL.Contains("?format=png"))
            {
                inputURL = (inputURL.Substring(0, inputURL.IndexOf("?format")) + ".png");
            }
            // is jpg
            else if (inputURL.Contains("?format=jpg"))
            {
                inputURL = (inputURL.Substring(0, inputURL.IndexOf("?format")) + ".jpg");
            }
            // is not valid
            else
            {
                Console.WriteLine("Sorry I can't handle that file type");
            }

            return inputURL;

        }

        /// <summary>
        /// Connects and downloads a copy of the image from the given URL
        /// </summary>
        /// <param name="newImageURL"></param>
        private static void DownloadImageFromTwitterURLAsync(string newImageURL)


        {
            try
            {
                // verify image is loaded successfully from URL
                // saves to C:\Users\sebco\source\GitHub\ASCIIArtTerminal\bin\Debug\netcoreapp3.1
                using (var client = new WebClient())
                {
                    client.DownloadFile(newImageURL, "testImage.jpg");
                }

                // todo this needs changing to load and process IN STREAM

                Console.WriteLine("success!");

            }

            catch (Exception e)
            {
                Console.WriteLine("Image not found");
                Console.WriteLine(e.Message);

            }

        }


        /// <summary>
        /// Create a 2D array of pixel RGBA32 values for the given image input
        /// </summary>
        private static void MapPixelsToArray(Image<Rgba32> imageInput)
        {

            pixelBuffer = new Rgba32[imageInput.Width, imageInput.Height];


            for (int yHeight = 0; yHeight < imageInput.Height; yHeight++)
            {
                Span<Rgba32> pixelRowSpan = imageInput.GetPixelRowSpan(yHeight);
                for (int xWidth = 0; xWidth < imageInput.Width; xWidth++)
                {
                    // get pixel data as a tuple (no need for a 3D array as tuple is self contained)
                    pixelBuffer[xWidth, yHeight] = pixelRowSpan[xWidth];
                    //  Console.WriteLine($"Value of pixel Y {yHeight}, X {xWidth} is {pixelBuffer[xWidth, yHeight]}");
                }
            }
        }

        /// <summary>
        /// Converts an array's RGB values into int lightness values
        /// </summary>
        /// <param name="pixelBuffer"></param>
        private static void ConvertRGBAToLightnessValues(Rgba32[,] pixelBuffer, Image<Rgba32> imageInput)
        {
            pixelLightnessBuffer = new int[imageInput.Width, imageInput.Height];

            for (int y = 0; y < imageInput.Height; y++)
            {
                for (int x = 0; x < imageInput.Width; x++)
                {
                    pixelLightnessBuffer[x, y] = (pixelBuffer[x, y].R + pixelBuffer[x, y].G + pixelBuffer[x, y].B) / 3;
                }
            }

        }

        private static void ConvertLightnessValueToASCIIChar(int[,] pixelLightnessBuffer, Image<Rgba32> imageInput)
        {
            char[] charRangeArray = "`^\":;I!i~_-?[}{)(|\\/tfrxvczYUJLQ0Zmwpdbhao#MW&8%B@$456".ToCharArray();
            ASCIIArray = new string[imageInput.Height];

            for (int y = 0; y < imageInput.Height; y++)
            {
                string xLine = null;
                for (int x = 0; x < imageInput.Width; x++)
                {
                    var i = pixelLightnessBuffer[x, y];
                    xLine = $"{xLine}{charRangeArray[(i - (i % 5)) / 5]}";
                }
                ASCIIArray[y] = xLine;
            }
        }

        /// <summary>
        /// Print ASCII array to the console
        /// </summary>
        /// <param name="arrayInput"></param>
        private static void PrintASCIIImage(string[] arrayInput, Image<Rgba32> imageInput)
        {
            for (int i = 0; i < imageInput.Height; i++)
            {
                Console.WriteLine(arrayInput[i]);
            }
        }
    }
}
