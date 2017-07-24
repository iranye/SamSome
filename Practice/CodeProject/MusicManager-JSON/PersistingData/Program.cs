using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PersistingData
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "file.json";
            if (args.Length < 2)
            {
                Usage();
                return;
            }
            switch (args[0])
            {
                case "-ws":
                    try
                    {
                        GetFileNameForWriting(args[1], out fileName, true);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex.Message);
                    }
                    try
                    {
                        TestWriteSingleMovieData(fileName);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex.Message);
                    }
                    break;
                case "-wc":
                    try
                    {
                        GetFileNameForWriting(args[1], out fileName, true);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex.Message);
                    }
                    try
                    {
                        TestWriteCollectionData(fileName);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex.Message);
                    }
                    break;
                case "-rs":

                    try
                    {
                        GetFileNameForReading(args[1], out fileName);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex.Message);
                    }
                    try
                    {
                        var movie = TestReadSingleMovieData<Movie>(fileName);
                        Console.WriteLine(movie);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex.Message);
                    }
                    break;
                case "-rc":
                    try
                    {
                        GetFileNameForReading(args[1], out fileName);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex.Message);
                    }
                    try
                    {
                        List<Movie> movies = TestReadCollectionMovieData<Movie>(fileName);
                        foreach (var movie in movies)
                        {
                            Console.WriteLine(movie);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex.Message);
                    }
                    break;
                default:
                    Usage();
                    break;
            }
        }

        #region ReadFromFile
        private static T TestReadSingleMovieData<T>(string fileName) where T:new()
        {
            T obj = new T();
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                obj = JsonConvert.DeserializeObject<T>(json);
            }
            
            return obj;
        }

        private static List<T> TestReadCollectionMovieData<T>(string fileName) where T : new()
        {
            List<T> objCol = new List<T>();
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();
                objCol = JsonConvert.DeserializeObject<List<T>>(json);
            }
            return objCol;
        }

        #endregion

        private static void Usage()
        {
            Console.Error.WriteLine("\nUsage: $0 [options] <filename> for options in: [-w{s|c} : write to file {single or collection}|-r{s|c} : read from file]");
        }

        #region WriteToFile
        private static void TestWriteCollectionData(string fileName)
        {
            List<Movie> movies = new List<Movie>
            {
                new Movie {Name = "Matrix", Year = 1998},
                new Movie {Name = "Gone with the Wind", Year = 1944}
            };
            File.WriteAllText(fileName, JsonConvert.SerializeObject(movies.ToArray()));
            Console.WriteLine($"Wrote CollectionData to JSON file: '{fileName}'");
        }

        private static void TestWriteSingleMovieData(string fileName)
        {
            var movie = new Movie
            {
                Name = "Bad Boys",
                Year = 1995
            };
            File.WriteAllText(fileName, JsonConvert.SerializeObject(movie));
            Console.WriteLine($"Wrote Single Movie data to JSON file: '{fileName}'");
        }
        #endregion

        private static void GetFileNameForReading(string arg, out string fileName)
        {
            if (String.IsNullOrWhiteSpace(arg))
            {
                throw new ArgumentException("arg is NULL or empty string");
            }
            fileName = arg;
            if (!File.Exists(fileName))
            {
                throw new ArgumentException("Invalid file: '${fileName}'");
            }
        }

        private static void GetFileNameForWriting(string arg, out string fileName, bool overWrite)
        {
            if (String.IsNullOrWhiteSpace(arg))
            {
                throw new ArgumentException("arg is NULL or empty string");
            }
            fileName = arg;
            if (File.Exists(fileName) && !overWrite)
            {
                throw new ArgumentException($"file exists and overWrite flag is false: '{fileName}'");
            }
        }
    }

    public class Movie
    {
        public string Name { get; set; }
        public int Year { get; set; }

        public override string ToString()
        {
            return $"The movie {Name} was made in {Year}";
        }
    }
}
