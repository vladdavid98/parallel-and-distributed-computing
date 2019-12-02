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

        stopTime = System.currentTimeMillis();
        elapsedTime = stopTime - startTime;

        ///// Write Matrices ///////
        System.out.println("\nA*B");
        printMatrix(matrix_prod_ab);

        System.out.println("\nA*B*C");
        printMatrix(matrix_prod_abc);
        ////////////////////////////

        System.out.println("Elapsed time(ms): " + elapsedTime);


        //verify zeroes
        for (int[] ints : matrix_prod_abc)
            for (int j = 0; j < matrix_prod_abc[0].length; j++)
                if (ints[j] == 0) System.out.println("ERROR");

        System.out.println("DONE");
    }

    private static void printMatrix(int[][] matrix) {
        for (int[] ints : matrix) {
            for (int j = 0; j < matrix[0].length; j++) {
                System.out.print(ints[j] + " ");
            }
            System.out.println();
        }
    }

    static class MatrixProdAB extends Thread {
        int row;

        MatrixProdAB(int row) {
            this.row = row;
        }

        public void run() {
            lock.lock();
            doMatrixProd(matrix_a, matrix_b, matrix_prod_ab, row);
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
            doMatrixProd(matrix_c, matrix_prod_ab, matrix_prod_abc, row);
            lock.unlock();
        }
    }

    private static void doMatrixProd(int[][] matrix_a, int[][] matrix_b, int[][] matrix_prod, int rowNr) {
        for (int j = 0; j < matrix_b[rowNr].length; j++) {
            for (int k = 0; k < matrix_a[rowNr].length; k++) {
                matrix_prod[rowNr][j] += matrix_a[rowNr][k] * matrix_b[k][j];
            }
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
