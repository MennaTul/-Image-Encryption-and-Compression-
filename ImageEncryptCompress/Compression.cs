using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace ImageEncryptCompress
{
    class Compression
    {
        public void comp(RGBPixel[,] image, string image_path, string IntialSeed, string TapPosition)
        {
            int width = ImageOperations.GetWidth(image);
            int height = ImageOperations.GetHeight(image);


            Dictionary<short, int> RedF = new Dictionary<short, int>();
            Dictionary<short, int> GreenF = new Dictionary<short, int>();
            Dictionary<short, int> BlueF = new Dictionary<short, int>();

            for (int h = 0; h < height; h++)
            {
                for (int w = 0; w < width; w++)
                {
                    RGBPixel pixel = image[h, w];
                    if (RedF.ContainsKey(pixel.red))
                    {
                        RedF[pixel.red]++;
                    }
                    else
                    {
                        RedF.Add(pixel.red, 1);
                    }

                    if (GreenF.ContainsKey(pixel.green))
                    {
                        GreenF[pixel.green]++;
                    }
                    else
                    {
                        GreenF.Add(pixel.green, 1);
                    }

                    if (BlueF.ContainsKey(pixel.blue))
                    {
                        BlueF[pixel.blue]++;
                    }
                    else
                    {
                        BlueF.Add(pixel.blue, 1);
                    }

                }
            }


            Huffman Redhuffman = new Huffman();
            Redhuffman.buildbinarytree(RedF);

            Huffman Greenhuffman = new Huffman();
            Greenhuffman.buildbinarytree(GreenF);

            Huffman Bluehuffman = new Huffman();
            Bluehuffman.buildbinarytree(BlueF);





















            //Console.WriteLine("image Code is: " + imgcode + "\n\n");


            //Console.WriteLine("Green Code is: " + greencode + "\n\n");


            //Console.WriteLine("Blue Code is: " + bluecode + "\n\n");


            List<byte> compressedDataRed = CompressAndPackImageData(image, Redhuffman.ColorCode, 'R');

            List<byte> compressedDataGreen = CompressAndPackImageData(image, Greenhuffman.ColorCode, 'G');

            List<byte> compressedDataBlue = CompressAndPackImageData(image, Bluehuffman.ColorCode, 'B');



            


            SerializeHuffmanAndCompressedData(Redhuffman, Greenhuffman, Bluehuffman, compressedDataRed, compressedDataGreen, compressedDataBlue, image_path + "compressed_data.bin", width, height, IntialSeed, TapPosition);





        }


        //public static void printCodes(Huffman Redhuffman, Huffman Greenhuffman, Huffman Bluehuffman, string image_path)
        //{
        //    StreamWriter sw = new StreamWriter(image_path + ".txt");
        //    sw.Write("Color\t\tFrequency\t\t\tHuffmanCode\t\t\t\t\tTotal Bits\n-Red-\n");
        //    while (Redhuffman.queue.Count() > 0)
        //    {

        //        if (Redhuffman.queue.Peek() != null && Redhuffman.ColorCode.ContainsKey(Redhuffman.queue.Peek().pixel))
        //        {
        //            sw.Write(Redhuffman.queue.Peek().pixel + "\t\t\t" + Redhuffman.queue.Peek().frequency + "\t\t\t" + Redhuffman.queue.Peek().colorcode);

        //            Redhuffman.queue.Dequeue();

        //        }


        //    }




        //}

        ////public static void storetree(Huffman Redhuffman, Huffman Greenhuffman, Huffman Bluehuffman, string image_path, List<uint> Redcode, List<uint> Greencode, List<uint> Bluecode)
        ////{
        ////    string filePath = image_path + "example.bin";
        ////    BinaryFormatter formatter = new BinaryFormatter();
        ////    FileStream fs = new FileStream(filePath + ".bin", FileMode.Create, FileAccess.Write);



        ////        foreach (uint num in Redcode)
        ////        {
        ////            fs.Write(num);
        ////        }
        ////        fs.Write((int)-2);
        ////        foreach (uint num in Redcode)
        ////        {
        ////            fs.Write(num);
        ////        }
        ////        fs.Write((int)-2);
        ////        foreach (uint num in Redcode)
        ////        {
        ////            fs.Write(num);
        ////        }

        ////        writer.Serialize(Redhuffman.queue.Peek());
        ////        //writer.Write((short)-2);
        ////        //WriteHuffmanTree(Redhuffman.queue.Peek(), writer);
        ////        //writer.Write((short)-2);
        ////        //WriteHuffmanTree(Greenhuffman.queue.Peek(), writer);
        ////        //writer.Write((short)-2);
        ////        //WriteHuffmanTree(Bluehuffman.queue.Peek(), writer);
        ////        //writer.Write((short)-2);

        ////        MessageBox.Show("The file path is:" + filePath);


        ////}

        ////public static void WriteHuffmanTree(huffmannode node, BinaryWriter writer)
        ////{
        ////    if (node == null)
        ////    {
        ////        writer.Write(false); // Indicates a null node
        ////        return;
        ////    }

        ////    writer.Write(true); // Indicates a non-null node
        ////    writer.Write(node.pixel);

        ////    WriteHuffmanTree(node.left, writer);
        ////    WriteHuffmanTree(node.right, writer);
        ////}

        ////public static void read(string image_path)
        ////{
        ////    string filePath = image_path + "example.bin";

        ////    if (File.Exists(filePath))
        ////    {
        ////        // Open the binary file for reading
        ////        using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
        ////        {
        ////            // Read data from the file
        ////            short Value = reader.ReadInt16();
        ////            short left;
        ////            short right;
        ////            while (Value != -2)
        ////            {
        ////                left = reader.ReadInt16();
        ////                right = reader.ReadInt16();

        ////                // Output the read data
        ////                Console.WriteLine("Int value: " + Value + "Left : " + left + "Right: " + right);

        ////                Value = reader.ReadInt16();
        ////            }

        ////            //Value = reader.ReadInt16();

        ////            //while (Value != -2)
        ////            //{
        ////            //    left = reader.ReadInt16();
        ////            //    right = reader.ReadInt16();

        ////            //    // Output the read data
        ////            //    Console.WriteLine("Int value: " + Value + "Left : " + left + "Right: " + right);

        ////            //    Value = reader.ReadInt16();
        ////            //}

        ////            //Value = reader.ReadInt16();

        ////            //while (Value != -2)
        ////            //{
        ////            //    left = reader.ReadInt16();
        ////            //    right = reader.ReadInt16();

        ////            //    // Output the read data
        ////            //    Console.WriteLine("Int value: " + Value + "Left : " + left + "Right: " + right);

        ////            //    Value = reader.ReadInt16();
        ////            //}

        ////        }
        ////    }
        ////    else
        ////    {
        ////        Console.WriteLine("File does not exist: " + filePath);
        ////    }
        ////}


        ////public static List<uint> SplitBinaryStringBuilderToUIntegers(StringBuilder binaryStr)
        ////{
        ////    int chunkSize = 32;
        ////    List<uint> uintegersList = new List<uint>(binaryStr.Length / chunkSize + 1); // Initialize with expected capacity

        ////    for (int i = 0; i < binaryStr.Length; i += chunkSize)
        ////    {
        ////        string chunk = binaryStr.ToString(i, Math.Min(chunkSize, binaryStr.Length - i));
        ////        uint uintValue = Convert.ToUInt32(chunk, 2);
        ////        uintegersList.Add(uintValue);
        ////    }

        ////    return uintegersList;
        ////}

        //public static int CalcSize(Huffman tree)
        //{
        //    int size = 0;

        //    while (tree.queue.Count() > 0 && tree.queue.Peek() != null)
        //    {
        //        if (tree.ColorCode.ContainsKey(tree.queue.Peek().pixel))
        //        {
        //            size += tree.queue.Peek().frequency * tree.queue.Peek().colorcode.Length;
        //        }
        //        tree.queue.Dequeue();
        //    }

        //    return size;
        //}


        private List<byte> CompressAndPackImageData(RGBPixel[,] image, Dictionary<short, string> colorCodes, Char color)
        {
            if (color == 'R')
            {
                List<byte> packedData = new List<byte>();
                string temp = "";
                string chunk;

                foreach (RGBPixel pixel in image)
                {

                    string redCode = colorCodes[pixel.red];


                    string combinedCode = temp + redCode;

                    temp = "";


                    for (int i = 0; i < combinedCode.Length; i += 8)
                    {
                        if (combinedCode.Length - i < 8)
                        {
                            temp = combinedCode.Substring(i, combinedCode.Length - i);
                            break;
                        }
                        chunk = combinedCode.Substring(i, Math.Min(8, combinedCode.Length - i));
                        chunk = chunk.PadRight(8, '0'); // Pad with zeros if needed
                        packedData.Add(ConvertBinaryStringToByte(chunk));

                    }
                }
                chunk = temp.PadRight(8, '0'); // Pad with zeros if needed
                packedData.Add(ConvertBinaryStringToByte(chunk));

                return packedData;
            }





            else if (color == 'G')
            {
                List<byte> packedData = new List<byte>();
                string temp = "";
                string chunk;

                foreach (RGBPixel pixel in image)
                {

                    string greenCode = colorCodes[pixel.green];


                    string combinedCode = temp + greenCode;

                    temp = "";


                    for (int i = 0; i < combinedCode.Length; i += 8)
                    {
                        if (combinedCode.Length - i < 8)
                        {
                            temp = combinedCode.Substring(i, combinedCode.Length - i);
                            break;
                        }
                        chunk = combinedCode.Substring(i, Math.Min(8, combinedCode.Length - i));
                        chunk = chunk.PadRight(8, '0'); // Pad with zeros if needed
                        packedData.Add(ConvertBinaryStringToByte(chunk));



                    }
                }
                chunk = temp.PadRight(8, '0'); // Pad with zeros if needed
                packedData.Add(ConvertBinaryStringToByte(chunk));

                return packedData;
            }



            else
            {
                List<byte> packedData = new List<byte>();
                string temp = "";
                string chunk;

                foreach (RGBPixel pixel in image)
                {

                    string blueCode = colorCodes[pixel.blue];


                    string combinedCode = temp + blueCode;

                    temp = "";


                    for (int i = 0; i < combinedCode.Length; i += 8)
                    {
                        if (combinedCode.Length - i < 8)
                        {
                            temp = combinedCode.Substring(i, combinedCode.Length - i);
                            break;
                        }
                        chunk = combinedCode.Substring(i, Math.Min(8, combinedCode.Length - i));
                        chunk = chunk.PadRight(8, '0'); // Pad with zeros if needed
                        packedData.Add(ConvertBinaryStringToByte(chunk));



                    }
                }
                chunk = temp.PadRight(8, '0'); // Pad with zeros if needed
                packedData.Add(ConvertBinaryStringToByte(chunk));

                return packedData;
            }

        }

        private byte ConvertBinaryStringToByte(string binaryString)
        {
            return Convert.ToByte(binaryString, 2);
        }

        private void SerializeHuffmanAndCompressedData(Huffman redHuffman, Huffman greenHuffman, Huffman blueHuffman, List<byte> compressedDataRed, List<byte> compressedDataGreen, List<byte> compressedDataBlue, string filename, int width, int height, string InitialSeed, string TapPosition)
        {

            long Initial_seed = Convert.ToInt64(InitialSeed, 2);

            byte Tap_position = Convert.ToByte(TapPosition);

            using (FileStream fs = new FileStream(filename, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fs))
            {
                writer.Write(Initial_seed);
                writer.Write((int)InitialSeed.Length);
                writer.Write(Tap_position);
                writer.Write(width);
                writer.Write(height);
                writer.Write((int)compressedDataRed.Count);
                writer.Write((int)compressedDataGreen.Count);
                writer.Write((int)compressedDataBlue.Count);

                SerializeHuffmanTree(writer, redHuffman);
                
                SerializeHuffmanTree(writer, greenHuffman);
                
                SerializeHuffmanTree(writer, blueHuffman);
                

                foreach (byte packedValue in compressedDataRed)
                {
                    writer.Write(packedValue);
                    
                }



                foreach (byte packedValue in compressedDataGreen)
                {
                    writer.Write(packedValue);
                   
                }



                foreach (byte packedValue in compressedDataBlue)
                {
                    writer.Write(packedValue);
                    
                }

            }

            WriteTextFile(redHuffman, greenHuffman, blueHuffman, filename, width, height);

            MessageBox.Show("The image is compressed and saved!");
        }

        private void SerializeHuffmanTree(BinaryWriter writer, Huffman huffman)
        {

            // Separating between each Tree 


            SerializeTreeNode(writer, huffman.queue.Peek());
        }

        private void SerializeTreeNode(BinaryWriter writer, huffmannode node)
        {

            writer.Write(node.pixel);
            //Console.WriteLine(node.pixel);



            if (node.left != null)
            {
                //writer.Write(true); 
                SerializeTreeNode(writer, node.left);


            }

            if (node.right != null)
            {
                //writer.Write(true); 
                SerializeTreeNode(writer, node.right);
            }

        }

        private static int Total = 0;

        private void WriteTextFile(Huffman redHuffman, Huffman greenHuffman, Huffman blueHuffman, string filename, int width, int height)
        {
            float OriginalSize = width * height * 24;

            float CompressedSize;

            float ratio;

            using (StreamWriter sw = new StreamWriter(filename + ".txt"))
            {

                sw.Write("Color\t\tFrequency\t\tHuffmanCode\t\t\tTotal Bits\n-Red-\n");

                int TotalRedBits = WriteNodeText(sw, redHuffman.queue.Peek());
                Total = 0;
                sw.Write("Total Red Bits = " + TotalRedBits + "\n\n");

                sw.Write("--------------------------------------------------------------------------------------- \n\n");


                sw.Write("Color\t\tFrequency\t\tHuffmanCode\t\t\tTotal Bits\n-Green-\n");

                int TotalGreenBits = WriteNodeText(sw, greenHuffman.queue.Peek());
                Total = 0;
                sw.Write("Total Green Bits = " + TotalGreenBits + "\n\n");

                sw.Write("--------------------------------------------------------------------------------------- \n\n");


                sw.Write("Color\t\tFrequency\t\tHuffmanCode\t\t\tTotal Bits\n-Blue-\n");

                int TotalBlueBits = WriteNodeText(sw, blueHuffman.queue.Peek());
                Total = 0;
                sw.Write("Total Blue Bits = " + TotalBlueBits + "\n\n");


                CompressedSize = TotalRedBits + TotalGreenBits + TotalBlueBits;

                sw.Write("Original Image = " + OriginalSize + "\n\n");

                sw.Write("Compressed Image = " + CompressedSize + "\n\n");

                ratio = (CompressedSize / OriginalSize) * 100;

                sw.Write("The ratio = " + ratio);

            }






        }

        private int WriteNodeText(StreamWriter sw, huffmannode node)
        {

            if (node.pixel >= 0)
            {
                int loctotBits = node.frequency * node.colorcode.Length;
                Total += loctotBits;
                sw.Write(node.pixel + "\t\t\t" + node.frequency + "\t\t\t" + node.colorcode + "\t\t\t" + loctotBits + "\n\n");


            }


            if (node.left != null)
            {
                WriteNodeText(sw, node.left);
            }

            if (node.right != null)
            {
                WriteNodeText(sw, node.right);
            }


            return Total;


        }



















        #region



        //Decomp


        public static string initialseed;
        public static string taposition;

        public RGBPixel[,] Decomp()
        {

            string selectedFilePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Set initial directory to the desktop or any other desired directory
            //openFileDialog.InitialDirectory = @"D:\Fcis\Year3\Semester 2\Algo\Project\[1] Image Encryption and Compression\Startup Code\[TEMPLATE] ImageEncryptCompress\ImageEncryptCompress\bin\Debug";
            //openFileDialog.InitialDirectory = Application.StartupPath;
            // Filter for .bin files
            openFileDialog.Filter = "Binary Files (*.bin)|*.bin";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFilePath = openFileDialog.FileName;

            }

            huffmannode Rednode = new huffmannode();
            huffmannode Greennode = new huffmannode();
            huffmannode Bluenode = new huffmannode();
            long seed = 0;
            int seedLength = 0;
            byte tap = 0;
            int w = 0;
            int h = 0;
            int RedLength = 0;
            int GreenLength = 0;
            int BlueLength = 0;
            StringBuilder BinarycodeRed = new StringBuilder();
            StringBuilder BinarycodeGreen = new StringBuilder();
            StringBuilder BinarycodeBlue = new StringBuilder();
            //StringBuilder Binarycode2 = new StringBuilder();
            byte bin;
            using (FileStream fs = new FileStream(selectedFilePath, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(fs))
            {
                seed = reader.ReadInt64();
                seedLength = reader.ReadInt32();
                tap = reader.ReadByte();

                initialseed = Convert.ToString(seed, 2).PadLeft(seedLength, '0');
                taposition = tap.ToString();

                w = reader.ReadInt32();
                h = reader.ReadInt32();
                RedLength = reader.ReadInt32();
                GreenLength = reader.ReadInt32();
                BlueLength = reader.ReadInt32();

                Rednode.pixel = reader.ReadInt16();
                DeserializeTreeNode(reader, ref Rednode);



                Greennode.pixel = reader.ReadInt16();
                DeserializeTreeNode(reader, ref Greennode);


                Bluenode.pixel = reader.ReadInt16();
                DeserializeTreeNode(reader, ref Bluenode);



                






                /// READ BINARY CODE
                /// 


                while (RedLength-- > 0)
                {
                    bin = reader.ReadByte();
                    //Console.Write(bin);
                    BinarycodeRed.Append(ConvertByteToBinaryString(bin));
                }
                while (GreenLength-- >0)
                {
                    bin = reader.ReadByte();
                    //Console.Write(bin);
                    BinarycodeGreen.Append(ConvertByteToBinaryString(bin));
                }
                while (BlueLength-- > 0)
                {
                    bin = reader.ReadByte();
                    //Console.Write(bin);
                    BinarycodeBlue.Append(ConvertByteToBinaryString(bin));
                }



            }


            RGBPixel[,] Image = BuildImage(Rednode, Greennode, Bluenode, w, h, BinarycodeRed.ToString(), BinarycodeGreen.ToString(), BinarycodeBlue.ToString());


            return Image;
            //Console.WriteLine("\n\n\n\n\n\nRead binary code IS: " + Binarycode.ToString());

            //Console.WriteLine("Red tree: \n");
            //Printtreenode(Rednode);

            //Console.WriteLine(" \n \n Green tree: \n");
            //Printtreenode(Greennode);

            //Console.WriteLine("\n\n Blue tree: \n");
            //Printtreenode(Bluenode);

        }


        private void DeserializeTreeNode(BinaryReader reader, ref huffmannode node)
        {


            if (node.pixel >= 0) // This is leaf
            {

                return;
            }
            // Left
            short templeft = reader.ReadInt16();
            node.left = new huffmannode(templeft);
            DeserializeTreeNode(reader, ref node.left);




            //Right
            short tempright = reader.ReadInt16();
            node.right = new huffmannode(tempright);
            DeserializeTreeNode(reader, ref node.right);

            // -3   24   231  -1 -3  158  97  -1  -3  121  134  -1




        }

        

        private string ConvertByteToBinaryString(byte value)
        {
            return Convert.ToString(value, 2).PadLeft(8, '0');
        }

        private RGBPixel[,] BuildImage(huffmannode rednode, huffmannode greennode, huffmannode bluenode, int width, int height, string BinaryCodeRed, string BinaryCodeGreen, string BinaryCodeBlue)
        {
            RGBPixel[,] Image = new RGBPixel[height, width];
            int w = 0, h = 0;
            huffmannode tempred = rednode;

            foreach (char C in BinaryCodeRed)
            {     

                if (C == '0')
                {
                    tempred = tempred.left;
                }
                else if (C == '1')
                {
                    tempred = tempred.right;
                }

                if (tempred.pixel >= 0) // This is Leaf
                {
                    Image[h, w].red = (byte)tempred.pixel;

                    tempred = rednode;

                    w++;
                    if (w >= width)
                    {
                        h++;
                        w = 0;
                        if (h >= height)
                        {
                            break;
                        }
                    }

                }



            }


            huffmannode tempgreen = greennode;
            w = 0; h = 0;
            foreach (char C in BinaryCodeGreen)
            {


                if (C == '0')
                {
                    tempgreen = tempgreen.left;
                }
                else if (C == '1')
                {
                    tempgreen = tempgreen.right;
                }

                if (tempgreen.pixel >= 0)
                {
                    Image[h, w].green = (byte)tempgreen.pixel;

                    tempgreen = greennode;
                    w++;
                    if (w >= width)
                    {
                        h++;
                        w = 0;
                        if (h >= height)
                        {
                            break;
                        }
                    }
                }


            }


            huffmannode tempblue = bluenode;
            w = 0; h = 0;
            foreach (char C in BinaryCodeBlue)
            {

                

                if (C == '0')
                {
                    tempblue = tempblue.left;
                }
                else if (C == '1')
                {
                    tempblue = tempblue.right;
                }

                if (tempblue.pixel >= 0)
                {
                    Image[h, w].blue = (byte)tempblue.pixel;

                    tempblue = bluenode;

                    w++;
                    if (w >= width)
                    {
                        h++;
                        w = 0;
                        if (h >= height)
                        {
                            break;
                        }
                    }


                }


            }

            return Image;
        }








        #endregion

        



    }
}
