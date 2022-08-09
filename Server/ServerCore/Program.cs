using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class Program
    {
        volatile static bool _stop = false; // volatile을 사용하지 않으면 쓰레드 간 충돌이 일어나서 버그가 발생할 수 있음.

        static void ThreadMain()
        {
            Console.WriteLine("쓰레드 시작!");

            while(_stop == false)
            {
                // 누군가가 stop신호를 해주길 기다린다.
            }

            Console.WriteLine("쓰레드 종료!");
        }

        static void Main(string[] args)
        {
            Task t = new Task(ThreadMain); // ThreadPool에서 쓰레드를 끌어와서 ThreadMain실행.
            t.Start();

            Thread.Sleep(1000); // 쓰레드를 1초동안 Sleep시킴.

            _stop = true;

            Console.WriteLine("Stop 호출");
            Console.WriteLine("종료 대기중");

            t.Wait(); // 쓰레드가 종료될때까지 기다림.

            Console.WriteLine("종료 성공");
        }
    }
}
