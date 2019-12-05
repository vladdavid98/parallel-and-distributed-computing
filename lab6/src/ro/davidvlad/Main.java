package ro.davidvlad;

import java.util.concurrent.ExecutionException;

public class Main {
    public static void main(String[] args) throws InterruptedException, ExecutionException {

        String str1 = "1234";
        String str2 = "5275";
//        String str1 = BigNumberMultiplication.RandomNumberCreator(15000);
//        String str2 = BigNumberMultiplication.RandomNumberCreator(15000);
        Polynomial p = new Polynomial(3);
        Polynomial q = new Polynomial(3);
//		Polynomial p = new Polynomial(new ArrayList<Integer>(Arrays.asList(5, 4, 2, 4)));
//		Polynomial q = new Polynomial(new ArrayList<Integer>(Arrays.asList(6, 3, 7)));

        System.out.println("pol p:" + p);
        System.out.println("pol q:" + q);
        System.out.println("\n");


        //Simple
        System.out.println(multiplication1(p, q).toString() + "\n");
        System.out.println(multiplication2(p, q).toString() + "\n");

        //Karatsuba
        System.out.println(multiplication3(p, q).toString() + "\n");
        System.out.println(multiplication4(p, q).toString() + "\n");

        System.out.println(bigNumberMultiplicationSequential(str1, str2));
    }

    private static Polynomial multiplication4(Polynomial p, Polynomial q) throws ExecutionException,
            InterruptedException {
        long startTime = System.currentTimeMillis();
        Polynomial result4 = PolynomialOperations.multiplicationKaratsubaParallelizedForm(p, q, 4);
        long endTime = System.currentTimeMillis();
        System.out.println("Karatsuba parallel multiplication of polynomials: ");
        System.out.println("Execution time : " + (endTime - startTime) + " ms");
        return result4;
    }

    private static Polynomial multiplication3(Polynomial p, Polynomial q) {
        long startTime = System.currentTimeMillis();
        Polynomial result3 = PolynomialOperations.multiplicationKaratsubaSequentialForm(p, q);
        long endTime = System.currentTimeMillis();
        System.out.println("Karatsuba sequential multiplication of polynomials: ");
        System.out.println("Execution time : " + (endTime - startTime) + " ms");
        return result3;
    }

    private static Polynomial multiplication2(Polynomial p, Polynomial q) throws InterruptedException {
        long startTime = System.currentTimeMillis();
        Polynomial result2 = PolynomialOperations.multiplicationParallelizedForm(p, q, 4);
        long endTime = System.currentTimeMillis();
        System.out.println("Simple parallel multiplication of polynomials: ");
        System.out.println("Execution time : " + (endTime - startTime) + " ms");
        return result2;
    }

    private static Polynomial multiplication1(Polynomial p, Polynomial q) {
        long startTime = System.currentTimeMillis();
        Polynomial result1 = PolynomialOperations.multiplicationSequentialForm(p, q);
        long endTime = System.currentTimeMillis();
        System.out.println("Simple sequential multiplication of polynomials: ");
        System.out.println("Execution time : " + (endTime - startTime) + " ms");
        return result1;
    }

    private static String bigNumberMultiplicationSequential(String str1, String str2){
        long startTime = System.currentTimeMillis();
        String result = BigNumberOperations.MultiplySingleThread(str1, str2);
        long endTime = System.currentTimeMillis();
        System.out.println("Simple big number multiplication based on strings: ");
        System.out.println("Execution time : " + (endTime - startTime) + " ms");
        return result;
    }
}
