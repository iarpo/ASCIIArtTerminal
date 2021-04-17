using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ASCII_Art_Terminal
{
    class Program
    {

        static Image image; //todo Find out why these need to be static
        static bool imagePathIsValid = false;
        static string filePath;
        static string folderPath = @"D:\Repos\AdvancedBeginner\ASCII\Images\";

        static void Main(string[] args)
        {
            filePath = GetFilePath(folderPath);
            while (!imagePathIsValid)
            {
                TryLoadImage();
            }
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

        static void TryLoadImage() //Todo Make this method private and setup proper parameter inputs
        {
            try
            {
                image = Image.Load(filePath);
                Console.WriteLine($"Correct image loaded: {filePath}");
                Console.WriteLine($"Width:   {image.Width}, Height: {image.Height}");
                image.Save(RemoveFileExtension(filePath) + ".jpg"); //todo This doesn't save a new file
                imagePathIsValid = true;
            }
            catch (System.IO.IOException e)
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
    }
}


