using System;
using System.IO;
using System.Collections.Generic;
using static System.Console;

namespace Neiro{

    public class Net{
        public static void Neironka(){

            Layer[] layers = new Layer[3];
            WriteLine("1");
            layers[0] = new Layer(225);
            WriteLine("2");
            layers[1] = new Layer(200,layers[0]);
            WriteLine("3");
            layers[2] = new Layer(10,layers[1]);
            WriteLine("4");
            
            int number = 0;
            int variation = 0;

            for (int k = 0;k < 1000;k++){
                
                if (number > 9){number = 0;}
                if (variation > 5){variation = 0;}
                
                WriteLine("5");

                layers[0].SetData(GetData(number,variation));
                WriteLine("6");

                for (int j = 1;j < 3;j++){
                    for (int i = 0;i < layers[j].length;i++){
                        layers[j].neurons[i].Activation();
                        WriteLine("Layer = {0}\tneuronPosition = {1}\tvalue = {2}",layers[j].neurons[i].layer.currentCount,i,layers[j].neurons[i].value);
                    }
                }

                number++;
                variation++;

            }
            


        }

        static byte[] GetData(int number,int variation){
            string path = $@"C:\Users\User\Desktop\a\Kod\CS\app\dataset\{number}({variation}).data";

            WriteLine(path);

            byte[] data = new byte[675];
            byte[] newData = new byte[225];

            try{
                using (BinaryReader reader = new BinaryReader(File.Open(path,FileMode.Open)))
                {
                    reader.Read(data,0,data.Length);
                }
            }
            catch{
                WriteLine("\nFile does't exist.");
                return null;
            }

            int counter = 0;
            for (int i = 0;i < data.Length;i += 3){
                if (data[i] == 255){
                    newData[i - counter*2] = 0;
                }else{
                    newData[i - counter*2] = 1;
                }
                counter++;
            }
            /*
            counter = 0;
            for (int i = 0;i < 225;i++){
                if (newData[i] == 0){
                    Write("#");
                }else{
                    Write(" ");
                }
                if ((i + 1) % 15 == 0 && i != 0){
                    counter++;
                    WriteLine();
                }
            }*/

            return newData;

        }

    }

    class Layer{

        public static int Count = 0;
        public int currentCount = 0;
        public Neuron[] neurons;
        public int length;
        public Layer previousLayer;
        public Layer(int length,Layer previousLayer = null){

            this.currentCount = Count;
            Count++;

            this.previousLayer = previousLayer;
            this.neurons = new Neuron[length];
            this.length = length;
    
            for (int i = 0;i < length;i++){
                this.neurons[i] = new Neuron(this);
            }

        }

        public void SetData(byte[] data){
            for (int i = 0;i < this.length;i++){
                neurons[i].value = data[i];
            }
        }
    }

    class Neuron{
        public double[] connections;
        public double value;
        public Layer layer;

        public Neuron(Layer layer){

            Random rand = new Random();

            this.layer = layer;
            this.value = 0;

            if (this.layer.previousLayer != null){
                
                this.connections = new double[this.layer.previousLayer.length];

                for(int i = 0;i < this.layer.previousLayer.length;i++){
                    this.connections[i] = rand.Next(-1,1)*rand.NextDouble();
                }
            }
        }

        public void Activation(){
            double sum = 0;

            WriteLine("Layer = {0}",this.layer.currentCount);
            for (int i = 0;i < this.layer.previousLayer.length;i++){
                sum += this.connections[i]*this.layer.previousLayer.neurons[i].value;
            }
            this.value = 1/(1 + Math.Exp(sum*(-1)));
        }
    }

}