package ro.davidvlad;

import java.util.Random;
import java.util.concurrent.*;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReentrantLock;

public class Main {
    private static Random rand = new Random(2);

    private static int[][] matrix_a = GenerateRandomMatrix(25, 25);
    private static int[][] matrix_b = GenerateRandomMatrix(25, 25);
    private static int[][] matrix_c = GenerateRandomMatrix(25, 25);
    private static int[][] matrix_prod_ab = new int[matrix_a[0].length][matrix_b.length];
    private static int[][] matrix_prod_abc = new int[matrix_a[0].length][matrix_b.length];

    private static final Lock lock = new ReentrantLock();

    private static final Condition rowDone = lock.newCondition();

    public static void main(String[] args) throws InterruptedException {

        //start timer
        long startTime;
        long stopTime;
        long elapsedTime;

        startTime = System.currentTimeMillis();

        //Create all threads for product
        ExecutorService executorServiceAB = Executors.newFixedThreadPool(3);
        for (int rows = 0; rows < matrix_a.length; rows++) {
            Main.MatrixProdAB thr = new MatrixProdAB(rows);
            executorServiceAB.submit(thr);
        }

        ExecutorService executorServiceABC = Executors.newFixedThreadPool(1);
        for (int rows = 0; rows < matrix_a.length; rows++) {
            Main.MatrixProdABC thr = new MatrixProdABC(rows);
            executorServiceABC.submit(thr);
        }

        executorServiceAB.shutdown();
        executorServiceABC.shutdown();
        if (!executorServiceAB.awaitTermination(100, TimeUnit.SECONDS))
            executorServiceAB.shutdownNow();
        if (!executorServiceABC.awaitTermination(100, TimeUnit.SECONDS))
            executorServiceABC.shutdownNow();


        ///// Write Matrices ///////
        System.out.println("\nA*B");
        for (int[] ints : matrix_prod_ab) {
            for (int j = 0; j < matrix_prod_ab[0].length; j++) {
                System.out.print(ints[j] + " ");
            }
            System.out.println();
        }

        System.out.println("\nA*B*C");
        for (int[] ints : matrix_prod_abc) {
            for (int j = 0; j < matrix_prod_abc[0].length; j++) {
                System.out.print(ints[j] + " ");
            }
            System.out.println();
        }
        ////////////////////////////

        stopTime = System.currentTimeMillis();
        elapsedTime = stopTime - startTime;
        System.out.println("Elapsed time(ms): " + elapsedTime);


        //verify zeroes
        for (int[] ints : matrix_prod_abc)
            for (int j = 0; j < matrix_prod_abc[0].length; j++)
                if (ints[j] == 0) System.out.println("ERROR");

        System.out.println("DONE");
    }

    static class MatrixProdAB extends Thread {
        int row;

        MatrixProdAB(int row) {
            this.row = row;
        }

        public void run() {
            lock.lock();

            for (int j = 0; j < matrix_b[row].length; j++) {
                for (int k = 0; k < matrix_a[row].length; k++) {
                    matrix_prod_ab[row][j] += matrix_a[row][k] * matrix_b[k][j];
                }
            }
            rowDone.signal();
            lock.unlock();
        }
    }

    static class MatrixProdABC extends Thread {
        int row;

        MatrixProdABC(int row) {
            this.row = row;
        }

        public void run() {
            lock.lock();

            try {
                while (!rowIsFilled(matrix_prod_ab, row)) {
                    rowDone.await();
                }
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
            for (int j = 0; j < matrix_prod_ab[row].length; j++)
                for (int k = 0; k < matrix_c[row].length; k++) {
                    matrix_prod_abc[row][j] += matrix_prod_ab[row][k] * matrix_c[k][j];
                }
            lock.unlock();
        }
    }

    private static boolean rowIsFilled(int[][] mat, int row) {
        for (int i = 0; i < mat.length; i++) {
            if (mat[row][i] == 0) return false;
        }
        return true;
    }


    private static int[][] GenerateRandomMatrix(int nrOfRows, int nrOfColumns) {
        int[][] result = new int[nrOfRows][nrOfColumns];

        for (int i = 0; i < nrOfRows; i++) for (int j = 0; j < nrOfColumns; j++) result[i][j] = rand.nextInt(10);

        return result;
    }


}
