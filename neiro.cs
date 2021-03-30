using System;
using System.IO;
using System.Collections.Generic;
using static System.Console;

namespace Neiro{

    public class Net{
        public static void Neironka(){

            double[] correctValues = new double[10]{0.1,0.1,0.2,0.3,0.4,0.5,0.6,0.7,0.8,0.9};

            Layer.SetNeedValues(correctValues);

            Layer[] layers = new Layer[3];
            layers[0] = new Layer(225);
            layers[1] = new Layer(200);
            layers[2] = new Layer(10);
            
            WriteLine(" rqweijhropi{0}",layers[2].neurons[0].rightConnections);
            
            int number = 0;
            int variation = 0;

            for (int k = 0;k < 1000;k++){
                
                if (number > 9){number = 0;}
                if (variation > 5){variation = 0;}
                

                layers[0].SetData(GetData(number,variation));

                for (int j = 1;j < 3;j++){
                    for (int i = 0;i < layers[j].length;i++){
                        layers[j].neurons[i].Activation();
                        if (j == 2){
                            WriteLine("Layer = {0}\tneuronPosition = {1}\tvalue = {2}",layers[j].currentCount,i,layers[j].neurons[i].value);
                        }
                    }
                }
                for (int j = 1;j < 3;j++){
                    for (int i = 0;i < layers[j].length;i++){
                        layers[j].neurons[i].ErrorCalculation();
                        if (j == 2){
                            WriteLine("Layer = {0}\tneuronPosition = {1}\tWrongValue = {2}",layers[j].currentCount,i,layers[j].neurons[i].wrongValue);
                        }
                    }
                }

                number++;
                variation++;

            }

            WriteLine(layers[2].neurons[0].wrongValue);
            


        }

        static byte[] GetData(int number,int variation){
            string path = $@"C:\Users\User\Desktop\a\Kod\CS\app\dataset\{number}({variation}).data";

            //WriteLine(path);

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
        public static double[] correctValues;
        public static int Count = 0;
        public int currentCount;
        public Neuron[] neurons;
        public int length;
        public static Layer previousLayer = null;
        public Layer prevLayer;
        public Layer nextLayer;
        public Layer(int length){
            Random rand = new Random();

            WriteLine(Count);

            this.prevLayer = previousLayer;            
            this.neurons = new Neuron[length];
            this.length = length;

            for (int i = 0;i < length;i++){
                this.neurons[i] = new Neuron(this);
                this.neurons[i].count = i;
            }

            if (previousLayer != null)
            {
                for (int i = 0;i < length;i++){
                    this.neurons[i].leftConnections = new double[this.prevLayer.length];
                    for (int j = 0; j < prevLayer.length;j++){
                         this.neurons[i].leftConnections[j] = rand.Next(-1,1)*rand.NextDouble();
                    }
                }
                for (int i = 0;i < this.prevLayer.length;i++){
                    this.prevLayer.neurons[i].rightConnections = new double[length];
                    for (int j = 0; j < length;j++){
                         this.prevLayer.neurons[i].rightConnections[j] = rand.Next(-1,1)*rand.NextDouble();
                    }
                }


                this.prevLayer.nextLayer = this;
            }


            this.currentCount = Count;
            Count++;

            previousLayer = this;

        }

        public void SetData(byte[] data){
            if (this.currentCount != 0){
                throw new Exception("This layer is not first");
            }
            for (int i = 0;i < this.length;i++){
                this.neurons[i].value = data[i];
            }
        }

        public static void SetNeedValues(double[] values){
            correctValues = values;
        }
    }

    class Neuron{
        public double[] leftConnections;
        public double[] rightConnections = null;
        public int count;
        public double value;
        public double wrongValue;
        public Layer layer;

        public Neuron(Layer layer){

            this.layer = layer;
            this.value = 0;

        }

        public void Activation(){
            double sum = 0;

            if (this.layer.prevLayer == null){
                return;
            }

            for (int i = 0;i < this.layer.prevLayer.length;i++){
                sum += this.leftConnections[i] * this.layer.prevLayer.neurons[i].value;
            }
            this.value = 1/(1 + Math.Exp(sum*(-1)));
        }

        public void ErrorCalculation(){
            double sum = 0;

            if (this.rightConnections == null){
                this.wrongValue = Layer.correctValues[this.count] - this.value;
                return;
            }else{
                for (int i = 0;i < this.layer.nextLayer.length;i++){
                    sum += this.layer.nextLayer.neurons[i].wrongValue * this.rightConnections[i];
                }
            }

            this.wrongValue = sum; 
            
        }

    }

}