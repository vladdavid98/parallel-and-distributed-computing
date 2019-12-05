package ro.davidvlad;

import java.util.Random;
import java.util.concurrent.Executors;
import java.util.concurrent.ThreadPoolExecutor;
import java.util.concurrent.TimeUnit;

class BigNumberOperations {
    private static final int BOUND = 10;

    // Multiplies str1 and str2, and prints result.
    private static String multiply(String num1, String num2) {
        int len1 = num1.length();
        int len2 = num2.length();
        if (len1 == 0 || len2 == 0)
            return "0";

        // will keep the result number in vector
        // in reverse order
        int[] result = new int[len1 + len2];

        // Below two indexes are used to
        // find positions in result.
        int i_n1 = 0;
        int i_n2 = 0;

        // Go from right to left in num1
        for (int i = len1 - 1; i >= 0; i--) {
            int carry = 0;
            int n1 = num1.charAt(i) - '0';

            i_n2 = 0;

            // Go from right to left in num2
            for (int j = len2 - 1; j >= 0; j--) {
                int n2 = num2.charAt(j) - '0';
                int sum = n1 * n2 + result[i_n1 + i_n2] + carry;
                carry = sum / 10;
                result[i_n1 + i_n2] = sum % 10;
                i_n2++;
            }

            if (carry > 0)
                result[i_n1 + i_n2] += carry;

            i_n1++;
        }

        // ignore '0's from the right
        int i = result.length - 1;
        while (i >= 0 && result[i] == 0)
            i--;

        // If all were '0's - means either both
        // or one of num1 or num2 were '0'
        if (i == -1)
            return "0";

        StringBuilder s = new StringBuilder();

        while (i >= 0)
            s.append(result[i--]);

        return s.toString();
    }


    public static String MultiplySingleThread(String str1, String str2) {
        String sign = "";

        if ((str1.charAt(0) == '-' || str2.charAt(0) == '-') &&
                (str1.charAt(0) != '-' || str2.charAt(0) != '-'))
            sign += "-";

        if (str1.charAt(0) == '-' &&
                str2.charAt(0) != '-') {
            str1 = str1.substring(1);
        } else if (str1.charAt(0) != '-' &&
                str2.charAt(0) == '-') {
            str2 = str2.substring(1);
        } else if (str1.charAt(0) == '-' &&
                str2.charAt(0) == '-') {
            str1 = str1.substring(1);
            str2 = str2.substring(1);
        }
        return sign + multiply(str1, str2);
    }

    public static String MultiplyMultithreaded(String str1, String str2, int nrThreads) throws InterruptedException {
        int str1len = str1.length();
        int str2len = str2.length();
        int lenForEachThread = str1len / nrThreads;
        int remainingLen = str1len % nrThreads;

        StringBuffer resultBuff = new StringBuffer(str1len + str2len + 1);

        if(remainingLen>0)nrThreads++;

        ThreadPoolExecutor executor = (ThreadPoolExecutor) Executors.newFixedThreadPool(nrThreads);

        // inmulteste str2 cu (2 cate 2) din str1;

        int startIndex = 0;
        int endIndex = 0;

        for(int i=0;i<str1len/lenForEachThread;i++){
            startIndex = endIndex;
            endIndex=startIndex+lenForEachThread;

            // TODO: trebe pe task sa il fac sa modifice buffer-u rezultat, + sa puna unde trebe numerele (grija la 10^n)

            MultiplicationTaskBigNumber task = new MultiplicationTaskBigNumber(resultBuff,str1.substring(startIndex,endIndex), str2);
            executor.execute(task);
        }















        executor.shutdown();
        executor.awaitTermination(50, TimeUnit.SECONDS);
        return resultBuff.toString();
    }



    public static String RandomNumberCreator(int length) {
        StringBuilder s = new StringBuilder();
        Random randomGenerator = new Random();
        for (int i = 0; i < length; i++) {
            s.append(randomGenerator.nextInt(BOUND));
        }
        return s.toString();
    }
}



