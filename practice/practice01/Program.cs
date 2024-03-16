namespace practice01;

class Program
{
    static void Main(string[] args)
    {
        int[] arr1 = { 1, 2, 3, 4, 5,500 };
        Console.WriteLine(Avg(arr1));
    }

    public static float Avg(int[] arr)
    {
        float avg = 0;
        int sum = 0;
        foreach (int i in arr)
            sum += i;
        avg = (float)sum / arr.Length;
        return avg;
    }
}