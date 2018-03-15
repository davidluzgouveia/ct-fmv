using System;
using System.IO;

public static class Program
{
    public static void Main(string[] args)
    {
        var defaultColor = Console.ForegroundColor;

        try
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Chrono Trigger (Steam) FMV Encoder / Decoder\n");

            Console.ForegroundColor = defaultColor;
            Console.WriteLine(
                "A tool that encodes or decodes the FMV files in the game (the 001-008.dat on the game root)\n");

            if (args.Length != 1)
            {
                throw new Exception("Drag a .dat or .mp4 file into this executable");
            }

            var inputPath = args[0];

            if (!File.Exists(inputPath))
            {
                throw new Exception($"{inputPath} is not a file");
            }

            var isDat = inputPath.EndsWith(".dat");
            var isMp4 = inputPath.EndsWith(".mp4");
            if (!isDat && !isMp4)
            {
                throw new Exception($"{inputPath} is not a .dat nor a .mp4 file");
            }

            var outputPath =
                isDat ? inputPath.Replace(".dat", ".decoded.mp4") : inputPath.Replace(".mp4", ".encoded.dat");

            if (File.Exists(outputPath))
            {
                throw new Exception($"There's already a file at {outputPath}, remove it first");
            }

            var before = File.ReadAllBytes(inputPath);
            var after = Decode(before);

            File.WriteAllBytes(outputPath, after);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Converted {inputPath} into {outputPath}\n");
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message + "\n");
        }
        finally
        {
            Console.ForegroundColor = defaultColor;
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    private static byte[] Decode(byte[] input)
    {
        var output = new byte[input.Length];
        var counter = 0xFF;
        for (var i = 0; i < input.Length; i++)
        {
            output[i] = (byte)(input[i] ^ counter);
            counter = counter == 0 ? 0xFF : counter - 1;
        }

        return output;
    }
}