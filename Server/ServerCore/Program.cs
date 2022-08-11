using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    // 메모리 배리어
    // A) 코드 재배치 억제
    // B) 가시성

    // 1) Full Memory Barrier (ASM MFENCE, C# Thread.MemoryBerrier) : Store/Load 둘다 막는다.
    // 2) Store Memory Barrier (ASM SFENCE) : Store만 막는다.
    // 3) Load Memory Barrier (ASM LFENCE) : Load만 막는다.

    class Program
    {
        static int _answer;
        static bool _complete;

        static void A()
        {
            _answer = 123;
            Thread.MemoryBarrier(); // Barrier 1
            _complete = true;
            Thread.MemoryBarrier(); // Barrier 2 <= 또 다른 Store를 위한 메모리 배리어.
        }

        static void B()
        {
            Thread.MemoryBarrier(); // Barrier 3 <= Load를 위한 메모리 배리어.
            if (_complete)
            {
                Thread.MemoryBarrier(); // Barrier 4 <= 새로 Load를 하기 위한 메모리 배리어.
                Console.WriteLine(_answer);
            }
        }
        

        static void Main(string[] args)
        {
            Task t1 = new Task(A);
            Task t2 = new Task(B);

            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);
        }
    }
}
