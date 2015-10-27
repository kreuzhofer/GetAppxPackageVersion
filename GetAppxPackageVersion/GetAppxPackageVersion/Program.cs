using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GetAppxPackageVersion
{
    class Program
    {
        private const string FileName = "Package.appxmanifest";

        static int Main(string[] args)
        {
            try
            {
                var doc = new XmlDocument();
                if (args.Length == 0)
                {
                    if (!File.Exists(FileName))
                    {
                        Usage();
                        return 1;
                    }
                    doc.Load(FileName);

                }
                else if (args.Length == 1)
                {
                    if (!File.Exists(args[0]))
                    {
                        Console.WriteLine("File not found: " + args[0]);
                        Usage();
                        return 1;
                    }
                    doc.Load(args[0]);
                }
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("x", "http://schemas.microsoft.com/appx/2010/manifest");
                var root = doc.DocumentElement;
                var identityNode = root.SelectSingleNode("descendant::x:Identity", nsmgr);
                var versionAttribute = identityNode.Attributes["Version"];
                Console.Write(versionAttribute.Value);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: "+ex.Message);
                Usage();
                return 2;
            }
        }

        private static void Usage()
        {
            Console.WriteLine("Usage: GetAppxPackageVersion <appx manifest file name>");
        }
    }
}
