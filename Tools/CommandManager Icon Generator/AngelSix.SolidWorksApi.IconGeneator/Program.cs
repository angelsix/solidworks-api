using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace AngelSix.SolidWorksApi.IconGeneator
{
    class Program
    {
        /// <summary>
        /// Drag and drop images onto the exe to generate SolidWorks toolbar sprites 
        /// or open by double clicking to interactively select files and an output name
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // The filename to prepend to the output files
            var filenamePrepend = "icons";

            Console.WriteLine("");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("   AngelSix SolidWorks Icon Generator   ");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("");

            // 
            //   NOTE: 
            //
            //   We expect a list of images in, and a name to prepend the filename as the last argument
            //
            //   From that we will combine them into lists and resize them 
            //   from the top size down to the smallest size
            //

            // All output sizes
            var possibleSizes = new List<int>(new[] { 20, 32, 40, 64, 96, 128 });

            // Add any command line args
            var images = new List<string>();
            if (args?.Length > 0)
                images.AddRange(args);

            // If we have no images then simply ask the user to start specifying the image paths
            if (images.Count < 1)
            {
                // Wipe any previous data
                images = new List<string>();

                // Start asking user to enter image paths
                var result = " ";
                while (!string.IsNullOrEmpty(result))
                {
                    Console.ResetColor();
                    Console.WriteLine($"Enter the path to the {NthNumber(images.Count + 1)}. Once done press enter");
                    result = Console.ReadLine();

                    // Check if done
                    if (string.IsNullOrEmpty(result))
                        break;

                    // Make sure the file exists
                    if (!File.Exists(result))
                    {
                        // Try and find it with .png to the name
                        if (!result.Contains('.'))
                            result += ".png";

                        // Check if it exists again
                        if (!File.Exists(result))
                        {
                            // Let user know file not found
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Image not found '{result}'");
                        }
                        else
                            // Add this to the list and carry on
                            images.Add(result);
                    }
                    else
                    {
                        // Add this to the list and carry on
                        images.Add(result);
                    }
                }

                // Get filename to append
                Console.ResetColor();
                Console.WriteLine($"Enter the name to prepend to the output files");
                filenamePrepend = Console.ReadLine();
            }

            // Now create an image from each of the iamges, for each file size
            possibleSizes.ForEach(size =>
            {
                // Check all files exist
                if (images.Any(image => !File.Exists(image)))
                {
                    Console.WriteLine($"One or more of the files do not exist. Press enter to exit");
                    Console.ReadLine();
                    return;
                }

                // Combine all bitmaps
                using (var combinedImage = CombineBitmap(images, size))
                {
                    combinedImage.Save($"{filenamePrepend}{size}.png");
                }
            });
        }

        /// <summary>
        /// Adds the "st", "nd" etc... to a number, such as 1st, 6th, 23rd
        /// </summary>
        /// <param name="number">The number to use</param>
        /// <returns></returns>
        private static string NthNumber(int number)
        {
            // Base10 the number
            number = number % 10;

            switch (number)
            {
                case 1:
                    return $"{number}st";
                case 2:
                    return $"{number}nd";
                case 3:
                    return $"{number}rd";
                default:
                    return $"{number}th";
            }
        }

        /// <summary>
        /// Combines images into a sprite horizontally
        /// </summary>
        /// <param name="files">The files to combine</param>
        /// <param name="iconSize">The sprite size</param>
        /// <returns></returns>
        private static Bitmap CombineBitmap(List<string> files, int iconSize)
        {
            // Read all images into memory
            Bitmap finalImage = null;
            var images = new List<Bitmap>();

            try
            {
                // Get size
                int width = iconSize * files.Count;
                int height = iconSize;

                // Create a bitmap to hold the combined image
                finalImage = new Bitmap(width, height);

                // Get a graphics object from the image so we can draw on it
                using (var g = Graphics.FromImage(finalImage))
                {
                    // Set background color
                    g.Clear(Color.Transparent);

                    // Go through each image and draw it on the final image
                    int offset = 0;
                    files.ForEach(file =>
                    {
                        // Read this image
                        var bitmap = new Bitmap(file);
                        images.Add(bitmap);

                        // Scale it to the sprite size
                        var scaleFactor = (float)iconSize / Math.Max(bitmap.Width, bitmap.Height);

                        // Draw it onto the new image
                        g.DrawImage(bitmap, new Rectangle(offset, 0, (int)(scaleFactor * bitmap.Width), (int)(scaleFactor * bitmap.Height)));

                        // Move offset to next position
                        offset += iconSize;                       

                    });
                }

                // Return the final image
                return finalImage;
            }
            catch (Exception)
            {
                // Cleanup
                finalImage?.Dispose();
                throw;
            }
            finally
            {
                // Cleanup
                images.ForEach(image => image?.Dispose());
            }
        }
    }
}
