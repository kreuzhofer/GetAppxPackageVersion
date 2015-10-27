using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IncreaseAppxPackageVersion
{
    class Program
    {
        private const string FileName = "Package.appxmanifest";

        static int Main(string[] args)
        {
            try
            {
                var doc = new XmlDocument();
                var filePath = "";
                if (args.Length == 0)
                {
                    if (!File.Exists(FileName))
                    {
                        Usage();
                        return 1;
                    }
                    filePath = FileName;
                }
                else if (args.Length == 1)
                {
                    if (!File.Exists(args[0]))
                    {
                        Console.WriteLine("File not found: " + args[0]);
                        Usage();
                        return 1;
                    }
                    filePath = args[0];
                }
                doc.Load(filePath);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("x", "http://schemas.microsoft.com/appx/2010/manifest");
                var root = doc.DocumentElement;
                var identityNode = root.SelectSingleNode("descendant::x:Identity", nsmgr);
                var versionAttribute = identityNode.Attributes["Version"];
                var parts = versionAttribute.Value.Split('.');
                parts[3] = (int.Parse(parts[3]) + 1).ToString();
                versionAttribute.Value = String.Join(".", parts);
                Console.Write(versionAttribute.Value);
                doc.Save(filePath);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Usage();
                return 2;
            }
        }

        private static void Usage()
        {
            Console.WriteLine("Usage: IncreaseAppxPackageVersion <appx manifest file name>");
        }
    }
}
