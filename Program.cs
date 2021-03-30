using System;
using static System.Console;
using System.Numerics;
using System.Diagnostics;
using Neiro;

namespace Proga
{
    public class Diagonal
{        
  public static int Main(){
          
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Neiro.Net.Neironka();//
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);

            stopWatch.Start();
            //diagonal(20,5);//
            stopWatch.Stop();
            ts = stopWatch.Elapsed;

            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            
            Console.WriteLine("RunTime " + elapsedTime);

            return 0;
        }
        public static BigInteger diagonal(int n, int p)
        {
          BigInteger sum = 1;
          BigInteger cum,cs = 1,sc = 0;
          int stop = p;
          for (int k = 0;k <= (n - stop);k++){

              cum = sum;
              sum = fac(k+1,p)/facFast(p-k);
              cs = sum - cum;
              sc += cs;  

              
              WriteLine($"sum\tcum\tcs\tsc/k\n{sum}\t{cum}\t{cs}\t{(double)sc/k}\n\n");

              
            //Console.WriteLine("\nsum = {0}\nn = {1}\nk = {2}\nfac = {3}\n",sum,n,k,facFast(p)/(facFast(k)*facFast(p-k)));
            if (sum % 1000 == 0){
              //Console.WriteLine(sum);
            }
            p++;
          }
          
          return sum;
        }
  
        public static BigInteger fac(BigInteger k,int n){
          for (BigInteger i = k+1;i <= n;i++){
            k *= i;
            //Console.WriteLine(i);
          }
          return k;
        }
        public static BigInteger facFast(int n){
          int[] mass = new int[n+1];
          int p = 2;
          BigInteger sum = 1;
          BigInteger k = 1;
          for (int i = 0;i <= n;i++){
            mass[i] = i;
            //Console.WriteLine("mass[{0}] = {1}",mass[i],i);
          }

          ////////
          int t = 0;
          while (t != p){
            t = p;
            for (int i = 2;(i*p) <= n;i++){
              mass[i*p] = 0;
              //Console.WriteLine("\ni*p = {0}\n",i*p);
            }
            for (int i = p+1;i < n;i++){
              if (mass[i] != 0){
                p = mass[i];
                break;
              }
            }
          }
          ////////
          for (int j = 2;j <= n;j++){

            p = mass[j];
            mass[j] = 0;
            k = 1;
            //Console.WriteLine("\nP = {0}\n j = {1}\n",p,j);
            for (int i = 1;(n/k) > 1;i++){
              if (p == 0){break;}
              k = pow(p,i);
              mass[j] += n/(int)k;
             //Console.WriteLine("\np = {0}\nk = {1}\nmass[j] = {2}\n",p,k,mass[j]);
            }
          }
          for (int i = 2;i < n+1;i++){
            sum *= pow(i,mass[i]);
          }
          return sum;
            
        }
        public static BigInteger pow(int n,int k){
          BigInteger res = 1;
          for (int i = 0;i < k;i++){
            res *= n;
          }
          return res;
        }
  
        public static void time(){

        }

  }

  
}

