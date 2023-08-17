using System.Xml.Linq;
using FileLib;

namespace TestProject1;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    [TestCase("11.01.2004", "Ferrari", 11000)]
    [TestCase("05.07.2020", "Ford", 3000)]
    [TestCase("03.10.2007", "Skoda", 5000)]
    public void TestReadWriteArrayFromTextFile(string date, string brandname, int price)
    {
        FileLibrary lib = new FileLibrary();
        lib.TextFilename = "test.txt";
        File.WriteAllText(lib.path + $"/{lib.TextFilename}", String.Empty);
        StreamWriter sw = new StreamWriter(lib.path + $"/{lib.TextFilename}");
        sw.WriteLine(date + " " + brandname + " " + price);
        sw.Close();
        lib.ReadFromTextFile("test.txt");
        Assert.AreEqual(lib.cars.First().Date.ToString("dd.MM.yyyy"), date);
        Assert.AreEqual(lib.cars.First().BrandName, brandname);
        Assert.AreEqual(lib.cars.First().Price, price);

    }
    [Test]
    [TestCase("helo", "Toyota", 5000)]
    [TestCase("1213", "Porshe", 12000)]
    [TestCase("2333", "Skoda", 6000)]
    public void TestInvalidDateXmlFormatException(string date, string brandname, int price)
    {
        FileLibrary lib = new FileLibrary();
        lib.XmlFileName = "testxml.xml";
        File.WriteAllText(lib.path + $"/{lib.XmlFileName}", String.Empty);
        XDocument newXDoc = new XDocument(new XElement("Document"));
        newXDoc.Root.Add(new XElement("Car",
                new XAttribute("ID", 0),
                new XElement("Date", date),
                new XElement("BrandName", brandname),
                new XElement("Price", price)
            ));
        newXDoc.Declaration = new XDeclaration("1.0", "utf-8", "true");
        
        newXDoc.Save(lib.path + $"/{lib.XmlFileName}");
        
        var exception = Assert.Throws<FormatException>(()=> lib.ReadFromXmlFile(lib.XmlFileName));
        
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception.Message, Is.EqualTo("Invalid date format"));
    }
    
    
    
}