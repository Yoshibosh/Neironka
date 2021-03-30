using System;
using System.IO;
using System.Collections.Generic;
using static System.Console;

namespace Neiro{

    public class Net{
        public static void Neironka(){

            Layer.n = 0.5;

            double[] correctValues = new double[10]{0.01,0.1,0.2,0.3,0.4,0.5,0.6,0.7,0.8,0.9};

            Layer.SetNeedValues(correctValues);

            Layer[] layers = new Layer[3];
            layers[0] = new Layer(225);
            layers[1] = new Layer(200);
            layers[2] = new Layer(10,true);
                        
            int number = 0;
            int variation = 0;
            int count = 0;
            double SredOjibka = 1; 

            for (int k = 0;Math.Abs(SredOjibka) > 0.01;k++){

                SredOjibka = 0;
                
                if (number > 9){number = 0;}
                if (variation > 5){variation = 0;}
                

                layers[0].SetData(GetData(number,variation));

                for (int j = 1;j < 3;j++){
                    for (int i = 0;i < layers[j].length;i++){
                        layers[j].neurons[i].Activation();
                        if (true){
                            //WriteLine("Layer = {0}\tneuronPosition = {1}\tvalue = {2}",layers[j].currentCount,i,layers[j].neurons[i].value);
                        }
                    }
                }
                for (int j = 1;j < 3;j++){
                    for (int i = 0;i < layers[j].length;i++){
                        layers[j].neurons[i].ErrorCalculation();
                        if (true){
                            //WriteLine("Layer = {0}\tneuronPosition = {1}\tWrongValue = {2}",layers[j].currentCount,i,layers[j].neurons[i].wrongValue);
                        }
                    }
                }
                
                
                for (int i = 0; i < layers[2].length;i++){
                    SredOjibka += Math.Abs(layers[2].neurons[i].wrongValue);
                } 
                SredOjibka /= layers[2].length;


                number++;
                variation++;
                count++;

                for (int i = 0; i < layers[2].length;i++){
                    WriteLine("Value {0} = {1}\tojibka = {2}\tSredOjibka = {3}",correctValues[i],layers[2].neurons[i].value,layers[2].neurons[i].wrongValue,SredOjibka);
                }  
                //WriteLine("Value {0} = {1}\tojibka = {2}\tSredOjibka = {3}",correctValues[9],layers[2].neurons[9].value,layers[2].neurons[9].wrongValue,SredOjibka);

            }
        
            for (int i = 0; i < layers[2].length;i++){
                WriteLine("Value {0} = {1}\tojibka = {2}\tSredOjibka = {3}",correctValues[i],layers[2].neurons[i].value,layers[2].neurons[i].wrongValue,SredOjibka);
            } 

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
        public static double n; 
        public static double[] correctValues;
        public static int Count = 0;
        public int currentCount;
        public Neuron[] neurons;
        public int length;
        public bool last;
        public static Layer previousLayer = null;
        public Layer prevLayer;
        public Layer nextLayer;
        public Layer(int length,bool last = false){
            Random rand = new Random();

            WriteLine(Count);

            

            if (!last){
                length += 1;
            }

            this.prevLayer = previousLayer;            
            this.neurons = new Neuron[length];
            this.length = length;

            for (int i = 0;i < length;i++){
                this.neurons[i] = new Neuron(this);
                this.neurons[i].count = i;
            }

            if (!last){
                this.neurons[length - 1].offsetNeuron = true;
                this.neurons[length - 1].value = 1;
            }

            if (previousLayer != null)
            {
                for (int i = 0;i < length;i++){
                    this.neurons[i].leftConnections = new double[this.prevLayer.length];
                    for (int j = 0; j < prevLayer.length;j++){
                        if(!this.neurons[i].offsetNeuron){
                            this.neurons[i].leftConnections[j] = rand.Next(-1,1)*rand.NextDouble();
                        }
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
            for (int i = 0;i < this.length - 1;i++){
                this.neurons[i].value = data[i];
            }
        }

        public static void SetNeedValues(double[] values){
            correctValues = values;
        }
    }

    class Neuron{
        public bool offsetNeuron;
        public double[] leftConnections;
        public double[] rightConnections = null;
        public int count;
        public double value;
        public double wrongValue;

        public double sum;
        public Layer layer;

        public Neuron(Layer layer){

            this.layer = layer;
            this.value = 0;

        }

        public void Activation(){
            sum = 0;

            if (this.layer.prevLayer == null || this.offsetNeuron){
                return;
            }

            for (int i = 0;i < this.layer.prevLayer.length;i++){
                sum += this.leftConnections[i] * this.layer.prevLayer.neurons[i].value;
            }
            this.value = 1/(1 + Math.Exp(sum*(-1)));
        }

        public void ErrorCalculation(){
            double summ = 0;

            if (this.offsetNeuron){return;}

            if (this.rightConnections == null){
                this.wrongValue = Layer.correctValues[this.count] - this.value;
                return;
            }else{
                for (int i = 0;i < this.layer.nextLayer.length;i++){
                    summ += this.layer.nextLayer.neurons[i].wrongValue * this.rightConnections[i];
                }
            }

            this.wrongValue = summ; 
            
        }

        public void ErorAdjustment(){

            for (int i = 0;i < this.rightConnections.Length;i++){
                this.rightConnections[i] = this.rightConnections[i] + Layer.n * this.layer.nextLayer.neurons[i].wrongValue*(Math.Exp(sum)/Math.Pow(1 + Math.Exp(sum),2))*this.value;
            }
        }

    }

}