using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace duplicate_finder
{
	class Program
	{
		static void Main(string[] args)
		{
		    if (args.Length != 2)
		    {
		        Console.WriteLine("Syntax is duplicate-finder <source folder> <target folder>");
		        return;
            }

		    var sourceFolder = args[0];
            if (string.IsNullOrWhiteSpace(sourceFolder))
		    {
		        Console.WriteLine("Source folder required");
		        return;
		    }

		    var targetFolder = args[1];
            if (string.IsNullOrWhiteSpace(targetFolder))
		    {
		        Console.WriteLine("Source folder required");
		        return;
		    }

		    var allFiles = Directory.GetFiles(sourceFolder, "*.*", SearchOption.AllDirectories);
		    Console.WriteLine($"Processing {allFiles.Length} files");

		    foreach (var sourceFile in allFiles)
		    {
		        var fileName = Path.GetFileName(sourceFile);
		        // ReSharper disable once AssignNullToNotNullAttribute
		        var targetFiles = Directory.GetFiles(targetFolder, fileName, SearchOption.AllDirectories);
		        if (targetFiles.Any())
		        {
		            foreach (var targetFile in targetFiles)
		            {
		                //Console.WriteLine(sourceFile);

                        if (FileEquals(sourceFile, targetFile))
		                {
		                    Console.WriteLine(targetFile);
                        }
                    }
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
		}

	    static bool FileEquals(string fileName1, string fileName2)
	    {
	        // Check the file size and CRC equality here.. if they are equal...    
	        using (var file1 = new FileStream(fileName1, FileMode.Open))
	        using (var file2 = new FileStream(fileName2, FileMode.Open))
	            return FileStreamEquals(file1, file2);
	    }

	    static bool FileStreamEquals(Stream stream1, Stream stream2)
	    {
	        const int bufferSize = 2048;
	        byte[] buffer1 = new byte[bufferSize]; //buffer size
	        byte[] buffer2 = new byte[bufferSize];
	        while (true)
	        {
	            int count1 = stream1.Read(buffer1, 0, bufferSize);
	            int count2 = stream2.Read(buffer2, 0, bufferSize);

	            if (count1 != count2)
	                return false;

	            if (count1 == 0)
	                return true;

	            // You might replace the following with an efficient "memcmp"
	            if (!buffer1.Take(count1).SequenceEqual(buffer2.Take(count2)))
	                return false;
	        }
	    }
    }
}
