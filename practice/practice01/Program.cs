namespace practice01;

class Program
{
    static void Main(string[] args)
    {
        int[] arr1 = { 1, 2, 3, 4, 5,500 };
        int[] arr2 = { 1, 2, 3, 4, 5,-500 };
        
        Console.WriteLine("average: " + Avg(arr1));
        Console.WriteLine("average: " + Avg(arr2));
        Console.WriteLine("max: " + Max(arr1));
    }

    public static float Avg(int[] arr)
    {
        float avg = 0;
        int sum = 0;
        foreach (int a in arr)
            sum += a;
        avg = (float)sum / arr.Length;
        return avg;
    }

    public static int Max(int[] arr)
    {
        int max = int.MinValue;
        foreach (int i in arr)
        {
            if (i > max) max = i;
        }
        return max;
    }
}