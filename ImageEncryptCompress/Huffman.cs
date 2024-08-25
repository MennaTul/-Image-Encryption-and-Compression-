using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEncryptCompress
{
    public class huffmannode : IComparable<huffmannode> //abstract function
    {

        public short pixel; //key
        public int frequency; //value
        public huffmannode left;
        public huffmannode right;
        public int type; //0 or 1
        public string colorcode;



        public huffmannode()
        {
           

        }
        public huffmannode(short pixel)
        {
            this.pixel = pixel;

        }


        public huffmannode(int frequncy)
        {
            this.frequency = frequncy;


        }

        public huffmannode(short pixel, int frequency) //constructor
        {
            this.pixel = pixel;
            this.frequency = frequency;
            left = right = null;
        }
        public int CompareTo(huffmannode other)
        {
            return frequency.CompareTo(other.frequency);
        }


    }





    public class Huffman
    {
        public PriorityQueue<huffmannode> queue = new PriorityQueue<huffmannode>();

        public void buildbinarytree(Dictionary<short, int> color)
        {
            

            foreach (var node in color)
            {
                queue.Enqueue(new huffmannode(node.Key, node.Value));
            }

            huffmannode left, right, top;

            int length = queue.Count();
            short counter = -3;
            while (length != 1)
            {
                right = queue.Dequeue();
                right.type = 1;
                left = queue.Dequeue();
                left.type = 0;

                top = new huffmannode(left.frequency + right.frequency);
                top.left = left;
                top.right = right;
                top.pixel = counter;  
                queue.Enqueue(top);

                length = queue.Count();
                counter--;
            }
            


            Encode(queue.Peek(), new StringBuilder());


        }
        public Dictionary<short, string> ColorCode = new Dictionary<short, string>();
        public void Encode(huffmannode root, StringBuilder code)
        {
            if (root.left == null && root.right == null)
            {
                ColorCode.Add(root.pixel, code.ToString());
                root.colorcode = code.ToString();
                return;
            }

            code.Append("0");
            Encode(root.left, code);

            code[code.Length - 1] = '1';
            Encode(root.right, code);

            code.Remove(code.Length - 1, 1);

        }



    }


}
