using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace FileLib;

public class FileLibrary
{
    public string path = "/Users/oleksandrsuslov/Desktop/librarytesting";
    public List<Car> cars = new List<Car>();
    public string TextFilename { get; set; }
    public string XmlFileName { get; set; }
    
    public string BinFileName { get; set; }

    public void SetDefaultPath(string _path)
    {
        path = _path;
    }
    public void ReadFromTextFile(string name)
    {
        TextFilename = name;
        string line;
        try
        {
            StreamReader sr = new StreamReader(path + $"/{name}");
            line = sr.ReadLine();
            int count = 0;
            DateTime result;
            while (line != null)
            {
                var temp = line.Split(" ");

                if (!DateTime.TryParseExact(temp[0], "dd.MM.yyyy", null, DateTimeStyles.None, out result))
                {
                    throw new FormatException("Invalid date format");
                }

                cars.Add(new Car()
                {
                    Id = count,
                    Date = DateTime.ParseExact(temp[0], "dd.MM.yyyy", CultureInfo.InvariantCulture),
                    BrandName = temp[1],
                    Price = Int32.Parse(temp[2])
                });
                count++;
                Console.WriteLine(line);
                line = sr.ReadLine();
            }

            sr.Close();
        }
        catch (FormatException exception)
        {
            Console.WriteLine(exception.Message);
        }
        catch(Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
        finally
        {
            Console.WriteLine("Executing finally block.");
        }
    }

    public void UpdateRecordFile(int id)
    {
        Console.WriteLine("Enter edited record");   
        string? line = Console.ReadLine();
        var temp = line.Split(" ");
        cars.Where(w=> w.Id  == id).ToList().ForEach(i => { i.Id = id;
            i.Date = DateTime.Parse(temp[0]);
            i.BrandName = temp[1];
            i.Price = Int32.Parse(temp[2]);
        });
    }

    public void WriteToTextFile()
    {
        try
        {
            StreamWriter sw = new StreamWriter(path + $"/{TextFilename}");
            cars.ForEach(car =>
            {
                sw.WriteLine(car.Date.ToString("dd.MM.yyyy") + " " + car.BrandName + " " + car.Price);
            });
            sw.Close();
        }
        catch(Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
        finally
        {
            Console.WriteLine("Executing finally block.");
        }
    }
    
    public void AddRecordTextFile()
    {
        Console.WriteLine("Enter new record to add");   
        string? line = Console.ReadLine();
        var temp = line.Split(" ");
        cars.Add(new Car()
        {
            Id = cars.Last().Id + 1,
            Date = DateTime.ParseExact(temp[0], "dd.MM.yyyy", CultureInfo.InvariantCulture),
            BrandName = temp[1],
            Price = Int32.Parse(temp[2])
        });
    }
    
    public void DeleteRecordFile(int id)
    {
        var item = cars.Single(w => w.Id == id);
        cars.Remove(item);
    }

    public void WriteToXmlFile()
    {
        try
        {
            XDocument newXDoc = new XDocument(new XElement("Document"));
            int count = 0;
            cars.ForEach(car =>
            {
                newXDoc.Root.Add(new XElement("Car",
                    new XAttribute("ID", count++),
                    new XElement("Date", car.Date.ToString("dd.MM.yyyy")),
                    new XElement("BrandName", car.BrandName),
                    new XElement("Price", car.Price)
                    ));
            });
            newXDoc.Declaration = new XDeclaration("1.0", "utf-8", "true");
            Console.WriteLine(newXDoc);
            Console.WriteLine("enter name for xml file");
            string? xmlfile = Console.ReadLine();
            XmlFileName = xmlfile;
            newXDoc.Save(path + $"/{xmlfile}");
        }
        catch(Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
        finally
        {
            Console.WriteLine("Executing finally block.");
        }
    }
    
    public void UpdateRecordXmlFile(int id)
    {
        var carsXML = XElement.Load(path + $"/{XmlFileName}"); 
        Console.WriteLine("Enter edited record");   
        string? line = Console.ReadLine();
        var temp = line.Split(" ");
        var car = carsXML.Elements("Car")
            .Where(car => (int)car.Attribute("ID") == id)
            .FirstOrDefault();
        Console.WriteLine(car);
        car.Element("Date").Value = temp[0];
        car.Element("BrandName").Value = temp[1];
        car.Element("Price").Value = temp[2];
        carsXML.Save(path + $"/{XmlFileName}");
    }
    public void DeleteRecordXmlFile(int id)
    {
        var carsXML = XElement.Load(path + $"/{XmlFileName}");
        carsXML.Elements("Car")
            .Where(car => (int)car.Attribute("ID") == id)
            .Remove();
        carsXML.Save(path + $"/{XmlFileName}");
    }
    
    public void AddRecordXmlFile()
    {
        Console.WriteLine("Enter new record to add");   
        string? line = Console.ReadLine();
        var temp = line.Split(" ");
        var carsXML = XElement.Load(path + $"/{XmlFileName}");
        carsXML.Add(new XElement("Car",
            new XAttribute("ID", (int)carsXML.Elements("Car").Last().Attribute("ID")+1),
            new XElement("Date", temp[0]),
            new XElement("BrandName", temp[1]),
            new XElement("Price", temp[2])
        ));
        carsXML.Save(path + $"/{XmlFileName}");
        
    }
    

    public void ReadFromXmlFile(string xmlfilename)
    {
        try
        {
            XmlFileName = xmlfilename;
            var carsXML = XElement.Load(path + $"/{XmlFileName}");
            int count = 0;
            DateTime result;
            foreach (var car in carsXML.Elements("Car"))
            {
                if (!DateTime.TryParseExact(car.Element("Date").Value, "dd.MM.yyyy", null, DateTimeStyles.None,
                        out result))
                {
                    throw new FormatException("Invalid date format");
                }

                cars.Add(new Car()
                {
                    Id = count,
                    Date = DateTime.ParseExact(car.Element("Date").Value, "dd.MM.yyyy", CultureInfo.InvariantCulture),
                    BrandName = car.Element("BrandName").Value,
                    Price = Int32.Parse(car.Element("Price").Value)
                });
                Console.WriteLine(car.Element("Date").Value + " " + car.Element("BrandName").Value
                                  + " " + car.Element("Price").Value);
                count++;
            }

            Console.WriteLine();
        }
        
        // commented catches because we are testing exceptions in unit tests.
        
        // catch (FormatException exception)
        // {
        //     Console.WriteLine(exception.Message);
        // }
        // catch(Exception e)
        // {
        //     Console.WriteLine("Exception: " + e.Message);
        // }
        finally
        {
            Console.WriteLine("Executing finally block.");
        }
    }
    
    public void WriteToBinaryFile()
    {
        try
        {
            Console.WriteLine("enter name for bin file");
            string? binfile = Console.ReadLine();
            using (BinaryWriter binWriter = new BinaryWriter(File.Open(path + $"/{binfile}", FileMode.Create)))
            {
                binWriter.Write((short)0x2526); // 2 bytes
                binWriter.Write(cars.Count); // 4 bytes
                cars.ForEach(car =>
                {
                    byte[] dayBytes = BitConverter.GetBytes((ushort)car.Date.Day); // 2 bytes
                    byte[] monthBytes = BitConverter.GetBytes((ushort)car.Date.Month); // 2 bytes
                    byte[] yearBytes = BitConverter.GetBytes(car.Date.Year); // 4 bytes

                    binWriter.Write(dayBytes);
                    binWriter.Write(monthBytes);
                    binWriter.Write(yearBytes);
                    binWriter.Write((short)car.BrandName.Length); // 2 bytes

                    byte[] stringBytes = Encoding.ASCII.GetBytes(car.BrandName);

                    // Padding the string to length*2 bytes
                    byte[] paddedStringBytes = new byte[car.BrandName.Length * 2];
                    Array.Copy(stringBytes, paddedStringBytes, Math.Min(stringBytes.Length, car.BrandName.Length * 2));

                    binWriter.Write(paddedStringBytes); // 0... 2*BrandName length

                    binWriter.Write(car.Price); // 4 bytes
                });
            }


        }
        catch(Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
        finally
        {
            Console.WriteLine("Executing finally block.");
        }
    }
    
    public void ReadFromBinFile(string binfilename)
    {

        try
        {
            BinFileName = binfilename;

            using (BinaryReader binReader = new BinaryReader(File.Open(path + $"/{binfilename}", FileMode.Open)))
            {
                binReader.BaseStream.Seek(2, SeekOrigin.Begin);

                // Read the 4-byte integer records count
                int count = binReader.ReadInt32();
                if (count < 0)
                {
                    throw new ArgumentException("Record count cannot be negative");
                }

                for (int i = 0; i < count; i++)
                {
                    byte[] dayBytes = binReader.ReadBytes(2);
                    byte[] monthBytes = binReader.ReadBytes(2);
                    byte[] yearBytes = binReader.ReadBytes(4);

                    // Convert bytes back to day, month, and year
                    ushort day = BitConverter.ToUInt16(dayBytes, 0);

                    if (day > 31)
                    {
                        throw new FormatException("wrong value for date format");
                    }

                    ushort month = BitConverter.ToUInt16(monthBytes, 0);

                    if (month > 12)
                    {
                        throw new FormatException("wrong value for month format");
                    }

                    int year = BitConverter.ToInt32(yearBytes, 0);

                    DateTime date = new DateTime(year, month, day);

                    short stringLength = binReader.ReadInt16(); // length of brandname

                    if (stringLength < 0)
                    {
                        throw new ArgumentException("brandname length cannot be negative");
                    }

                    // Read the bytes for the string
                    byte[] stringBytes = binReader.ReadBytes(stringLength);

                    string brandname = Encoding.UTF8.GetString(stringBytes);

                    binReader.BaseStream.Seek(stringLength, SeekOrigin.Current);

                    // (4 bytes)
                    int price = binReader.ReadInt32();
                    if (price < 0)
                    {
                        throw new ArgumentException("price cannot be negative");
                    }

                    cars.Add(new Car()
                    {
                        Id = i,
                        Date = date.Date,
                        BrandName = brandname,
                        Price = price
                    });
                    Console.WriteLine(date.ToString("dd.MM.yyyy") + " " + brandname + " " + price);
                }
            }

            Console.WriteLine();
        }
        catch (FormatException exception)
        {
            Console.WriteLine(exception.Message);
        }
        catch (ArgumentException exception)
        {
            Console.WriteLine(exception.Message);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
        }
    }
    
}