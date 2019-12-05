package ro.davidvlad;

public class MultiplicationTaskBigNumber implements Runnable {
    private StringBuffer result;
    private String str1, str2;

    public MultiplicationTaskBigNumber(StringBuffer result, String str1, String str2) {
        this.result = result;
        this.str1 = str1;
        this.str2 = str2;
    }

    @Override
    public void run() {

    }


}